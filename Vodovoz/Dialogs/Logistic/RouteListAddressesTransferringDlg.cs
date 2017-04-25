﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gamma.ColumnConfig;
using Gamma.GtkWidgets;
using Gtk;
using QSOrmProject;
using QSProjectsLib;
using QSTDI;
using Vodovoz.Domain.Logistic;
using Vodovoz.ViewModel;

namespace Vodovoz
{
	public partial class RouteListAddressesTransferringDlg : TdiTabBase, ITdiDialog, IOrmDialog
	{
		private IUnitOfWork uow = UnitOfWorkFactory.CreateWithoutRoot();
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();

		private bool hasChanges = false;

		public bool HasChanges { 
			get {
				return uow.HasChanges || hasChanges;
			}
		}

		#region IOrmDialog implementation

		public IUnitOfWork UoW
		{
			get
			{
				return uow;
			}
		}

		public object EntityObject {
			get {
				throw new NotImplementedException();
			}
		}

		public enum OpenParameter { Sender, Receiver }

		#endregion

		#region Конструкторы

		public RouteListAddressesTransferringDlg()
		{
			this.Build();
			TabName = "Перенос адресов маршрутных листов";
			ConfigureDlg();
		}

		public RouteListAddressesTransferringDlg (RouteList routeList, OpenParameter param) : this ()
		{
			var rl = UoW.GetById<RouteList> (routeList.Id);
			switch (param) {
			case OpenParameter.Sender:
				yentryreferenceRLFrom.Subject = rl;
				break;
			case OpenParameter.Receiver:
				yentryreferenceRLTo.Subject = rl;
				break;
			}
		}
		#endregion

		#region Методы

		private void ConfigureDlg()
		{
			var vm = new RouteListsVM ();
			vm.Filter.OnlyStatuses = new [] {
				RouteListStatus.EnRoute,
				RouteListStatus.MileageCheck,
				RouteListStatus.OnClosing,
				RouteListStatus.Closed,
			};
			vm.Filter.SetFilterDates (DateTime.Today.AddDays (-3), DateTime.Today.AddDays (1));
			yentryreferenceRLFrom.RepresentationModel = vm;

			vm = new RouteListsVM ();
			vm.Filter.OnlyStatuses = new [] {
				RouteListStatus.New,
				RouteListStatus.InLoading,
				RouteListStatus.EnRoute,
				RouteListStatus.OnClosing,
			};
			vm.Filter.SetFilterDates (DateTime.Today.AddDays (-3), DateTime.Today.AddDays (1));
			yentryreferenceRLTo.RepresentationModel = vm;

			yentryreferenceRLFrom.Changed += YentryreferenceRLFrom_Changed;
			yentryreferenceRLTo	 .Changed += YentryreferenceRLTo_Changed;

			//Для каждой TreeView нужен свой экземпляр ColumnsConfig
			ytreeviewRLFrom	.ColumnsConfig = GetColumnsConfig(false);
			ytreeviewRLTo	.ColumnsConfig = GetColumnsConfig(true);

			ytreeviewRLFrom .Selection.Mode = Gtk.SelectionMode.Multiple;
			ytreeviewRLTo	.Selection.Mode = Gtk.SelectionMode.Multiple;

			ytreeviewRLFrom .Selection.Changed += YtreeviewRLFrom_OnSelectionChanged;
			ytreeviewRLTo	.Selection.Changed += YtreeviewRLTo_OnSelectionChanged;
		}
		
		void YtreeviewRLFrom_OnSelectionChanged (object sender, EventArgs e)
		{
			CheckSensitivities();
		}

		void YtreeviewRLTo_OnSelectionChanged (object sender, EventArgs e)
		{
			CheckSensitivities();
		}

		private IColumnsConfig GetColumnsConfig (bool canEdit)
		{
			var colorGreen = new Gdk.Color(0x44, 0xcc, 0x49);
			var colorWhite = new Gdk.Color(0xff, 0xff, 0xff);
			
			return ColumnsConfigFactory.Create<RouteListItemNode>()
				.AddColumn("Заказ")			.AddTextRenderer	(node => node.Id)
				.AddColumn("Дата")			.AddTextRenderer	(node => node.Date)
				.AddColumn("Адрес")			.AddTextRenderer	(node => node.Address)
				.AddColumn("Бутыли")		.AddTextRenderer	(node => node.BottlesCount)
				.AddColumn("Статус")		.AddEnumRenderer	(node => node.Status)
				.AddColumn("Нужна загрузка").AddToggleRenderer	(node => node.NeedToReload)
					.Editing(canEdit)
				.AddColumn("Комментарий")	.AddTextRenderer	(node => node.Comment)
				.RowCells().AddSetter<CellRenderer>((cell,node) => cell.CellBackgroundGdk = node.WasTransfered ? colorGreen: colorWhite)
				.Finish();
		}

