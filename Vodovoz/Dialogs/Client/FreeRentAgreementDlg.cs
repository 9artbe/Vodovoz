﻿using System;
using NLog;
using QS.Dialog;
using QS.DomainModel.UoW;
using QS.Project.Domain;
using QS.Project.Repositories;
using QS.Project.Services;
using QS.Validation.GtkUI;
using Vodovoz.DocTemplates;
using Vodovoz.Domain;
using Vodovoz.Domain.Client;
using Vodovoz.ViewModelBased;

namespace Vodovoz
{
	public partial class FreeRentAgreementDlg : QS.Dialog.Gtk.EntityDialogBase<FreeRentAgreement>, IAgreementSaved, IEditableDialog
	{
		public event EventHandler<AgreementSavedEventArgs> AgreementSaved;

		protected static Logger logger = LogManager.GetCurrentClassLogger ();

		bool isEditable = true;

		public bool IsEditable { 
			get { return isEditable; } 
			set {
				isEditable = value;
				buttonSave.Sensitive = 
					dateStart.Sensitive = freerentpackagesview1.IsEditable = value;
			} 
		}

		private FreeRentPackage FreeRentPackage { get; set; }

		public FreeRentAgreementDlg (CounterpartyContract contract)
		{
			this.Build ();
			UoWGeneric = FreeRentAgreement.Create (contract);
			ConfigureDlg ();
		}

		public FreeRentAgreementDlg (CounterpartyContract contract, DeliveryPoint point, DateTime? IssueDate)
		{
			this.Build();
			UoWGeneric = FreeRentAgreement.Create(contract);
			UoWGeneric.Root.DeliveryPoint = point;
			if(IssueDate.HasValue)
				UoWGeneric.Root.IssueDate = UoWGeneric.Root.StartDate = IssueDate.Value;
			ConfigureDlg();
		}

		public FreeRentAgreementDlg(CounterpartyContract contract, DeliveryPoint point, DateTime? IssueDate, FreeRentPackage freeRentPackage)
		{
			this.Build();
			UoWGeneric = FreeRentAgreement.Create(contract);
			UoWGeneric.Root.DeliveryPoint = point;
			if(IssueDate.HasValue)
				UoWGeneric.Root.IssueDate = UoWGeneric.Root.StartDate = IssueDate.Value;
			ConfigureDlg();
			FreeRentPackage = freeRentPackage;
			freerentpackagesview1.FreeRentPackage = FreeRentPackage;
			freerentpackagesview1.AddEquipment(FreeRentPackage);
		}

		public FreeRentAgreementDlg (FreeRentAgreement sub) : this (sub.Id)
		{
		}

		public FreeRentAgreementDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<FreeRentAgreement> (id);
			ConfigureDlg ();
		}

		public FreeRentAgreementDlg(IEntityUoWBuilder uowBuilder, IUnitOfWorkFactory unitOfWorkFactory)
		{
			this.Build();
			UoWGeneric = uowBuilder.CreateUoW<FreeRentAgreement>(unitOfWorkFactory);
			ConfigureDlg();
		}

		private void ConfigureDlg ()
		{
			freerentpackagesview1.IsEditable = true;
			dateIssue.Sensitive = dateStart.Sensitive = false;
			dateIssue.Binding.AddBinding (Entity, e => e.IssueDate, w => w.Date).InitializeFromSource();
			dateStart.Binding.AddBinding (Entity, e => e.StartDate, w => w.Date).InitializeFromSource ();
			referenceDeliveryPoint.Sensitive = false;
			referenceDeliveryPoint.RepresentationModel = new ViewModel.ClientDeliveryPointsVM (UoW, Entity.Contract.Counterparty);
			referenceDeliveryPoint.Binding.AddBinding (Entity, e => e.DeliveryPoint, w => w.Subject).InitializeFromSource ();
			ylabelNumber.Binding.AddBinding(Entity, e => e.FullNumberText, w => w.LabelProp).InitializeFromSource();
			freerentpackagesview1.AgreementUoW = UoWGeneric;

			if (Entity.DocumentTemplate == null && Entity.Contract != null)
				Entity.UpdateContractTemplate(UoW);

			if (Entity.DocumentTemplate != null)
				(Entity.DocumentTemplate.DocParser as FreeRentAgreementParser).RootObject = Entity;

			templatewidget1.CanRevertCommon = ServicesConfig.CommonServices.CurrentPermissionService.ValidatePresetPermission("can_set_common_additionalagreement");
			templatewidget1.Binding.AddBinding(Entity, e => e.DocumentTemplate, w => w.Template).InitializeFromSource();
			templatewidget1.Binding.AddBinding(Entity, e => e.ChangedTemplateFile, w => w.ChangedDoc).InitializeFromSource();
		}

		public override bool Save ()
		{
			var valid = new QSValidator<FreeRentAgreement> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем доп. соглашение...");
			UoWGeneric.Save ();
			logger.Info ("Ok");
			AgreementSaved?.Invoke(this, new AgreementSavedEventArgs(UoWGeneric.Root));
			return true;
		}
	}
}

