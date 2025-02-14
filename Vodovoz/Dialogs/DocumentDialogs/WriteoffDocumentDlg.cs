﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QSOrmProject;
using QS.Validation;
using Vodovoz.Additions.Store;
using Vodovoz.Infrastructure.Permissions;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Documents;
using Vodovoz.ViewModel;
using Vodovoz.EntityRepositories.Employees;
using Vodovoz.PermissionExtensions;
using QS.DomainModel.Entity.EntityPermissions.EntityExtendedPermission;
using QS.Project.Services;

namespace Vodovoz
{
	public partial class WriteoffDocumentDlg : QS.Dialog.Gtk.EntityDialogBase<WriteoffDocument>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		private readonly IEmployeeRepository _employeeRepository = new EmployeeRepository();

		public WriteoffDocumentDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<WriteoffDocument>();
			Entity.Author = Entity.ResponsibleEmployee = _employeeRepository.GetEmployeeForCurrentUser(UoW);
			if(Entity.Author == null)
			{
				MessageDialogHelper.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете создавать складские документы, так как некого указывать в качестве кладовщика.");
				FailInitialize = true;
				return;
			}

			Entity.WriteoffWarehouse = StoreDocumentHelper.GetDefaultWarehouse(UoW, WarehousePermissions.WriteoffEdit);
			
