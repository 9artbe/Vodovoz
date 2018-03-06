﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using Gtk;
using QSOrmProject;
using Vodovoz.Domain;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Logistic;
using Vodovoz.Repository.Logistics;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RouteListDiscrepancyView : WidgetOnDialogBase
	{
		public RouteListDiscrepancyView()
		{
			this.Build();
			Configure();
			Items = new List<Discrepancy>();
		}

		List<Discrepancy> items;
		public List<Discrepancy> Items{ 
			get{
				return items;
			}
			set{
				items = value;
				ytreeview2.ItemsDataSource = items;
			}
		}

		protected void Configure()
		{
			var colorRed = new Gdk.Color(0xee, 0x66, 0x66);
			var colorWhite = new Gdk.Color(0xff, 0xff, 0xff);
			ytreeview2.ColumnsConfig = ColumnsConfigFactory.Create<Discrepancy>()
				.AddColumn("Название")
					.AddTextRenderer(node => node.Name)
				.AddColumn("Выг-\nрузка")
					.AddNumericRenderer(node => node.ToWarehouse)
				.AddColumn("Не-\nдовоз")
					.AddTextRenderer(node => node.Returns)
				.AddColumn("От \nклиента")
					.AddNumericRenderer(node=>node.PickedUpFromClient)
				.AddColumn("Расхо-\nждения")
					.AddNumericRenderer(node=>node.Remainder)
						.Adjustment(new Gtk.Adjustment(0, 0, 9999, 1, 1, 0))
				.AddColumn("Ущерб")
					.AddTextRenderer(x => x.SumOfDamage.ToString("C"))
				.AddColumn("Штраф")
					.AddToggleRenderer(x => x.UseFine).ToggledEvent(UseFine_Toggled)
				.RowCells()					
					.AddSetter<CellRenderer>((cell,node) => cell.CellBackgroundGdk = node.Remainder==0 ? colorWhite : colorRed)
				.Finish();
		}

		public event EventHandler FineChanged;

		void UseFine_Toggled (object o, ToggledArgs args)
		{
			//Вызываем через Application.Invoke что бы событие вызывалось уже после того как поле обновилось.
			Application.Invoke(delegate {
				if (FineChanged != null)
					FineChanged(this, EventArgs.Empty);
			});
		}	

		public void FindDiscrepancies(IList<RouteListItem> items, List<RouteListRepository.ReturnsNode> allReturnsToWarehouse)
		{
			Items = GetDiscrepancies(items, allReturnsToWarehouse);
		}

		List<Discrepancy> GetDiscrepancies(IList<RouteListItem> items, List<RouteListRepository.ReturnsNode> allReturnsToWarehouse)
		{
			List<Discrepancy> result = new List<Discrepancy>();

			//ТОВАРЫ
			var orderClosingItems = items
				.Where(item => item.TransferedTo == null || item.TransferedTo.NeedToReload)
				.SelectMany(item => item.Order.OrderItems)
				.Where(item => Nomenclature.GetCategoriesForShipment().Contains(item.Nomenclature.Category))
				.ToList();
			foreach(var orderItem in orderClosingItems) {
				var discrepancy = new Discrepancy();
				discrepancy.Nomenclature = orderItem.Nomenclature;
				if(!orderItem.IsDelivered) {
					discrepancy.FromCancelledOrders = orderItem.Count;
				}
				discrepancy.ClientRejected = orderItem.ReturnedCount;
				discrepancy.ToWarehouse = allReturnsToWarehouse.Where(x => x.NomenclatureId == orderItem.Nomenclature.Id).Select(x => x.Amount).FirstOrDefault();
				discrepancy.Name = orderItem.Nomenclature.Name;
				result.Add(discrepancy);
			}

			//ОБОРУДОВАНИЕ
			var orderEquipments = items
				.Where(item => item.TransferedTo == null || item.TransferedTo.NeedToReload)
				.SelectMany(item => item.Order.OrderEquipments)
				.Where(item => Nomenclature.GetCategoriesForShipment().Contains(item.Nomenclature.Category))
				//По тем номенклатурам которых нет в товарах
				.Where(item => !result.Select(x => x.NomenclatureId).ToArray().Contains(item.Nomenclature.Id))
				.ToList();
			foreach(var orderEquip in orderEquipments) {
				var discrepancy = new Discrepancy();
				discrepancy.Nomenclature = orderEquip.Nomenclature;
				if(orderEquip.Direction == Domain.Orders.Direction.Deliver){
					if(!orderEquip.IsDelivered) {
						discrepancy.FromCancelledOrders = orderEquip.Count;
					}
					discrepancy.ClientRejected = orderEquip.Count - orderEquip.ActualCount;
				}else {
					discrepancy.PickedUpFromClient = orderEquip.Count;
				}
				discrepancy.ToWarehouse = allReturnsToWarehouse.Where(x => x.NomenclatureId == orderEquip.Nomenclature.Id).Select(x => x.Amount).FirstOrDefault();
				discrepancy.Name = orderEquip.Nomenclature.Name;
				result.Add(discrepancy);
			}

			//ДОСТАВЛЕНО НА СКЛАД
			var warehouseItems = allReturnsToWarehouse
				//По тем номенклатурам которых нет в товарах и оборудовании
				.Where(item => !result.Select(x => x.NomenclatureId).ToArray().Contains(item.NomenclatureId))
				.ToList();
			foreach(var item in warehouseItems) {
				var discrepancy = new Discrepancy();
				discrepancy.ToWarehouse = item.Amount;
				discrepancy.Name = item.Name;
				result.Add(discrepancy);
			}

			return result;
		}

		//FIXME НЕ используется. При вводе оборудования с серийнымы номерами, возможно понадобится. Удалить если не будет использоваться
		IList<Discrepancy> GetGoodsDiscrepancies(IList<RouteListItem> items, List<RouteListRepository.ReturnsNode> allReturnsToWarehouse)
		{
			var discrepancies = new List<Discrepancy>();
			var orderClosingItems = items
				.Where(item => item.TransferedTo == null || item.TransferedTo.NeedToReload)
				.SelectMany(item => item.Order.OrderItems)
				.Where(item => Nomenclature.GetCategoriesForShipment().Contains(item.Nomenclature.Category))
				.ToList();
			var goodsReturnedFromClient = orderClosingItems.Where(item => !item.Nomenclature.IsSerial)
				.GroupBy(item => item.Nomenclature,
					item => item.ReturnedCount,
					(nomenclature, amounts) => new
					RouteListRepository.ReturnsNode {
						Name = nomenclature.Name,
						NomenclatureId = nomenclature.Id,
						NomenclatureCategory = nomenclature.Category,
						Amount = amounts.Sum(i => i),
						Trackable=false
					}).ToList();
			var goodsToWarehouse = allReturnsToWarehouse.Where(item => !item.Trackable).ToList();
			foreach (var itemFromClient in goodsReturnedFromClient)
			{
				var itemToWarehouse = 
					goodsToWarehouse.FirstOrDefault(item => item.NomenclatureId == itemFromClient.NomenclatureId);

				var failedDeliveryGoodsCount = items.Where(item => !item.IsDelivered())
					.Where(item => item.TransferedTo == null || item.TransferedTo.NeedToReload)
					.SelectMany(item => item.Order.OrderItems)
					.Where(item => Nomenclature.GetCategoriesForShipment().Contains(item.Nomenclature.Category))
					.Where(item => item.Nomenclature.Id == itemFromClient.NomenclatureId)
					.Sum(item => item.ReturnedCount);
				discrepancies.Add(new Discrepancy
					{
						Name = itemFromClient.Name,
						NomenclatureId = itemFromClient.NomenclatureId,
						FromCancelledOrders = failedDeliveryGoodsCount,
						ClientRejected = itemFromClient.Amount-failedDeliveryGoodsCount,
						ToWarehouse = itemToWarehouse?.Amount ?? 0,
						Trackable = false,
					});
			}

			//Заполняем номенклатуры которые были сданы на склад, но в заказах их не было. Их тоже нужно показывать в расхождения.
			foreach(var item in goodsToWarehouse.Where(x => x.NomenclatureCategory != NomenclatureCategory.bottle))
			{
				if (discrepancies.Any (x => x.NomenclatureId == item.NomenclatureId))
					continue;

				discrepancies.Add (new Discrepancy {
					Name = item.Name,
					NomenclatureId = item.NomenclatureId,
					FromCancelledOrders = 0,
					ClientRejected = 0,
					ToWarehouse = item.Amount,
					Trackable = false,
				});
			}

			return discrepancies;
		}

		//FIXME НЕ используется. При вводе оборудования с серийнымы номерами, возможно понадобится. Удалить если не будет использоваться
		IList<Discrepancy> GetEquipmentDiscrepancies(IList<RouteListItem> items, List<RouteListRepository.ReturnsNode> allReturnsToWarehouse)
		{
			var discrepancies = new List<Discrepancy>();
			var equipmentRejectedItems = items
				.SelectMany(item => item.Order.OrderEquipments).Where(item => item.Equipment != null)
				.Where(item=>item.Direction==Vodovoz.Domain.Orders.Direction.Deliver)
				.ToList();

			var equipmentPickedUpItems = items
				.SelectMany(item => item.Order.OrderEquipments).Where(item => item.Equipment != null)
				.Where(item => item.Direction == Vodovoz.Domain.Orders.Direction.PickUp)
				.ToList();

			var equipmentRejectedTypes = equipmentRejectedItems
				.GroupBy(
					item => item.Equipment.Nomenclature.Type,
					item => item.Confirmed ? 0 : 1,
					EquipmentTypeGroupingResult.Selector
				).Where(item=>item.Amount>0);

			var equipmentPickedUpTypes = equipmentPickedUpItems
				.GroupBy(
					item => item.Equipment.Nomenclature.Type,
					item => item.Confirmed ? 1 : 0,
					EquipmentTypeGroupingResult.Selector
				).Where(item => item.Amount > 0);

			var equipmentToWarehouseTypes = allReturnsToWarehouse.Where(item => item.Trackable)
				.GroupBy(
					item => item.EquipmentType,
					item => (int)item.Amount,
					EquipmentTypeGroupingResult.Selector
				).Where(item => item.Amount > 0);

			foreach (var fromClient in equipmentRejectedTypes)
			{
				var toWarehouse = equipmentToWarehouseTypes
					.FirstOrDefault(item => item.EquipmentType.Id == fromClient.EquipmentType.Id);
				var pickedUp = equipmentPickedUpTypes
					.FirstOrDefault(item => item.EquipmentType.Id == fromClient.EquipmentType.Id);
				discrepancies.Add(new Discrepancy
					{
						Name=fromClient.EquipmentType.Name,
						ClientRejected = fromClient.Amount,
						ToWarehouse = toWarehouse!=null ? toWarehouse.Amount : 0,
						PickedUpFromClient = pickedUp!=null ? pickedUp.Amount : 0
					});
			}

			foreach (var toWarehouse in equipmentToWarehouseTypes)
			{
				var fromClient = equipmentRejectedTypes
					.FirstOrDefault(item => item.EquipmentType.Id == toWarehouse.EquipmentType.Id);
				var pickedUp = equipmentPickedUpTypes
					.FirstOrDefault(item => item.EquipmentType.Id == toWarehouse.EquipmentType.Id);
				if (fromClient == null)
				{
					discrepancies.Add(new Discrepancy
						{
							Name=toWarehouse.EquipmentType.Name,
							ClientRejected = 0,
							ToWarehouse = toWarehouse.Amount,
							PickedUpFromClient = pickedUp!=null ? pickedUp.Amount : 0
						});
				}
			}

			foreach (var pickedUp in equipmentPickedUpTypes)
			{
				var fromClient = equipmentRejectedTypes
					.FirstOrDefault(item => item.EquipmentType.Id == pickedUp.EquipmentType.Id);
				var toWarehouse = equipmentToWarehouseTypes
					.FirstOrDefault(item => item.EquipmentType.Id == pickedUp.EquipmentType.Id);
				if (fromClient == null && toWarehouse == null)
				{
					discrepancies.Add(new Discrepancy
						{
							Name = pickedUp.EquipmentType.Name,
							ClientRejected = 0,
							ToWarehouse = 0,
							PickedUpFromClient = pickedUp.Amount
						});
				}
			}
			return discrepancies;
		}
	}

	public class EquipmentTypeGroupingResult
	{
		public EquipmentType EquipmentType{get;set;}
		public int Amount{get;set;}
		public static EquipmentTypeGroupingResult Selector(EquipmentType type, IEnumerable<int> amounts)
		{
			return new EquipmentTypeGroupingResult
			{
				EquipmentType = type,
				Amount = amounts.Sum()
			};
		}
	}

	public class Discrepancy
	{
		public int Id { get; set; }
		public string Name{get;set;}

		private Nomenclature nomenclature;
		public Nomenclature Nomenclature {
			get {
				return nomenclature;
			}

			set {
				nomenclature = value;
				NomenclatureId = nomenclature.Id;
				NomenclatureCategory = nomenclature.Category;
			}
		}
		public int NomenclatureId { get; set; }
		public NomenclatureCategory NomenclatureCategory { get; set; }
		public decimal PickedUpFromClient{ get; set; }
		public decimal ClientRejected{ get; set; }
		public decimal ToWarehouse{ get; set;}
		public decimal FromCancelledOrders{ get; set;}
		public decimal Remainder{
			get{
				return ToWarehouse - ClientRejected - FromCancelledOrders - PickedUpFromClient;
			}
		}
		public string Returns{
			get{
				return ClientRejected > 0 
					? String.Format("{0}({1:+0;-0})", FromCancelledOrders, ClientRejected) 
						: String.Format("{0}", FromCancelledOrders);
			}
		}
		public bool Trackable{ get; set; }
		public string Serial{ get { 
				if (Trackable) {
					return Id > 0 ? Id.ToString () : "(не определен)";
				} else
					return String.Empty;
			}
		}

		public bool UseFine{ get; set;}

		public decimal SumOfDamage{
			get
			{
				if (Nomenclature == null)
					return 0;
				return Nomenclature.SumOfDamage * (-Remainder);
			}
		}
	}

}

