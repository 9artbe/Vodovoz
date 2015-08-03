﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using Vodovoz.Domain.Orders;

namespace Vodovoz.Domain.Logistic
{
	[OrmSubject (Gender = QSProjectsLib.GrammaticalGender.Masculine,
		NominativePlural = "маршрутные листы",
		Nominative = "маршрутный лист")]
	public class RouteList: BusinessObjectBase<RouteList>, IDomainObject, IValidatableObject
	{
		public virtual int Id { get; set; }

		Employee driver;

		[Display (Name = "Водитель")]
		public virtual Employee Driver {
			get { return driver; }
			set { SetField (ref driver, value, () => Driver); }
		}

		Employee forwarder;

		[Display (Name = "Экспедитор")]
		public virtual Employee Forwarder {
			get { return forwarder; }
			set { SetField (ref forwarder, value, () => Forwarder); }
		}

		Car car;

		[Display (Name = "Машина")]
		public virtual Car Car {
			get { return car; }
			set { 
				SetField (ref car, value, () => Car); 
				if (value.Driver != null)
					Driver = value.Driver;
			}
		}

		DateTime date;

		[Display (Name = "Дата")]
		public virtual DateTime Date {
			get { return date; }
			set { SetField (ref date, value, () => Date); }
		}

		Decimal plannedDistance;

		[Display (Name = "Планируемое расстояние")]
		public virtual Decimal PlannedDistance {
			get { return plannedDistance; }
			set { SetField (ref plannedDistance, value, () => PlannedDistance); }
		}

		Decimal actualDistance;

		[Display (Name = "Фактическое расстояние")]
		public virtual Decimal ActualDistance {
			get { return actualDistance; }
			set { SetField (ref actualDistance, value, () => ActualDistance); }
		}

		RouteListStatus status;

		[Display (Name = "Статус")]
		public virtual RouteListStatus Status {
			get { return status; }
			set { SetField (ref status, value, () => Status); }
		}

		IList<RouteListItem> addresses = new List<RouteListItem> ();

		[Display (Name = "Адреса в маршрутном листе")]
		public virtual IList<RouteListItem> Addresses {
			get { return addresses; }
			set { 
				SetField (ref addresses, value, () => Addresses); 
				observableAddresses = null;
			}
		}

		GenericObservableList<RouteListItem> observableAddresses;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public GenericObservableList<RouteListItem> ObservableAddresses {
			get {
				if (observableAddresses == null)
					observableAddresses = new GenericObservableList<RouteListItem> (addresses);
				return observableAddresses;
			}
		}

		public virtual string Number { get { return Id.ToString (); } }

		public virtual string DateString { get { return Date.ToShortDateString (); } }

		public virtual string StatusString { get { return Status.GetEnumTitle (); } }

		public virtual string DriverInfo { get { return String.Format ("{0} - {1}", Driver.FullName, Car.Title); } }

		public RouteListItem AddAddressFromOrder (Order order)
		{
			var item = new RouteListItem (this, order);
			ObservableAddresses.Add (item);
			return item;
		}

		public void RemoveAddress (RouteListItem address)
		{
			address.RemovedFromRoute ();
			UoW.Delete (address);
			ObservableAddresses.Remove (address);
		}

		#region IValidatableObject implementation

		public IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			if (Driver == null)
				yield return new ValidationResult ("Не заполнен водитель.");
			if (Car == null)
				yield return new ValidationResult ("На заполнен автомобиль.");
		}

		#endregion

		public RouteList ()
		{
			Date = DateTime.Now;
		}
	}

	public enum RouteListStatus
	{
		[ItemTitleAttribute ("Новый")] New,
		[ItemTitleAttribute ("Готов к отгрузке")] Ready,
		[ItemTitleAttribute ("На погрузке")]InLoading,
		[ItemTitleAttribute ("В пути")]EnRoute,
		[ItemTitleAttribute ("Не сдан")]NotDelivered,
		[ItemTitleAttribute ("Закрыт")]Closed
	}

	public class RouteListStatusStringType : NHibernate.Type.EnumStringType
	{
		public RouteListStatusStringType () : base (typeof(RouteListStatus))
		{
		}
	}
}

