﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Gamma.Binding;
using Gamma.ColumnConfig;
using Gdk;
using GMap.NET;
using GMap.NET.GtkSharp;
using GMap.NET.GtkSharp.Markers;
using GMap.NET.MapProviders;
using QSOrmProject;
using QSProjectsLib;
using QSTDI;
using Vodovoz.Additions.Logistic;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Orders;
using QSWidgetLib;

namespace Vodovoz
{
	public partial class RoutesAtDayDlg : TdiTabBase, ITdiDialog
	{
		static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();
		private IUnitOfWork uow = UnitOfWorkFactory.CreateWithoutRoot();
		private readonly GMapOverlay addressesOverlay = new GMapOverlay("addresses");
		IList<Order> ordersAtDay;
		IList<RouteList> routesAtDay;
		int addressesWithoutCoordinats, addressesWithoutRoutes;
		Pixbuf[] pixbufMarkers;

		#region Свойства
		private bool hasNoChanges;

		public bool HasNoChanges  {
			get {return hasNoChanges; }

			private set {
				hasNoChanges = value;

				ydateForRoutes.Sensitive = checkShowCompleted.Sensitive
					= hasNoChanges;
			}
		}

		#endregion

		public override string TabName
		{
			get
			{
				return String.Format("Маршруты на {0:d}", ydateForRoutes.Date);
			}
			protected set
			{
				throw new InvalidOperationException("Установка протеворечит логике работы.");
			}
		}

		public RoutesAtDayDlg()
		{
			this.Build();

			if (progressOrders.Adjustment == null)
				progressOrders.Adjustment = new Gtk.Adjustment(0, 0, 0, 1, 1, 0);

			//Configure map
			gmapWidget.MapProvider = GMapProviders.OpenStreetMap;
			gmapWidget.Position = new PointLatLng(59.93900, 30.31646);
			gmapWidget.HeightRequest = 150;
			gmapWidget.HasFrame = true;
			gmapWidget.Overlays.Add(addressesOverlay);
			gmapWidget.DisableAltForSelection = true;
			gmapWidget.OnSelectionChange += GmapWidget_OnSelectionChange;

			yenumcomboMapType.ItemsEnum = typeof(MapProviders);

			ytreeRoutes.ColumnsConfig = FluentColumnsConfig <object>.Create()
				.AddColumn("МЛ/Адрес").AddTextRenderer(x => GetRowTitle(x))
				.AddColumn("Время").AddTextRenderer(x => GetRowTime(x))
				.AddColumn("Бутылей").AddTextRenderer(x => GetRowBottles(x))
				.AddColumn("Маркер").AddPixbufRenderer(x => GetRowMarker(x))
				.Finish();

			ytreeRoutes.Selection.Changed += YtreeRoutes_Selection_Changed;

			ydateForRoutes.Date = DateTime.Today;

			OrmMain.GetObjectDescription<RouteList>().ObjectUpdatedGeneric += RouteListExternalUpdated;
		}

		void RouteListExternalUpdated (object sender, QSOrmProject.UpdateNotification.OrmObjectUpdatedGenericEventArgs<RouteList> e)
		{
			List<RouteList> routeLists = e.UpdatedSubjects
											.Where(rl => rl.Date.Date == ydateForRoutes.Date.Date)
											.ToList<RouteList>();
			
			bool foundRL = routeLists?.Count > 0;

			if (foundRL)
			{
				bool answer;
				if (HasNoChanges)
					answer = false;
				else
					answer = MessageDialogWorks.RunQuestionDialog(
						"Сохраненный маршрут открыт на вкадке маршруты за день." +
						"При продолжении работы в этой вкладке внесенные внешние измменения могут быть потеряны. " +
						"При отмене, данные в этом диалоге будут перезаписаны." +
						"\nПродолжить работу в этой вкладке?");
				if (!answer)
					FillDialogAtDay();
			}
		}

		void YtreeRoutes_Selection_Changed (object sender, EventArgs e)
		{
			var row = ytreeRoutes.GetSelectedObject();
			buttonRemoveAddress.Sensitive = row is RouteListItem && !checkShowCompleted.Active;
			buttonOpen.Sensitive = (row is RouteListItem) || (row is RouteList);
		}