			ConfigureDlg ();
		}

		public WriteoffDocumentDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<WriteoffDocument> (id);
			comboType.Sensitive = false;
			ConfigureDlg ();
		}

		public WriteoffDocumentDlg (WriteoffDocument sub) : this (sub.Id)
		{
		}

		void ConfigureDlg ()
		{
			if(StoreDocumentHelper.CheckAllPermissions(UoW.IsNew, WarehousePermissions.WriteoffEdit, Entity.WriteoffWarehouse)) {
				FailInitialize = true;
				return;
			}

			var editing = StoreDocumentHelper.CanEditDocument(WarehousePermissions.WriteoffEdit, Entity.WriteoffWarehouse);
			repEntryEmployee.IsEditable = textComment.Editable = editing;
			writeoffdocumentitemsview1.Sensitive = editing && (Entity.WriteoffWarehouse != null || Entity.Client != null);

			textComment.Binding.AddBinding (Entity, e => e.Comment, w => w.Buffer.Text).InitializeFromSource ();
			labelTimeStamp.Binding.AddBinding (Entity, e => e.DateString, w => w.LabelProp).InitializeFromSource ();

			referenceCounterparty.RepresentationModel = new ViewModel.CounterpartyVM(new CounterpartyFilter(UoW));
			referenceCounterparty.Binding.AddBinding(Entity, e => e.Client, w => w.Subject).InitializeFromSource();

			ySpecCmbWarehouses.ItemsList = StoreDocumentHelper.GetRestrictedWarehousesList(UoW, WarehousePermissions.WriteoffEdit);
			ySpecCmbWarehouses.Binding.AddBinding (Entity, e => e.WriteoffWarehouse, w => w.SelectedItem).InitializeFromSource ();
			ySpecCmbWarehouses.ItemSelected += (sender, e) => {
				writeoffdocumentitemsview1.Sensitive = editing && (Entity.WriteoffWarehouse != null || Entity.Client != null);
			};

			referenceDeliveryPoint.SubjectType = typeof(DeliveryPoint);
			referenceDeliveryPoint.CanEditReference = false;
			referenceDeliveryPoint.Binding.AddBinding (Entity, e => e.DeliveryPoint, w => w.Subject).InitializeFromSource ();
			
			var userHasOnlyAccessToWarehouseAndComplaints =
				ServicesConfig.CommonServices.CurrentPermissionService.ValidatePresetPermission("user_have_access_only_to_warehouse_and_complaints")
				&& !ServicesConfig.CommonServices.UserService.GetCurrentUser(UoW).IsAdmin;

			if(userHasOnlyAccessToWarehouseAndComplaints)
			{
				repEntryEmployee.CanEditReference = false;
			}
			
			repEntryEmployee.RepresentationModel = new EmployeesVM();
			repEntryEmployee.Binding.AddBinding (Entity, e => e.ResponsibleEmployee, w => w.Subject).InitializeFromSource ();
			comboType.ItemsEnum = typeof(WriteoffType);
			referenceDeliveryPoint.Sensitive = referenceCounterparty.Sensitive = (UoWGeneric.Root.Client != null);
			comboType.EnumItemSelected += (object sender, Gamma.Widgets.ItemSelectedEventArgs e) => {
				ySpecCmbWarehouses.Sensitive = WriteoffType.warehouse.Equals(comboType.SelectedItem);
				referenceDeliveryPoint.Sensitive = WriteoffType.counterparty.Equals(comboType.SelectedItem) && UoWGeneric.Root.Client != null;
				referenceCounterparty.Sensitive = WriteoffType.counterparty.Equals(comboType.SelectedItem);
			};
			//FIXME Списание с контрагента не реализовано. Поэтому блокирует выбор типа списания.
			comboType.Sensitive = false;
			comboType.SelectedItem = UoWGeneric.Root.Client != null ?
				WriteoffType.counterparty :
				WriteoffType.warehouse;

			writeoffdocumentitemsview1.DocumentUoW = UoWGeneric;

			Entity.ObservableItems.ElementAdded += (aList, aIdx) => ySpecCmbWarehouses.Sensitive = !Entity.ObservableItems.Any();
			Entity.ObservableItems.ElementRemoved += (aList, aIdx, aObject) => ySpecCmbWarehouses.Sensitive = !Entity.ObservableItems.Any();
			ySpecCmbWarehouses.Sensitive = editing && !Entity.Items.Any();

			var permmissionValidator =
				new EntityExtendedPermissionValidator(PermissionExtensionSingletonStore.GetInstance(), _employeeRepository);
			
			Entity.CanEdit =
				permmissionValidator.Validate(
					typeof(WriteoffDocument), ServicesConfig.UserService.CurrentUserId, nameof(RetroactivelyClosePermission));
			
			if(!Entity.CanEdit && Entity.TimeStamp.Date != DateTime.Now.Date) {
				ySpecCmbWarehouses.Binding.AddFuncBinding(Entity, e => e.CanEdit, w => w.Sensitive).InitializeFromSource();
				referenceCounterparty.Sensitive = false;
				referenceDeliveryPoint.Sensitive = false;
				comboType.Sensitive = false;
				repEntryEmployee.Sensitive = false;
				textComment.Sensitive = false;
				writeoffdocumentitemsview1.Sensitive = false;

				buttonSave.Sensitive = false;
			} else {
				Entity.CanEdit = true;
			}
		}

		public override bool Save ()
		{
			if(!Entity.CanEdit)
				return false;

			var valid = new QSValidator<WriteoffDocument>(UoWGeneric.Root);
			if (valid.RunDlgIfNotValid((Gtk.Window)this.Toplevel))
				return false;

			Entity.LastEditor = _employeeRepository.GetEmployeeForCurrentUser(UoW);
			Entity.LastEditedTime = DateTime.Now;
			if(Entity.LastEditor == null)
			{
				MessageDialogHelper.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете изменять складские документы, так как некого указывать в качестве кладовщика.");
				return false;
			}

			logger.Info ("Сохраняем акт списания...");
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}

		protected void OnReferenceCounterpartyChanged (object sender, EventArgs e)
		{
			referenceDeliveryPoint.Sensitive = referenceCounterparty.Subject != null;
			if (referenceCounterparty.Subject != null) {
				var points = ((Counterparty)referenceCounterparty.Subject).DeliveryPoints.Select (o => o.Id).ToList ();
				referenceDeliveryPoint.ItemsCriteria = UoW.Session.CreateCriteria<DeliveryPoint> ()
					.Add (Restrictions.In ("Id", points));
			}
		}

		protected void OnButtonPrintClicked(object sender, EventArgs e)
		{
			if (UoWGeneric.HasChanges && CommonDialogs.SaveBeforePrint (typeof(WriteoffDocument), "акта выбраковки"))
				Save ();

			var reportInfo = new QS.Report.ReportInfo {
				Title = String.Format ("Акт выбраковки №{0} от {1:d}", Entity.Id, Entity.TimeStamp),
				Identifier = "Store.WriteOff",
				Parameters = new Dictionary<string, object> {
					{ "writeoff_id",  Entity.Id }
				}
			};

			TabParent.OpenTab(
				QSReport.ReportViewDlg.GenerateHashName(reportInfo),
				() => new QSReport.ReportViewDlg (reportInfo)
			);
		}
	}
}

