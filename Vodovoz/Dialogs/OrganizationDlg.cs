﻿using System;
using QSTDI;
using QSOrmProject;
using NHibernate;
using System.Data.Bindings;
using NLog;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OrganizationDlg : Gtk.Bin, QSTDI.ITdiDialog, IOrmDialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private ISession session;
		private Adaptor adaptorOrg = new Adaptor();
		private Organization subject;
		private bool NewItem = false;

		public event EventHandler<TdiTabNameChangedEventArgs> TabNameChanged;
		public event EventHandler<TdiTabCloseEventArgs> CloseTab;
		public bool HasChanges { 
			get{return NewItem || Session.IsDirty();}
		}

		private string _tabName = "Новая организация";
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

		public ISession Session
		{
			get
			{
				if (session == null)
					session = OrmMain.Sessions.OpenSession();
				return session;
			}
			set
			{
				session = value;
			}
		}

		public object Subject
		{
			get {return subject;}
			set {
				if (value is Organization)
					subject = value as Organization;
			}
		}

		public OrganizationDlg()
		{
			this.Build();
			NewItem = true;
			subject = new Organization();
			ConfigureDlg();
		}

		public OrganizationDlg(int id)
		{
			this.Build();
			subject = Session.Load<Organization>(id);
			TabName = subject.Name;
			ConfigureDlg();
		}

		public OrganizationDlg(Organization sub)
		{
			this.Build();
			subject = Session.Load<Organization>(sub.Id);
			TabName = subject.Name;
			ConfigureDlg();
		}

		private void ConfigureDlg()
		{
			adaptorOrg.Target = subject;
			datatableMain.DataSource = adaptorOrg;
			datatextviewAddress.Editable = true;
			datatextviewJurAddress.Editable = true;
			notebookMain.Page = 0;
			notebookMain.ShowTabs = false;
		}

		public bool Save()
		{
			logger.Info("Сохраняем организацию...");
			Session.SaveOrUpdate(subject);
			Session.Flush();
			OrmMain.NotifyObjectUpdated(subject);
			return true;
		}

		public override void Destroy()
		{
			Session.Close();
			//adaptorOrg.Disconnect();
			base.Destroy();
		}

		protected void OnButtonSaveClicked(object sender, EventArgs e)
		{
			if (!this.HasChanges || Save())
				OnCloseTab(false);
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			OnCloseTab(false);
		}

		protected void OnCloseTab(bool askSave)
		{
			if (CloseTab != null)
				CloseTab(this, new TdiTabCloseEventArgs(askSave));
		}

		protected void OnRadioTabInfoToggled(object sender, EventArgs e)
		{
			if (radioTabInfo.Active)
				notebookMain.CurrentPage = 0;
		}

		protected void OnRadioTabAccountsToggled(object sender, EventArgs e)
		{
			if (radioTabAccounts.Active)
				notebookMain.CurrentPage = 1;
		}
	}
}

