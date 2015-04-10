﻿using System;
using System.Data.Bindings;
using NHibernate;
using NLog;
using QSOrmProject;
using QSTDI;
using QSValidation;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class AdditionalAgreementDailyRent : Gtk.Bin, ITdiDialog, IOrmSlaveDialog
	{
		protected DailyRentAgreement subjectCopy;
		protected bool isSaveButton;
		protected static Logger logger = LogManager.GetCurrentClassLogger ();
		protected ISession session;
		protected Adaptor adaptor = new Adaptor ();
		protected IAdditionalAgreementOwner AgreementOwner;

		public bool HasChanges {
			get { return false; }
		}

		#region ITdiTab implementation

		public event EventHandler<TdiTabNameChangedEventArgs> TabNameChanged;
		public event EventHandler<TdiTabCloseEventArgs> CloseTab;

		public QSTDI.ITdiTabParent TabParent { get ; set ; }

		protected string _tabName = "Новое доп. соглашение";

		public string TabName {
			get { return _tabName; }
			set {
				if (_tabName == value)
					return;
				_tabName = value;
				if (TabNameChanged != null)
					TabNameChanged (this, new TdiTabNameChangedEventArgs (value));
			}
		}

		#endregion

		#region IOrmDialog implementation

		public NHibernate.ISession Session {
			get {
				if (session == null)
					Session = OrmMain.Sessions.OpenSession ();
				return session;
			}
			set { session = value; }
		}

		#endregion

		OrmParentReference parentReference;

		public OrmParentReference ParentReference {
			set {
				parentReference = value;
				if (parentReference != null) {
					Session = parentReference.Session;
					if (!(parentReference.ParentObject is IAdditionalAgreementOwner)) {
						throw new ArgumentException (String.Format ("Родительский объект в parentReference должен реализовывать интерфейс {0}", typeof(IAdditionalAgreementOwner)));
					}
					AgreementOwner = (IAdditionalAgreementOwner)parentReference.ParentObject;
				}
			}
			get { return parentReference; }
		}

		protected void OnButtonSaveClicked (object sender, EventArgs e)
		{
			if (Save ()) {
				isSaveButton = true;
				OnCloseTab (false);
			}
		}

		protected void OnCloseTab (bool askSave)
		{
			if (TabParent.CheckClosingSlaveTabs ((ITdiTab)this))
				return;

			if (CloseTab != null)
				CloseTab (this, new TdiTabCloseEventArgs (askSave));
		}

		public override void Destroy ()
		{
			if (!isSaveButton) {
				if (subject.IsNew)
					(parentReference.ParentObject as IAdditionalAgreementOwner).AdditionalAgreements.Remove (subject);
				else
					ObjectCloner.FieldsCopy<DailyRentAgreement> (subjectCopy, ref subject);
			}
			adaptor.Disconnect ();
			base.Destroy ();
		}


		private DailyRentAgreement subject;

		public object Subject {
			get { return subject; }
			set {
				if (value is DailyRentAgreement)
					subject = value as DailyRentAgreement;
			}
		}

		public AdditionalAgreementDailyRent (OrmParentReference parentReference, DailyRentAgreement sub)
		{
			this.Build ();
			subjectCopy = ObjectCloner.Clone<DailyRentAgreement> (sub);
			ParentReference = parentReference;
			subject = sub;
			TabName = subject.AgreementTypeTitle + " " + subject.AgreementNumber;
			ConfigureDlg ();
		}

		private void ConfigureDlg ()
		{
			adaptor.Target = subject;
			datatable1.DataSource = adaptor;
			entryAgreementNumber.IsEditable = true;

			var identifiers = new List<object> ();
			foreach (DeliveryPoint d in (parentReference.ParentObject as CounterpartyContract).Counterparty.DeliveryPoints)
				identifiers.Add (d.Id);
			referenceDeliveryPoint.SubjectType = typeof(DeliveryPoint);
			referenceDeliveryPoint.ItemsCriteria = Session.CreateCriteria<DeliveryPoint> ()
				.Add (Restrictions.In ("Id", identifiers));
			dataAgreementType.Text = (parentReference.ParentObject as CounterpartyContract).Number + " - А";

			paidrentpackagesview1.ParentReference = new OrmParentReference (session, subject, "Equipment");
			paidrentpackagesview1.DailyRent = true;
		}

		public bool Save ()
		{
			var valid = new QSValidator<DailyRentAgreement> (subject);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			subject.IsNew = false;
			OrmMain.DelayedNotifyObjectUpdated (ParentReference.ParentObject, subject);
			return true;
		}

		protected void OnSpinRentDaysValueChanged (object sender, EventArgs e)
		{
			paidrentpackagesview1.UpdateTotalLabels ();
		}
	}
}

