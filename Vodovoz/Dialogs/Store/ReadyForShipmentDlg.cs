﻿using System;
using QSTDI;
using QSOrmProject;
using Vodovoz.Domain.Store;
using Vodovoz.Domain.Orders;
using Vodovoz.Domain;
using Vodovoz.Domain.Logistic;
using NHibernate.Criterion;
using System.Collections.Generic;
using NHibernate.Transform;
using Vodovoz.Domain.Operations;
using Vodovoz.Repository;
using Vodovoz.Domain.Documents;
using QSProjectsLib;
using System.Linq;

namespace Vodovoz
{
	public partial class ReadyForShipmentDlg : TdiTabBase
	{
		private IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot ();

		private Dictionary<int,decimal> inStock;
		private Dictionary<int,decimal> equipmentInStock;

		ShipmentDocumentType shipmentType;
		int shipmentId;

		Vodovoz.Domain.Orders.Order order;
		RouteList routelist;

		List<ShipmentItemsNode> ShipmentList = new List<ShipmentItemsNode> ();

		/// <param name="type">Маршрутный лист или заказ</param>
		/// <param name="id">Номер маршрутного листа или заказа.</param>
		/// <param name="stock">Склад отгрузки.</param>
		public ReadyForShipmentDlg (ShipmentDocumentType type, int id, Warehouse stock)
		{
			Build ();
			shipmentType = type;
			shipmentId = id;

			this.TabName = "Товар на погрузку";
			ycomboboxWarehouse.ItemsList = Repository.Store.WarehouseRepository.WarehouseForShipment (UoW, type, id);

			inStock = new Dictionary<int, decimal> ();

			var colorBlack = new Gdk.Color (0, 0, 0);
			var colorRed = new Gdk.Color (0xff, 0, 0);

			ytreeItems.ColumnsConfig = Gamma.GtkWidgets.ColumnsConfigFactory.Create<ShipmentItemsNode> ()
				.AddColumn ("Номенклатуры").AddTextRenderer (node => node.NomenclatureName)
				.AddColumn ("Серийный номер").AddTextRenderer (node => node.SerialNumberText)
				.AddColumn ("Количество").AddTextRenderer (node => node.AmountText)
				.AddSetter((cell,node)=>cell.Text = node.IsAvailable ? node.AmountText : node.AmountText + " (нет в наличии)")
				.AddSetter(((cell,node) => cell.ForegroundGdk = node.IsAvailable ? colorBlack : colorRed))
				.AddColumn("На складе").AddTextRenderer(node=>node.InStockText)
				.Finish ();

			if (stock != null)
				ycomboboxWarehouse.SelectedItem = stock;

			switch (shipmentType) {
			case ShipmentDocumentType.Order:
				order = UoW.GetById<Vodovoz.Domain.Orders.Order> (id);
				textviewShipmentInfo.Buffer.Text =
					String.Format ("Самовывоз заказа №{0}\nКлиент: {1}",
					id,
					order.Client.FullName
				);
				TabName = String.Format ("Отгрузка заказа №{0}", id);
				break;
			case ShipmentDocumentType.RouteList:
				routelist = UoW.GetById<RouteList> (id);
				textviewShipmentInfo.Buffer.Text =
					String.Format ("Маршрутный лист №{0} от {1:d}\nВодитель: {2}\nМашина: {3}({4})\nЭкспедитор: {5}",
					id,
					routelist.Date,
					routelist.Driver.FullName,
					routelist.Car.Model,
					routelist.Car.RegistrationNumber,
					routelist.Forwarder != null ? routelist.Forwarder.FullName : "(Отсутствует)" 
				);
				TabName = String.Format ("Отгрузка маршрутного листа №{0}", id);
				break;
			}
		}

