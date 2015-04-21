﻿using System;
using System.Data.Bindings.Collections.Generic;
using System.Collections.Generic;
using QSOrmProject;
using NHibernate;
using QSTDI;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class CounterpartyContractsView : Gtk.Bin
	{
		private IContractOwner contractOwner;
		private GenericObservableList<CounterpartyContract> CounterpartyContracts;
		private ISession session;

		public ISession Session {
			get { return session; }
			set { session = value; }
		}

		public IContractOwner ContractOwner {
			get { return contractOwner; }
			set {
				contractOwner = value;
				if (ContractOwner.CounterpartyContracts == null)
					ContractOwner.CounterpartyContracts = new List<CounterpartyContract> ();
				CounterpartyContracts = new GenericObservableList<CounterpartyContract> (contractOwner.CounterpartyContracts);
				treeCounterpartyContracts.ItemsDataSource = CounterpartyContracts;
			}
		}

		OrmParentReference parentReference;

		public OrmParentReference ParentReference {
			set {
				parentReference = value;
				if (parentReference != null) {
					Session = parentReference.Session;
					if (!(parentReference.ParentObject is IContractOwner)) {
						throw new ArgumentException (String.Format ("Родительский объект в parentReference должен реализовывать интерфейс {0}", typeof(IContractOwner)));
					}
					contractOwner = (IContractOwner)parentReference.ParentObject;
				}
			}
			get { return parentReference; }
		}

		public CounterpartyContractsView ()
		{
			this.Build ();
			treeCounterpartyContracts.Selection.Changed += OnSelectionChanged;
		}

		void OnSelectionChanged (object sender, EventArgs e)
		{
			bool selected = treeCounterpartyContracts.Selection.CountSelectedRows () > 0;
			buttonEdit.Sensitive = buttonDelete.Sensitive = selected;
		}

		void OnButtonAddClicked (object sender, EventArgs e)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;

			CounterpartyContract contract = new CounterpartyContract ();
			contract.IsNew = true;
			CounterpartyContracts.Add (contract);

			ITdiDialog dlg = new CounterpartyContractDlg (ParentReference, contract);
			mytab.TabParent.AddSlaveTab (mytab, dlg);
		}

		protected void OnButtonEditClicked (object sender, EventArgs e)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;

			ITdiDialog dlg = OrmMain.CreateObjectDialog (ParentReference, treeCounterpartyContracts.GetSelectedObjects () [0]);
			mytab.TabParent.AddSlaveTab (mytab, dlg);
		}

		protected void OnTreeCounterpartyContractsRowActivated (object o, Gtk.RowActivatedArgs args)
		{
			buttonEdit.Click ();
		}

		protected void OnButtonDeleteClicked (object sender, EventArgs e)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;

			CounterpartyContracts.Remove (treeCounterpartyContracts.GetSelectedObjects () [0] as CounterpartyContract);
		}
	}
}