		void GmapWidget_OnSelectionChange (RectLatLng Selection, bool ZoomToFit)
		{
			var selected = addressesOverlay.Markers.Where(m => Selection.Contains(m.Position)).ToList();
			var selectedBottle = selected.Select(x => x.Tag).Cast<Order>().Sum(o => o.TotalDeliveredBottles);
			labelSelected.LabelProp = String.Format("Выбрано адресов: {0}\nБутылей: {1}", selected.Count, selectedBottle);
			menuAddToRL.Sensitive = selected.Count > 0 && routesAtDay.Count > 0 && !checkShowCompleted.Active;
		}

		string GetRowTitle(object row)
		{
			if(row is RouteList)
			{
				var rl = (RouteList)row;
				return String.Format("Маршрутный лист №{0} - {1}({2})",
					rl.Id,
					rl.Driver.ShortName,
					rl.Car.RegistrationNumber
				);
			}
			if(row is RouteListItem)
			{
				var rli = (RouteListItem)row;
				return rli.Order.DeliveryPoint.ShortAddress;
			}
			return null;
		}

		string GetRowTime(object row)
		{
			var rl = row as RouteList;
			if (rl != null)
				return rl.Addresses.Count.ToString();
			return (row as RouteListItem)?.Order.DeliverySchedule.Name;
		}

		string GetRowBottles(object row)
		{
			var rl = row as RouteList;
			if (rl != null)
				return rl.Addresses.Sum(x => x.Order.TotalDeliveredBottles).ToString();
			
			var rli = row as RouteListItem;
			if(rli != null)
				return rli.Order.TotalDeliveredBottles.ToString();
			return null;
		}

		Pixbuf GetRowMarker(object row)
		{
			var rl = row as RouteList;
			if (rl == null)
			{
				var rli = row as RouteListItem;
				if (rli != null)
					rl = rli.RouteList;
			}
			if (rl != null)
				return pixbufMarkers[routesAtDay.IndexOf(rl)];
			else
				return null;
		}

		void FillDialogAtDay()
		{
			logger.Info("Загружаем заказы на {0:d}...", ydateForRoutes.Date);
			uow.Session.Clear();

			var ordersQuery = Repository.OrderRepository.GetOrdersForRLEditingQuery(ydateForRoutes.Date, checkShowCompleted.Active)
				.GetExecutableQueryOver(uow.Session)
				.Fetch(x => x.DeliveryPoint).Eager
				.Future();

			Repository.OrderRepository.GetOrdersForRLEditingQuery(ydateForRoutes.Date, checkShowCompleted.Active)
				.GetExecutableQueryOver(uow.Session)
				.Fetch(x => x.OrderItems).Eager
				.Future();

			ordersAtDay = ordersQuery.ToList();

			var routesQuery1 = Repository.Logistics.RouteListRepository.GetRoutesAtDay(ydateForRoutes.Date)
				.GetExecutableQueryOver(uow.Session);
			if (!checkShowCompleted.Active)
				routesQuery1.Where(x => x.Status == RouteListStatus.New);
			var routesQuery = routesQuery1
				.Fetch(x => x.Addresses).Default
				.Future();

			var routesQuery2 = Repository.Logistics.RouteListRepository.GetRoutesAtDay(ydateForRoutes.Date)
				.GetExecutableQueryOver(uow.Session);
			if (!checkShowCompleted.Active)
				routesQuery2.Where(x => x.Status == RouteListStatus.New);
			routesQuery2
				.Where(x => x.Status == RouteListStatus.New)
				.Fetch(x => x.Driver).Eager
				.Fetch(x => x.Car).Eager
				.Future();

			routesAtDay = routesQuery.ToList();
			routesAtDay.ToList().ForEach(rl => rl.UoW = uow);

			UpdateRoutesPixBuf();
			UpdateRoutesButton();

			var levels = LevelConfigFactory.FirstLevel<RouteList, RouteListItem>(x => x.Addresses).LastLevel(c => c.RouteList).EndConfig();
			ytreeRoutes.YTreeModel = new LevelTreeModel<RouteList>(routesAtDay, levels);

			UpdateAddressesOnMap();
		}

