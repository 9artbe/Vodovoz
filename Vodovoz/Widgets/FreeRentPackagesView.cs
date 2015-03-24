﻿using System;
using QSTDI;
using QSOrmProject;
using System.Data.Bindings.Collections.Generic;
using NHibernate;
using System.Collections.Generic;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class FreeRentPackagesView : Gtk.Bin
	{
		private IFreeRentEquipmentOwner equipmentOwner;
		private GenericObservableList<FreeRentEquipment> Equipments;
		private ISession session;
		Decimal TotalDeposit = 0;
		int TotalWaterAmount = 0;

		public ISession Session {
			get {
				return session;
			}
			set {
				session = value;
			}
		}

		public IFreeRentEquipmentOwner EquipmentOwner {
			get {
				return equipmentOwner;
			}
			set {
				equipmentOwner = value;
				if (equipmentOwner.Equipment == null)
					equipmentOwner.Equipment = new List<FreeRentEquipment> ();
				Equipments = new GenericObservableList<FreeRentEquipment> (EquipmentOwner.Equipment);
				treeRentPackages.ItemsDataSource = Equipments;
			}
		}

		OrmParentReference parentReference;

		public OrmParentReference ParentReference {
			set {
				parentReference = value;
				if (parentReference != null) {
					Session = parentReference.Session;
					if (!(parentReference.ParentObject is IFreeRentEquipmentOwner)) {
						throw new ArgumentException (String.Format ("Родительский объект в parentReference должен реализовывать интерфейс {0}", typeof(IAdditionalAgreementOwner)));
					}
					EquipmentOwner = (IFreeRentEquipmentOwner)parentReference.ParentObject;
				}
			}
			get {
				return parentReference;
			}
		}

		public FreeRentPackagesView ()
		{
			this.Build ();
			treeRentPackages.Selection.Changed += OnSelectionChanged;
		}

		void OnSelectionChanged (object sender, EventArgs e)
		{
			bool selected = treeRentPackages.Selection.CountSelectedRows () > 0;
			buttonEdit.Sensitive = buttonDelete.Sensitive = selected;
		}

		protected void OnButtonAddClicked (object sender, EventArgs e)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;
			FreeRentEquipment equipment = new FreeRentEquipment ();
			Equipments.Add (equipment);
			ITdiDialog dlg = new FreeRentEquipmentDlg (ParentReference, equipment);
			mytab.TabParent.AddSlaveTab (mytab, dlg);
		}

		protected void OnButtonEditClicked (object sender, EventArgs e)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;

			ITdiDialog dlg = OrmMain.CreateObjectDialog (ParentReference, treeRentPackages.GetSelectedObjects () [0]);
			mytab.TabParent.AddSlaveTab (mytab, dlg);
		}

		protected void OnButtonDeleteClicked (object sender, EventArgs e)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;

			Equipments.Remove (treeRentPackages.GetSelectedObjects () [0] as FreeRentEquipment);
		}

		protected void OnTreeRentPackagesRowActivated (object o, Gtk.RowActivatedArgs args)
		{
			buttonEdit.Click ();
		}
	}
}

