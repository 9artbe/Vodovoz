﻿using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using Gtk;
using NHibernate;
using QSOrmProject;
using QSTDI;
using Vodovoz.Domain;
using System.Linq;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class AdditionalAgreementsView : Gtk.Bin
	{
		private IAdditionalAgreementOwner agreementOwner;
		private GenericObservableList<AdditionalAgreement> additionalAgreements;
		private ISession session;

		public ISession Session {
			get { return session; }
			set { session = value; }
		}

		bool isEditable = true;

		public bool IsEditable { 
			get { return isEditable; } 
			set {
				isEditable = value; 
				buttonAdd.Sensitive = buttonDelete.Sensitive = 
					treeAdditionalAgreements.Sensitive = buttonEdit.Sensitive = value;
			}
		}

		public IAdditionalAgreementOwner AgreementOwner {
			get { return agreementOwner; }
			set {
				agreementOwner = value;
				if (agreementOwner.AdditionalAgreements == null)
					AgreementOwner.AdditionalAgreements = new List<AdditionalAgreement> ();
				additionalAgreements = new GenericObservableList<AdditionalAgreement> (AgreementOwner.AdditionalAgreements);
				treeAdditionalAgreements.ItemsDataSource = additionalAgreements;
			}
		}

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

		public AdditionalAgreementsView ()
		{
			this.Build ();
			treeAdditionalAgreements.Selection.Changed += OnSelectionChanged;
		}

		void OnSelectionChanged (object sender, EventArgs e)
		{
			bool selected = treeAdditionalAgreements.Selection.CountSelectedRows () > 0;
			buttonEdit.Sensitive = buttonDelete.Sensitive = selected;
		}

		void OnButtonAddClicked (AgreementType type)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;
			
			ITdiDialog dlg;
			switch (type) {
			case AgreementType.FreeRent:
				dlg = new AdditionalAgreementFreeRent (agreementOwner as CounterpartyContract);
				break;
			case AgreementType.NonfreeRent:
				dlg = new AdditionalAgreementNonFreeRent (agreementOwner as CounterpartyContract);
				break;
			case AgreementType.WaterSales:
				dlg = new AdditionalAgreementWater (agreementOwner as CounterpartyContract);
				break;
			case AgreementType.DailyRent:
				dlg = new AdditionalAgreementDailyRent (agreementOwner as CounterpartyContract);
				break;
			case AgreementType.Repair:
				if (additionalAgreements.Any (a => a.Type == AgreementType.Repair)) {
					MessageDialog md = new MessageDialog (null,
						                   DialogFlags.Modal,
						                   MessageType.Warning,
						                   ButtonsType.Ok,
						                   "Доп. соглашение на ремонт оборудования уже существует. " +
						                   "Нельзя создать более одного доп. соглашения данного типа.");
					md.SetPosition (WindowPosition.Center);
					md.ShowAll ();
					md.Run ();
					md.Destroy ();
					return;
				}
				dlg = new AdditionalAgreementRepair (agreementOwner as CounterpartyContract);
				break;
			default:
				throw new NotSupportedException (String.Format ("Тип {0} пока не поддерживается.", type));
			}
			mytab.TabParent.AddSlaveTab (mytab, dlg);
		}

		protected void OnButtonEditClicked (object sender, EventArgs e)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;

			if (treeAdditionalAgreements.GetSelectedObjects ().GetLength (0) > 0) {
				ITdiDialog dlg = OrmMain.CreateObjectDialog (treeAdditionalAgreements.GetSelectedObjects () [0]);
				mytab.TabParent.AddSlaveTab (mytab, dlg);
			}
		}

		protected void OnTreeAdditionalAgreementsRowActivated (object o, Gtk.RowActivatedArgs args)
		{
			buttonEdit.Click ();
		}

		protected void OnButtonDeleteClicked (object sender, EventArgs e)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;

			additionalAgreements.Remove (treeAdditionalAgreements.GetSelectedObjects () [0] as AdditionalAgreement);
		}

		protected void OnButtonAddEnumItemClicked (object sender, EnumItemClickedEventArgs e)
		{
			OnButtonAddClicked ((AgreementType)e.ItemEnum);
		}
	}
}