		void YentryreferenceRLFrom_Changed (object sender, EventArgs e)
		{
			if (yentryreferenceRLFrom.Subject == null)
				return;
			
			RouteList routeList = yentryreferenceRLFrom.Subject as RouteList;

			if (TabParent != null) {
				var tab = TabParent.FindTab (OrmMain.GenerateDialogHashName<RouteList> (routeList.Id));

				if (!(tab is RouteListClosingDlg)) { 
					if (tab != null) {
						MessageDialogWorks.RunErrorDialog ("Маршрутный лист уже открыт в другой вкладке");
						yentryreferenceRLFrom.Subject = null;
						return;
					}
				}
			}
			
			CheckSensitivities();

			IList<RouteListItemNode> items = new List<RouteListItemNode>();
			foreach (var item in routeList.Addresses)
				items.Add(new RouteListItemNode{RouteListItem = item});
			ytreeviewRLFrom.ItemsDataSource = items;
		}

		void YentryreferenceRLTo_Changed (object sender, EventArgs e)
		{
			if (yentryreferenceRLTo.Subject == null)
				return;
			
			RouteList routeList = yentryreferenceRLTo.Subject as RouteList;

			if (TabParent != null) {
				var tab = TabParent.FindTab (OrmMain.GenerateDialogHashName<RouteList> (routeList.Id));
				if (!(tab is RouteListClosingDlg)) {
					if (tab != null) {
						MessageDialogWorks.RunErrorDialog ("Маршрутный лист уже открыт в другой вкладке");
						yentryreferenceRLTo.Subject = null;
						return;
					}
				}
			}

			CheckSensitivities();

			IList<RouteListItemNode> items = new List<RouteListItemNode>();
			foreach (var item in routeList.Addresses)
				items.Add(new RouteListItemNode{RouteListItem = item});
			ytreeviewRLTo.ItemsDataSource = items;
		}

		private void UpdateNodes()
		{
			YentryreferenceRLFrom_Changed(null, null);
			YentryreferenceRLTo_Changed(null, null);
		}
		
		public event EventHandler<EntitySavedEventArgs> EntitySaved;

		public bool Save()
		{
			RouteList routeListTo 	= yentryreferenceRLTo.Subject as RouteList;
			RouteList routeListFrom = yentryreferenceRLFrom.Subject as RouteList;

			if (routeListTo == null || routeListFrom == null)
				return false;
			
			uow.Save(routeListTo);
			uow.Save(routeListFrom);

			uow.Commit();
			hasChanges = false;
			CheckSensitivities();
			return true;
		}

		public void SaveAndClose() {}
		
		protected void OnButtonTransferClicked (object sender, EventArgs e)
		{
			//Дополнительные проверки
			RouteList routeListTo 	= yentryreferenceRLTo.Subject as RouteList;
			RouteList routeListFrom = yentryreferenceRLFrom.Subject as RouteList;

			if (routeListTo == null || routeListFrom == null || routeListTo.Id == routeListFrom.Id)
				return;
			
			foreach (var row in ytreeviewRLFrom.GetSelectedObjects())
			{
				RouteListItem item = (row as RouteListItemNode)?.RouteListItem;
				logger.Debug("Проверка адреса с номером {0}", item?.Id.ToString() ?? "Неправильный адрес");

				if (item == null || item.Status == RouteListItemStatus.Transfered)
					continue;

				RouteListItem newItem = new RouteListItem(routeListTo, item.Order, RouteListItemStatus.Completed);
				newItem.WasTransfered = true;
				routeListTo.ObservableAddresses.Add(newItem);
//				uow.Save(newItem);

				item.TransferedTo = newItem;
				//				uow.Save(item);

				if (routeListTo.ClosingFilled)
					newItem.FirstFillClosing (UoW);
				
				hasChanges = true;
			}
			UpdateNodes();
		}

		private void CheckSensitivities ()
		{
			bool routeListToIsSelected = yentryreferenceRLTo.Subject != null;
			bool existToTransfer = ytreeviewRLFrom.GetSelectedObjects<RouteListItemNode> ().Any (x => x.Status != RouteListItemStatus.Transfered);

			buttonTransfer.Sensitive = existToTransfer && routeListToIsSelected;

			yentryreferenceRLTo.Sensitive = yentryreferenceRLFrom.Sensitive = !HasChanges;
		}

		protected void OnButtonSaveClicked (object sender, EventArgs e)
		{
			Save();
		}


		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			OnCloseTab(false);
		}
		#endregion
	}

	public class RouteListItemNode {
		public string Id {
			get {return RouteListItem.Order.Id.ToString();}
		}

		public string Date {
			get {return RouteListItem.Order.DeliveryDate.Value.ToString("d");}
		}

		public string Address {
			get {return RouteListItem.Order.DeliveryPoint?.ShortAddress ?? "Нет адреса";}
		}

		public RouteListItemStatus Status {
			get {return RouteListItem.Status;}
		}

		public bool NeedToReload {
			get {return RouteListItem.NeedToReload;}
			set {
				if(RouteListItem.WasTransfered)
					RouteListItem.NeedToReload = value;
			}
		}

		public bool WasTransfered {
			get {return RouteListItem.WasTransfered;}
		}

		public string Comment {
			get {return RouteListItem.Comment ?? "";}
		}

		public string BottlesCount {
			get {return RouteListItem.Order.OrderItems
					.Where(bot => bot.Nomenclature.Category == Vodovoz.Domain.Goods.NomenclatureCategory.water
						|| bot.Nomenclature.Category == Vodovoz.Domain.Goods.NomenclatureCategory.disposableBottleWater)
					.Sum(bot => bot.Count).ToString();}
		}
			
		public RouteListItem RouteListItem { get; set; }
	}
}

