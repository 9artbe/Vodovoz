﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using Gtk;
using NHibernate.Criterion;
using NLog;
using QS.Dialog.Gtk;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QS.Project.Dialogs;
using QS.Project.Dialogs.GtkUI;
using QS.Project.Repositories;
using QSOrmProject;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Orders;
using Vodovoz.Domain.Sale;
using Vodovoz.EntityRepositories.Logistic;
using Vodovoz.Repositories.Orders;
using Order = Vodovoz.Domain.Orders.Order;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RouteListCreateItemsView : WidgetOnTdiTabBase
	{
		static Logger logger = LogManager.GetCurrentClassLogger();

		private int goodsColumnsCount = -1;
		private bool isEditable = true;

		private IList<RouteColumn> _columnsInfo;

		private IList<RouteColumn> ColumnsInfo => _columnsInfo ?? Repository.Logistics.RouteColumnRepository.ActiveColumns(RouteListUoW);

		private IUnitOfWorkGeneric<RouteList> routeListUoW;

		public IUnitOfWorkGeneric<RouteList> RouteListUoW {
			get => routeListUoW;
			set {
				if(routeListUoW == value)
					return;
				routeListUoW = value;
				if(RouteListUoW.Root.Addresses == null)
					RouteListUoW.Root.Addresses = new List<RouteListItem>();
				items = RouteListUoW.Root.ObservableAddresses;
				items.ElementChanged += Items_ElementChanged;
				items.ListChanged += Items_ListChanged;
				items.ElementAdded += Items_ElementAdded;

				UpdateColumns();

				ytreeviewItems.ItemsDataSource = items;
				ytreeviewItems.Reorderable = true;
				CalculateTotal();
			}
		}

		private bool CanEditRows => UserPermissionRepository.CurrentUserPresetPermissions["logistican"]
										&& RouteListUoW.Root.Status != RouteListStatus.Closed
										&& RouteListUoW.Root.Status != RouteListStatus.MileageCheck;

		private bool disableColumnsUpdate;

		public bool DisableColumnsUpdate {
			get => disableColumnsUpdate;
			set {
				if(disableColumnsUpdate == value)
					return;

				disableColumnsUpdate = value;
				if(!disableColumnsUpdate)
					UpdateColumns();
			}
		}

		void Items_ElementAdded(object aList, int[] aIdx)
		{
			UpdateColumns();
			CalculateTotal();
		}

		void Items_ListChanged(object aList)
		{
			UpdateColumns();
		}

		public void OnForwarderChanged()
		{
			UpdateColumns();
		}

		private void UpdateColumns()
		{
			if(disableColumnsUpdate)
				return;

			var goodsColumns = items.SelectMany(i => i.GoodsByRouteColumns.Keys).Distinct().ToArray();

			var config = ColumnsConfigFactory.Create<RouteListItem>()
			.AddColumn("Заказ").SetDataProperty(node => node.Order.Id)
			.AddColumn("Адрес").AddTextRenderer(node => node.Order.DeliveryPoint == null ? "Точка доставки не установлена" : String.Format("{0} д.{1}", node.Order.DeliveryPoint.Street, node.Order.DeliveryPoint.Building))
			.AddColumn("Время").AddTextRenderer(node => node.Order.DeliverySchedule == null ? "" : node.Order.DeliverySchedule.Name);
			if(goodsColumnsCount != goodsColumns.Length) {
				goodsColumnsCount = goodsColumns.Length;

				foreach(var column in ColumnsInfo) {
					if(!goodsColumns.Contains(column.Id))
						continue;
					int id = column.Id;
					config = config.AddColumn(column.Name).AddTextRenderer(a => a.GetGoodsAmountForColumn(id).ToString());
				}
			}
			if(RouteListUoW.Root.Forwarder != null) {
				config
					.AddColumn("C экспедитором")
					.AddToggleRenderer(node => node.WithForwarder).Editing(CanEditRows);
			}
			config
				.AddColumn("Товары").AddTextRenderer(x => ShowAdditional(x.Order.OrderItems))
				.AddColumn("К клиенту")
				.AddTextRenderer(x => x.EquipmentsToClientText, expand: false)
				.AddColumn("От клиента")
				.AddTextRenderer(x => x.EquipmentsFromClientText, expand: false);
			ytreeviewItems.ColumnsConfig =
				config.RowCells().AddSetter<CellRendererText>((c, n) => c.Foreground = n.Order.RowColor)
				.Finish();
		}

		private string ShowAdditional(IList<OrderItem> items)
		{
			List<string> stringParts = new List<string>();

			foreach(var item in items) {
				if(item.Nomenclature.Category == NomenclatureCategory.additional) {
					stringParts.Add(
						string.Format("{0}: {1}", item.Nomenclature.Name, item.Count));
				}
			}

			return string.Join("\n", stringParts);
		}

		public void IsEditable(bool val = false)
		{
			isEditable = val;
			enumbuttonAddOrder.Sensitive = val;
			OnSelectionChanged(this, EventArgs.Empty);
		}

		void Items_ElementChanged(object aList, int[] aIdx)
		{
			UpdateColumns();
			CalculateTotal();
		}

		public RouteListCreateItemsView()
		{
			this.Build();
			enumbuttonAddOrder.ItemsEnum = typeof(AddOrderEnum);
			ytreeviewItems.Selection.Changed += OnSelectionChanged;
		}

		void OnSelectionChanged(object sender, EventArgs e)
		{
			bool selected = ytreeviewItems.Selection.CountSelectedRows() > 0;
			buttonOpenOrder.Sensitive = selected;
			buttonDelete.Sensitive = selected && isEditable;
		}

		GenericObservableList<RouteListItem> items;

		protected void OnButtonDeleteClicked(object sender, EventArgs e)
		{
			if(!RouteListUoW.Root.TryRemoveAddress(ytreeviewItems.GetSelectedObject() as RouteListItem, out string message, new RouteListItemRepository()))
				MessageDialogHelper.RunWarningDialog(
					"Невозможно удалить",
					message,
					ButtonsType.Ok
				);
			CalculateTotal();
		}

		protected void OnEnumbuttonAddOrderEnumItemClicked(object sender, EnumItemClickedEventArgs e)
		{
			AddOrderEnum choice = (AddOrderEnum)e.ItemEnum;
			switch(choice) {
				case AddOrderEnum.AddOrders:
					AddOrders();
					break;
				case AddOrderEnum.AddAllForRegion:
					AddOrdersFromRegion();
					break;
				default:
					break;
			}
		}

		protected void AddOrders()
		{

			var filter = new OrdersFilter(UnitOfWorkFactory.CreateWithoutRoot()) {
				ExceptIds = RouteListUoW.Root.Addresses.Select(address => address.Order.Id).ToArray()
			};

			var geoGrpIds = RouteListUoW.Root.GeographicGroups.Select(x => x.Id).ToArray();
			if(geoGrpIds.Any()) {
				GeographicGroup geographicGroupAlias = null;
				var districtIds = RouteListUoW.Session.QueryOver<ScheduleRestrictedDistrict>()
													  .Left.JoinAlias(d => d.GeographicGroups, () => geographicGroupAlias)
													  .Where(() => geographicGroupAlias.Id.IsIn(geoGrpIds))
													  .Select(
															  Projections.Distinct(
															  Projections.Property<ScheduleRestrictedDistrict>(x => x.Id)
														  )
													  )
													  .List<int>()
													  .ToArray();

				filter.IncludeDistrictsIds = districtIds;
			}

			filter.SetAndRefilterAtOnce(
				x => x.RestrictStartDate = RouteListUoW.Root.Date.Date,
				x => x.RestrictEndDate = RouteListUoW.Root.Date.Date,
				x => x.RestrictStatus = OrderStatus.Accepted,
				x => x.RestrictSelfDelivery = false
			);

			ViewModel.OrdersVM vm = new ViewModel.OrdersVM(filter) {
				CanToggleVisibilityOfColumns = true
			};
			PermissionControlledRepresentationJournal SelectDialog = new PermissionControlledRepresentationJournal(vm, Buttons.None) {
				Mode = JournalSelectMode.Multiple
			};
			SelectDialog.ObjectSelected += (s, ea) => {
				var selectedIds = ea.GetSelectedIds();
				if(!selectedIds.Any()) {
					return;
				}
				foreach(var selectedId in selectedIds) {
					var order = RouteListUoW.GetById<Order>(selectedId);
					RouteListUoW.Root.AddAddressFromOrder(order);
				}
			};
			MyTab.TabParent.AddSlaveTab(MyTab, SelectDialog);
		}

		protected void AddOrdersFromRegion()
		{
			OrmReference SelectDialog = new OrmReference(typeof(ScheduleRestrictedDistrict), RouteListUoW) {
				Mode = OrmReferenceMode.Select,
				ButtonMode = ReferenceButtonMode.CanEdit
			};
			SelectDialog.ObjectSelected += (s, ea) => {
				if(ea.Subject != null) {
					foreach(var order in OrderRepository.GetAcceptedOrdersForRegion(RouteListUoW, RouteListUoW.Root.Date, ea.Subject as ScheduleRestrictedDistrict))
						RouteListUoW.Root.AddAddressFromOrder(order);
				}
			};
			MyTab.TabParent.AddSlaveTab(MyTab, SelectDialog);
		}

		void CalculateTotal()
		{
			var total = routeListUoW.Root.Addresses.SelectMany(a => a.Order.OrderItems)
				.Where(i => i.Nomenclature.Category == NomenclatureCategory.water && i.Nomenclature.TareVolume == TareVolume.Vol19L)
				.Sum(i => i.Count);

			labelSum.LabelProp = String.Format("Всего бутылей: {0}", total);
			UpdateWeightInfo();
		}

		public virtual void UpdateWeightInfo()
		{
			if(RouteListUoW != null && RouteListUoW.Root.Car != null) {
				string maxWeight = RouteListUoW.Root.Car.MaxWeight > 0
								   ? RouteListUoW.Root.Car.MaxWeight.ToString()
								   : " ?";
				string weight = RouteListUoW.Root.HasOverweight()
											? String.Format("<span foreground = \"red\">Перегруз на {0} кг.</span>", RouteListUoW.Root.Overweight())
											: String.Format("<span foreground = \"blue\">Вес груза: {0}/{1} кг.</span>", RouteListUoW.Root.GetTotalWeight(), maxWeight);
				lblWeight.LabelProp = weight;
			}
		}

		protected void OnButtonOpenOrderClicked(object sender, EventArgs e)
		{
			var selected = ytreeviewItems.GetSelectedObject<RouteListItem>();
			if(selected != null) {
				MyTab.TabParent.OpenTab(
					DialogHelper.GenerateDialogHashName<Order>(selected.Order.Id),
					() => new OrderDlg(selected.Order)
				);
			}
		}

		protected void OnYtreeviewItemsRowActivated(object o, RowActivatedArgs args)
		{
			buttonOpenOrder.Click();
		}
	}

	public enum AddOrderEnum
	{
		[Display(Name = "Выбрать заказы...")] AddOrders,
		[Display(Name = "Все заказы для логистического района")] AddAllForRegion
	}
}