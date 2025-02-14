﻿using System;
using System.Collections.Generic;
using System.Linq;
using QS.Commands;
using QS.Dialog;
using QS.DomainModel.UoW;
using QS.Osm.Loaders;
using QS.Project.Domain;
using QS.Project.Journal.EntitySelector;
using QS.Services;
using QS.Tdi;
using QS.ViewModels;
using Vodovoz.Domain;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Employees;
using Vodovoz.EntityRepositories;
using Vodovoz.EntityRepositories.Counterparties;
using Vodovoz.Models;
using Vodovoz.Services;
using Vodovoz.SidePanel;
using Vodovoz.SidePanel.InfoProviders;
using Vodovoz.TempAdapters;
using Vodovoz.ViewModels.Infrastructure.InfoProviders;
using Vodovoz.ViewModels.TempAdapters;
using Vodovoz.ViewModels.ViewModels.Contacts;
using Vodovoz.ViewModels.ViewModels.Goods;

namespace Vodovoz.ViewModels.ViewModels.Counterparty
{
	public class DeliveryPointViewModel : EntityTabViewModelBase<DeliveryPoint>, IDeliveryPointInfoProvider, ITDICloseControlTab
	{
		private int _currentPage = 0;
		private User _currentUser;
		private bool _isNotSaving = true;
		private FixedPricesViewModel _fixedPricesViewModel;
		private List<DeliveryPointResponsiblePerson> _responsiblePersons;
		private readonly IGtkTabsOpener _gtkTabsOpener;
		private readonly IUserRepository _userRepository;
		private readonly IFixedPricesModel _fixedPricesModel;

		#region Свойства

		//permissions
		public readonly bool CanArchiveDeliveryPoint;
		public readonly bool CanSetFreeDelivery;
		public readonly bool CanEditOrderLimits;

		//widget binds
		public int CurrentPage
		{
			get => _currentPage;
			private set => SetField(ref _currentPage, value);
		}
		public bool IsNotSaving
		{
			get => _isNotSaving;
			private set => SetField(ref _isNotSaving, value);
		}
		public bool CurrentUserIsAdmin => CurrentUser.IsAdmin;
		public bool CoordsWasChanged => Entity.СoordsLastChangeUser != null;
		public string CoordsLastChangeUserName => Entity.СoordsLastChangeUser.Name;

		//widget init
		public FixedPricesViewModel FixedPricesViewModel =>
			_fixedPricesViewModel ??
			(_fixedPricesViewModel = new FixedPricesViewModel(UoW, _fixedPricesModel, NomenclatureSelectorFactory, this));

		public List<DeliveryPointResponsiblePerson> ResponsiblePersons =>
			_responsiblePersons ?? (_responsiblePersons = new List<DeliveryPointResponsiblePerson>());

		public PhonesViewModel PhonesViewModel { get; }
		public ICitiesDataLoader CitiesDataLoader { get; }
		public IStreetsDataLoader StreetsDataLoader { get; }
		public IHousesDataLoader HousesDataLoader { get; }
		public IOrderedEnumerable<DeliveryPointCategory> DeliveryPointCategories { get; }
		public INomenclatureSelectorFactory NomenclatureSelectorFactory { get; }
		public IEntityAutocompleteSelectorFactory DeliveryScheduleSelectorFactory { get; }

		#endregion

		public override bool HasChanges
		{
			get
			{
				PhonesViewModel.RemoveEmpty();
				return base.HasChanges;
			}
			set => base.HasChanges = value;
		}

		#region IDeliveryPointInfoProvider

		public DeliveryPoint DeliveryPoint => Entity;
		public PanelViewType[] InfoWidgets => new[] {PanelViewType.DeliveryPricePanelView};
		public event EventHandler<CurrentObjectChangedArgs> CurrentObjectChanged;

		#endregion

		public DeliveryPointViewModel(
			IUserRepository userRepository,
			IGtkTabsOpener gtkTabsOpener,
			IPhoneRepository phoneRepository,
			IContactsParameters contactsParameters,
			ICitiesDataLoader citiesDataLoader,
			IStreetsDataLoader streetsDataLoader,
			IHousesDataLoader housesDataLoader,
			INomenclatureSelectorFactory nomenclatureSelectorFactory,
			NomenclatureFixedPriceController nomenclatureFixedPriceController,
			IDeliveryPointRepository deliveryPointRepository,
			IDeliveryScheduleSelectorFactory deliveryScheduleSelectorFactory,
			IEntityUoWBuilder uowBuilder, IUnitOfWorkFactory unitOfWorkFactory, ICommonServices commonServices,
			Domain.Client.Counterparty client = null)
			: base(uowBuilder, unitOfWorkFactory, commonServices)
		{
			if(client != null && uowBuilder.IsNewEntity)
			{
				Entity.Counterparty = client;
			}
			else if(client == null && uowBuilder.IsNewEntity)
			{
				throw new ArgumentNullException(nameof(client), "Нельзя создать точку доставки без указания клиента");
			}

			if(phoneRepository == null)
			{
				throw new ArgumentNullException(nameof(phoneRepository));
			}

			if(contactsParameters == null)
			{
				throw new ArgumentNullException(nameof(contactsParameters));
			}

			if(nomenclatureFixedPriceController == null)
			{
				throw new ArgumentNullException(nameof(nomenclatureFixedPriceController));
			}

			_gtkTabsOpener = gtkTabsOpener ?? throw new ArgumentNullException(nameof(gtkTabsOpener));
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

			NomenclatureSelectorFactory =
				nomenclatureSelectorFactory ?? throw new ArgumentNullException(nameof(nomenclatureSelectorFactory));

			_fixedPricesModel = new DeliveryPointFixedPricesModel(UoW, Entity, nomenclatureFixedPriceController);
			PhonesViewModel = new PhonesViewModel(phoneRepository, UoW, contactsParameters) {PhonesList = Entity.ObservablePhones};

			CitiesDataLoader = citiesDataLoader ?? throw new ArgumentNullException(nameof(citiesDataLoader));
			StreetsDataLoader = streetsDataLoader ?? throw new ArgumentNullException(nameof(streetsDataLoader));
			HousesDataLoader = housesDataLoader ?? throw new ArgumentNullException(nameof(housesDataLoader));

			CanArchiveDeliveryPoint =
				commonServices.CurrentPermissionService.ValidatePresetPermission("can_arc_counterparty_and_deliverypoint");
			CanSetFreeDelivery = commonServices.CurrentPermissionService.ValidatePresetPermission("can_set_free_delivery");
			CanEditOrderLimits = commonServices.CurrentPermissionService.ValidatePresetPermission("user_can_edit_orders_limits");

			DeliveryPointCategories =
				deliveryPointRepository?.GetActiveDeliveryPointCategories(UoW)
				?? throw new ArgumentNullException(nameof(deliveryPointRepository));
			DeliveryScheduleSelectorFactory =
				deliveryScheduleSelectorFactory?.CreateDeliveryScheduleAutocompleteSelectorFactory()
				?? throw new ArgumentNullException(nameof(deliveryScheduleSelectorFactory));

			Entity.PropertyChanged += (sender, e) =>
			{
				switch (e.PropertyName)
				{ // от этого события зависит панель цен доставки, которые в свою очередь зависят от района и, возможно, фиксов
					case nameof(Entity.District):
						CurrentObjectChanged?.Invoke(this, new CurrentObjectChangedArgs(Entity));
						break;
				}
			};
		}

