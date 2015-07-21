﻿using System;
using QSOrmProject;
using QSOrmProject.RepresentationModel;
using Vodovoz.Domain;

namespace Vodovoz
{
	[OrmDefaultIsFiltered (false)]
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ClientBalanceFilter : Gtk.Bin, IRepresentationFilter
	{
		IUnitOfWork uow;

		public IUnitOfWork UoW {
			get {
				return uow;
			}
			set {
				uow = value;
				entryreferenceClient.RepresentationModel = new ViewModel.CounterpartyVM (uow);
			}
		}

		public ClientBalanceFilter (IUnitOfWork uow) : this()
		{
			UoW = uow;
		}

		public ClientBalanceFilter ()
		{
			this.Build ();
			IsFiltred = false;
		}

		#region IReferenceFilter implementation

		public event EventHandler Refiltered;

		void OnRefiltered ()
		{
			if (Refiltered != null)
				Refiltered (this, new EventArgs ());
		}

		public bool IsFiltred { get; private set; }

		#endregion

		void UpdateCreteria ()
		{
			OnRefiltered ();
		}

		public Counterparty RestrictCounterparty {
			get { return entryreferenceClient.Subject as Counterparty;}
			set { entryreferenceClient.Subject = value;
				entryreferenceClient.Sensitive = false;
			}
		
		}

		public DeliveryPoint RestrictDeliveryPoint {
			get { return entryreferencePoint.Subject as DeliveryPoint;}
			set { entryreferencePoint.Subject = value;
				entryreferencePoint.Sensitive = false;
			}

		}

		protected void OnSpeccomboStockItemSelected (object sender, EnumItemClickedEventArgs e)
		{
			OnRefiltered ();
		}

		protected void OnEntryreferenceClientChanged (object sender, EventArgs e)
		{
			entryreferencePoint.Sensitive = RestrictCounterparty != null;
			if (RestrictCounterparty == null)
				entryreferencePoint.Subject = null;
			else
			{
				entryreferencePoint.Subject = null;
				entryreferencePoint.RepresentationModel = new ViewModel.DeliveryPointsVM (UoW, RestrictCounterparty);
			}
			OnRefiltered ();
		}

		protected void OnEntryreferencePointChanged (object sender, EventArgs e)
		{
			OnRefiltered ();
		}
	}
}

