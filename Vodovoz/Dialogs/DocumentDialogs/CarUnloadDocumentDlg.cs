﻿using System;
using System.Collections.Generic;
using System.Linq;
using QSOrmProject;
using QSProjectsLib;
using Vodovoz.Additions.Store;
using Vodovoz.Core.Permissions;
using Vodovoz.Domain.Documents;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Service;
using Vodovoz.Repository.Store;
using Vodovoz.ViewWidgets.Store;
using Vodovoz.Domain.Store;

namespace Vodovoz
{
	public partial class CarUnloadDocumentDlg : OrmGtkDialogBase<CarUnloadDocument>
	{
		static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();

		IList<Equipment> alreadyUnloadedEquipment;

		public override bool HasChanges {
			get {
				return true;
			}
		}

		#region Конструкторы
		public CarUnloadDocumentDlg()
		{
			this.Build();
			ConfigureNewDoc();
			ConfigureDlg();
		}
 

		public CarUnloadDocumentDlg (int routeListId, int? warehouseId)
		{
			this.Build();
			ConfigureNewDoc();

			if(warehouseId.HasValue)
				Entity.Warehouse = UoW.GetById<Warehouse>(warehouseId.Value);
			Entity.RouteList = UoW.GetById<RouteList>(routeListId);
			ConfigureDlg();
		}

		public CarUnloadDocumentDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<CarUnloadDocument> (id);
			ConfigureDlg ();
		}

		public CarUnloadDocumentDlg (CarUnloadDocument sub) : this (sub.Id)
		{
		}
		#endregion

		#region Методы

		void ConfigureNewDoc()
		{
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<CarUnloadDocument>();
			Entity.Author = Repository.EmployeeRepository.GetEmployeeForCurrentUser(UoW);
			if(Entity.Author == null) {
				MessageDialogWorks.RunErrorDialog("Ваш пользователь не привязан к действующему сотруднику, вы не можете создавать складские документы, так как некого указывать в качестве кладовщика.");
				FailInitialize = true;
				return;
			}
			Entity.Warehouse = StoreDocumentHelper.GetDefaultWarehouse(UoW, WarehousePermissions.CarUnloadEdit);
		}

		void ConfigureDlg ()
		{
			if(StoreDocumentHelper.CheckAllPermissions(UoW.IsNew, WarehousePermissions.CarUnloadEdit, Entity.Warehouse)) {
				FailInitialize = true;
				return;
			}

			var editing = StoreDocumentHelper.CanEditDocument(WarehousePermissions.CarUnloadEdit, Entity.Warehouse);
			yentryrefRouteList.IsEditable = yentryrefWarehouse.IsEditable = ytextviewCommnet.Editable = editing;
			returnsreceptionview1.Sensitive = bottlereceptionview1.Sensitive = nonserialequipmentreceptionview1.Sensitive = editing;
			
			bottlereceptionview1.UoW = UoW;
			returnsreceptionview1.UoW = UoW;

			ylabelDate.Binding.AddFuncBinding(Entity, e => e.TimeStamp.ToString("g"), w => w.LabelProp).InitializeFromSource();
			yentryrefWarehouse.ItemsQuery = StoreDocumentHelper.GetRestrictedWarehouseQuery(WarehousePermissions.CarUnloadEdit);
			yentryrefWarehouse.Binding.AddBinding(Entity, e => e.Warehouse, w => w.Subject).InitializeFromSource();
			ytextviewCommnet.Binding.AddBinding(Entity, e => e.Comment, w => w.Buffer.Text).InitializeFromSource();
			var filter = new RouteListsFilter(UoW);
			filter.RestrictStatus = RouteListStatus.EnRoute;
			yentryrefRouteList.RepresentationModel = new ViewModel.RouteListsVM(filter);
			yentryrefRouteList.Binding.AddBinding(Entity, e => e.RouteList, w => w.Subject).InitializeFromSource();

			returnsreceptionview1.Warehouse = Entity.Warehouse;

			UpdateWidgetsVisible();
			if(!UoW.IsNew)
				LoadReception();
		}

