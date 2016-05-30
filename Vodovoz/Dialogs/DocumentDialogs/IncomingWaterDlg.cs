﻿using System;
using QSOrmProject;
using QSProjectsLib;
using QSValidation;
using Vodovoz.Domain.Documents;
using Vodovoz.Domain.Store;
using Vodovoz.Domain.Goods;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class IncomingWaterDlg : OrmGtkDialogBase<IncomingWater>
	{
		static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();

		public IncomingWaterDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<IncomingWater> ();
			Entity.Author = Repository.EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			if(Entity.Author == null)
			{
				MessageDialogWorks.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете создавать складские документы, так как некого указывать в качестве кладовщика.");
				FailInitialize = true;
				return;
			}
			ConfigureDlg ();
		}

		public IncomingWaterDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<IncomingWater> (id);
			ConfigureDlg ();
		}

		public IncomingWaterDlg (IncomingWater sub) : this (sub.Id)
		{
		}

		void ConfigureDlg ()
		{
			tableWater.DataSource = subjectAdaptor;
			referenceProduct.SubjectType = typeof(Nomenclature);
			referenceSrcWarehouse.SubjectType = typeof(Warehouse);
			referenceDstWarehouse.SubjectType = typeof(Warehouse);
			incomingwatermaterialview1.DocumentUoW = UoWGeneric;
		}

		public override bool Save ()
		{
			var valid = new QSValidator<IncomingWater> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			Entity.LastEditor = Repository.EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			Entity.LastEditedTime = DateTime.Now;
			if(Entity.LastEditor == null)
			{
				MessageDialogWorks.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете изменять складские документы, так как некого указывать в качестве кладовщика.");
				return false;
			}

			logger.Info ("Сохраняем документ производства...");
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}

		protected void OnButtonFillClicked (object sender, EventArgs e)
		{
			OrmReference SelectDialog = new OrmReference (typeof(ProductSpecification), UoWGeneric);
			SelectDialog.Mode = OrmReferenceMode.Select;
			SelectDialog.ObjectSelected += SelectDialog_ObjectSelected;

			TabParent.AddSlaveTab (this, SelectDialog);
		}

		void SelectDialog_ObjectSelected (object sender, OrmReferenceObjectSectedEventArgs e)
		{
			var spec = e.Subject as ProductSpecification;
			UoWGeneric.Root.Product = spec.Product;
			UoWGeneric.Root.ObservableMaterials.Clear ();
			foreach (var material in spec.Materials) {
				UoWGeneric.Root.AddMaterial (material);
			}
		}
	}
}

