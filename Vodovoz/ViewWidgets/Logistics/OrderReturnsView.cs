﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Gamma.GtkWidgets;
using QSProjectsLib;
using QSTDI;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Orders;
using QSOrmProject;

namespace Vodovoz
{
	public partial class OrderReturnsView : TdiTabBase
	{
		List<OrderItemReturnsNode> equipmentFromClient;
		List<OrderItemReturnsNode> itemsToClient;

		IUnitOfWork uow;

		public IUnitOfWork UoW {
			get {
				return uow;
			}
			set {
				uow = value;
				depositrefunditemsview1.Configure(uow, routeListItem.Order, true);
			}
		}

		RouteListItem routeListItem;

		public OrderReturnsView(RouteListItem routeListItem)
		{
			this.Build();
			this.routeListItem = routeListItem;
			this.TabName = "Изменение заказа №" + routeListItem.Order.Id;

			entryTotal.Sensitive = yenumcomboOrderPayment.Sensitive =
				routeListItem.Status != RouteListItemStatus.Transfered;

			ytreeToClient.Sensitive = routeListItem.IsDelivered();
			ytreeFromClient.Sensitive = routeListItem.IsDelivered();
			Configure();
			itemsToClient = new List<OrderItemReturnsNode>();
			var nomenclatures = routeListItem.Order.OrderItems
				.Where(item => Nomenclature.GetCategoriesForShipment().Contains(item.Nomenclature.Category))
				.Where(item => !item.Nomenclature.IsSerial).ToList();
			foreach(var item in nomenclatures)
			{
				itemsToClient.Add(new OrderItemReturnsNode(item));
				item.PropertyChanged += OnOrderChanged;
			}
			var equipments = routeListItem.Order.OrderEquipments
				.Where(item => item.Direction == Vodovoz.Domain.Orders.Direction.Deliver);
			foreach(var item in equipments)
			{				
				itemsToClient.Add(new OrderItemReturnsNode(item));
				item.PropertyChanged += OnOrderChanged;
			}
			//Добавление в список услуг
			var services = routeListItem.Order.OrderItems
				.Where(item => item.Nomenclature.Category == NomenclatureCategory.service).ToList();
			foreach (var item in services)
			{
				itemsToClient.Add(new OrderItemReturnsNode(item));
				item.PropertyChanged += OnOrderChanged;
			}

			//От клиента
			equipmentFromClient = new List<OrderItemReturnsNode>();
			var fromClient = routeListItem.Order.OrderEquipments
				.Where(equipment => equipment.Direction == Vodovoz.Domain.Orders.Direction.PickUp).ToList();
			foreach (var item in fromClient)
			{
				var newOrderEquipmentNode = new OrderItemReturnsNode(item);
				equipmentFromClient.Add(newOrderEquipmentNode);
			}
			entryTotal.Text = CurrencyWorks.GetShortCurrencyString(routeListItem.Order.ActualGoodsTotalSum);

			ytreeToClient.ItemsDataSource = itemsToClient;
			ytreeFromClient.ItemsDataSource = equipmentFromClient;
			UpdateButtonsState();
		}

		public void OnOrderChanged(object sender, PropertyChangedEventArgs args)
		{
			entryTotal.Text = CurrencyWorks.GetShortCurrencyString(routeListItem.Order.ActualGoodsTotalSum);
		}

