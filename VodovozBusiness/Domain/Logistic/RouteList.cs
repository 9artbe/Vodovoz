﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using QSOrmProject;
using QSValidation;
using Vodovoz.Domain.Employees;
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
			set { if(NHibernate.NHibernateUtil.IsInitialized(Addresses) && (forwarder == null || value == null))
				{
					foreach (var address in Addresses)
						address.WithForwarder = value != null;
				}
				SetField (ref forwarder, value, () => Forwarder); }
		}

		Employee logistican;

		[Display (Name = "Логист")]
		public virtual Employee Logistican {
			get { return logistican; }
			set { SetField (ref logistican, value, () => Logistican); }
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

		DeliveryShift shift;

		[Display (Name = "Смена доставки")]
		public virtual DeliveryShift Shift {
			get { return shift; }
			set { 
				SetField (ref shift, value, () => Shift); 
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

		Decimal confirmedDistance;

		public virtual Decimal ConfirmedDistance {
			get{ return confirmedDistance; }
			set
			{
				SetField(ref confirmedDistance, value, () => ConfirmedDistance);
			}
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
				SetNullToObservableAddresses ();
			}
		}

		GenericObservableList<RouteListItem> observableAddresses;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<RouteListItem> ObservableAddresses {
			get {
				if (observableAddresses == null) {
					observableAddresses = new GenericObservableList<RouteListItem> (addresses);
					observableAddresses.ElementAdded += ObservableAddresses_ElementAdded;
					observableAddresses.ElementRemoved += ObservableAddresses_ElementRemoved;
				}
				return observableAddresses;
			}
		}

		void ObservableAddresses_ElementRemoved (object aList, int[] aIdx, object aObject)
		{
			CheckAddressOrder ();
		}

		void ObservableAddresses_ElementAdded (object aList, int[] aIdx)
		{
			CheckAddressOrder ();
		}

		public virtual string Title { get { return String.Format ("Маршрутный лист №{0}", Id); } }

		#region Функции

		public virtual RouteListItem AddAddressFromOrder (Order order)
		{
			if (order.DeliveryPoint == null)
				throw new NullReferenceException ("В маршрутный нельзя добавить заказ без точки доставки.");
			var item = new RouteListItem (this, order);
			item.WithForwarder = Forwarder != null;
			ObservableAddresses.Add (item);
			return item;
		}

		public virtual void RemoveAddress (RouteListItem address)
		{
			address.RemovedFromRoute ();
			UoW.Delete (address);
			ObservableAddresses.Remove (address);
		}

		public virtual void ReorderAddressesByTime()
		{
			var orderedList = Addresses
				.OrderBy(x => x.Order.DeliverySchedule.From)
				.ThenBy(x => x.Order.DeliverySchedule.To)
				.ToList();
			for(int i = 0; i < ObservableAddresses.Count; i++)
			{
				if (orderedList[i] == ObservableAddresses[i])
					continue;

				ObservableAddresses.Remove(orderedList[i]);
				ObservableAddresses.Insert(i, orderedList[i]);
			}
		}

		private void CheckAddressOrder ()
		{
			int i = 0;
			foreach (var address in Addresses) {
				if (address.IndexInRoute != i)
					address.IndexInRoute = i;
				i++;
			}
		}

		private void SetNullToObservableAddresses ()
		{
			if (observableAddresses == null)
				return;
			observableAddresses.ElementAdded -= ObservableAddresses_ElementAdded;
			observableAddresses.ElementRemoved -= ObservableAddresses_ElementRemoved;
			observableAddresses = null;
		}

		public virtual void CompleteRoute(){
			Status = RouteListStatus.OnClosing;
			foreach (var item in Addresses) {
				item.Order.OrderStatus = OrderStatus.UnloadingOnStock;
			}
			var track = Repository.Logistics.TrackRepository.GetTrackForRouteList(UoW, Id);
			if (track != null)
			{
				track.CalculateDistance();
				UoW.Save(track);
			}
		}

		public virtual bool ShipIfCan(IUnitOfWork uow)
		{
			var inLoaded = Repository.Logistics.RouteListRepository.AllGoodsLoaded(uow, this);
			var goods = Repository.Logistics.RouteListRepository.GetGoodsInRLWithoutEquipments(uow, this);

			bool closed = true;
			foreach(var good in goods)
			{
				var loaded = inLoaded.FirstOrDefault(x => x.NomenclatureId == good.NomenclatureId);
				if(loaded == null || loaded.Amount < good.Amount)
				{
					closed = false;
					break;
				}
			}
			#if !SHORT
			if(closed == true)
			{
				var equipmentsInRoute = Repository.Logistics.RouteListRepository.GetEquipmentsInRL(uow, this);
				foreach(var equipment in equipmentsInRoute)
				{
					var loaded = inLoaded.FirstOrDefault(x => x.EquipmentId == equipment.EquipmentId);
					if(loaded == null || loaded.Amount < equipment.Amount)
					{
						closed = false;
						break;
					}
				}
			}
			#endif

			if (closed)
				ChangeStatus(RouteListStatus.EnRoute);
			else
				ChangeStatus(RouteListStatus.InLoading);
			return closed;
		}

		public virtual void ConfirmMileage()
		{
			Status = RouteListStatus.Closed;
		}

		public virtual void ChangeStatus(RouteListStatus newStatus)
		{
			if (newStatus == Status)
				return;

			if(newStatus == RouteListStatus.EnRoute)
			{
				if (Status == RouteListStatus.InLoading)
				{
					Status = RouteListStatus.EnRoute;
					foreach (var item in Addresses) {
						item.Order.OrderStatus = OrderStatus.OnTheWay;
					}
				}
				else
					throw new NotImplementedException();
			}
			else if(newStatus == RouteListStatus.InLoading)
			{
				if (Status == RouteListStatus.EnRoute)
				{
					Status = RouteListStatus.InLoading;
					foreach (var item in Addresses)
					{
						item.Order.ChangeStatus(OrderStatus.OnLoading);
					}
				}
				else if (Status == RouteListStatus.New)
					Status = RouteListStatus.InLoading;
				else
					throw new NotImplementedException();
			}
			else if(newStatus == RouteListStatus.New)
			{
				if (Status == RouteListStatus.InLoading)
					Status = RouteListStatus.New;
				else
					throw new NotImplementedException();
			}
		}

		#endregion

		#region IValidatableObject implementation

		public virtual IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			if (validationContext.Items.ContainsKey ("NewStatus")) {
				RouteListStatus newStatus = (RouteListStatus)validationContext.Items ["NewStatus"];
				if (newStatus == RouteListStatus.InLoading) {
				}
				if (newStatus == RouteListStatus.Closed)
				{
					if (ConfirmedDistance <= 0)
						yield return new ValidationResult("Подтвержденное расстояние не может быть меньше 0",
							new[] {Gamma.Utilities.PropertyUtil.GetPropertyName(this, o=>o.ConfirmedDistance)});
				}
				if(newStatus == RouteListStatus.MileageCheck)
				{
					foreach(var address in Addresses)
					{
						var valid = new QSValidator<Order> (address.Order, 
							new Dictionary<object, object> {
							{ "NewStatus", OrderStatus.Closed }
						});

						foreach(var result in valid.Results){
							yield return result;
						}
					}
				}
			}

			if (Shift == null)
				yield return new ValidationResult ("Смена маршрутного листа должна быть заполнена.",
					new[] { Gamma.Utilities.PropertyUtil.GetPropertyName (this, o => o.Shift) });

			if (Driver == null)
				yield return new ValidationResult ("Не заполнен водитель.",
					new[] { Gamma.Utilities.PropertyUtil.GetPropertyName (this, o => o.Driver) });
			if (Car == null)
				yield return new ValidationResult ("На заполнен автомобиль.",
					new[] { Gamma.Utilities.PropertyUtil.GetPropertyName (this, o => o.Car) });
		}

		#endregion

		public RouteList ()
		{
			Date = DateTime.Today;
		}
	}

	public enum RouteListStatus
	{
		[Display (Name = "Новый")] New,
		[Display (Name = "На погрузке")] InLoading,
		[Display (Name = "В пути")] EnRoute,
		[Display (Name = "Сдаётся")] OnClosing,
		[Display (Name = "Не сдан")] NotDelivered,
		[Display (Name = "Проверка километража")] MileageCheck,
		[Display (Name = "Закрыт")] Closed
	}

	public class RouteListStatusStringType : NHibernate.Type.EnumStringType
	{
		public RouteListStatusStringType () : base (typeof(RouteListStatus))
		{
		}
	}
}

