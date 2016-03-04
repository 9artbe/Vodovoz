﻿using System;
using QSOrmProject.RepresentationModel;
using QSOrmProject;
using Vodovoz.Domain.Service;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ServiceClaimFilter : Gtk.Bin, IRepresentationFilter
	{
		public ServiceClaimFilter (IUnitOfWork uow) : this ()
		{
			UoW = uow;
			
		}

		public ServiceClaimFilter ()
		{
			this.Build ();
		}

		#region IRepresentationFilter implementation

		public event EventHandler Refiltered;

		void OnRefiltered ()
		{
			if (Refiltered != null)
				Refiltered (this, new EventArgs ());
		}

		IUnitOfWork uow;

		public IUnitOfWork UoW {
			get {
				return uow;
			}
			set {
				uow = value;
				comboStatus.ItemsEnum = typeof(ServiceClaimStatus);
				comboType.ItemsEnum = typeof(ServiceClaimType);
			}
		}

		#endregion

		public ServiceClaimStatus? RestrictServiceClaimStatus {
			get { return comboStatus.SelectedItem as ServiceClaimStatus?; }
			set {
				comboStatus.SelectedItem = value;
				comboStatus.Sensitive = false;
			}
		}

		public ServiceClaimType? RestrictServiceClaimType {
			get { return comboType.SelectedItem as ServiceClaimType?; }
			set {
				comboType.SelectedItem = value;
				comboType.Sensitive = false;
			}
		}

		protected void OnComboTypeChanged(object sender, EventArgs e)
		{
			OnRefiltered ();
		}

		protected void OnComboStatusChanged(object sender, EventArgs e)
		{
			OnRefiltered ();
		}
	}
}