		void UpdateAddressesOnMap()
		{
			logger.Info("Обновляем адреса на карте...");
			addressesWithoutCoordinats = 0;
			addressesWithoutRoutes = 0;
			addressesOverlay.Clear();

			foreach(var order in ordersAtDay)
			{
				var route = routesAtDay.FirstOrDefault(rl => rl.Addresses.Any(a => a.Order.Id == order.Id));

				if (route == null)
					addressesWithoutRoutes++;

				if (order.DeliveryPoint.Latitude.HasValue && order.DeliveryPoint.Longitude.HasValue)
				{
					GMarkerGoogleType type;
					if (route == null)
						type = GMarkerGoogleType.black_small;
					else
						type = GetAddressMarker(routesAtDay.IndexOf(route));
					var addressMarker = new GMarkerGoogle(new PointLatLng((double)order.DeliveryPoint.Latitude, (double)order.DeliveryPoint.Longitude),	type);
					addressMarker.Tag = order;
					addressMarker.ToolTipText = String.Format("{0}\nБутылей: {1}",
						order.DeliveryPoint.ShortAddress,
						order.TotalDeliveredBottles
					);
					if (route != null)
						addressMarker.ToolTipText += String.Format(" Везёт: {0}", route.Driver.ShortName);
					addressesOverlay.Markers.Add(addressMarker);
				}
				else
					addressesWithoutCoordinats++;
			}
			UpdateOrdersInfo();
			logger.Info("Ок.");
		}

		protected void OnYdateForRoutesDateChanged(object sender, EventArgs e)
		{
			FillDialogAtDay();
			OnTabNameChanged();
		}

		protected void OnYenumcomboMapTypeChangedByUser(object sender, EventArgs e)
		{
			gmapWidget.MapProvider = MapProvidersHelper.GetPovider((MapProviders)yenumcomboMapType.SelectedItem);
		}

		void UpdateOrdersInfo()
		{
			var text = new List<string>();
			text.Add(RusNumber.FormatCase(ordersAtDay.Count, "На день {0} заказ.", "На день {0} заказа.", "На день {0} заказов."));
			if (addressesWithoutCoordinats > 0)
				text.Add(String.Format("Из них {0} без координат.", addressesWithoutCoordinats));
			if (addressesWithoutRoutes > 0)
				text.Add(String.Format("Из них {0} без маршрутных листов.", addressesWithoutRoutes));
			
			text.Add(RusNumber.FormatCase(routesAtDay.Count, "Всего {0} маршрутный лист.", "Всего {0} маршрутных листа.", "Всего {0} маршрутных листов.") );

			textOrdersInfo.Buffer.Text = String.Join("\n", text);

			progressOrders.Adjustment.Upper = ordersAtDay.Count;
			progressOrders.Adjustment.Value = ordersAtDay.Count - addressesWithoutRoutes;
			if (ordersAtDay.Count == 0)
				progressOrders.Text = String.Empty;
			else if (addressesWithoutRoutes == 0)
				progressOrders.Text = "Готово.";
			else
				progressOrders.Text = RusNumber.FormatCase(addressesWithoutRoutes, "Остался {0} заказ", "Осталось {0} заказа", "Осталось {0} заказов");
		}

		private GMarkerGoogleType[] pointMarkers = new []{
			GMarkerGoogleType.blue_small,
			GMarkerGoogleType.brown_small,
			GMarkerGoogleType.green_small,
			GMarkerGoogleType.yellow_small,
			GMarkerGoogleType.orange_small,
			GMarkerGoogleType.purple_small,
			GMarkerGoogleType.red_small,
			GMarkerGoogleType.gray_small,
			GMarkerGoogleType.white_small,
		};

		GMarkerGoogleType GetAddressMarker(int routeNum)
		{
			var markerNum = routeNum % 9;
			return pointMarkers[markerNum];
		}

		void UpdateRoutesPixBuf()
		{
			if (pixbufMarkers != null && routesAtDay.Count == pixbufMarkers.Length)
				return;
			pixbufMarkers = new Pixbuf[routesAtDay.Count];
			for(int i = 0; i < routesAtDay.Count; i++)
			{
				pixbufMarkers[i] =  PixbufFromBitmap(GMarkerGoogle.GetIcon(GetAddressMarker(i).ToString()));
			}
		}

		void RoutesWasUpdated()
		{
			HasNoChanges = false;
			ytreeRoutes.YTreeModel.EmitModelChanged();
		}