		protected void Configure()
		{
			yentryCounterparty.SubjectType = typeof(Counterparty);
			yentryCounterparty.Binding.AddBinding(routeListItem.Order, o => o.Client, w => w.Subject).InitializeFromSource();
			yentryCounterparty.CanEditReference = false;

			yentryDeliveryPoint.SubjectType = typeof(DeliveryPoint);
			yentryDeliveryPoint.Binding.AddBinding(routeListItem.Order, o => o.DeliveryPoint, w => w.Subject).InitializeFromSource();
			yentryDeliveryPoint.CanEditReference = false;

			ytreeToClient.ColumnsConfig = ColumnsConfigFactory.Create<OrderItemReturnsNode>()
				.AddColumn("Название")
					.AddTextRenderer(node => node.Name)
				.AddColumn("Кол-во")
					.AddNumericRenderer(node => node.Count)
						.AddSetter((c, node) => c.Digits = node.Nomenclature.Unit == null ? 0 : (uint)node.Nomenclature.Unit.Digits)
					.AddTextRenderer(node => node.Nomenclature.Unit == null ? String.Empty : node.Nomenclature.Unit.Name, false)
				.AddColumn("Кол-во по факту")
					.AddToggleRenderer(node => node.IsDelivered, false)
						.AddSetter((cell, node) => cell.Visible = node.IsSerialEquipment)
					.AddNumericRenderer(node => node.ActualCount, false)
				.AddSetter((cell, node) => {
					if(node.Nomenclature.Category == NomenclatureCategory.rent 
				       || node.Nomenclature.Category == NomenclatureCategory.deposit){
						cell.Editable = false;
					}
				})
						.Adjustment(new Gtk.Adjustment(0, 0, 9999, 1, 1, 0))
					.AddTextRenderer(node => node.Nomenclature.Unit == null ? String.Empty : node.Nomenclature.Unit.Name, false)
				.AddColumn("Цена")
					.AddNumericRenderer(node=>node.Price)
						.Adjustment(new Gtk.Adjustment(0,0,99999,1,100,0))
						.AddSetter((cell,node)=>cell.Editable = node.HasPrice)
					.AddTextRenderer (node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn("Стоимость")
					.AddNumericRenderer(node=>node.Sum)
					.AddTextRenderer(node=>CurrencyWorks.CurrencyShortName)
				.AddColumn("")
				.Finish();

			ytreeFromClient.ColumnsConfig = ColumnsConfigFactory.Create<OrderItemReturnsNode>()
				.AddColumn("Название")
					.AddTextRenderer(node => node.Name)
				.AddColumn("Забрано у клиента")
					.AddToggleRenderer(node => node.IsDelivered)
				.AddColumn("Причина незабора").AddTextRenderer(x => x.ConfirmedComments).Editable()
				.Finish();

			var order = routeListItem.Order;
			yenumcomboOrderPayment.ItemsEnum = typeof(PaymentType);
			yenumcomboOrderPayment.Binding.AddBinding(order, o => o.PaymentType, w => w.SelectedItem).InitializeFromSource();
			yenumcomboOrderPayment.Changed += YenumcomboOrderPayment_Changed;
		}

		void YenumcomboOrderPayment_Changed (object sender, EventArgs e)
		{
			routeListItem.RecalculateTotalCash();

			if(routeListItem.Order.PaymentType == PaymentType.cashless && routeListItem.TotalCash != 0)
			{
				routeListItem.RecalculateTotalCash();
			}
		}

		protected void OnButtonNotDeliveredClicked(object sender, EventArgs e)
		{
			routeListItem.UpdateStatus(UoW, RouteListItemStatus.Overdue);
			UpdateButtonsState();
			this.OnCloseTab(false);
		}

		protected void OnButtonDeliveryCanseledClicked(object sender, EventArgs e)
		{
			routeListItem.UpdateStatus(UoW, RouteListItemStatus.Canceled);
			UpdateButtonsState();
            this.OnCloseTab(false);
		}

		protected void OnButtonDeliveredClicked(object sender, EventArgs e)
		{
			routeListItem.UpdateStatus(UoW, RouteListItemStatus.Completed);
            routeListItem.FirstFillClosing (UoW);
			UpdateButtonsState();
		}

		void UpdateButtonsState()
		{
			bool isTransfered = routeListItem.Status == RouteListItemStatus.Transfered;
			buttonDeliveryCanceled.Sensitive = !isTransfered && routeListItem.Status != RouteListItemStatus.Canceled;
			buttonNotDelivered.Sensitive = !isTransfered && routeListItem.Status != RouteListItemStatus.Overdue;
			buttonDelivered.Sensitive = !isTransfered && routeListItem.Status != RouteListItemStatus.Completed && routeListItem.Status != RouteListItemStatus.EnRoute;
		}
	}

	public class OrderItemReturnsNode{
		OrderItem orderItem;
		OrderEquipment orderEquipment;

		public OrderItemReturnsNode(OrderItem item){			
			orderItem = item;
		}

		public OrderItemReturnsNode(OrderEquipment equipment){			
			orderEquipment = equipment;
		}

		public bool IsEquipment{ 
			get{
				return orderEquipment != null;
			}
		}

		public bool IsSerialEquipment{
			get	{
				return 
					IsEquipment 
					&& orderEquipment.Equipment != null
					&& orderEquipment.Equipment.Nomenclature.IsSerial;
			}
		}

		public bool IsDelivered{
			get{
				return ActualCount > 0;
			}
			set{
				if(IsEquipment && IsSerialEquipment) {
					ActualCount = value ? 1 : 0;
				}
			}
		}
		public int ActualCount{
			get{
				if (IsEquipment)
				{
					if(IsSerialEquipment) {
						return orderEquipment.Confirmed ? 1 : 0;
					}
					return orderEquipment.ActualCount;
				}
				else
				{
					return orderItem.ActualCount;
				}
			}
			set{
				if (IsEquipment){
					if(IsSerialEquipment) {
						orderEquipment.ActualCount = value > 0 ? 1 : 0;
					}
					orderEquipment.ActualCount = value;
				}
				else{
					orderItem.ActualCount = value;
				}

			}
		}
		public Nomenclature Nomenclature{
			get{
				if (IsEquipment)
				{
					if(IsSerialEquipment) {
						return orderEquipment.Equipment.Nomenclature;
					}
					return orderEquipment.Nomenclature;
				}
				else
				{
					return orderItem.Nomenclature;
				}
			}
		}
		public int Count{ 
			get{
				if(IsEquipment) {
					return orderEquipment.Count;
				}
				return orderItem.Count;
			}
		}

		public string Name{
			get{
				return IsEquipment ? orderEquipment.NameString : orderItem.NomenclatureString;
			}
		}

		public bool HasPrice{
			get{
				return !IsEquipment || orderEquipment.OrderItem != null;
			}
		}

		public string ConfirmedComments{
			get{
				return IsEquipment ? orderEquipment.ConfirmedComment : null;
			}
			set{
				if (IsEquipment)
					orderEquipment.ConfirmedComment = value;
			}
		}

		public decimal Price{
			get{
				if (IsEquipment)
				{
					return orderEquipment.OrderItem != null ? orderEquipment.OrderItem.Price : 0;
				}
				else
					return orderItem.Price;
			}
			set{
				if (IsEquipment)
				{
					if (orderEquipment.OrderItem != null)
						orderEquipment.OrderItem.Price = value;					
				}
				else
					orderItem.Price = value;
			}
		}
		public decimal Sum{
			get{ 
				return Price * ActualCount; 
			}
		}

	}
}

