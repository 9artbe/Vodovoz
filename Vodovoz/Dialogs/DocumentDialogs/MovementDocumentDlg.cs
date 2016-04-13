﻿using System;
using System.Linq;
using NHibernate.Criterion;
using NLog;
using QSOrmProject;
using QSValidation;
using Vodovoz.Domain;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Documents;
using Vodovoz.Domain.Store;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class MovementDocumentDlg : OrmGtkDialogBase<MovementDocument>
	{
		static Logger logger = LogManager.GetCurrentClassLogger ();

		public MovementDocumentDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<MovementDocument> ();
			ConfigureDlg ();
			UoWGeneric.Root.ResponsiblePerson = Repository.EmployeeRepository.GetEmployeeForCurrentUser (UoW);
		}

		public MovementDocumentDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<MovementDocument> (id);
			ConfigureDlg ();
		}

		public MovementDocumentDlg (MovementDocument sub) : this (sub.Id)
		{
		}

		void ConfigureDlg ()
		{
			tableSender.DataSource = subjectAdaptor;
			tableCommon.DataSource = subjectAdaptor;
			tableReceiver.DataSource = subjectAdaptor;
			referenceCounterpartyTo.SubjectType = typeof(Counterparty);
			referenceCounterpartyFrom.SubjectType = typeof(Counterparty);
			referenceCounterpartyTo.ItemsCriteria = UoWGeneric.Session.CreateCriteria<Counterparty> ()
				.Add (Restrictions.Eq ("CounterpartyType", CounterpartyType.customer));
			referenceCounterpartyFrom.ItemsCriteria = UoWGeneric.Session.CreateCriteria<Counterparty> ()
				.Add (Restrictions.Eq ("CounterpartyType", CounterpartyType.customer));
			referenceWarehouseTo.SubjectType = typeof(Warehouse);
			referenceWarehouseFrom.SubjectType = typeof(Warehouse);
			referenceDeliveryPointTo.CanEditReference = false;
			referenceDeliveryPointTo.SubjectType = typeof(DeliveryPoint);
			referenceDeliveryPointFrom.CanEditReference = false;
			referenceDeliveryPointFrom.SubjectType = typeof(DeliveryPoint);
			referenceEmployee.SubjectType = typeof(Employee);

			enumMovementType.ItemsEnum = typeof(MovementDocumentCategory);
			enumMovementType.Binding.AddBinding(Entity, e => e.Category, w => w.SelectedItem).InitializeFromSource();

			movementdocumentitemsview1.DocumentUoW = UoWGeneric;
		}

		public override bool Save ()
		{
			var valid = new QSValidator<MovementDocument> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем документ перемещения...");
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}

		protected void OnEnumMovementTypeChanged (object sender, EventArgs e)
		{
			var selected = Entity.Category;
			referenceWarehouseTo.Visible = referenceWarehouseFrom.Visible = labelStockFrom.Visible = labelStockTo.Visible 
				= (selected == MovementDocumentCategory.warehouse);
			referenceCounterpartyTo.Visible = referenceCounterpartyFrom.Visible = labelClientFrom.Visible = labelClientTo.Visible
				= referenceDeliveryPointFrom.Visible = referenceDeliveryPointTo.Visible = labelPointFrom.Visible = labelPointTo.Visible
				= (selected == MovementDocumentCategory.counterparty);
			referenceDeliveryPointFrom.Sensitive = (referenceCounterpartyFrom.Subject != null && selected == MovementDocumentCategory.counterparty);
			referenceDeliveryPointTo.Sensitive = (referenceCounterpartyTo.Subject != null && selected == MovementDocumentCategory.counterparty);
		}

		protected void OnReferenceCounterpartyFromChanged (object sender, EventArgs e)
		{
			referenceDeliveryPointFrom.Sensitive = referenceCounterpartyFrom.Subject != null;
			if (referenceCounterpartyFrom.Subject != null) {
				var points = ((Counterparty)referenceCounterpartyFrom.Subject).DeliveryPoints.Select (o => o.Id).ToList ();
				referenceDeliveryPointFrom.ItemsCriteria = Session.CreateCriteria<DeliveryPoint> ()
					.Add (Restrictions.In ("Id", points));
			}
		}

		protected void OnReferenceCounterpartyToChanged (object sender, EventArgs e)
		{
			referenceDeliveryPointTo.Sensitive = referenceCounterpartyTo.Subject != null;
			if (referenceCounterpartyTo.Subject != null) {
				var points = ((Counterparty)referenceCounterpartyTo.Subject).DeliveryPoints.Select (o => o.Id).ToList ();
				referenceDeliveryPointTo.ItemsCriteria = Session.CreateCriteria<DeliveryPoint> ()
					.Add (Restrictions.In ("Id", points));
			}
		}
	}
}

