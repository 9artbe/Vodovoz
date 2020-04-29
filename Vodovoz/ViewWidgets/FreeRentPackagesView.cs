﻿using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using Gtk;
using NLog;
using QS.DomainModel.UoW;
using QS.Project.Dialogs;
using QS.Project.Dialogs.GtkUI;
using QSOrmProject;
using QSProjectsLib;
using Vodovoz.Domain;
using Vodovoz.Domain.Client;
using Vodovoz.Repositories;
using Vodovoz.Representations;
using Vodovoz.TempAdapters;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class FreeRentPackagesView : QS.Dialog.Gtk.WidgetOnDialogBase
	{
		static Logger logger = LogManager.GetCurrentClassLogger ();

		private GenericObservableList<FreeRentEquipment> freeRentEquipments;

		Decimal TotalDeposit = 0;

		int TotalWaterAmount = 0;

		private IUnitOfWorkGeneric<FreeRentAgreement> agreementUoW;

		bool isEditable = true;

		public bool IsEditable { 
			get { return isEditable; } 
			set {
				isEditable = value;
				buttonAdd.Sensitive = buttonDelete.Sensitive = treeRentPackages.Sensitive = value;
			} 
		}

		public IUnitOfWorkGeneric<FreeRentAgreement> AgreementUoW {
			get { return agreementUoW; }
			set {
				if (agreementUoW == value)
					return;
				agreementUoW = value;
				if (AgreementUoW.Root.Equipment == null)
					AgreementUoW.Root.Equipment = new List<FreeRentEquipment> ();
				freeRentEquipments = AgreementUoW.Root.ObservableEquipment;
				freeRentEquipments.ElementChanged += Equipment_ElementChanged; 
				treeRentPackages.ItemsDataSource = freeRentEquipments;
				UpdateTotalLabels ();
			}
		}

		public FreeRentPackage FreeRentPackage { get; set; }

		void Equipment_ElementChanged (object aList, int[] aIdx)
		{
			UpdateTotalLabels ();
		}

		void UpdateTotalLabels ()
		{
			TotalDeposit = TotalWaterAmount = 0;
			if (freeRentEquipments != null)
				foreach (FreeRentEquipment eq in freeRentEquipments) {
					TotalDeposit += eq.Deposit;
					TotalWaterAmount += eq.WaterAmount;
				}
			if (AgreementUoW != null) {
				labelTotalWaterAmount.Text = String.Format ("{0} " + RusNumber.Case (TotalWaterAmount, "бутыль", "бутыли", "бутылей"), TotalWaterAmount);
				labelTotalDeposit.Text = CurrencyWorks.GetShortCurrencyString (TotalDeposit);
			}
		}

		public FreeRentPackagesView ()
		{
			this.Build ();

			treeRentPackages.ColumnsConfig = ColumnsConfigFactory.Create<FreeRentEquipment>()
				.AddColumn("Пакет").AddTextRenderer(x => x.PackageName)
				.AddColumn("Оборудование").AddTextRenderer(x => x.EquipmentName)
				.AddColumn("Количество").AddNumericRenderer(x => x.Count)
				.Adjustment(new Adjustment(0, 0, 1000000, 1, 100, 0)).Editing(true)
				.AddColumn("Сумма залога").AddTextRenderer(x => x.DepositString)
				.AddColumn("Минимальное количество воды").AddTextRenderer(x => x.WaterAmountString)
				.Finish();
			
			treeRentPackages.Selection.Changed += OnSelectionChanged;
			UpdateTotalLabels ();
		}

		void OnSelectionChanged (object sender, EventArgs e)
		{
			bool selected = treeRentPackages.Selection.CountSelectedRows () > 0;
			buttonDelete.Sensitive = selected;
		}

		protected void OnButtonAddClicked (object sender, EventArgs e)
		{
			if(FreeRentPackage == null) {
				OrmReference refWin = new OrmReference(typeof(FreeRentPackage));
				refWin.Mode = OrmReferenceMode.Select;
				refWin.ObjectSelected += (innerSender, ee) => {
					AddEquipmentManually((ee.Subject as FreeRentPackage));
				};
				MyTab.TabParent.AddSlaveTab(MyTab, refWin);
			} else {
				AddEquipmentManually(FreeRentPackage);
			}
		}

		private void AddEquipmentManually(FreeRentPackage freeRentPackage)
		{
			PermissionControlledRepresentationJournal SelectDialog = new PermissionControlledRepresentationJournal(new EquipmentsNonSerialForRentVM(AgreementUoW, freeRentPackage.EquipmentType));
			SelectDialog.Mode = JournalSelectMode.Single;
			SelectDialog.CustomTabName("Оборудование для аренды");
			SelectDialog.ObjectSelected += EquipmentSelected;
			MyTab.TabParent.AddSlaveTab(MyTab, SelectDialog);
		}

		void EquipmentSelected (object sender, JournalObjectSelectedEventArgs e)
		{
			var selectedNode = e.GetNodes<EquipmentRepositoryForViews.NomenclatureForRentVMNode>().FirstOrDefault();
			if(selectedNode == null) {
				return;
			}

			var rentPackage = RentPackageRepository.GetFreeRentPackage(AgreementUoW, selectedNode.Type);
			if (rentPackage == null)
			{
				MessageDialogWorks.RunErrorDialog("Для выбранного типа оборудования нет пакета бесплатной аренды.");
				return;
			}

			if(selectedNode.Available == 0) {
				if(!MessageDialogWorks.RunQuestionDialog("Не найдено свободного оборудования выбранного типа!\nДобавить принудительно?")) {
					return;
				}
			}

			FreeRentEquipment eq = new FreeRentEquipment ();
			eq.Equipment = null;
			eq.Nomenclature = selectedNode.Nomenclature;
			eq.Deposit = rentPackage.Deposit;
			eq.FreeRentPackage = rentPackage;
			eq.Count = 1;
			eq.WaterAmount = rentPackage.MinWaterAmount;
			freeRentEquipments.Add (eq);
			UpdateTotalLabels ();
		}

		protected void OnButtonDeleteClicked (object sender, EventArgs e)
		{
			var selectedObjects = treeRentPackages.GetSelectedObjects();
			if(selectedObjects.Length == 1) {
				freeRentEquipments.Remove(selectedObjects[0] as FreeRentEquipment);
				UpdateTotalLabels();
			}
		}

		protected void OnButtonAddByTypeClicked(object sender, EventArgs e)
		{
			PermissionControlledRepresentationJournal SelectDialog = new PermissionControlledRepresentationJournal (new ViewModel.EquipmentTypesForRentVM (MyOrmDialog.UoW));
			SelectDialog.Mode = JournalSelectMode.Single;
			SelectDialog.CustomTabName("Выберите тип оборудования");
			SelectDialog.ObjectSelected += EquipmentByTypeSelected;

			MyTab.TabParent.AddSlaveTab (MyTab, SelectDialog);
		}

		void EquipmentByTypeSelected (object sender, JournalObjectSelectedEventArgs args)
		{
			var selectedId = args.GetSelectedIds().FirstOrDefault();
			if(selectedId == 0) {
				return;
			}
			var equipmentType = AgreementUoW.GetById<EquipmentType>(selectedId);

			var rentPackage = RentPackageRepository.GetFreeRentPackage(AgreementUoW, equipmentType);
			if(rentPackage == null) {
				MessageDialogWorks.RunErrorDialog("Для выбранного типа оборудования нет пакета бесплатной аренды.");
				return;
			}

			AddEquipmentByRentPackage(rentPackage);
		}

		private void AddEquipmentByRentPackage(FreeRentPackage freeRentPackage)
		{
			var anyNomenclature = EquipmentRepository.GetFirstAnyNomenclatureForRent(AgreementUoW, freeRentPackage.EquipmentType);
			if(anyNomenclature == null) {
				MessageDialogWorks.RunErrorDialog("Для выбранного типа оборудования нет оборудования в справочнике номенклатур.");
				return;
			}

			var excludeNomenclatures = freeRentEquipments.Select(e => e.Nomenclature.Id).ToArray();

			var selectedNomenclature = EquipmentRepositoryForViews.GetAvailableNonSerialEquipmentForRent(AgreementUoW, freeRentPackage.EquipmentType, excludeNomenclatures);
			if(selectedNomenclature == null) {
				if(!MessageDialogWorks.RunQuestionDialog("Не найдено свободного оборудования выбранного типа!\nДобавить принудительно?")) {
					return;
				} else {
					selectedNomenclature = anyNomenclature;
				}
			}

			FreeRentEquipment eq = new FreeRentEquipment();
			eq.Nomenclature = selectedNomenclature;
			eq.Deposit = freeRentPackage.Deposit;
			eq.FreeRentPackage = freeRentPackage;
			eq.Count = 1;
			eq.WaterAmount = freeRentPackage.MinWaterAmount;
			freeRentEquipments.Add(eq);
			UpdateTotalLabels();
		}

		public void AddEquipment(FreeRentPackage freeRentPackage)
		{
			if(MessageDialogWorks.RunQuestionDialog("Подобрать оборудование автоматически по типу?")) {
				AddEquipmentByRentPackage(freeRentPackage);
			}
		}

	}
}

