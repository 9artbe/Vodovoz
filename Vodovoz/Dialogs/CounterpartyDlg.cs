﻿using System;
using QSOrmProject;
using QSTDI;
using NLog;
using NHibernate;
using System.Data.Bindings;
using System.Collections.Generic;
using QSContacts;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class CounterpartyDlg : Gtk.Bin, QSTDI.ITdiDialog, IOrmDialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private ISession session;
		private Adaptor adaptor = new Adaptor();
		private Counterparty subject;
		private bool NewItem = false;

		public CounterpartyDlg()
		{
			this.Build();
			NewItem = true;
			subject = new Counterparty();
			ConfigureDlg();
		}

		public CounterpartyDlg(int id)
		{
			this.Build();
			subject = Session.Load<Counterparty>(id);
			TabName = subject.Name;
			ConfigureDlg();
		}

		public CounterpartyDlg(Counterparty sub)
		{
			this.Build();
			subject = Session.Load<Counterparty>(sub.Id);
			TabName = subject.Name;
			ConfigureDlg();
		}

		private void ConfigureDlg()
		{
			entryName.IsEditable = true;
			entryFullName.IsEditable = true;
			notebook1.CurrentPage = 0;
			notebook1.ShowTabs = false;
			adaptor.Target = subject;
			datatable1.DataSource = adaptor;
			datatable2.DataSource = adaptor;
			referenceSignificance.SubjectType = typeof(Significance);
			referenceStatus.SubjectType = typeof(CounterpartyStatus);
			referenceAccountant.SubjectType = typeof(Employee);
			referenceBottleManager.SubjectType = typeof(Employee);
			referenceSalesManager.SubjectType = typeof(Employee);
			enumPayment.DataSource = adaptor;
			enumPersonType.DataSource = adaptor;
			enumCounterpartyType.DataSource = adaptor;
			contactsview1.Contact = (IContact)Subject;
			contactsview1.Session = Session;
			emailsView.Session = Session;
			if (subject.Emails == null)
				subject.Emails = new List<Email>();
			emailsView.Emails = subject.Emails;
			phonesView.Session = Session;
			if (subject.Phones == null)
				subject.Phones = new List<Phone>();
			phonesView.Phones = subject.Phones;
		}

		#region ITdiTab implementation

		public event EventHandler<QSTDI.TdiTabNameChangedEventArgs> TabNameChanged;

		public event EventHandler<QSTDI.TdiTabCloseEventArgs> CloseTab;

		private string _tabName = "Новый контрагент";
		public string TabName
		{
			get{return _tabName;}
			set{
				if (_tabName == value)
					return;
				_tabName = value;
				if (TabNameChanged != null)
					TabNameChanged(this, new TdiTabNameChangedEventArgs(value));
			}

		}

		public QSTDI.ITdiTabParent TabParent { get ; set ; }

		#endregion

		#region ITdiDialog implementation

		public bool Save ()
		{
			logger.Info("Сохраняем контрагента...");
			if (entryName.Text == String.Empty) {
				logger.Error ("Не введено имя контрагента.");
				return false;
			}
			Session.SaveOrUpdate(subject);
			phonesView.SaveChanges();
			emailsView.SaveChanges ();
			Session.Flush();
			foreach (Contact c in (Subject as Counterparty).Contacts) {
				OrmMain.NotifyObjectUpdated (c);
			}
			OrmMain.NotifyObjectUpdated(subject);

			return true;
		}

		public bool HasChanges {
			get {return NewItem || Session.IsDirty();}
		}

		#endregion

		#region IOrmDialog implementation

		public NHibernate.ISession Session {
			get {
				if (session == null)
					Session = OrmMain.Sessions.OpenSession ();
				return session;
			}
			set {
				session = value;
			}
		}

		public object Subject {
			get {return subject;}
			set {
				if (value is Counterparty)
					subject = value as Counterparty;
			}
		}
		#endregion

		protected void OnButtonSaveClicked (object sender, EventArgs e)
		{
			if (!this.HasChanges || Save())
				OnCloseTab(false);
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			OnCloseTab(false);
		}

		protected void OnCloseTab(bool askSave)
		{
			if (CloseTab != null)
				CloseTab(this, new TdiTabCloseEventArgs(askSave));
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
	}
}