		public override bool Save ()
		{
			UpdateReceivedItemsOnEntity();

			var valid = new QSValidation.QSValidator<CarUnloadDocument> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			Entity.LastEditor = Repository.EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			Entity.LastEditedTime = DateTime.Now;
			if(Entity.LastEditor == null)
			{
				MessageDialogWorks.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете изменять складские документы, так как некого указывать в качестве кладовщика.");
				return false;
			}

			logger.Info ("Сохраняем разгрузочный талон...");
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}

		void UpdateRouteListInfo()
		{
			if(Entity.RouteList == null)
			{
				ytextviewRouteListInfo.Buffer.Text = String.Empty;
				return;
			}

			ytextviewRouteListInfo.Buffer.Text =
				String.Format ("Маршрутный лист №{0} от {1:d}\nВодитель: {2}\nМашина: {3}({4})\nЭкспедитор: {5}",
					Entity.RouteList.Id,
					Entity.RouteList.Date,
					Entity.RouteList.Driver.FullName,
					Entity.RouteList.Car.Model,
					Entity.RouteList.Car.RegistrationNumber,
					Entity.RouteList.Forwarder != null ? Entity.RouteList.Forwarder.FullName : "(Отсутствует)" 
				);
		}

		void UpdateAlreadyUnloaded()
		{
			alreadyUnloadedEquipment = Repository.EquipmentRepository.GetEquipmentUnloadedTo(UoW, Entity.RouteList);
			returnsreceptionview1.AlreadyUnloadedEquipment = alreadyUnloadedEquipment;
		}

		void SetupForNewRouteList()
		{
			UpdateRouteListInfo();
			if (Entity.RouteList != null)
			{
				UpdateAlreadyUnloaded();
			}
			nonserialequipmentreceptionview1.RouteList = Entity.RouteList;
			returnsreceptionview1.RouteList = Entity.RouteList;
		}

		private void UpdateWidgetsVisible()
		{
			bottlereceptionview1.Visible = Entity.Warehouse != null && Entity.Warehouse.CanReceiveBottles;
			nonserialequipmentreceptionview1.Visible = Entity.Warehouse != null && Entity.Warehouse.CanReceiveEquipment;
		}

		void LoadReception()
		{
			foreach(var item in Entity.Items)
			{
				var bottle = bottlereceptionview1.Items.FirstOrDefault(x => x.NomenclatureId == item.MovementOperation.Nomenclature.Id);
				if(bottle != null)
				{
					bottle.Amount = (int)item.MovementOperation.Amount;
					continue;
				}

				var returned = item.MovementOperation.Equipment != null
					? returnsreceptionview1.Items.FirstOrDefault(x => x.EquipmentId == item.MovementOperation.Equipment.Id)
					: returnsreceptionview1.Items.FirstOrDefault(x => x.NomenclatureId == item.MovementOperation.Nomenclature.Id);
				if(returned != null)
				{
					returned.Amount = (int)item.MovementOperation.Amount;
					continue;
				}

				if(item.ReciveType == ReciveTypes.Equipment) {
					var equipmentByNomenclature = nonserialequipmentreceptionview1.Items.FirstOrDefault(x => x.NomenclatureId == item.MovementOperation.Nomenclature.Id);
					if(equipmentByNomenclature != null) {
						equipmentByNomenclature.Amount = (int)item.MovementOperation.Amount;
						continue;
					} else {
						nonserialequipmentreceptionview1.Items.Add(new ReceptionNonSerialEquipmentItemNode {
							NomenclatureCategory = NomenclatureCategory.equipment,
							NomenclatureId = item.MovementOperation.Nomenclature.Id,
							Amount = (int)item.MovementOperation.Amount,
							Name = item.MovementOperation.Nomenclature.Name
						});
						continue;
					}
				}

				logger.Warn ("Номенклатура {0} не найдена в заказа мл, добавляем отдельно...", item.MovementOperation.Nomenclature);
				var newItem = new ReceptionItemNode (item.MovementOperation.Nomenclature, (int)item.MovementOperation.Amount);
				if (item.MovementOperation.Equipment != null)
				{
					newItem.EquipmentId = item.MovementOperation.Equipment.Id;
				}			
				returnsreceptionview1.AddItem (newItem);
			}

			foreach(var item in bottlereceptionview1.Items)
			{
				var returned = Entity.Items.FirstOrDefault(x => x.MovementOperation.Nomenclature.Id == item.NomenclatureId);
				item.Amount = returned != null ? (int)returned.MovementOperation.Amount : 0;
			}
		}

