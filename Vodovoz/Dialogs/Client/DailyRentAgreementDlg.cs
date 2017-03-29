﻿using System;
using NLog;
using QSOrmProject;
using QSValidation;
using Vodovoz.Domain.Client;
using Vodovoz.DocTemplates;

namespace Vodovoz
{
	public partial class DailyRentAgreementDlg : OrmGtkDialogBase<DailyRentAgreement>, IAgreementSaved, IEditableDialog
	{
		public event EventHandler<AgreementSavedEventArgs> AgreementSaved;

		protected static Logger logger = LogManager.GetCurrentClassLogger ();

		bool isEditable = true;

		public bool IsEditable { 
			get { return isEditable; } 
			set {
				isEditable = value;
				buttonSave.Sensitive = 
					dateEnd.Sensitive = dateStart.Sensitive = 
						dailyrentpackagesview1.IsEditable = value;
			} 
		}

		public DailyRentAgreementDlg (CounterpartyContract contract)
		{
			this.Build ();
			UoWGeneric = DailyRentAgreement.Create (contract);
			ConfigureDlg ();
		}

		public DailyRentAgreementDlg (CounterpartyContract contract, DeliveryPoint point, DateTime? IssueDate) : this (contract)
		{
			UoWGeneric.Root.DeliveryPoint = point;
			if(IssueDate.HasValue)
				UoWGeneric.Root.IssueDate = UoWGeneric.Root.StartDate = IssueDate.Value;
		}

		public DailyRentAgreementDlg (DailyRentAgreement sub) : this (sub.Id)
		{
		}

		public DailyRentAgreementDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<DailyRentAgreement> (id);
			ConfigureDlg ();
		}

		private void ConfigureDlg ()
		{
			spinRentDays.Sensitive = false;
			dateIssue.Sensitive = dateStart.Sensitive = false;
			dateIssue.Binding.AddBinding (Entity, e => e.IssueDate, w => w.Date).InitializeFromSource ();
			dateStart.Binding.AddBinding (Entity, e => e.StartDate, w => w.Date).InitializeFromSource ();
			dateEnd.Binding.AddBinding (Entity, e => e.EndDate, w => w.Date).InitializeFromSource ();

			referenceDeliveryPoint.Sensitive = false;
			referenceDeliveryPoint.RepresentationModel = new ViewModel.ClientDeliveryPointsVM (UoW, Entity.Contract.Counterparty);
			referenceDeliveryPoint.Binding.AddBinding (Entity, e => e.DeliveryPoint, w => w.Subject).InitializeFromSource ();
			ylabelNumber.Binding.AddBinding(Entity, e => e.FullNumberText, w => w.LabelProp).InitializeFromSource();

			spinRentDays.Binding.AddBinding (Entity, e => e.RentDays, w => w.ValueAsInt).InitializeFromSource ();

			dailyrentpackagesview1.IsEditable = true;
			dailyrentpackagesview1.AgreementUoW = UoWGeneric;

			dateEnd.Date = UoWGeneric.Root.StartDate.AddDays (UoWGeneric.Root.RentDays);

			if (Entity.AgreementTemplate == null && Entity.Contract != null)
				Entity.UpdateContractTemplate(UoW);

			if (Entity.AgreementTemplate != null)
				(Entity.AgreementTemplate.DocParser as ShortRentAgreementParser).RootObject = Entity;
			templatewidget3.Binding.AddBinding(Entity, e => e.AgreementTemplate, w => w.Template).InitializeFromSource();
			templatewidget3.Binding.AddBinding(Entity, e => e.ChangedTemplateFile, w => w.ChangedDoc).InitializeFromSource();
		}

		public override bool Save ()
		{
			var valid = new QSValidator<DailyRentAgreement> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем доп. соглашение...");
			UoWGeneric.Save ();
			logger.Info ("Ok");
			if (AgreementSaved != null)
				AgreementSaved (this, new AgreementSavedEventArgs (UoWGeneric.Root));
			return true;
		}

		protected void OnSpinRentDaysValueChanged (object sender, EventArgs e)
		{
			dailyrentpackagesview1.UpdateTotalLabels ();
		}

		protected void OnDateStartDateChanged (object sender, EventArgs e)
		{
			RecalcRentPeriod ();
		}

		protected void OnDateEndDateChanged (object sender, EventArgs e)
		{
			RecalcRentPeriod ();
		}

		protected void RecalcRentPeriod ()
		{
			spinRentDays.Value = (dateEnd.Date.Date - dateStart.Date.Date).Days;
			dailyrentpackagesview1.UpdateTotalLabels ();
		}
	}
}

