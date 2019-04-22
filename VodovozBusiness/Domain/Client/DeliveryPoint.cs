﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.Utilities;
using NetTopologySuite.Geometries;
using QS.DomainModel.Entity;
using QS.DomainModel.Entity.EntityPermissions;
using QS.DomainModel.UoW;
using QS.HistoryLog;
using QSContacts;
using QSOsm;
using QSOsm.DTO;
using QSOsm.Osrm;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Sale;
using Vodovoz.Repositories.Sale;

namespace Vodovoz.Domain.Client
{
	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "точки доставки",
		Nominative = "точка доставки",
		Accusative = "точки доставки"
	)]
	[HistoryTrace]
	[EntityPermission]
	public class DeliveryPoint : PropertyChangedBase, IDomainObject
	{
		static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		#region Свойства

		public virtual int Id { get; set; }

		int minutesToUnload;

		[Display(Name = "Время разгрузки")]
		public virtual int MinutesToUnload {
			get { return minutesToUnload; }
			set { SetField(ref minutesToUnload, value, () => MinutesToUnload); }
		}

		string letter;

		[Display(Name = "Литера")]
		public virtual string Letter {
			get { return letter; }
			set { SetField(ref letter, value, () => Letter); }
		}

		string addressAddition;

		[Display(Name = "Дополнение к адресу")]
		public virtual string АddressAddition {
			get { return addressAddition; }
			set { SetField(ref addressAddition, value, () => АddressAddition); }
		}

		string placement;

		[Display(Name = "Помещение")]
		public virtual string Placement {
			get { return placement; }
			set { SetField(ref placement, value, () => Placement); }
		}

		string floor;

		[Display(Name = "Этаж")]
		public virtual string Floor {
			get { return floor; }
			set { SetField(ref floor, value, () => Floor); }
		}

		EntranceType entranceType;

		[Display(Name = "Тип входа")]
		public virtual EntranceType EntranceType {
			get { return entranceType; }
			set { SetField(ref entranceType, value, () => EntranceType); }
		}

		string entrance;

		[Display(Name = "Парадная")]
		public virtual string Entrance {
			get { return entrance; }
			set { SetField(ref entrance, value, () => Entrance); }
		}

		public virtual string Title {
			get { return String.IsNullOrWhiteSpace(CompiledAddress) ? "АДРЕС ПУСТОЙ" : CompiledAddress; }
		}

		[Display(Name = "Полный адрес")]
		public virtual string CompiledAddress {
			get {
				string address = String.Empty;
				if(!String.IsNullOrWhiteSpace(City))
					address += String.Format("{0} {1}, ", LocalityType.GetEnumShortTitle(), City);
				if(!String.IsNullOrWhiteSpace(Street))
					address += String.Format("{0}, ", Street);
				if(!String.IsNullOrWhiteSpace(Building))
					address += String.Format("д.{0}, ", Building);
				if(!String.IsNullOrWhiteSpace(Letter))
					address += String.Format("лит.{0}, ", Letter);
				if(!string.IsNullOrWhiteSpace(Entrance))
					address += String.Format("{0} {1}, ", entranceType.GetEnumShortTitle(), Entrance);
				if(!string.IsNullOrWhiteSpace(Floor))
					address += String.Format("эт.{0}, ", Floor);
				if(!String.IsNullOrWhiteSpace(Room))
					address += String.Format("{0} {1}, ", RoomType.GetEnumShortTitle(), Room);
				if(!String.IsNullOrWhiteSpace(АddressAddition))
					address += String.Format("{0}, ", АddressAddition);

				return address.TrimEnd(',', ' ');
			}
			set { }
		}

		[Display(Name = "Адрес без дополнения")]
		public virtual string CompiledAddressWOAddition {
			get {
				string address = String.Empty;
				if(!String.IsNullOrWhiteSpace(City))
					address += String.Format("{0} {1}, ", LocalityType.GetEnumShortTitle(), City);
				if(!String.IsNullOrWhiteSpace(Street))
					address += String.Format("{0}, ", Street);
				if(!String.IsNullOrWhiteSpace(Building))
					address += String.Format("д.{0}, ", Building);
				if(!String.IsNullOrWhiteSpace(Letter))
					address += String.Format("лит.{0}, ", Letter);
				if(!string.IsNullOrWhiteSpace(Entrance))
					address += String.Format("{0} {1}, ", entranceType.GetEnumShortTitle(), Entrance);
				if(!string.IsNullOrWhiteSpace(Floor))
					address += String.Format("эт.{0}, ", Floor);
				if(!String.IsNullOrWhiteSpace(Room))
					address += String.Format("{0} {1}, ", RoomType.GetEnumShortTitle(), Room);

				return address.TrimEnd(',', ' ');
			}
		}

		string shortAddress;

		[Display(Name = "Сокращенный адрес")]
		public virtual string ShortAddress {
			get {
				string address = String.Empty;
				if(!String.IsNullOrWhiteSpace(City) && City != "Санкт-Петербург")
					address += String.Format("{0} {1}, ", LocalityType.GetEnumShortTitle(), AddressHelper.ShortenCity(City));
				if(!String.IsNullOrWhiteSpace(Street))
					address += String.Format("{0}, ", AddressHelper.ShortenStreet(Street));
				if(!String.IsNullOrWhiteSpace(Building))
					address += String.Format("д.{0}, ", Building);
				if(!String.IsNullOrWhiteSpace(Letter))
					address += String.Format("лит.{0}, ", Letter);
				if(!string.IsNullOrWhiteSpace(Entrance))
					address += String.Format("{0} {1}, ", entranceType.GetEnumShortTitle(), Entrance);
				if(!string.IsNullOrWhiteSpace(Floor))
					address += String.Format("эт.{0}, ", Floor);
				if(!String.IsNullOrWhiteSpace(Room))
					address += String.Format("{0} {1}, ", RoomType.GetEnumShortTitle(), Room);

				return address.TrimEnd(',', ' ');
			}
			set { }
		}

		string city;

		[Display(Name = "Город")]
		[Required(ErrorMessage = "Город должен быть заполнен.")]
		[StringLength(45)]
		public virtual string City {
			get { return city; }
			set { SetField(ref city, value, () => City); }
		}

		LocalityType localityType;

		[Display(Name = "Тип населенного пункта")]
		public virtual LocalityType LocalityType {
			get { return localityType; }
			set { SetField(ref localityType, value, () => LocalityType); }
		}

		string cityDistrict;

		[Display(Name = "Район области")]
		public virtual string CityDistrict {
			get { return cityDistrict; }
			set { SetField(ref cityDistrict, value, () => CityDistrict); }
		}

		string street;

		[Display(Name = "Улица")]
		[Required(ErrorMessage = "Улица должна быть заполнена.")]
		[StringLength(50)]
		public virtual string Street {
			get { return street; }
			set { SetField(ref street, value, () => Street); }
		}

		string streetDistrict;

		[Display(Name = "Район города")]
		public virtual string StreetDistrict {
			get { return streetDistrict; }
			set { SetField(ref streetDistrict, value, () => StreetDistrict); }
		}


		string building;

		[Display(Name = "Номер дома")]
		[Required(ErrorMessage = "Номер дома должен быть заполнен.")]
		public virtual string Building {
			get { return building; }
			set { SetField(ref building, value, () => Building); }
		}

		RoomType roomType;

		[Display(Name = "Тип помещения")]
		public virtual RoomType RoomType {
			get { return roomType; }
			set { SetField(ref roomType, value, () => RoomType); }
		}

		string room;

		[Display(Name = "Офис/Квартира")]
		public virtual string Room {
			get { return room; }
			set { SetField(ref room, value, () => Room); }
		}

		string comment;

		[Display(Name = "Комментарий")]
		public virtual string Comment {
			get { return comment; }
			set { SetField(ref comment, value, () => Comment); }
		}

		decimal? latitude;

		/// <summary>
		/// Широта. Для установки координат используйте метод SetСoordinates
		/// </summary>
		[Display(Name = "Широта")]
		[PropertyChangedAlso("СoordinatesText")]
		public virtual decimal? Latitude {
			get { return latitude; }
			protected set { SetField(ref latitude, value, () => Latitude); }
		}

		decimal? longitude;

		/// <summary>
		/// Долгота. Для установки координат используйте метод SetСoordinates
		/// </summary>
		[Display(Name = "Долгота")]
		[PropertyChangedAlso("СoordinatesText")]
		public virtual decimal? Longitude {
			get { return longitude; }
			protected set { SetField(ref longitude, value, () => Longitude); }
		}

		bool isActive = true;

		[Display(Name = "Активный")]
		public virtual bool IsActive {
			get { return isActive; }
			set { SetField(ref isActive, value, () => IsActive); }
		}

		private IList<Contact> contacts = new List<Contact>();

		[Display(Name = "Ответственные лица")]
		public virtual IList<Contact> Contacts {
			get { return contacts; }
			set { SetField(ref contacts, value, () => Contacts); }
		}

		GenericObservableList<Contact> observableContacts;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<Contact> ObservableContacts {
			get {
				if(observableContacts == null)
					observableContacts = new GenericObservableList<Contact>(Contacts);
				return observableContacts;
			}
		}

		LogisticsArea logisticsArea;

		[Display(Name = "Логистический район")]
		public virtual LogisticsArea LogisticsArea {
			get { return logisticsArea; }
			set { SetField(ref logisticsArea, value, () => LogisticsArea); }
		}

		DeliverySchedule deliverySchedule;

		[Display(Name = "График доставки")]
		public virtual DeliverySchedule DeliverySchedule {
			get { return deliverySchedule; }
			set { SetField(ref deliverySchedule, value, () => DeliverySchedule); }
		}

		bool foundOnOsm;

		[Display(Name = "Адрес найден на карте OSM")]
		public virtual bool FoundOnOsm {
			get { return foundOnOsm; }
			set { SetField(ref foundOnOsm, value, () => FoundOnOsm); }
		}

		bool manualCoordinates;

		[Display(Name = "Ручные координаты")]
		public virtual bool ManualCoordinates {
			get { return manualCoordinates; }
			set { SetField(ref manualCoordinates, value, () => ManualCoordinates); }
		}

		bool isFixedInOsm;

		[Display(Name = "Исправлен в OSM")]
		public virtual bool IsFixedInOsm {
			get { return isFixedInOsm; }
			set { SetField(ref isFixedInOsm, value, () => IsFixedInOsm); }
		}

		Counterparty counterparty;

		[Required]
		[Display(Name = "Контрагент")]
		public virtual Counterparty Counterparty {
			get { return counterparty; }
			protected set { SetField(ref counterparty, value, () => Counterparty); }
		}

		private string address1c;

		[Display(Name = "Адрес 1С")]
		public virtual string Address1c {
			get { return address1c; }
			set { SetField(ref address1c, value, () => Address1c); }
		}

		string code1c;

		[Display(Name = "Код в 1С")]
		/// Код уникален только внутри контрагента
		public virtual string Code1c {
			get { return code1c; }
			set { SetField(ref code1c, value, () => Code1c); }
		}

		int bottleReserv;

		[Display(Name = "Резерв бутылей")]
		public virtual int BottleReserv {
			get { return bottleReserv; }
			set { SetField(ref bottleReserv, value, () => BottleReserv); }
		}

		Nomenclature defaultWaterNomenclature;

		[Display(Name = "Вода по умолчанию")]
		public virtual Nomenclature DefaultWaterNomenclature {
			get { return defaultWaterNomenclature; }
			set { SetField(ref defaultWaterNomenclature, value, () => DefaultWaterNomenclature); }
		}

		bool alwaysFreeDelivery;

		[Display(Name = "Всегда бесплатная доставка")]
		public virtual bool AlwaysFreeDelivery {
			get { return alwaysFreeDelivery; }
			set { SetField(ref alwaysFreeDelivery, value, () => AlwaysFreeDelivery); }
		}

		User coordsLastChangeUser;

		[Display(Name = "Последнее изменение пользователем")]
		public virtual User СoordsLastChangeUser {
			get { return coordsLastChangeUser; }
			set { SetField(ref coordsLastChangeUser, value, () => СoordsLastChangeUser); }
		}

		private int? distanceFromBaseMeters;

		[Display(Name = "Расстояние от базы в метрах")]
		public virtual int? DistanceFromBaseMeters {
			get { return distanceFromBaseMeters; }
			set { SetField(ref distanceFromBaseMeters, value, () => DistanceFromBaseMeters); }
		}

		IList<Phone> phones = new List<Phone>();

		[Display(Name = "Телефоны")]
		public virtual IList<Phone> Phones {
			get { return phones; }
			set { SetField(ref phones, value, () => Phones); }
		}

		GenericObservableList<Phone> observablePhones;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<Phone> ObservablePhones {
			get {
				if(observablePhones == null)
					observablePhones = new GenericObservableList<Phone>(Phones);
				return observablePhones;
			}
		}

		private bool? haveResidue;

		[Display(Name = "Посчитан ввод остатков")]
		public virtual bool? HaveResidue {
			get { return haveResidue; }
			set { SetField(ref haveResidue, value, () => HaveResidue); }
		}

		#region Временные поля для хранения фиксированных цен из 1с

		private decimal fixPrice1;
		/// <summary>
		/// Фикса Семиозерье из 1с
		/// </summary>
		[Display(Name = "Фикса Семиозерье из 1с")]
		public virtual decimal FixPrice1 {
			get { return fixPrice1; }
			set { SetField(ref fixPrice1, value, () => FixPrice1); }
		}

		private decimal fixPrice2;
		/// <summary>
		/// Фикса Кислородная из 1с
		/// </summary>
		[Display(Name = "Фикса Кислородная из 1с")]
		public virtual decimal FixPrice2 {
			get { return fixPrice2; }
			set { SetField(ref fixPrice2, value, () => FixPrice2); }
		}

		private decimal fixPrice3;
		/// <summary>
		/// Фикса Снятогорская из 1с
		/// </summary>
		[Display(Name = "Фикса Снятогорская из 1с")]
		public virtual decimal FixPrice3 {
			get { return fixPrice3; }
			set { SetField(ref fixPrice3, value, () => FixPrice3); }
		}

		private decimal fixPrice4;
		/// <summary>
		/// Фикса Стройка из 1с
		/// </summary>
		[Display(Name = "Фикса Стройка из 1с")]
		public virtual decimal FixPrice4 {
			get { return fixPrice4; }
			set { SetField(ref fixPrice4, value, () => FixPrice4); }
		}

		private decimal fixPrice5;
		/// <summary>
		/// Фикса С Ручками из 1с
		/// </summary>
		[Display(Name = "Фикса С Ручками из 1с")]
		public virtual decimal FixPrice5 {
			get { return fixPrice5; }
			set { SetField(ref fixPrice5, value, () => FixPrice5); }
		}

		ScheduleRestrictedDistrict district;

		[Display(Name = "Район доставки")]
		public virtual ScheduleRestrictedDistrict District {
			get => district;
			set => SetField(ref district, value, () => District);
		}

		bool addCertificatesAlways;
		[Display(Name = "Всегда добавлять сертификаты")]
		public virtual bool AddCertificatesAlways {
			get => addCertificatesAlways;
			set => SetField(ref addCertificatesAlways, value, () => AddCertificatesAlways);
		}

		#endregion


		#endregion

		#region Расчетные

		public virtual string CoordinatesText {
			get {
				if(Latitude == null || Longitude == null)
					return String.Empty;
				return String.Format("(ш. {0:F5}, д. {1:F5})", Latitude, Longitude);
			}
		}

		public virtual bool CoordinatesExist => Latitude.HasValue && Longitude.HasValue;

		public virtual Point NetTopologyPoint => CoordinatesExist ? new Point((double)Latitude, (double)Longitude) : null;

		public virtual PointOnEarth PointOnEarth => new PointOnEarth(Latitude.Value, Longitude.Value);

		public virtual GMap.NET.PointLatLng GmapPoint => new GMap.NET.PointLatLng((double)Latitude, (double)Longitude);

		public virtual long СoordinatesHash => CachedDistance.GetHash(this);

		#endregion

		/// <summary>
		/// Возврат районов доставки, в которые попадает точка доставки
		/// </summary>
		/// <param name="uow">UnitOfWork через который будет получены все районы доставки,
		/// среди которых будет производится поиск подходящего района</param>
		public virtual IList<ScheduleRestrictedDistrict> CalculateDistricts(IUnitOfWork uow)
		{
			return CalculateDistricts(ScheduleRestrictionRepository.AreasWithGeometry(uow));
		}

		/// <summary>
		/// Возврат районов доставки, в которые попадает точка доставки
		/// </summary>
		/// <returns>Районы доставки</returns>
		/// <param name="districtsSource">Районы доставки, среди которых будет производится поиск подходящего района</param>
		public virtual IList<ScheduleRestrictedDistrict> CalculateDistricts(IList<ScheduleRestrictedDistrict> districtsSource)
		{
			List<ScheduleRestrictedDistrict> districts = new List<ScheduleRestrictedDistrict>();
			if(CoordinatesExist) {
				districts = districtsSource.Where(x => x.DistrictBorder.Contains(NetTopologyPoint))
										   .ToList();

				if(districts.Any())
					return districts;

				foreach(var point in Get4PointsInRadiusOfXMetersFromBasePoint(NetTopologyPoint)) {
					districts = districtsSource.Where(x => x.DistrictBorder.Contains(point))
											   .ToList();
					if(districts.Any())
						return districts;
				}
			}
			return districts;
		}

		/// <summary>
		/// Поиск района города, в котором находится текущая точка доставки
		/// </summary>
		/// <returns><c>true</c>, если район города найден</returns>
		/// <param name="uow">UnitOfWork через который будет производится поиск подходящего района города</param>
		public bool FindAndAssociateDistrict(IUnitOfWork uow)
		{
			District = CalculateDistricts(uow).FirstOrDefault();
			return District != null;
		}

		public bool FindAndAssociateDistrict(IList<ScheduleRestrictedDistrict> districtsSource)
		{
			District = CalculateDistricts(districtsSource).FirstOrDefault();
			return District != null;
		}

		/// <summary>
		/// Получение 4 точек, отстоящих от базовой точки на <paramref name="distanceInMeters"/> вправо, влево, вверх и вниз.
		/// </summary>
		/// <param name="basePoint">Базовая точка</param>
		/// <param name="distanceInMeters">Дистанция отступа от базовой точки <paramref name="basePoint"/></param>
		Point[] Get4PointsInRadiusOfXMetersFromBasePoint(Point basePoint, double distanceInMeters = 100)
		{
			Point[] array = new Point[4];
			array[0] = new Point(GetNewLatitude(basePoint.X, distanceInMeters), basePoint.Y);
			array[1] = new Point(basePoint.X, GetNewLongitude(basePoint.Y, distanceInMeters));
			array[2] = new Point(GetNewLatitude(basePoint.X, -distanceInMeters), basePoint.Y);
			array[3] = new Point(basePoint.X, GetNewLongitude(basePoint.Y, -distanceInMeters));
			return array;
		}

		double GetNewLatitude(double lat, double metersToAdd)
		{
			double earth = 6378.137; //radius of the earth in kilometer
			double pi = Math.PI;
			double m = 1 / (2 * pi / 360 * earth) / 1000;  //1 meter in degree

			double newLatitude = lat + (metersToAdd * m);

			return newLatitude;
		}

		double GetNewLongitude(double lon, double metersToAdd)
		{
			double earth = 6378.137;  //radius of the earth in kilometer
			double pi = Math.PI;
			double m = 1 / (2 * pi / 360 * earth) / 1000;  //1 meter in degree

			double newLongitude = lon + metersToAdd * m / Math.Cos(lon * (pi / 180));
			return newLongitude;
		}


		public DeliveryPoint()
		{
			CompiledAddress = String.Empty;
			City = "Санкт-Петербург";
			LocalityType = LocalityType.city;
			Street = String.Empty;
			Building = String.Empty;
			Room = String.Empty;
			Comment = String.Empty;
		}

		public virtual void AddContact(Contact contact)
		{
			if(Contacts.Any(x => x.Id == contact.Id))
				return;
			ObservableContacts.Add(contact);
		}

		/// <summary>
		/// Устанавливает правильно координты точки.
		/// </summary>
		/// <returns><c>true</c>, если координаты установлены</returns>
		/// <param name="latitude">Широта</param>
		/// <param name="longitude">Долгота</param>
		/// <param name="uow">UnitOfWork через который будет производится поиск подходящего района города
		/// для определения расстояния до базы</param>
		public virtual bool SetСoordinates(decimal? latitude, decimal? longitude, IUnitOfWork uow = null)
		{
			Latitude = latitude;
			Longitude = longitude;

			if(Longitude == null || Latitude == null || !FindAndAssociateDistrict(uow))
				return true;
			var gg = District.GeographicGroups.FirstOrDefault();
			var route = new List<PointOnEarth>(2) {
				new PointOnEarth(gg.BaseLatitude.Value, gg.BaseLongitude.Value),
				new PointOnEarth(Latitude.Value, Longitude.Value)
			};

			var result = OsrmMain.GetRoute(route, false, GeometryOverview.False);
			if(result == null) {
				logger.Error("Сервер расчета расстояний не вернул ответа.");
				return false;
			}
			if(result.Code != "Ok") {
				logger.Error("Сервер расчета расстояний вернул следующее сообщение:\n" + result.StatusMessageRus);
				return false;
			}
			DistanceFromBaseMeters = result.Routes[0].TotalDistance;
			return true;
		}

		public static IUnitOfWorkGeneric<DeliveryPoint> CreateUowForNew(Counterparty counterparty)
		{
			var uow = UnitOfWorkFactory.CreateWithNewRoot<DeliveryPoint>();
			uow.Root.Counterparty = counterparty;
			return uow;
		}

		public static DeliveryPoint Create(Counterparty counterparty)
		{
			var point = new DeliveryPoint {
				Counterparty = counterparty
			};
			counterparty.DeliveryPoints.Add(point);
			return point;
		}
	}

	public enum EntranceType
	{
		[Display(Name = "Парадная", ShortName = "пар.")]
		Entrance,
		[Display(Name = "Торговый центр", ShortName = "ТЦ")]
		TradeCenter,
		[Display(Name = "Торговый комплекс", ShortName = "ТК")]
		TradeComplex,
		[Display(Name = "Бизнесс центр", ShortName = "БЦ")]
		BusinessCenter,
		[Display(Name = "Школа", ShortName = "шк.")]
		School,
		[Display(Name = "Общежитие", ShortName = "общ.")]
		Hostel
	}

	public class EntranceTypeStringType : NHibernate.Type.EnumStringType
	{
		public EntranceTypeStringType() : base(typeof(EntranceType)) { }
	}
}

