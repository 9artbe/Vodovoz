﻿using System;
using QSOrmProject;
using Vodovoz.Domain.Service;
using NLog;
using Vodovoz.Domain.Orders;
using QSValidation;
using Vodovoz.Domain;
using Vodovoz.Repository;
using System.Linq;
using QSTDI;
using QSProjectsLib;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ServiceClaimDlg : OrmGtkDialogBase<ServiceClaim>
	{
		protected static Logger logger = LogManager.GetCurrentClassLogger ();

		public ServiceClaimDlg (Order order)
		{
			this.Build ();
			UoWGeneric = ServiceClaim.Create (order);
			ConfigureDlg ();
		}

		public ServiceClaimDlg (ServiceClaim sub) : this (sub.Id)
		{
		}

		public ServiceClaimDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<ServiceClaim> (id);
			ConfigureDlg ();
		}

		void ConfigureDlg ()
		{
			subjectAdaptor.Target = UoWGeneric.Root;

			datatable1.DataSource = subjectAdaptor;
			enumPaymentType.DataSource = subjectAdaptor;
			enumStatus.DataSource = subjectAdaptor;

			referenceCounterparty.SubjectType = typeof(Counterparty);
			referenceDeliveryPoint.SubjectType = typeof(DeliveryPoint);
			referenceEngineer.SubjectType = typeof(Employee);
			referenceEquipment.SubjectType = typeof(Equipment);
			referenceNomenclature.SubjectType = typeof(Nomenclature);

			referenceDeliveryPoint.Sensitive = (UoWGeneric.Root.Counterparty != null);
			referenceEquipment.Sensitive = (UoWGeneric.Root.Nomenclature != null);

			referenceNomenclature.ItemsQuery = NomenclatureRepository.NomenclatureOfItemsForService ();
		}

		#region implemented abstract members of OrmGtkDialogBase

		public override bool Save ()
		{
			var valid = new QSValidator<ServiceClaim> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			CounterpartyContract contract = CounterpartyContractRepository.GetCounterpartyContractByPaymentType 
				(UoW, UoWGeneric.Root.Counterparty, UoWGeneric.Root.Payment);

			if (contract == null) {
				RunContractCreateDialog ();
				return false;
			}

			if (!contract.RepairAgreementExists ()) {
				RunAgreementCreateDialog (contract);
				return false;
			}

			if (UoWGeneric.Root.InitialOrder != null) {
				if (UoWGeneric.Root.InitialOrder.ObservableOrderEquipments.FirstOrDefault (eq => eq.Equipment.Id == UoWGeneric.Root.Equipment.Id) == null) {
					UoWGeneric.Root.InitialOrder.ObservableOrderEquipments.Add (new OrderEquipment { 
						Direction = Vodovoz.Domain.Orders.Direction.PickUp,
						Equipment = UoWGeneric.Root.Equipment,
						OrderItem = null,
						Reason = Reason.Service
					});
				}
			}

			if (UoWGeneric.Root.FinalOrder != null) {
				if (UoWGeneric.Root.FinalOrder.ObservableOrderEquipments.FirstOrDefault (eq => eq.Equipment.Id == UoWGeneric.Root.Equipment.Id) == null) {
					UoWGeneric.Root.FinalOrder.ObservableOrderEquipments.Add (new OrderEquipment { 
						Direction = Vodovoz.Domain.Orders.Direction.Deliver,
						Equipment = UoWGeneric.Root.Equipment,
						OrderItem = null,
						Reason = Reason.Service
					});
				}

				//TODO FIXME Добавить строку сервиса OrderItems
			}

			//TODO FIXME Добавление в закрывающий заказ.

			logger.Info ("Сохраняем заявку на обслуживание...");
			UoWGeneric.Save ();
			logger.Info ("Ok");
			return true;
		}

		#endregion

		protected void OnReferenceNomenclatureChanged (object sender, EventArgs e)
		{
			referenceEquipment.Sensitive = (UoWGeneric.Root.Nomenclature != null);

			if (UoWGeneric.Root.Equipment != null &&
			    UoWGeneric.Root.Equipment.Nomenclature.Id != UoWGeneric.Root.Nomenclature.Id) {
			
				UoWGeneric.Root.Equipment = null;
			}
			referenceEquipment.ItemsQuery = EquipmentRepository.GetEquipmentByNomenclature (UoWGeneric.Root.Nomenclature);
		}

		protected void OnReferenceCounterpartyChanged (object sender, EventArgs e)
		{
			referenceDeliveryPoint.Sensitive = (UoWGeneric.Root.Counterparty != null);
				
			if (UoWGeneric.Root.DeliveryPoint != null &&
			    UoWGeneric.Root.DeliveryPoint.Counterparty.Id != UoWGeneric.Root.Counterparty.Id) {

				UoWGeneric.Root.DeliveryPoint = null;
			}
			referenceDeliveryPoint.ItemsQuery = DeliveryPointRepository.DeliveryPointsForCounterpartyQuery (UoWGeneric.Root.Counterparty);
		}

		void RunContractCreateDialog ()
		{
			ITdiTab dlg;
			string question = "Отсутствует договор с клиентом для " +
			                  (UoWGeneric.Root.Payment == PaymentType.cash ? "наличной" : "безналичной") +
			                  " формы оплаты. Создать?";
			if (MessageDialogWorks.RunQuestionDialog (question)) {
				dlg = new CounterpartyContractDlg (UoWGeneric.Root.Counterparty, 
					(UoWGeneric.Root.Payment == PaymentType.cash ?
							OrganizationRepository.GetCashOrganization (UoWGeneric) :
							OrganizationRepository.GetCashlessOrganization (UoWGeneric)));
				(dlg as IContractSaved).ContractSaved += (sender, e) => {
					if (UoWGeneric.Root.InitialOrder != null)
						UoWGeneric.Root.InitialOrder.ObservableOrderDocuments.Add (new OrderContract { 
							Order = UoWGeneric.Root.InitialOrder,
							Contract = e.Contract
						});
				};
				TabParent.AddSlaveTab (this, dlg);
			}
		}

		void RunAgreementCreateDialog (CounterpartyContract contract)
		{
			ITdiTab dlg;
			string question = "Отсутствует доп. соглашение сервиса с клиентом в договоре для " +
			                  (UoWGeneric.Root.Payment == PaymentType.cash ? "наличной" : "безналичной") +
			                  " формы оплаты. Создать?";
			if (MessageDialogWorks.RunQuestionDialog (question)) {
				dlg = new AdditionalAgreementRepair (contract);
				(dlg as IAgreementSaved).AgreementSaved += (sender, e) => {
					if (UoWGeneric.Root.InitialOrder != null)
						UoWGeneric.Root.InitialOrder.ObservableOrderDocuments.Add (new OrderAgreement { 
							Order = UoWGeneric.Root.InitialOrder,
							AdditionalAgreement = e.Agreement
						});
				};
				TabParent.AddSlaveTab (this, dlg);
			}
		}
	}
}

