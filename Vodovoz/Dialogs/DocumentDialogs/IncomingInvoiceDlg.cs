﻿using System;
using NLog;
using QSOrmProject;
using QSProjectsLib;
using QSValidation;
using Vodovoz.Additions.Store;
using Vodovoz.Core.Permissions;
using Vodovoz.Domain.Documents;
using Vodovoz.Domain.Store;

namespace Vodovoz
{
	public partial class IncomingInvoiceDlg : OrmGtkDialogBase<IncomingInvoice>
	{
		static Logger logger = LogManager.GetCurrentClassLogger();

		public IncomingInvoiceDlg()
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<IncomingInvoice>();
			Entity.Author = Repository.EmployeeRepository.GetEmployeeForCurrentUser(UoW);
			if (Entity.Author == null)
			{
				MessageDialogWorks.RunErrorDialog("Ваш пользователь не привязан к действующему сотруднику, вы не можете создавать складские документы, так как некого указывать в качестве кладовщика.");
				FailInitialize = true;
				return;
			}
			Entity.Warehouse = StoreDocumentHelper.GetDefaultWarehouse(UoW, WarehousePermissions.IncomingInvoiceEdit);

			ConfigureDlg();
		}

		public IncomingInvoiceDlg(int id)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<IncomingInvoice>(id);
			ConfigureDlg();
		}

		public IncomingInvoiceDlg(IncomingInvoice sub) : this(sub.Id)
		{
		}

		void ConfigureDlg()
		{
			if(StoreDocumentHelper.CheckAllPermissions(UoW.IsNew, WarehousePermissions.IncomingInvoiceEdit, Entity.Warehouse)) {
				FailInitialize = true;
				return;
			}

			var editing = StoreDocumentHelper.CanEditDocument(WarehousePermissions.IncomingInvoiceEdit, Entity.Warehouse);
			entryInvoiceNumber.IsEditable = entryWaybillNumber.IsEditable = ytextviewComment.Editable 
				= referenceContractor.IsEditable = referenceWarehouse.IsEditable = editing;
			incominginvoiceitemsview1.Sensitive = editing;

			entryInvoiceNumber.Binding.AddBinding(Entity, e => e.InvoiceNumber, w => w.Text).InitializeFromSource();
			entryWaybillNumber.Binding.AddBinding(Entity, e => e.WaybillNumber, w => w.Text).InitializeFromSource();
			labelTimeStamp.Binding.AddBinding(Entity, e => e.DateString, w => w.LabelProp).InitializeFromSource();

			referenceWarehouse.ItemsQuery = StoreDocumentHelper.GetRestrictedWarehouseQuery(WarehousePermissions.IncomingInvoiceEdit);
			referenceWarehouse.Binding.AddBinding(Entity, e => e.Warehouse, w => w.Subject).InitializeFromSource();

			referenceContractor.RepresentationModel = new ViewModel.CounterpartyVM(new CounterpartyFilter(UoW));
			referenceContractor.Binding.AddBinding(Entity, e => e.Contractor, w => w.Subject);

			incominginvoiceitemsview1.DocumentUoW = UoWGeneric;
			ytextviewComment.Binding.AddBinding(Entity, e => e.Comment, w => w.Buffer.Text).InitializeFromSource();
		}

		public override bool Save ()
		{
			var valid = new QSValidator<IncomingInvoice> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			Entity.LastEditor = Repository.EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			Entity.LastEditedTime = DateTime.Now;
			if(Entity.LastEditor == null)
			{
				MessageDialogWorks.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете изменять складские документы, так как некого указывать в качестве кладовщика.");
				return false;
			}

			logger.Info ("Сохраняем входящую накладную...");
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}
	}
}

