﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using Gtk;
using QSOrmProject;
using QSProjectsLib;
using QSTDI;
using Vodovoz.Domain.Logistic;
using Vodovoz.Repository.Logistics;
using Vodovoz.Domain.Employees;
using System.Reflection;
using Chat;
using System.ServiceModel;
using Vodovoz.Repository;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RouteListKeepingDlg : OrmGtkDialogBase<RouteList>
	{		
		public RouteListKeepingDlg(int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<RouteList>(id);
			TabName = String.Format("Ведение маршрутного листа №{0}",Entity.Id);
			ConfigureDlg ();
		}

		Dictionary<RouteListItemStatus, Gdk.Pixbuf> statusIcons = new Dictionary<RouteListItemStatus, Gdk.Pixbuf>();

		List<RouteListKeepingItemNode> items;

		public void ConfigureDlg(){
			referenceCar.Binding.AddBinding(Entity, rl => rl.Car, widget => widget.Subject).InitializeFromSource();
			referenceCar.Sensitive = false;

			referenceDriver.ItemsQuery = Repository.EmployeeRepository.DriversQuery ();
			referenceDriver.Binding.AddBinding(Entity, rl => rl.Driver, widget => widget.Subject).InitializeFromSource();
			referenceDriver.SetObjectDisplayFunc<Employee> (r => StringWorks.PersonNameWithInitials (r.LastName, r.Name, r.Patronymic));
			referenceDriver.Sensitive = false;

			referenceForwarder.ItemsQuery = Repository.EmployeeRepository.ForwarderQuery ();
			referenceForwarder.Binding.AddBinding(Entity, rl => rl.Forwarder, widget => widget.Subject).InitializeFromSource();
			referenceForwarder.SetObjectDisplayFunc<Employee> (r => StringWorks.PersonNameWithInitials (r.LastName, r.Name, r.Patronymic));
			referenceForwarder.Sensitive = false;

			referenceLogistican.ItemsQuery = Repository.EmployeeRepository.ActiveEmployeeQuery();
			referenceLogistican.Binding.AddBinding(Entity, rl => rl.Logistican, widget => widget.Subject).InitializeFromSource();
			referenceLogistican.SetObjectDisplayFunc<Employee> (r => StringWorks.PersonNameWithInitials (r.LastName, r.Name, r.Patronymic));
			referenceLogistican.Sensitive = false;

			speccomboShift.ItemsList = DeliveryShiftRepository.ActiveShifts(UoW);
			speccomboShift.Binding.AddBinding(Entity, rl => rl.Shift, widget => widget.SelectedItem).InitializeFromSource();
			speccomboShift.Sensitive = false;

			yspinPlannedDistance.Binding.AddBinding(Entity, rl => rl.PlannedDistance, widget => widget.ValueAsDecimal).InitializeFromSource();
			yspinPlannedDistance.Sensitive = false;

			yspinActualDistance.Binding.AddBinding(Entity, rl => rl.ActualDistance, widget => widget.ValueAsDecimal).InitializeFromSource();
			yspinActualDistance.Sensitive = false;

			datePickerDate.Binding.AddBinding(Entity, rl => rl.Date, widget => widget.Date).InitializeFromSource();
			datePickerDate.Sensitive = false;

			//Заполняем иконки
			var ass = Assembly.GetAssembly(typeof(MainClass));
			statusIcons.Add(RouteListItemStatus.EnRoute, new Gdk.Pixbuf(ass, "Vodovoz.icons.status.car.png"));
			statusIcons.Add(RouteListItemStatus.Completed, new Gdk.Pixbuf(ass, "Vodovoz.icons.status.face-smile-grin.png"));
			statusIcons.Add(RouteListItemStatus.Overdue, new Gdk.Pixbuf(ass, "Vodovoz.icons.status.face-angry.png"));
			statusIcons.Add(RouteListItemStatus.Canceled, new Gdk.Pixbuf(ass, "Vodovoz.icons.status.face-crying.png"));

			ytreeviewAddresses.ColumnsConfig = ColumnsConfigFactory.Create<RouteListKeepingItemNode>()
				.AddColumn("Заказ")
					.AddTextRenderer(node => node.RouteListItem.Order.Id.ToString())					
				.AddColumn("Адрес")
					.AddTextRenderer(node => String.Format("{0} д.{1}", node.RouteListItem.Order.DeliveryPoint.Street, node.RouteListItem.Order.DeliveryPoint.Building))					
				.AddColumn("Время")
					.AddTextRenderer(node => node.RouteListItem.Order.DeliverySchedule == null ? "" : node.RouteListItem.Order.DeliverySchedule.Name)					
				.AddColumn("Статус")
					.AddPixbufRenderer(x => statusIcons[x.Status])
					.AddEnumRenderer(node => node.Status).Editing(true)					
				.AddColumn("Последнее редактирование")
					.AddTextRenderer(node => node.LastUpdate)
				.AddColumn("Комментарий")
					.AddTextRenderer(node => node.Comment)
						.Editable(true)
				.RowCells ()
					.AddSetter<CellRenderer> ((cell, node) => cell.CellBackgroundGdk = node.RowColor)
				.Finish();
			ytreeviewAddresses.Selection.Mode = SelectionMode.Multiple;
			ytreeviewAddresses.Selection.Changed += OnSelectionChanged;
			UpdateNodes();
		}

		public void UpdateNodes(){
			items = new List<RouteListKeepingItemNode>();
			foreach (var item in Entity.Addresses)
				items.Add(new RouteListKeepingItemNode{ RouteListItem = item });
			ytreeviewAddresses.ItemsDataSource = items;
		}

		public void OnSelectionChanged(object sender, EventArgs args){
			buttonNewRouteList.Sensitive = ytreeviewAddresses.GetSelectedObjects().Count() > 0;
		}

		#region implemented abstract members of OrmGtkDialogBase

		public override bool Save()
		{
			List<int> changedRouteLists = new List<int>();
			foreach (var address in items.Where(item=>item.HasChanged).Select(item=>item.RouteListItem))
			{
				switch (address.Status)
				{
					case RouteListItemStatus.Canceled:
						address.Order.ChangeStatus(Vodovoz.Domain.Orders.OrderStatus.DeliveryCanceled);
						break;
					case RouteListItemStatus.Completed:
						address.Order.ChangeStatus(Vodovoz.Domain.Orders.OrderStatus.Shipped);
						break;
					case RouteListItemStatus.EnRoute:
						address.Order.ChangeStatus(Vodovoz.Domain.Orders.OrderStatus.OnTheWay);
						break;
					case RouteListItemStatus.Overdue:
						address.Order.ChangeStatus(Vodovoz.Domain.Orders.OrderStatus.NotDelivered);
						break;
				}
				UoWGeneric.Save(address.Order);
				changedRouteLists.Add(address.Id);
			}

			UoWGeneric.Save();

			foreach (var id in changedRouteLists)
				getChatService().SendOrderStatusNotificationToDriver(
					EmployeeRepository.GetEmployeeForCurrentUser(UoWGeneric).Id,
					id
				);
			return true;
		}
		#endregion

		protected void OnButtonNewRouteListClicked (object sender, EventArgs e)
		{
			if (TabParent.CheckClosingSlaveTabs(this))
				return;
			if (UoWGeneric.HasChanges && CommonDialogs.SaveBeforeCreateSlaveEntity(EntityObject.GetType(), typeof(RouteList)))
			{
				if (!Save())
					return;
			}
			var selectedObjects = ytreeviewAddresses.GetSelectedObjects();
			var selectedAddreses = selectedObjects
				.Cast<RouteListKeepingItemNode>()
				.Select(item=>item.RouteListItem)
				.Where(item=>item.Status==RouteListItemStatus.EnRoute);

			var dlg = new RouteListCreateDlg(Entity, selectedAddreses);
			dlg.EntitySaved += OnNewRouteListCreated;
			TabParent.AddSlaveTab(this,dlg);
		}

		public void OnNewRouteListCreated(object sender, EntitySavedEventArgs args){
			var newRouteList = args.Entity as RouteList;
			foreach (var address in newRouteList.Addresses)
			{
				var transferedAddress = Entity.ObservableAddresses.FirstOrDefault(item => item.Order.Id == address.Order.Id);
				if (transferedAddress != null)
					Entity.RemoveAddress(transferedAddress);
			}
			UpdateNodes();
			Save();
		}

		static IChatService getChatService()
		{
			return new ChannelFactory<IChatService>(
				new BasicHttpBinding(), 
				"http://vod-srv.qsolution.ru:9000/ChatService").CreateChannel();
		}
	}	

	public class RouteListKeepingItemNode : PropertyChangedBase
	{
		public bool HasChanged=false;
		public Gdk.Color RowColor{
			get{
				switch (RouteListItem.Status){						
					case RouteListItemStatus.Overdue:							
						return new Gdk.Color(0xee,0x66,0x66);
					case RouteListItemStatus.Completed:
						return new Gdk.Color(0x66,0xee,0x66);
					case RouteListItemStatus.Canceled:
						return new Gdk.Color(0xaf,0xaf,0xaf);
					default:
						return new Gdk.Color(0xff,0xff,0xff);
				}
			}
		}

		public RouteListItemStatus Status{
			get{
				return RouteListItem.Status;
			}
			set{
				RouteListItem.UpdateStatus(value);
				HasChanged = true;
				OnPropertyChanged<RouteListItemStatus>(() => Status);
			}
		}

		public string Comment{
			get { return RouteListItem.Comment; }
			set{
				RouteListItem.Comment = value;
				OnPropertyChanged<string>(() => Comment);
			}
		}

		public string LastUpdate {
			get{
				var maybeLastUpdate = RouteListItem.StatusLastUpdate;
				if (maybeLastUpdate.HasValue)
				{
					if (maybeLastUpdate.Value.Date == DateTime.Today)
					{
						return maybeLastUpdate.Value.ToShortTimeString();
					}
					else
						return maybeLastUpdate.Value.ToString();
				}
				else
				{
					return "";
				}
			}
		}

		public RouteListItem RouteListItem{get;set;}
	}
}