		void UpdateItemsList ()
		{
			ShipmentList.Clear ();

			//FIXME Добавить проверку, не отгружен ли товар уже.
			Warehouse CurrentStock = ycomboboxWarehouse.SelectedItem as Warehouse;
			if (CurrentStock == null)
				return;
	
			ShipmentItemsNode resultAlias = null;
			Vodovoz.Domain.Orders.Order orderAlias = null;
			OrderItem orderItemsAlias = null;
			OrderEquipment orderEquipmentAlias = null;
			Nomenclature OrderItemNomenclatureAlias = null, OrderEquipmentNomenclatureAlias = null;
			Equipment equipmentAlias = null;
			MeasurementUnits unitsAlias = null;

			RouteListItem routeListAddressAlias = null;

			var ordersQuery = QueryOver.Of<Vodovoz.Domain.Orders.Order> (() => orderAlias);

			switch (shipmentType) {
			case ShipmentDocumentType.Order:
				ordersQuery.Where (o => o.Id == shipmentId)
					.Select (o => o.Id);
				break;
			case ShipmentDocumentType.RouteList:
				var routeListItemsSubQuery = QueryOver.Of<RouteListItem> ()
					.Where (r => r.RouteList.Id == shipmentId)
					.Select (r => r.Order.Id);
				ordersQuery.WithSubquery.WhereProperty (o => o.Id).In (routeListItemsSubQuery).Select (o => o.Id);
				break;
			default:
				throw new NotSupportedException (shipmentType.ToString ());
			}

			var orderitems = UoW.Session.QueryOver<OrderItem> (() => orderItemsAlias)
				.WithSubquery.WhereProperty (i => i.Order.Id).In (ordersQuery)
				.JoinAlias (() => orderItemsAlias.Nomenclature, () => OrderItemNomenclatureAlias)
				.Where(()=>!OrderItemNomenclatureAlias.Serial)
				.JoinAlias (() => orderItemsAlias.Equipment, () => equipmentAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
				.JoinAlias (() => OrderItemNomenclatureAlias.Unit, () => unitsAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
				.Where (() => OrderItemNomenclatureAlias.Warehouse == CurrentStock)
				.SelectList (list => list
					.SelectGroup (() => OrderItemNomenclatureAlias.Id).WithAlias (() => resultAlias.Id)
					.Select (() => OrderItemNomenclatureAlias.Name).WithAlias (() => resultAlias.NomenclatureName)
					.SelectGroup (() => equipmentAlias.Id).WithAlias (() => resultAlias.EquipmentId)
					.SelectSum (() => orderItemsAlias.Count).WithAlias (() => resultAlias.Amount)
					.Select (() => unitsAlias.Name).WithAlias (() => resultAlias.UnitName)
			                 )
				.TransformUsing (Transformers.AliasToBean <ShipmentItemsNode> ())
				.List<ShipmentItemsNode> ();

			ShipmentList.AddRange (orderitems);
			ShipmentList.FindAll (node => node.EquipmentId > 0).ForEach (node => node.IsNew = true);

			var orderEquipments = UoW.Session.QueryOver<OrderEquipment> (() => orderEquipmentAlias)
				.WithSubquery.WhereProperty (i => i.Order.Id).In (ordersQuery)
				.JoinAlias (() => orderEquipmentAlias.Equipment, () => equipmentAlias)
				.Where (() => orderEquipmentAlias.Direction == Domain.Orders.Direction.Deliver)
				.JoinAlias (() => equipmentAlias.Nomenclature, () => OrderEquipmentNomenclatureAlias)
				.JoinAlias (() => OrderEquipmentNomenclatureAlias.Unit, () => unitsAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
				.Where (() => OrderEquipmentNomenclatureAlias.Warehouse == CurrentStock)
				.SelectList (list => list
					.SelectGroup (() => OrderEquipmentNomenclatureAlias.Id).WithAlias (() => resultAlias.Id)
					.Select (() => OrderEquipmentNomenclatureAlias.Name).WithAlias (() => resultAlias.NomenclatureName)
					.SelectGroup (() => equipmentAlias.Id).WithAlias (() => resultAlias.EquipmentId)
					.SelectSum (() => 1).WithAlias (() => resultAlias.Amount)
					.Select (() => unitsAlias.Name).WithAlias (() => resultAlias.UnitName)
			                      )
				.TransformUsing (Transformers.AliasToBean <ShipmentItemsNode> ())
				.List<ShipmentItemsNode> ();

			foreach (var node in orderEquipments) {
//				if (!ShipmentList.Exists (item => item.EquipmentId == node.EquipmentId))
					ShipmentList.Add (node);
			}
			/*
			var orderTrackableEquipment = UoW.Session.QueryOver<OrderEquipment> (() => orderEquipmentAlias)
				.WithSubquery.WhereProperty (i => i.Order.Id).In (ordersQuery)
				.JoinAlias (() => orderEquipmentAlias.Equipment, () => equipmentAlias)
				.Where (() => orderEquipmentAlias.Direction == Domain.Orders.Direction.Deliver)
				.JoinAlias (() => equipmentAlias.Nomenclature, () => OrderEquipmentNomenclatureAlias)
				.Where(()=>OrderEquipmentNomenclatureAlias.Serial)
				.JoinAlias (() => OrderEquipmentNomenclatureAlias.Unit, () => unitsAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
				.Where (() => OrderEquipmentNomenclatureAlias.Warehouse == CurrentStock)
				.SelectList (list => list
					.Select (() => OrderEquipmentNomenclatureAlias.Id).WithAlias (() => resultAlias.Id)
					.Select (() => OrderEquipmentNomenclatureAlias.Name).WithAlias (() => resultAlias.NomenclatureName)
					.Select (() => equipmentAlias.Id).WithAlias (() => resultAlias.EquipmentId)
					.Select (() => 1).WithAlias (() => resultAlias.Amount)
					.Select (() => unitsAlias.Name).WithAlias (() => resultAlias.UnitName)
				)
				.TransformUsing (Transformers.AliasToBean <ShipmentItemsNode> ())
				.List<ShipmentItemsNode> ();
		
			foreach (var node in orderTrackableEquipment) {
		//		if (!ShipmentList.Exists (item => item.EquipmentId == node.EquipmentId))
					ShipmentList.Add (node);
			}
			*/

			UpdateStockBalance ();

			buttonConfirmShipment.Sensitive = CanLoad ();

			ytreeItems.ItemsDataSource = ShipmentList;
		}

		protected void UpdateStockBalance(){
			Warehouse warehouse = ycomboboxWarehouse.SelectedItem as Warehouse;
			inStock = Repository.Store.WarehouseRepository.NomenclaturesInStock (UoW, warehouse,
				ShipmentList.Where(shipment=>!shipment.IsTrackable).Select (shipment => shipment.Id).ToArray());
			foreach (var item in inStock)
				ShipmentList.First (shipment => shipment.Id == item.Key).InStock = item.Value;
			equipmentInStock = Repository.Store.WarehouseRepository.EquipmentInStock (UoW, warehouse,
				ShipmentList.Where(shipment=>shipment.IsTrackable).Select (shipment => shipment.EquipmentId).ToArray ());
			foreach (var item in equipmentInStock)
				ShipmentList.First (shipment => shipment.EquipmentId == item.Key).InStock = item.Value;
		}

		protected bool CanLoad(){
			var warehousesLoadedFrom = 
				Vodovoz.Repository.Store.WarehouseRepository.WarehousesVisited (UoW, shipmentType, shipmentId);
			bool itemsAvailable = ShipmentList.All(node=>node.IsAvailable);
			bool alreadyLoaded = warehousesLoadedFrom.Contains(ycomboboxWarehouse.SelectedItem);
			return !alreadyLoaded && itemsAvailable;
		}

		protected void OnYcomboboxWarehouseItemSelected (object sender, Gamma.Widgets.ItemSelectedEventArgs e)
		{
			UpdateItemsList ();
		}

		protected void OnButtonConfirmShipmentClicked (object sender, EventArgs e)
		{
			UpdateStockBalance ();
			foreach (ShipmentItemsNode shipmentItem in ShipmentList) {				
				if (!shipmentItem.IsAvailable) {
					MessageDialogWorks.RunErrorDialog ("Невозможно подтвердить отгрузку т.к. не все товары в наличии на складе.");	
					return;
				}
			}
			var CarLoadDocumentUoW = UnitOfWorkFactory.CreateWithNewRoot <CarLoadDocument> ();

			CarLoadDocumentUoW.Root.Storekeeper = EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			if (CarLoadDocumentUoW.Root.Storekeeper == null) {
				MessageDialogWorks.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете загружать автомобили, так как некого указывать в качестве кладовщика.");
				return;
			}
			if (shipmentType == ShipmentDocumentType.Order)
				CarLoadDocumentUoW.Root.Order = UoW.GetById<Vodovoz.Domain.Orders.Order> (shipmentId);
			else
				CarLoadDocumentUoW.Root.RouteList = UoW.GetById<RouteList> (shipmentId);
			CarLoadDocumentUoW.Root.Warehouse = ycomboboxWarehouse.SelectedItem as Warehouse;

			foreach (var node in ShipmentList) {
				var warehouseMovementOperation = UnitOfWorkFactory.CreateWithNewRoot <WarehouseMovementOperation> ();
				warehouseMovementOperation.Root.Amount = node.Amount;
				warehouseMovementOperation.Root.Equipment = UoW.GetById<Equipment> (node.EquipmentId);
				warehouseMovementOperation.Root.Nomenclature = UoW.GetById <Nomenclature> (node.Id);
				warehouseMovementOperation.Root.WriteoffWarehouse = ycomboboxWarehouse.SelectedItem as Warehouse;
				warehouseMovementOperation.Root.OperationTime = DateTime.Now;
				warehouseMovementOperation.Save ();
				CarLoadDocumentUoW.Root.AddItem (new CarLoadDocumentItem { MovementOperation = warehouseMovementOperation.Root });
			}
			CarLoadDocumentUoW.Save ();
			ChangeShipmentStatus ();
			UoW.Save (routelist);
			UoW.Commit ();
			OnCloseTab (false);
		}			

		protected void ChangeShipmentStatus ()
		{
			var warehousesLeft = Vodovoz.Repository.Store.WarehouseRepository.WarehousesNotVisited (UoW, shipmentType, shipmentId);
			switch (shipmentType) {
			case ShipmentDocumentType.Order:				
				if(warehousesLeft.Count==0)
					order.Close ();
				break;
			case ShipmentDocumentType.RouteList:						
				if (warehousesLeft.Count == 0) {
					routelist.Status = RouteListStatus.EnRoute;
					foreach (var item in routelist.Addresses) {
						item.Order.OrderStatus = OrderStatus.OnTheWay;
					}
				}
				else
					routelist.Status = RouteListStatus.InLoading;
				break;
			default:
				throw new NotSupportedException (shipmentType.ToString ());
			}
		}

		public class ShipmentItemsNode
		{
			public int Id{ get; set; }

			public string NomenclatureName { get; set; }

			public string SerialNumber { get { return EquipmentId > 0 ? EquipmentId.ToString () : null; } }

			public int Amount { get; set; }

			public decimal InStock{ get; set; }

			public string InStockText{ get { return $"{InStock} {UnitName}"; } }

			public bool IsAvailable{ get { return InStock >= Amount; } }

			public bool IsNew { get; set; }

			public int EquipmentId { get; set; }

			public bool IsTrackable{ get {return EquipmentId > 0;} }

			public string UnitName{ get; set; }

			public string SerialNumberText {
				get { return IsNew ? String.Format ("{0}(новый)", SerialNumber) : String.Format ("{0}", SerialNumber); }
			}

			public string AmountText { get { return String.Format ("{0} {1}", 
					Amount,
					UnitName); } }
		}

	}
}

