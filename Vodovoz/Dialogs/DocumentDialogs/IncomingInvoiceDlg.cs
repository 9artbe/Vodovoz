﻿using System;
using NLog;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QS.Validation.GtkUI;
using Vodovoz.Additions.Store;
using Vodovoz.Core.Permissions;
using Vodovoz.Domain.Documents;
using Vodovoz.Domain.Permissions;
using Vodovoz.EntityRepositories;
using Vodovoz.EntityRepositories.Employees;
using Vodovoz.PermissionExtensions;
using Vodovoz.Repositories.HumanResources;
using Vodovoz.ViewModel;

namespace Vodovoz
{
	public partial class IncomingInvoiceDlg : QS.Dialog.Gtk.EntityDialogBase<IncomingInvoice>
	{
		static Logger logger = LogManager.GetCurrentClassLogger();

		public IncomingInvoiceDlg()
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<IncomingInvoice>();
			Entity.Author = EmployeeSingletonRepository.GetInstance().GetEmployeeForCurrentUser(UoW);
			if (Entity.Author == null)
			{
				MessageDialogHelper.RunErrorDialog("Ваш пользователь не привязан к действующему сотруднику, вы не можете создавать складские документы, так как некого указывать в качестве кладовщика.");
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

			referenceContractor.RepresentationModel = new CounterpartyVM(new CounterpartyFilter(UoW));
			referenceContractor.Binding.AddBinding(Entity, e => e.Contractor, w => w.Subject).InitializeFromSource();

			incominginvoiceitemsview1.DocumentUoW = UoWGeneric;
			ytextviewComment.Binding.AddBinding(Entity, e => e.Comment, w => w.Buffer.Text).InitializeFromSource();

			var permmissionValidator = new EntityExtendedPermissionValidator(PermissionExtensionSingletonStore.GetInstance(), EmployeeSingletonRepository.GetInstance(), UserSingletonRepository.GetInstance());
			Entity.CanEdit = permmissionValidator.Validate(typeof(IncomingInvoice), UserSingletonRepository.GetInstance().GetCurrentUser(UoW).Id, nameof(RetroactivelyClosePermission));
			if(!Entity.CanEdit && Entity.TimeStamp.Date != DateTime.Now.Date) {
				ytextviewComment.Binding.AddFuncBinding(Entity, e => e.CanEdit, w => w.Sensitive).InitializeFromSource();
				entryInvoiceNumber.Sensitive = false;
				entryWaybillNumber.Sensitive = false;
				referenceWarehouse.Sensitive = false;
				referenceContractor.Sensitive = false;
				incominginvoiceitemsview1.Sensitive = false;
				buttonSave.Sensitive = false;
			} else {
				Entity.CanEdit = true;
			}
		}

		public override bool Save ()
		{
			if(!Entity.CanEdit)
				return false;

			var valid = new QSValidator<IncomingInvoice> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			Entity.LastEditor = EmployeeSingletonRepository.GetInstance().GetEmployeeForCurrentUser (UoW);
			Entity.LastEditedTime = DateTime.Now;
			if(Entity.LastEditor == null)
			{
				MessageDialogHelper.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете изменять складские документы, так как некого указывать в качестве кладовщика.");
				return false;
			}

			logger.Info ("Сохраняем входящую накладную...");
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}
	}
}

