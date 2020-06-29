﻿using System;
using System.Linq;
using NLog;
using QS.Dialog;
using QS.DomainModel.UoW;
using QS.Project.Domain;
using QS.Project.Repositories;
using QS.Validation;
using Vodovoz.DocTemplates;
using Vodovoz.Domain;
using Vodovoz.Domain.Client;
using Vodovoz.ViewModelBased;
using QS.Project.Services;
using Vodovoz.Infrastructure.Converters;

namespace Vodovoz
{
	public partial class NonFreeRentAgreementDlg : QS.Dialog.Gtk.EntityDialogBase<NonfreeRentAgreement>, IAgreementSaved, IEditableDialog
	{
		public event EventHandler<AgreementSavedEventArgs> AgreementSaved;

		protected static Logger logger = LogManager.GetCurrentClassLogger ();

		bool isEditable = true;

		public bool IsEditable { 
			get { return isEditable; } 
			set {
				isEditable = value;
				buttonSave.Sensitive = 
					dateStart.Sensitive = paidrentpackagesview1.IsEditable = value;
			} 
		}

		private PaidRentPackage PaidRentPackage { get; set; }

		public NonFreeRentAgreementDlg (CounterpartyContract contract)
		{
			this.Build ();
			UoWGeneric = NonfreeRentAgreement.Create (contract);
			ConfigureDlg ();
		}

		public NonFreeRentAgreementDlg (CounterpartyContract contract, DeliveryPoint point, DateTime? IssueDate)// : this (contract)
		{
			this.Build();
			UoWGeneric = NonfreeRentAgreement.Create(contract);
			UoWGeneric.Root.DeliveryPoint = point;
			if(IssueDate.HasValue)
				UoWGeneric.Root.IssueDate = UoWGeneric.Root.StartDate = IssueDate.Value;
			ConfigureDlg();
		}

		/// <summary>
		/// Создание диалога долгосрочной аренды, с заранее выбранной услугой платной аренды.
		/// </summary>
		public NonFreeRentAgreementDlg(CounterpartyContract contract, DeliveryPoint point, DateTime? IssueDate, PaidRentPackage paidRentPackage)// : this (contract)
		{
			this.Build();
			UoWGeneric = NonfreeRentAgreement.Create(contract);
			UoWGeneric.Root.DeliveryPoint = point;
			if(IssueDate.HasValue)
				UoWGeneric.Root.IssueDate = UoWGeneric.Root.StartDate = IssueDate.Value;
			ConfigureDlg();
			PaidRentPackage = paidRentPackage;
			paidrentpackagesview1.PaidRentPackage = PaidRentPackage;
			paidrentpackagesview1.AddEquipment(PaidRentPackage);
		}

		public NonFreeRentAgreementDlg (NonfreeRentAgreement sub) : this (sub.Id)
		{
		}

		public NonFreeRentAgreementDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<NonfreeRentAgreement> (id);
			ConfigureDlg ();
		}

		public NonFreeRentAgreementDlg(IEntityUoWBuilder uowBuilder, IUnitOfWorkFactory unitOfWorkFactory)
		{
			this.Build();
			UoWGeneric = uowBuilder.CreateUoW<NonfreeRentAgreement>(unitOfWorkFactory);
			ConfigureDlg();
		}

		private void ConfigureDlg ()
		{
			dateIssue.Sensitive = dateStart.Sensitive = false;
			dateIssue.Binding.AddBinding (Entity, e => e.IssueDate, w => w.Date).InitializeFromSource ();
			dateStart.Binding.AddBinding (Entity, e => e.StartDate, w => w.Date).InitializeFromSource ();

			paidrentpackagesview1.IsEditable = true;
			referenceDeliveryPoint.Sensitive = false;
			referenceDeliveryPoint.RepresentationModel = new ViewModel.ClientDeliveryPointsVM (UoW, Entity.Contract.Counterparty);
			referenceDeliveryPoint.Binding.AddBinding (Entity, e => e.DeliveryPoint, w => w.Subject).InitializeFromSource ();
			ylabelNumber.Binding.AddBinding(Entity, e => e.FullNumberText, w => w.LabelProp).InitializeFromSource();

			entryMonths.ValidationMode = QSWidgetLib.ValidationType.numeric;
			entryMonths.Binding.AddBinding(Entity, e => e.RentMonths, w => w.Text, new IntToStringConverter()).InitializeFromSource();

			paidrentpackagesview1.AgreementUoW = UoWGeneric;
			if (Entity.DocumentTemplate == null && Entity.Contract != null)
				Entity.UpdateContractTemplate(UoW);

			if(Entity.DocumentTemplate != null) {
				(Entity.DocumentTemplate.DocParser as NonFreeRentAgreementParser).RootObject = Entity;
				(Entity.DocumentTemplate.DocParser as NonFreeRentAgreementParser).AddTableNomenclatures(Entity.PaidRentEquipments.ToList());
				(Entity.DocumentTemplate.DocParser as NonFreeRentAgreementParser).AddTableEquipmentTypes(Entity.PaidRentEquipments.ToList());
			}

			templatewidget1.CanRevertCommon = ServicesConfig.CommonServices.CurrentPermissionService.ValidatePresetPermission("can_set_common_additionalagreement");
			templatewidget1.Binding.AddBinding(Entity, e => e.DocumentTemplate, w => w.Template).InitializeFromSource();
			templatewidget1.Binding.AddBinding(Entity, e => e.ChangedTemplateFile, w => w.ChangedDoc).InitializeFromSource();
		}

		public override bool Save ()
		{
			var valid = new QSValidator<NonfreeRentAgreement> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем доп. соглашение...");
			UoWGeneric.Save ();
			logger.Info ("Ok");
			AgreementSaved?.Invoke(this, new AgreementSavedEventArgs(UoWGeneric.Root));
			return true;
		}

		protected void OnEntryMonthsChanged(object sender, EventArgs e)
		{
			if(Int32.TryParse(entryMonths.Text, out int result))
				Entity.RentMonths = result;
		}
	}
}