		void UpdateReceivedItemsOnEntity()
		{
			//Собираем список всего на возврат из разных виджетов.
			var tempItemList = new List<InternalItem>();

			foreach (var node in bottlereceptionview1.Items) 
			{
				if (node.Amount == 0)
					continue;

				var item = new InternalItem {
						ReciveType = ReciveTypes.Bottle,
						NomenclatureId = node.NomenclatureId,
						Amount = node.Amount
					};
					tempItemList.Add(item);
			}

			foreach (var node in returnsreceptionview1.Items) 
			{
				if (node.Amount == 0)
					continue;

				var	item = new InternalItem {
					ReciveType = ReciveTypes.Returnes,
						NomenclatureId = node.NomenclatureId,
						Amount = node.Amount
					};
					tempItemList.Add(item);
			}

			foreach (var node in nonserialequipmentreceptionview1.Items) 
			{
				if (node.Amount == 0)
					continue;

				var	item = new InternalItem {
						ReciveType = ReciveTypes.Equipment,
						NomenclatureId = node.NomenclatureId,
						Amount = node.Amount
					};
					tempItemList.Add(item);
			}

			//Обновляем Entity
			var nomenclatures = UoW.GetById<Nomenclature>(tempItemList.Select(x => x.NomenclatureId).ToArray());
			foreach (var tempItem in tempItemList) {
				var item = Entity.Items.FirstOrDefault(x => x.MovementOperation.Nomenclature.Id == tempItem.NomenclatureId);
				if (item == null) {
					var nomenclature = nomenclatures.First(x => x.Id == tempItem.NomenclatureId);
					Entity.AddItem(
						tempItem.ReciveType,
						nomenclature,
						null,
						tempItem.Amount,
						null
					);
				}
				else
				{
					item.MovementOperation.Amount = tempItem.Amount;
				}
			}

			foreach(var item in Entity.Items.ToList())
			{
				var exist = tempItemList.Any(x => x.NomenclatureId == item.MovementOperation.Nomenclature?.Id);
				
				if(!exist)
				{
					UoW.Delete(item.MovementOperation);
					Entity.ObservableItems.Remove(item);
				}
			}
		}
		#endregion

		#region События
		protected void OnButtonPrintClicked(object sender, EventArgs e)
		{
			if (UoWGeneric.HasChanges && CommonDialogs.SaveBeforePrint (typeof(CarUnloadDocument), "талона"))
				Save ();

			var reportInfo = new QSReport.ReportInfo
				{
					Title = Entity.Title,
					Identifier = "Store.CarUnloadDoc",
					Parameters = new System.Collections.Generic.Dictionary<string, object>
					{
						{ "id",  Entity.Id }
					}
				};

			TabParent.OpenTab(
				QSReport.ReportViewDlg.GenerateHashName(reportInfo),
				() => new QSReport.ReportViewDlg(reportInfo),
				this);
		}

		protected void OnYentryrefWarehouseChanged(object sender, EventArgs e)
		{
			UpdateWidgetsVisible();
			returnsreceptionview1.Warehouse = Entity.Warehouse;
		}

		protected void OnYentryrefRouteListChanged(object sender, EventArgs e)
		{
			SetupForNewRouteList();
		}
		#endregion

		class InternalItem{
			
			public ReciveTypes ReciveType;
			public int NomenclatureId;

			public decimal Amount;
		}
	}
}