		void UpdateRoutesButton()
		{
			var menu = new Gtk.Menu();
			foreach(var route in routesAtDay)
			{
				var name = String.Format("МЛ №{0} - {1}", route.Id, route.Driver.ShortName);
				var item = new MenuItemId<RouteList>(name);
				item.ID = route;
				item.Activated += AddToRLItem_Activated;
				menu.Append(item);
			}
			menu.ShowAll();
			menuAddToRL.Menu = menu;
		}

		void AddToRLItem_Activated (object sender, EventArgs e)
		{
			var selectedOrders = addressesOverlay.Markers
				.Where(m => gmapWidget.SelectedArea.Contains(m.Position))
				.Select(x => x.Tag).Cast<Order>().ToList();

			var route = ((MenuItemId<RouteList>)sender).ID;

			foreach(var order in selectedOrders)
			{
				if(order.OrderStatus == OrderStatus.InTravelList)
				{
					var alreadyIn = routesAtDay.FirstOrDefault(rl => rl.Addresses.Any(a => a.Order.Id == order.Id));
					if (alreadyIn == null)
						throw new InvalidProgramException(String.Format("Маршрутный лист, в котором добавлен заказ {0} не найден.", order.Id));
					if (alreadyIn.Id == route.Id) // Уже в нужном маршрутном листе.
						continue;
						
					alreadyIn.RemoveAddress(alreadyIn.Addresses.First(x => x.Order.Id == order.Id));
					uow.Save(alreadyIn);
				}
				route.AddAddressFromOrder(order);
				uow.Save(route);
			}
			logger.Info("В МЛ №{0} добавлено {1} адресов.", route.Id, selectedOrders.Count);
			UpdateAddressesOnMap();
			RoutesWasUpdated();
		}

		private static Gdk.Pixbuf PixbufFromBitmap (Bitmap bitmap)
		{
			using(MemoryStream ms = new MemoryStream ())
			{
				bitmap.Save (ms, ImageFormat.Png);
				ms.Position = 0;
				return new Gdk.Pixbuf (ms); 
			}
		}

		protected void OnButtonSaveChangesClicked(object sender, EventArgs e)
		{
			Save();
		}

		protected void OnButtonCancelChangesClicked(object sender, EventArgs e)
		{
			uow.Session.Clear();
			HasNoChanges = true;
			FillDialogAtDay();
		}

		protected void OnButtonRemoveAddressClicked(object sender, EventArgs e)
		{
			var row = ytreeRoutes.GetSelectedObject<RouteListItem>();
			var route = row.RouteList;
			route.RemoveAddress(row);
			uow.Save(route);
			UpdateAddressesOnMap();
			RoutesWasUpdated();
		}

		protected void OnCheckShowCompletedToggled(object sender, EventArgs e)
		{
			FillDialogAtDay();
			buttonSaveChanges.Sensitive = !checkShowCompleted.Active;
		}

		#region TDIDialog

		public event EventHandler<EntitySavedEventArgs> EntitySaved;

		public bool Save()
		{
			uow.Commit();
			HasNoChanges = true;
			FillDialogAtDay();
			return true;
		}

		public void SaveAndClose()
		{
			throw new NotImplementedException();
		}

		public bool HasChanges
		{
			get
			{
				return uow.HasChanges;
			}
		}


		protected void OnButtonOpenClicked (object sender, EventArgs e)
		{
			var row = ytreeRoutes.GetSelectedObject();
			//Открываем заказ
			if (row is RouteListItem)
			{
				Order order = (row as RouteListItem).Order;
				TabParent.OpenTab(
					OrmMain.GenerateDialogHashName<Order>(order.Id),
					() => new OrderDlg (order)
				);
			}
			//Открываем МЛ
			if (row is RouteList)
			{
				RouteList routeList = row as RouteList;
				if (!HasNoChanges)
				{
					if (MessageDialogWorks.RunQuestionDialog("Сохранить маршрутный лист перед открытием?"))
						Save();
					else
						return;
				}
				TabParent.OpenTab(
					OrmMain.GenerateDialogHashName<RouteList>(routeList.Id),
					() => new RouteListCreateDlg (routeList)
				);
			}
		}
		#endregion
	}
}

