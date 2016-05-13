﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.Utilities;
using QSOrmProject;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Store;

namespace Vodovoz.Domain.Documents
{
	[OrmSubject (Gender = QSProjectsLib.GrammaticalGender.Masculine,
		NominativePlural = "документы перемещения ТМЦ",
		Nominative = "документ перемещения ТМЦ")]
	public class MovementDocument: Document, IValidatableObject
	{
		MovementDocumentCategory category;

		[Display (Name = "Тип документа перемещения")]
		public virtual MovementDocumentCategory Category {
			get { return category; }
			set {
				SetField (ref category, value, () => Category);
				switch (category) {
				case MovementDocumentCategory.counterparty:
					FromWarehouse = null;
					ToWarehouse = null;
					break;
				case MovementDocumentCategory.warehouse:
					FromClient = null;
					ToClient = null;
					break;
				}
			}
		}

		public override DateTime TimeStamp {
			get { return base.TimeStamp; }
			set {
				base.TimeStamp = value;
				foreach (var item in Items) {
					if (item.WarehouseMovementOperation != null && item.WarehouseMovementOperation.OperationTime != TimeStamp)
						item.WarehouseMovementOperation.OperationTime = TimeStamp;
					if (item.CounterpartyMovementOperation != null && item.CounterpartyMovementOperation.OperationTime != TimeStamp)
						item.CounterpartyMovementOperation.OperationTime = TimeStamp;
				}
			}
		}

		string comment;

		[Display (Name = "Комментарий")]
		public virtual string Comment {
			get { return comment; }
			set { SetField (ref comment, value, () => Comment); }
		}

		Employee responsiblePerson;

		[Required (ErrorMessage = "Должен быть указан ответственнй за перемещение.")]
		[Display (Name = "Ответственный")]
		public virtual Employee ResponsiblePerson {
			get { return responsiblePerson; }
			set { SetField (ref responsiblePerson, value, () => ResponsiblePerson); }
		}

		Counterparty fromClient;

		[Display (Name = "Клиент отправки")]
		public virtual Counterparty FromClient {
			get { return fromClient; }
			set {
				SetField (ref fromClient, value, () => FromClient);
				if (FromClient == null ||
				    (FromDeliveryPoint != null && FromClient.DeliveryPoints.All (p => p.Id != FromDeliveryPoint.Id))) {
					FromDeliveryPoint = null;
				}
				foreach (var item in Items) {
					if (item.CounterpartyMovementOperation != null && item.CounterpartyMovementOperation.WriteoffCounterparty != fromClient)
						item.CounterpartyMovementOperation.WriteoffCounterparty = fromClient;
				}
			}
		}

		Counterparty toClient;

		[Display (Name = "Клиент получения")]
		public virtual Counterparty ToClient {
			get { return toClient; }
			set {
				SetField (ref toClient, value, () => ToClient); 
				if (ToClient == null ||
				    (ToDeliveryPoint != null && ToClient.DeliveryPoints.All (p => p.Id != ToDeliveryPoint.Id))) {
					ToDeliveryPoint = null;
				}
				foreach (var item in Items) {
					if (item.CounterpartyMovementOperation != null && item.CounterpartyMovementOperation.IncomingCounterparty != toClient)
						item.CounterpartyMovementOperation.IncomingCounterparty = toClient;
				}
			}
		}

		DeliveryPoint fromDeliveryPoint;

		[Display (Name = "Точка отправки")]
		public virtual DeliveryPoint FromDeliveryPoint {
			get { return fromDeliveryPoint; }
			set {
				SetField (ref fromDeliveryPoint, value, () => FromDeliveryPoint); 
				foreach (var item in Items) {
					if (item.CounterpartyMovementOperation != null && item.CounterpartyMovementOperation.WriteoffDeliveryPoint != fromDeliveryPoint)
						item.CounterpartyMovementOperation.WriteoffDeliveryPoint = fromDeliveryPoint;
				}
			}
		}

		DeliveryPoint toDeliveryPoint;

		[Display (Name = "Точка получения")]
		public virtual DeliveryPoint ToDeliveryPoint {
			get { return toDeliveryPoint; }
			set {
				SetField (ref toDeliveryPoint, value, () => ToDeliveryPoint); 
				foreach (var item in Items) {
					if (item.CounterpartyMovementOperation != null && item.CounterpartyMovementOperation.IncomingDeliveryPoint != toDeliveryPoint)
						item.CounterpartyMovementOperation.IncomingDeliveryPoint = toDeliveryPoint;
				}
			}
		}

