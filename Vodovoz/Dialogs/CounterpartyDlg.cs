﻿using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NLog;
using QSContacts;
using QSOrmProject;
using QSProjectsLib;
using QSValidation;
using Vodovoz.Domain;
using QSBanks;
using Vodovoz.Domain.Cash;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class CounterpartyDlg : OrmGtkDialogBase<Counterparty>
	{
		static Logger logger = LogManager.GetCurrentClassLogger ();

		public CounterpartyDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Counterparty> ();
			TabName = "Новый контрагент";
			ConfigureDlg ();
		}

		public CounterpartyDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Counterparty> (id);
			ConfigureDlg ();
		}

		public CounterpartyDlg (Counterparty sub) : this (sub.Id)
		{
		}

		private void ConfigureDlg ()
		{
			notebook1.CurrentPage = 0;
			notebook1.ShowTabs = false;
			//Initializing null fields
			emailsView.Session = UoWGeneric.Session;
			phonesView.UoW = UoWGeneric;
			if (UoWGeneric.Root.Emails == null)
				UoWGeneric.Root.Emails = new List<Email> ();
			emailsView.Emails = UoWGeneric.Root.Emails;
			if (UoWGeneric.Root.Phones == null)
				UoWGeneric.Root.Phones = new List<Phone> ();
			phonesView.Phones = UoWGeneric.Root.Phones;
			if (UoWGeneric.Root.CounterpartyContracts == null) {
				UoWGeneric.Root.CounterpartyContracts = new List<CounterpartyContract> ();
			}
			//Setting up editable property
			entryJurAddress.IsEditable = entryFullName.IsEditable = true;
			dataComment.Editable = dataWaybillComment.Editable = true;
			//Other fields properties
			validatedINN.ValidationMode = validatedKPP.ValidationMode = QSWidgetLib.ValidationType.numeric;
			//Setting up fields sources
			datatable1.DataSource = datatable2.DataSource = datatable3.DataSource = datatable4.DataSource = subjectAdaptor;
			enumPayment.DataSource = enumPersonType.DataSource = enumCounterpartyType.DataSource = subjectAdaptor;
			validatedINN.DataSource = validatedKPP.DataSource = subjectAdaptor;
			//Setting subjects
			accountsView.ParentReference = new ParentReferenceGeneric<Counterparty, Account> (UoWGeneric, c => c.Accounts);
			deliveryPointView.DeliveryPointUoW = UoWGeneric;
			counterpartyContractsView.CounterpartyUoW = UoWGeneric;
			referenceSignificance.SubjectType = typeof(Significance);
			referenceStatus.SubjectType = typeof(CounterpartyStatus);
			referenceDefaultExpense.SubjectType = typeof(ExpenseCategory);
			referenceAccountant.SubjectType = referenceBottleManager.SubjectType = referenceSalesManager.SubjectType = typeof(Employee);
			referenceMainCounterparty.ItemsCriteria = UoWGeneric.Session.CreateCriteria<Counterparty> ()
				.Add (Restrictions.Not (Restrictions.Eq ("id", UoWGeneric.Root.Id)));
			referenceMainCounterparty.SubjectType = typeof(Counterparty);
			proxiesview1.CounterpartyUoW = UoWGeneric;
			dataentryMainContact.RepresentationModel = new ViewModel.ContactsVM (UoW, Entity);
			dataentryFinancialContact.RepresentationModel = new ViewModel.ContactsVM (UoW, Entity);
			//Setting Contacts
			contactsview1.CounterpartyUoW = UoWGeneric;
			//Setting permissions
			spinMaxCredit.Sensitive = QSMain.User.Permissions ["max_loan_amount"];
			datalegalname1.Binding.AddSource (Entity)
				.AddBinding (s => s.Name, t => t.OwnName)
				.AddBinding (s => s.TypeOfOwnership, t => t.Ownership)
				.AddBinding (s => s.FullName, t => t.FullName)
				.InitializeFromSource ();

			//make actions menu
			var menu = new Gtk.Menu ();
			var menuItem = new Gtk.MenuItem ("Все заказы контрагента");
			menuItem.Activated += AllOrders_Activated;
			menu.Add (menuItem);
			menuActions.Menu = menu;
			menu.ShowAll ();
		}

		void AllOrders_Activated (object sender, EventArgs e)
		{
			var filter = new OrdersFilter (UnitOfWorkFactory.CreateWithoutRoot ());
			filter.RestrictCounterparty = Entity;

			ReferenceRepresentation OrdersDialog = new ReferenceRepresentation (new ViewModel.OrdersVM (filter));
			OrdersDialog.Mode = OrmReferenceMode.Normal;

			TabParent.AddTab (OrdersDialog, this, false);
		}

		public override bool Save ()
		{
			var valid = new QSValidator<Counterparty> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем контрагента...");
			phonesView.SaveChanges ();
			emailsView.SaveChanges ();
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}

		protected void OnRadioInfoToggled (object sender, EventArgs e)
		{
			if (radioInfo.Active)
				notebook1.CurrentPage = 0;
		}

		protected void OnRadioContactsToggled (object sender, EventArgs e)
		{
			if (radioContacts.Active)
				notebook1.CurrentPage = 1;
		}

		protected void OnRadioDetailsToggled (object sender, EventArgs e)
		{
			if (radioDetails.Active)
				notebook1.CurrentPage = 2;
		}

		protected void OnRadioCuratorsToggled (object sender, EventArgs e)
		{
			if (radioCurators.Active)
				notebook1.CurrentPage = 3;
		}

		protected void OnRadioContactPersonsToggled (object sender, EventArgs e)
		{
			if (radioContactPersons.Active)
				notebook1.CurrentPage = 4;
		}

		protected void OnRadiobuttonProxiesToggled (object sender, EventArgs e)
		{
			if (radiobuttonProxies.Active)
				notebook1.CurrentPage = 5;
		}

		protected void OnRadioContractsToggled (object sender, EventArgs e)
		{
			if (radioContracts.Active)
				notebook1.CurrentPage = 6;
		}

		protected void OnRadioDeliveryPointToggled (object sender, EventArgs e)
		{
			if (radioDeliveryPoint.Active)
				notebook1.CurrentPage = 7;
		}

		protected void OnEnumPersonTypeChanged (object sender, EventArgs e)
		{
			labelFIO.Visible = entryFIO.Visible = Entity.PersonType == PersonType.natural;
			labelShort.Visible = datalegalname1.Visible = 
				labelFullName.Visible = entryFullName.Visible = 
					referenceMainCounterparty.Visible = labelMainCounterparty.Visible =
					radioDetails.Visible = radiobuttonProxies.Visible = Entity.PersonType == PersonType.legal;
		}

		protected void OnEnumCounterpartyTypeChanged (object sender, EventArgs e)
		{
			labelDefaultExpense.Visible = referenceDefaultExpense.Visible = Entity.CounterpartyType == CounterpartyType.supplier;
		}
	}
}

