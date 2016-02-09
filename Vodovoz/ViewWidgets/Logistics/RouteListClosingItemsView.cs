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

		IUnitOfWork uow;
		public IUnitOfWork UoW{ 
			get{
				return uow;
			}
			set{
				uow = value;
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
				UpdateNodes();

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

		private void UpdateNodes()
		{
			Items = routeList.ObservableAddresses;
			foreach (RouteListItem routeListitem in routeList.ObservableAddresses)
			{
				routeListitem.BottlesReturned = routeListitem.Order.BottlesReturn;
				routeListitem.DepositsCollected = routeListitem.Order.OrderDepositItems.Sum(depositItem => depositItem.Deposit);
				routeListitem.RecalculateWages();
			}
		}

		private void UpdateColumns ()
		{
			var goodsColumns = Items.SelectMany (i => i.GoodsByRouteColumns.Keys).Distinct ().ToArray ();
			if (goodsColumnsCount == goodsColumns.Length)
				return;

			goodsColumnsCount = goodsColumns.Length;

			var config = ColumnsConfigFactory.Create<RouteListItem>()
				.AddColumn("Заказ").AddTextRenderer(node => node.Order.Id.ToString())
				.AddColumn("Адрес").AddTextRenderer(node => String.Format("{0} д.{1}", node.Order.DeliveryPoint.Street, node.Order.DeliveryPoint.Building))
				.AddColumn("Время").AddTextRenderer(node => node.Order.DeliverySchedule == null ? "" : node.Order.DeliverySchedule.Name);

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
			config
				.AddColumn("Пустых бутылей")
					.AddNumericRenderer(node => node.BottlesReturned).Editing(true).Adjustment(new Adjustment(0, 0, 100000, 1, 1, 1))
					.AddTextRenderer(node => "шт", false)
				.AddColumn("Залоги за бутыли")
					.AddNumericRenderer(node => node.DepositsCollected)
						.Editing(true)
						.Adjustment(new Adjustment(0, 0, 100000, 100, 100, 1))
					.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn("Итого(нал.)")
					.AddNumericRenderer(node => node.TotalCash)
						.AddSetter((cell,node)=>cell.Editable=node.Order.PaymentType==PaymentType.cash)
						.Adjustment(new Adjustment(0, 0, 100000, 100, 100, 1))
					.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn("ЗП водителя")
					.AddNumericRenderer(node => node.DriverWage)
						.Editing(true)
						.Adjustment(new Adjustment(0, 0, 100000, 100, 100, 1))
						.AddSetter((c, node) => c.ForegroundGdk = node.HasUserSpecifiedDriverWage() ? colorBlue : colorBlack)
					.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn("ЗП экспедитора") 
					.AddNumericRenderer(node => node.ForwarderWage)
						.AddSetter((cell, node) => cell.Editable = !node.WithoutForwarder)
						.AddSetter((cell,node)=>cell.Sensitive = !node.WithoutForwarder)
						.Adjustment(new Adjustment(0, 0, 100000, 100, 100, 1))
						.AddSetter((c, node) => c.ForegroundGdk = node.HasUserSpecifiedForwarderWage() ? colorBlue : colorBlack)
						.AddSetter((c, node) => c.Alignment = Pango.Alignment.Right)
					.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
						.AddSetter((cell,node)=>cell.Sensitive = !node.WithoutForwarder)
				.AddColumn("").AddTextRenderer()
				.RowCells()
				.AddSetter<CellRenderer>((cell, node) => cell.CellBackgroundGdk = node.Status==RouteListItemStatus.Completed ? colorWhite : colorRed);
			ytreeviewItems.ColumnsConfig = config.Finish();
		}



		public RouteListClosingItemsView ()
		{
			this.Build ();
		}

		void OnYtreeviewItemsRowActivated(object sender, RowActivatedArgs args)
		{
			var node = ytreeviewItems.GetSelectedObject() as RouteListItem;
			if (node.Status == RouteListItemStatus.Completed)
			{
				var dlg = new OrderReturnsView(ytreeviewItems.GetSelectedObject() as RouteListItem);
				MyTab.TabParent.AddSlaveTab(MyTab, dlg);
			}
		}
	}

}