		Warehouse fromWarehouse;

		[Display (Name = "Склад отправки")]
		public virtual Warehouse FromWarehouse {
			get { return fromWarehouse; }
			set { 
				SetField (ref fromWarehouse, value, () => FromWarehouse);	
				foreach (var item in Items) {
					if (item.WarehouseMovementOperation != null && item.WarehouseMovementOperation.WriteoffWarehouse != fromWarehouse)
						item.WarehouseMovementOperation.WriteoffWarehouse = fromWarehouse;
				}
			}
		}

		Warehouse toWarehouse;

		[Display (Name = "Склад получения")]
		public virtual Warehouse ToWarehouse {
			get { return toWarehouse; }
			set { 
				SetField (ref toWarehouse, value, () => ToWarehouse); 
				foreach (var item in Items) {
					if (item.WarehouseMovementOperation != null && item.WarehouseMovementOperation.IncomingWarehouse != toWarehouse)
						item.WarehouseMovementOperation.IncomingWarehouse = toWarehouse;
				}
			}
		}

		IList<MovementDocumentItem> items = new List<MovementDocumentItem> ();

		[Display (Name = "Строки")]
		public virtual IList<MovementDocumentItem> Items {
			get { return items; }
			set {
				SetField (ref items, value, () => Items);
				observableItems = null;
			}
		}

		GenericObservableList<MovementDocumentItem> observableItems;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<MovementDocumentItem> ObservableItems {
			get {
				if (observableItems == null)
					observableItems = new GenericObservableList<MovementDocumentItem> (Items);
				return observableItems;
			}
		}

		public virtual string Title { 
			get { return String.Format ("Перемещение ТМЦ №{0} от {1:d}", Id, TimeStamp); }
		}

		#region IValidatableObject implementation

		public virtual System.Collections.Generic.IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			if (Category == MovementDocumentCategory.warehouse && FromWarehouse == ToWarehouse)
				yield return new ValidationResult ("Склады отправления и получения должны различатся.",
					new[] { this.GetPropertyName (o => o.FromWarehouse), this.GetPropertyName (o => o.ToWarehouse) });
			if (Category == MovementDocumentCategory.counterparty) {
				if (FromClient == null)
					yield return new ValidationResult ("Клиент отправитель должен быть указан.",
						new[] { this.GetPropertyName (o => o.FromClient) });
				if (ToClient == null)
					yield return new ValidationResult ("Клиент получатель должен быть указан.",
						new[] { this.GetPropertyName (o => o.ToClient) });
				if (FromDeliveryPoint == null)
					yield return new ValidationResult ("Точка доставки отправителя должена быть указана.",
						new[] { this.GetPropertyName (o => o.FromDeliveryPoint) });
				if (ToDeliveryPoint == null)
					yield return new ValidationResult ("Точка доставки получателя должена быть указана.",
						new[] { this.GetPropertyName (o => o.ToDeliveryPoint) });
				if (FromDeliveryPoint == ToDeliveryPoint)
					yield return new ValidationResult ("Точки отправления и получения должны различатся.",
						new[] { this.GetPropertyName (o => o.FromDeliveryPoint), this.GetPropertyName (o => o.ToDeliveryPoint) });
			}
		}

		#endregion

		public virtual void AddItem (Nomenclature nomenclature, decimal amount, decimal inStock)
		{
			var item = new MovementDocumentItem
			{
					Nomenclature = nomenclature,
					Amount = amount,
					AmountOnSource = inStock,
					Document = this
			};
			if (Category == MovementDocumentCategory.counterparty)
				item.CreateOperation(FromClient, FromDeliveryPoint, ToClient, ToDeliveryPoint, TimeStamp);
			else
				item.CreateOperation(FromWarehouse, ToWarehouse, TimeStamp);
			
			ObservableItems.Add (item);
		}

		public MovementDocument ()
		{
			Comment = String.Empty;
		}
	}

	public enum MovementDocumentCategory
	{
		[Display (Name = "Именное списание")]
		counterparty,
		[Display (Name = "Внутреннее перемещение")]
		warehouse
	}

	public class MovementDocumentCategoryStringType : NHibernate.Type.EnumStringType
	{
		public MovementDocumentCategoryStringType () : base (typeof(MovementDocumentCategory))
		{
		}
	}
}

