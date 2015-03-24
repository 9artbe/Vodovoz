﻿using System;
using System.Data.Bindings;
using QSOrmProject;
using QSTDI;
using NHibernate;
using NLog;
using System.Collections.Generic;
using NHibernate.Criterion;
using QSValidation;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class FreeRentEquipmentDlg : Gtk.Bin, QSTDI.ITdiDialog, IOrmDialog
	{
		static Logger logger = LogManager.GetCurrentClassLogger ();
		ISession session;
		Adaptor adaptor = new Adaptor ();
		FreeRentEquipment subject;
		bool firstCall = true;
		IFreeRentEquipmentOwner FreeRentOwner;

		public ITdiTabParent TabParent { set; get; }

		public event EventHandler<TdiTabNameChangedEventArgs> TabNameChanged;
		public event EventHandler<TdiTabCloseEventArgs> CloseTab;

		public bool HasChanges {
			get { return false; }
		}

		string _tabName = "Новый оборудование к доп. соглашению";

		public string TabName {
			get{ return _tabName; }
			set {
				if (_tabName == value)
					return;
				_tabName = value;
				if (TabNameChanged != null)
					TabNameChanged (this, new TdiTabNameChangedEventArgs (value));
			}

		}

		public ISession Session {
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
			get { return subject; }
			set {
				if (value is FreeRentEquipment)
					subject = value as FreeRentEquipment;
			}
		}

		OrmParentReference parentReference;

		public OrmParentReference ParentReference {
			set {
				parentReference = value;
				if (parentReference != null) {
					Session = parentReference.Session;
					if (!(parentReference.ParentObject is IFreeRentEquipmentOwner)) {
						throw new ArgumentException (String.Format ("Родительский объект в parentReference должен реализовывать интерфейс {0}", typeof(IFreeRentEquipmentOwner)));
					}
					FreeRentOwner = (IFreeRentEquipmentOwner)parentReference.ParentObject;
				}
			}
			get {
				return parentReference;
			}
		}

		public FreeRentEquipmentDlg (OrmParentReference parentReference, FreeRentEquipment sub)
		{
			this.Build ();
			ParentReference = parentReference;
			subject = sub;
			if (subject.Equipment != null && subject.FreeRentPackage != null)
				TabName = subject.EquipmentName + " " + subject.PackageName;
			ConfigureDlg ();
		}

		private void ConfigureDlg ()
		{
			adaptor.Target = subject;
			datatable1.DataSource = adaptor;
			referenceEquipment.SubjectType = typeof(Equipment);
			referenceFreeRentPackage.SubjectType = typeof(FreeRentPackage);
			if (referenceFreeRentPackage.Subject == null)
				referenceEquipment.Sensitive = false;
			referenceFreeRentPackage.Changed += OnReferenceFreeRentPackageChanged;
		}

		public bool Save ()
		{
			var valid = new QSValidator<FreeRentEquipment> (subject);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			OrmMain.DelayedNotifyObjectUpdated (ParentReference.ParentObject, subject);
			return true;
		}

		public override void Destroy ()
		{
			adaptor.Disconnect ();
			base.Destroy ();
		}

		protected void OnButtonSaveClicked (object sender, EventArgs e)
		{
			if (Save ())
				OnCloseTab (false);
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			OnCloseTab (false);
		}

		protected void OnCloseTab (bool askSave)
		{
			if (CloseTab != null)
				CloseTab (this, new TdiTabCloseEventArgs (askSave));
		}

		protected void OnReferenceFreeRentPackageChanged (object sender, EventArgs e)
		{
			if (referenceFreeRentPackage.Subject == null)
				referenceEquipment.Sensitive = false;
			else {
				referenceEquipment.Sensitive = true;
				EquipmentType type = (referenceFreeRentPackage.Subject as FreeRentPackage).EquipmentType;
				referenceEquipment.ItemsCriteria = Session.CreateCriteria<Equipment> ()
					.CreateAlias ("Nomenclature", "n")
					.Add (Restrictions.Eq ("n.Type", type));
				if (!firstCall) {
					subject.Deposit = (referenceFreeRentPackage.Subject as FreeRentPackage).Deposit;
					subject.WaterAmount = (referenceFreeRentPackage.Subject as FreeRentPackage).MinWaterAmount;
				} else
					firstCall = false;
				if (subject.Equipment != null &&
				    subject.Equipment.Nomenclature.Type != (referenceFreeRentPackage.Subject as FreeRentPackage).EquipmentType)
					subject.Equipment = null;
			}
		}
	}
}

