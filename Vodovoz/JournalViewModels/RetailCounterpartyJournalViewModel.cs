﻿using System;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Transform;
using Vodovoz.Domain.Contacts;
using QS.DomainModel.UoW;
using QS.Services;
using Vodovoz.Domain.Client;
using Vodovoz.Filters.ViewModels;
using Vodovoz.JournalNodes;
using QS.Project.Journal;
using Vodovoz.Domain.Retail;

namespace Vodovoz.JournalViewModels
{
	public class RetailCounterpartyJournalViewModel : FilterableSingleEntityJournalViewModelBase<Counterparty, CounterpartyDlg, RetailCounterpartyJournalNode, CounterpartyJournalFilterViewModel>
	{
		public RetailCounterpartyJournalViewModel(CounterpartyJournalFilterViewModel filterViewModel, IUnitOfWorkFactory unitOfWorkFactory, ICommonServices commonServices) : base(filterViewModel, unitOfWorkFactory, commonServices)
		{
			TabName = "Журнал контрагентов";
			UpdateOnChanges(
				typeof(Counterparty),
				typeof(CounterpartyContract),
				typeof(Phone),
				typeof(Tag),
				typeof(DeliveryPoint)
			);
		}

		protected override Func<IUnitOfWork, IQueryOver<Counterparty>> ItemsSourceQueryFunction => (uow) => {
			RetailCounterpartyJournalNode resultAlias = null;
			Counterparty counterpartyAlias = null;
			Counterparty counterpartyAliasForSubquery = null;
			CounterpartyContract contractAlias = null;
			Phone phoneAlias = null;
			Phone deliveryPointPhoneAlias = null;
			DeliveryPoint addressAlias = null;
			DeliveryPoint deliveryPointAlias = null;
			Tag tagAliasForSubquery = null;
			SalesChannel salesChannelAlias = null;

			var query = uow.Session.QueryOver<Counterparty>(() => counterpartyAlias);

			if (FilterViewModel != null && FilterViewModel.IsForRetail != null)
			{
				query.Where(c => c.IsForRetail == FilterViewModel.IsForRetail);

				if (FilterViewModel.SalesChannels.Any(x => x.Selected))
				{
					query.Left.JoinAlias(c => c.SalesChannels, () => salesChannelAlias);
					query.Where(() => salesChannelAlias.Id.IsIn(FilterViewModel.SalesChannels.Where(x => x.Selected).Select(x => x.Id).ToArray()));
                }
			}

			if (FilterViewModel != null && !FilterViewModel.RestrictIncludeArchive) {
				query.Where(c => !c.IsArchive);
			}

			if(FilterViewModel?.CounterpartyType != null) {
				query.Where(t => t.CounterpartyType == FilterViewModel.CounterpartyType);
			}

			var contractsSubquery = QueryOver.Of<CounterpartyContract>(() => contractAlias)
				.Left.JoinAlias(c => c.Counterparty, () => counterpartyAliasForSubquery)
				.Where(() => counterpartyAlias.Id == counterpartyAliasForSubquery.Id)
				.Select(Projections.SqlFunction(
											new SQLFunctionTemplate(NHibernateUtil.String, "GROUP_CONCAT( CONCAT(?2,' - ',?1) SEPARATOR ?3)"),
											NHibernateUtil.String,
											Projections.Property(() => contractAlias.ContractSubNumber),
											Projections.Property(() => counterpartyAliasForSubquery.VodovozInternalId),
											Projections.Constant("\n")));

			var addressSubquery = QueryOver.Of<DeliveryPoint>(() => addressAlias)
				.Where(d => d.Counterparty.Id == counterpartyAlias.Id)
				.Where(() => addressAlias.IsActive)
				.Select(Projections.SqlFunction(
					new SQLFunctionTemplate(NHibernateUtil.String, "GROUP_CONCAT( ?1 SEPARATOR ?2)"),
					NHibernateUtil.String,
					Projections.Property(() => addressAlias.CompiledAddress),
					Projections.Constant("\n")));

			var tagsSubquery = QueryOver.Of<Counterparty>(() => counterpartyAliasForSubquery)
				.Where(() => counterpartyAlias.Id == counterpartyAliasForSubquery.Id)
				.JoinAlias(c => c.Tags, () => tagAliasForSubquery)
				.Select(Projections.SqlFunction(
					new SQLFunctionTemplate(NHibernateUtil.String, "GROUP_CONCAT( CONCAT(' <span foreground=\"', ?1, '\"> ♥</span>', ?2) SEPARATOR '\n')"),
					NHibernateUtil.String,
					Projections.Property(() => tagAliasForSubquery.ColorText),
					Projections.Property(() => tagAliasForSubquery.Name)
				));

			if(FilterViewModel != null && FilterViewModel.Tag != null)
				query.JoinAlias(c => c.Tags, () => tagAliasForSubquery)
					 .Where(() => tagAliasForSubquery.Id == FilterViewModel.Tag.Id);

			query
				.Left.JoinAlias(c => c.Phones, () => phoneAlias)
				.Left.JoinAlias(() => counterpartyAlias.DeliveryPoints, () => deliveryPointAlias)
				.Left.JoinAlias(() => deliveryPointAlias.Phones, () => deliveryPointPhoneAlias);



			var searchHealperNew = new TempAdapters.SearchHelper(Search);

			var idParam = new TempAdapters.SearchParameter(() => counterpartyAlias.Id, TempAdapters.SearchParametrType.Id);
			var vodovozInternalIdParam = new TempAdapters.SearchParameter(() => counterpartyAlias.VodovozInternalId, TempAdapters.SearchParametrType.VodovozInternalId);
			var nameParam = new TempAdapters.SearchParameter(() => counterpartyAlias.Name, TempAdapters.SearchParametrType.Name);
			var INNParam = new TempAdapters.SearchParameter(() => counterpartyAlias.INN, TempAdapters.SearchParametrType.INN);
			var digitNumberParam = new TempAdapters.SearchParameter(() => phoneAlias.DigitsNumber, TempAdapters.SearchParametrType.DigitsNumber);
			var deliveryPointPhoneParam = new TempAdapters.SearchParameter(() => deliveryPointPhoneAlias.DigitsNumber, TempAdapters.SearchParametrType.DigitsNumber);
			var compiledAdressParam = new TempAdapters.SearchParameter(() => deliveryPointAlias.CompiledAddress, TempAdapters.SearchParametrType.CompiledAddress);

			query.Where(searchHealperNew.GetSearchCriterionNew(
				idParam,
				vodovozInternalIdParam,
				nameParam,
				INNParam,
				digitNumberParam,
				deliveryPointPhoneParam,
				compiledAdressParam
			));

			var counterpartyResultQuery = query.SelectList(list => list
				   .SelectGroup(c => c.Id).WithAlias(() => resultAlias.Id)
				   .SelectGroup(c => c.VodovozInternalId).WithAlias(() => resultAlias.InternalId)
				   .Select(c => c.Name).WithAlias(() => resultAlias.Name)
				   .Select(c => c.INN).WithAlias(() => resultAlias.INN)
				   .Select(c => c.IsArchive).WithAlias(() => resultAlias.IsArhive)
				   .SelectSubQuery(contractsSubquery).WithAlias(() => resultAlias.Contracts)
				   .Select(Projections.SqlFunction(
					   new SQLFunctionTemplate(NHibernateUtil.String, "GROUP_CONCAT(DISTINCT ?1 SEPARATOR ?2)"),
					   NHibernateUtil.String,
					   Projections.Property(() => phoneAlias.Number),
					   Projections.Constant("\n"))
					   ).WithAlias(() => resultAlias.Phones)			   
					.SelectSubQuery(addressSubquery).WithAlias(() => resultAlias.Addresses)
					.SelectSubQuery(tagsSubquery).WithAlias(() => resultAlias.Tags)
				)
				.TransformUsing(Transformers.AliasToBean<RetailCounterpartyJournalNode>())
				;

			return counterpartyResultQuery;
		};

		protected override Func<CounterpartyDlg> CreateDialogFunction => () => new CounterpartyDlg();

		protected override Func<RetailCounterpartyJournalNode, CounterpartyDlg> OpenDialogFunction => (node) => new CounterpartyDlg(node.Id);
	}
}
