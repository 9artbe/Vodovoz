﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using Gtk;
using NLog;
using QSOrmProject;
using QSTDI;
using Vodovoz.Domain;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Orders;
using Vodovoz.Repository;
using QSProjectsLib;
using System.ComponentModel;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class RouteListClosingItemsView : WidgetOnTdiTabBase
	{
		static Logger logger = LogManager.GetCurrentClassLogger ();

		public event RowActivatedHandler OnClosingItemActivated;

		public GenericObservableList<RouteListItem> Items{ get; set; }

		private int goodsColumnsCount = -1;

		private IList<RouteColumn> _columnsInfo;

		private IList<RouteColumn> columnsInfo {
			get {
				if (_columnsInfo == null && UoW!=null)
					_columnsInfo = Repository.Logistics.RouteColumnRepository.ActiveColumns (UoW);
				return _columnsInfo;
			}
		}

		private decimal bottleDepositPrice;

		IUnitOfWork uow;
		public IUnitOfWork UoW{ 
			get{
				return uow;
			}
			set{
				uow = value;
				bottleDepositPrice = NomenclatureRepository.GetBottleDeposit(UoW).GetPrice(1);
			}
		}

		RouteList routeList;
		public RouteList RouteList{
			get{ 
				return routeList;
			}
			set{
				if (routeList == value)
					return;
				routeList = value;
				if (routeList.Addresses == null)
					routeList.Addresses = new List<RouteListItem> ();				
				Items = routeList.ObservableAddresses;			

				routeList.ObservableAddresses.ListChanged += Items_ListChanged;

				UpdateColumns ();

				ytreeviewItems.ItemsDataSource = Items;
				ytreeviewItems.Reorderable = true;
			}
		}

		void Items_ListChanged (object aList)
		{
			UpdateColumns ();
		}

		private void UpdateColumns ()
		{
			var goodsColumns = Items.SelectMany (i => i.GoodsByRouteColumns.Keys).Distinct ().ToArray ();
			if (goodsColumnsCount == goodsColumns.Length)
				return;

			goodsColumnsCount = goodsColumns.Length;

			var config = ColumnsConfigFactory.Create<RouteListItem>()
				.AddColumn("Заказ").AddTextRenderer(node => node.Order.Id.ToString())
				.AddColumn("Адрес").AddTextRenderer(node => String.Format("{0} д.{1}", node.Order.DeliveryPoint.Street, node.Order.DeliveryPoint.Building));
			
			if (columnsInfo != null)
			{
				foreach (var column in columnsInfo)
				{
					if (!goodsColumns.Contains(column.Id))
						continue;
					int id = column.Id;
					config.AddColumn(column.Name).AddTextRenderer(a => a.GetGoodsAmountForColumn(id).ToString());
				}
			}
			var colorBlack = new Gdk.Color (0, 0, 0);
			var colorBlue = new Gdk.Color (0, 0, 0xff);
			var colorWhite = new Gdk.Color(0xff, 0xff, 0xff);
			var colorRed = new Gdk.Color(0xee, 0x66, 0x66);
			var colorLightBlue = new Gdk.Color(0xbb, 0xbb, 0xff);
			config
				.AddColumn("Доп. оборудование \n клиенту")
					.AddTextRenderer()
						.AddSetter((cell,node)=>cell.Markup=ToClientString(node))
				.AddColumn("Доп. оборуд. \n от клиента")
					.AddTextRenderer()
						.AddSetter((cell,node)=>cell.Markup=FromClientString(node))
				.AddColumn("Пустых \nбутылей")
					.AddNumericRenderer(node => node.BottlesReturned)
						.AddSetter((cell, node) => cell.Editable = node.IsDelivered())
						.Adjustment(new Adjustment(0, 0, 100000, 1, 1, 1))
					.AddTextRenderer(node => "шт", false)
				.AddColumn("Залоги \nза бутыли")
					.AddNumericRenderer(node => node.DepositsCollected)
						.Adjustment(new Adjustment(0, -100000, 100000, (double)bottleDepositPrice, (double)bottleDepositPrice, 1))
						.AddSetter((cell, node) => cell.Editable = node.IsDelivered())
						.AddSetter((cell,node) => {
					var expectedDeposits = (node.GetFullBottlesDeliveredCount()-node.BottlesReturned)*bottleDepositPrice;
					cell.ForegroundGdk = expectedDeposits!=node.DepositsCollected ? colorBlue : colorBlack;
					})
					.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn("Итого\n(нал.)")
					.AddNumericRenderer(node => node.TotalCash)
						.AddSetter((cell, node) => cell.Editable = node.Order.PaymentType == PaymentType.cash && 
													node.IsDelivered())
						.AddSetter((cell,node)=>cell.Sensitive = node.Order.PaymentType == PaymentType.cash)
						.Adjustment(new Adjustment(0, 0, 100000, 100, 100, 1))
					.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn("З/П \nводителя")
					.AddNumericRenderer(node => node.DriverWage)						
						.Adjustment(new Adjustment(0, 0, 100000, 100, 100, 1))
						.AddSetter((cell, node) => cell.Editable = node.IsDelivered())
						.AddSetter((c, node) => c.ForegroundGdk = node.HasUserSpecifiedDriverWage() ? colorBlue : colorBlack)
						.AddSetter((cell,node)=>cell.IsExpanded=false)
					.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn("З/П \nэкспедитора")
					.AddNumericRenderer(node => node.ForwarderWage)
						.AddSetter((cell, node) => cell.Editable = !node.WithoutForwarder)
						.AddSetter((cell, node) => cell.Sensitive = !node.WithoutForwarder)
						.Adjustment(new Adjustment(0, 0, 100000, 100, 100, 1))
						.AddSetter((c, node) => c.ForegroundGdk = node.HasUserSpecifiedForwarderWage() ? colorBlue : colorBlack)
						.AddSetter((c, node) => c.Alignment = Pango.Alignment.Right)
					.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
						.AddSetter((cell, node) => cell.Sensitive = !node.WithoutForwarder)
				.AddColumn("").AddTextRenderer()
				.RowCells()
				.AddSetter<CellRenderer>((cell, node) =>
				{
					var color = colorWhite;
						if (!node.IsDelivered())
							color = colorRed;
					else
					{
						var itemChanged = node.Order.OrderItems
							.Where(item => !item.Nomenclature.Serial)
							.Where(item => Nomenclature.GetCategoriesForShipment().Contains(item.Nomenclature.Category))
							.Any(item => item.Count != item.ActualCount);
						var equipmentChanged = node.Order.OrderEquipments
							.Any(eq => !eq.Confirmed);
						if (itemChanged || equipmentChanged)
							color = colorLightBlue;
					}
					cell.CellBackgroundGdk = color;
				});
				
			ytreeviewItems.ColumnsConfig = config.Finish();
		}

		public string ToClientString(RouteListItem item)
		{
			var stringParts = new List<string>();
			if (item.PlannedCoolersToClient > 0)
			{
				var formatString = item.CoolersToClient < item.PlannedCoolersToClient
						? "Кулеры:<b>{0}</b>({1})" 
						: "Кулеры:<b>{0}</b>";
				var coolerString = String.Format(formatString, item.CoolersToClient, item.PlannedCoolersToClient);
				stringParts.Add(coolerString);
			}
			if (item.PlannedPumpsToClient > 0)
			{
				var formatString = item.PumpsToClient < item.PlannedPumpsToClient
						? "Помпы:<b>{0}</b>({1})" 
						: "Помпы:<b>{0}</b>";
				var coolerString = String.Format(formatString, item.PumpsToClient, item.PlannedPumpsToClient);						
				stringParts.Add(coolerString);
			}
			if (item.UncategorisedEquipmentToClient > 0)
			{					
				var formatString = item.UncategorisedEquipmentToClient < item.PlannedUncategorisedEquipmentToClient
						? "Другое:<b>{0}</b>({1})" 
						: "Другое:<b>{0}</b>";
				var coolerString = String.Format(formatString, item.UncategorisedEquipmentToClient, item.PlannedUncategorisedEquipmentToClient);						
				stringParts.Add(coolerString);
			}

			return String.Join(",", stringParts);
		}	

		public string FromClientString(RouteListItem item)
		{
			var stringParts = new List<string>();
			if (item.PlannedCoolersFromClient > 0)
			{
				var formatString = item.CoolersFromClient < item.PlannedCoolersFromClient 
						? "Кулеры:<b>{0}</b>({1})" 
						: "Кулеры:<b>{0}</b>";
				var coolerString = String.Format(formatString, item.CoolersFromClient, item.PlannedCoolersFromClient);						
				stringParts.Add(coolerString);
			}
			if (item.PlannedPumpsFromClient > 0)
			{
				var formatString = item.PumpsFromClient < item.PlannedPumpsFromClient 
						? "Помпы:<b>{0}</b>({1})" 
						: "Помпы:<b>{0}</b>";
				var pumpString = String.Format(formatString, item.PumpsFromClient, item.PlannedPumpsFromClient);						
				stringParts.Add(pumpString);
			}
			return String.Join(",", stringParts);
		}

		public RouteListClosingItemsView ()
		{
			this.Build ();
		}

		void OnYtreeviewItemsRowActivated(object sender, RowActivatedArgs args)
		{
			OnClosingItemActivated(sender, args);
		}

		public RouteListItem GetSelectedRouteListItem()
		{
			return (ytreeviewItems.GetSelectedObject() as RouteListItem);
		}
	}		
}

