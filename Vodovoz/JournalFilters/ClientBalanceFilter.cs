﻿using System;
using QS.DomainModel.UoW;
using QS.Project.Journal.EntitySelector;
using QS.Project.Services;
using QSOrmProject;
using QSOrmProject.RepresentationModel;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Goods;
using Vodovoz.Filters.ViewModels;
using Vodovoz.JournalViewModels;
using Vodovoz.TempAdapters;
using Vodovoz.JournalSelector;
using Vodovoz.FilterViewModels.Goods;
using Vodovoz.Parameters;
using Vodovoz.EntityRepositories;
using Vodovoz.EntityRepositories.Goods;

namespace Vodovoz
{
	[OrmDefaultIsFiltered(true)]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ClientBalanceFilter : RepresentationFilterBase<ClientBalanceFilter>
	{
		private bool userHasOnlyAccessToWarehouseAndComplaints;
		
		protected override void ConfigureWithUow()
		{
			nomenclatureEntry.SetEntityAutocompleteSelectorFactory(
				new NomenclatureAutoCompleteSelectorFactory<Nomenclature, NomenclaturesJournalViewModel>(
					ServicesConfig.CommonServices, new NomenclatureFilterViewModel(), new CounterpartyJournalFactory().CreateCounterpartyAutocompleteSelectorFactory(),
					new NomenclatureRepository(new NomenclatureParametersProvider()), UserSingletonRepository.GetInstance()));

			entryClient.SetEntityAutocompleteSelectorFactory(new DefaultEntityAutocompleteSelectorFactory<Counterparty, CounterpartyJournalViewModel, CounterpartyJournalFilterViewModel>(QS.Project.Services.ServicesConfig.CommonServices));
		}

		public ClientBalanceFilter(IUnitOfWork uow) : this()
		{
			UoW = uow;
		}

		public ClientBalanceFilter()
		{
			this.Build();
		}

		public Counterparty RestrictCounterparty {
			get { return entryClient.Subject as Counterparty; }
			set {
				entryClient.Subject = value;
				entryClient.Sensitive = false;
			}
		}

		public Nomenclature RestrictNomenclature {
			get { return nomenclatureEntry.Subject as Nomenclature; }
			set {
				nomenclatureEntry.Subject = value;
				nomenclatureEntry.Sensitive = false;
			}
		}

		public DeliveryPoint RestrictDeliveryPoint {
			get { return entryreferencePoint.Subject as DeliveryPoint; }
			set {
				entryreferencePoint.Subject = value;
				entryreferencePoint.Sensitive = false;
			}
		}

		public bool RestrictIncludeSold {
			get { return checkIncludeSold.Active; }
			set {
				checkIncludeSold.Active = value;
				checkIncludeSold.Sensitive = false;
			}
		}

		protected void OnSpeccomboStockItemSelected(object sender, QS.Widgets.EnumItemClickedEventArgs e)
		{
			OnRefiltered();
		}

		protected void OnEntryClientChanged(object sender, EventArgs e)
		{
			entryreferencePoint.Sensitive = RestrictCounterparty != null;
			if(RestrictCounterparty == null)
				entryreferencePoint.Subject = null;
			else {
				entryreferencePoint.Subject = null;
				entryreferencePoint.RepresentationModel = new ViewModel.ClientDeliveryPointsVM(UoW, RestrictCounterparty);
			}
			OnRefiltered();
		}

		protected void OnEntryreferencePointChanged(object sender, EventArgs e)
		{
			OnRefiltered();
		}

		protected void OnCheckIncludeSoldToggled(object sender, EventArgs e)
		{
			OnRefiltered();
		}

		protected void OnEntryreferenceNomenclatureChangedByUser(object sender, EventArgs e)
		{
			OnRefiltered();
		}
	}
}

