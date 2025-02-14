﻿using System;
using QS.Project.Filter;
using QS.Project.Journal.EntitySelector;
using QS.Project.Services;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Orders;
using Vodovoz.FilterViewModels.Goods;
using QS.DomainModel.Entity;
using Vodovoz.EntityRepositories;
using Vodovoz.EntityRepositories.Goods;
using Vodovoz.JournalSelector;
using Vodovoz.JournalViewModels;
using Vodovoz.Parameters;
using Vodovoz.TempAdapters;

namespace Vodovoz.Filters.ViewModels
{
	public class DebtorsJournalFilterViewModel : FilterViewModelBase<DebtorsJournalFilterViewModel>
	{
		private Counterparty _client;
		private DeliveryPoint _address;
		private PersonType? _opf;
		private DateTime? _endDate;
		private DateTime? _startDate;
		private int? _debtBottlesTo;
		private int? _debtBottlesFrom;
		private bool _hideWithOneOrder;
		private int? _lastOrderBottlesTo;
		private int? _lastOrderBottlesFrom;
		private bool _hideActiveCounterparty;
		private DiscountReason _discountReason;
		private Nomenclature _lastOrderNomenclature;
		private IEntityAutocompleteSelectorFactory _counterpartySelectorFactory;
		private IEntityAutocompleteSelectorFactory _nomenclatureSelectorFactory;
		private IEntityAutocompleteSelectorFactory _deliveryPointSelectorFactory;


		public DebtorsJournalFilterViewModel()
		{
			UpdateWith(
				x => x.Client,
				x => x.Address,
				x => x.OPF,
				x => x.StartDate,
				x => x.EndDate,
				x => x.DebtBottlesFrom,
				x => x.DebtBottlesTo,
				x => x.LastOrderBottlesFrom,
				x => x.LastOrderBottlesTo,
				x => x.LastOrderNomenclature,
				x => x.DiscountReason,
				x => x.HideActiveCounterparty
			);
		}

		public Counterparty Client {
			get => _client;
			set => SetField(ref _client, value, () => Client);
		}

		public DeliveryPoint Address {
			get => _address;
			set => SetField(ref _address, value, () => Address);
		}

		public PersonType? OPF {
			get => _opf;
			set => SetField(ref _opf, value, () => OPF);
		}

		public DateTime? StartDate {
			get => _startDate;
			set => SetField(ref _startDate, value, () => StartDate);
		}

		[PropertyChangedAlso(nameof(ShowHideActiveCheck))]
		public DateTime? EndDate {
			get => _endDate;
			set => SetField(ref _endDate, value, () => EndDate);
		}

		public bool ShowHideActiveCheck => EndDate != null;

		public bool HideActiveCounterparty {
			get => _hideActiveCounterparty;
			set => SetField(ref _hideActiveCounterparty, value, () => HideActiveCounterparty);
		}

		public bool HideWithOneOrder {
			get => _hideWithOneOrder;
			set => UpdateFilterField(ref _hideWithOneOrder, value);
		}

		public int? DebtBottlesFrom {
			get => _debtBottlesFrom;
			set => SetField(ref _debtBottlesFrom, value, () => DebtBottlesFrom);
		}

		public int? DebtBottlesTo {
			get => _debtBottlesTo;
			set => SetField(ref _debtBottlesTo, value, () => DebtBottlesTo);
		}

		public int? LastOrderBottlesFrom {
			get => _lastOrderBottlesFrom;
			set => SetField(ref _lastOrderBottlesFrom, value, () => LastOrderBottlesFrom);
		}

		public int? LastOrderBottlesTo {
			get => _lastOrderBottlesTo;
			set => SetField(ref _lastOrderBottlesTo, value, () => LastOrderBottlesTo);
		}

		public Nomenclature LastOrderNomenclature {
			get => _lastOrderNomenclature;
			set => SetField(ref _lastOrderNomenclature, value, () => LastOrderNomenclature);
		}

		public DiscountReason DiscountReason {
			get => _discountReason;
			set => SetField(ref _discountReason, value, () => DiscountReason);
		}
		
		public DeliveryPointJournalFilterViewModel DeliveryPointJournalFilterViewModel { get; set; } 
			= new DeliveryPointJournalFilterViewModel();

		public virtual IEntityAutocompleteSelectorFactory DeliveryPointSelectorFactory =>
			_deliveryPointSelectorFactory ?? (_deliveryPointSelectorFactory =
				new DeliveryPointJournalFactory(DeliveryPointJournalFilterViewModel)
					.CreateDeliveryPointAutocompleteSelectorFactory());

		public virtual IEntityAutocompleteSelectorFactory CounterpartySelectorFactory =>
			_counterpartySelectorFactory ?? (_counterpartySelectorFactory =
				new DefaultEntityAutocompleteSelectorFactory<Counterparty, CounterpartyJournalViewModel,
					CounterpartyJournalFilterViewModel>(ServicesConfig.CommonServices));

		public virtual IEntityAutocompleteSelectorFactory NomenclatureSelectorFactory =>
			_nomenclatureSelectorFactory ?? (_nomenclatureSelectorFactory =
				new NomenclatureAutoCompleteSelectorFactory<Nomenclature, NomenclaturesJournalViewModel>(
					ServicesConfig.CommonServices, new NomenclatureFilterViewModel(), CounterpartySelectorFactory,
					new NomenclatureRepository(new NomenclatureParametersProvider(new ParametersProvider())), new UserRepository()));
	}
}