		public void OpenFixedPrices()
		{
			CurrentPage = 1;
		}

		public override bool Save(bool close)
		{
			try
			{
				IsNotSaving = false;
				if(!HasChanges)
				{
					return base.Save(close);
				}

				if(!Entity.CoordinatesExist &&
				   !CommonServices.InteractiveService.Question(
					   "Адрес точки доставки не найден на карте, вы точно хотите сохранить точку доставки?"))
				{
					return false;
				}

				if(Entity.District == null && !CommonServices.InteractiveService.Question(
					"Район доставки не найден. Это приведёт к невозможности отображения заказа на " +
					"эту точку доставки у логистов при составлении маршрутного листа. Укажите правильные координаты.\n" +
					"Продолжить сохранение точки доставки?",
					"Проверьте координаты!"))
				{
					return false;
				}

				return base.Save(close);
			}
			finally
			{
				IsNotSaving = true;
			}
		}

		public void ApplyOrderSumLimitsToAllDeliveryPointsOfClient()
		{
			foreach(var deliveryPoint in Entity.Counterparty.DeliveryPoints)
			{
				if(deliveryPoint.Id == Entity.Id)
				{
					continue;
				}

				deliveryPoint.MaximalOrderSumLimit = Entity.MaximalOrderSumLimit;
				deliveryPoint.MinimalOrderSumLimit = Entity.MinimalOrderSumLimit;
			}
		}

		public void SetCoordinatesFromBuffer(string buffer)
		{
			var error = true;
			var coordinates = buffer?.Split(',');
			if(coordinates?.Length == 2)
			{
				coordinates[0] = coordinates[0].Replace('.', ',');
				coordinates[1] = coordinates[1].Replace('.', ',');

				var goodLat = decimal.TryParse(coordinates[0].Trim(), out decimal latitude);
				var goodLon = decimal.TryParse(coordinates[1].Trim(), out decimal longitude);

				if(goodLat && goodLon)
				{
					WriteCoordinates(latitude, longitude, true);
					error = false;
				}
			}

			if(error)
			{
				CommonServices.InteractiveService.ShowMessage(ImportanceLevel.Error,
					"Буфер обмена не содержит координат или содержит неправильные координаты");
			}
		}

		public void WriteCoordinates(decimal? latitude, decimal? longitude, bool isManual)
		{
			Entity.ManualCoordinates = isManual;
			if(EqualCoords(Entity.Latitude, latitude) && EqualCoords(Entity.Longitude, longitude))
			{
				return;
			}

			Entity.SetСoordinates(latitude, longitude, UoW);
			Entity.СoordsLastChangeUser = _currentUser ?? (_currentUser = _userRepository.GetCurrentUser(UoW));
		}

		/// <summary>
		/// Сравнивает координаты с точностью 6 знаков после запятой
		/// </summary>
		/// <returns><c>true</c> если координаты равны, <c>false</c> иначе.</returns>
		private bool EqualCoords(decimal? coord1, decimal? coord2)
		{
			if(!coord1.HasValue || !coord2.HasValue)
			{
				return false;
			}

			decimal coordDiff = Math.Abs(coord1.Value - coord2.Value);
			return Math.Round(coordDiff, 6) == decimal.Zero;
		}

		#region ITDICloseControlTab

		public bool CanClose()
		{
			if(!_isNotSaving)
			{
				CommonServices.InteractiveService.ShowMessage(ImportanceLevel.Warning,
					"Дождитесь завершения сохранения точки доставки и повторите", "Сохранение...");
			}

			return _isNotSaving;
		}

		#endregion

		#region Commands

		private DelegateCommand _openCounterpartyCommand;

		public DelegateCommand OpenCounterpartyCommand => _openCounterpartyCommand ?? (_openCounterpartyCommand = new DelegateCommand(
			() => _gtkTabsOpener.OpenCounterpartyDlg(this, Entity.Counterparty.Id),
			() => Entity.Counterparty != null
		));

		#endregion
	}
}
