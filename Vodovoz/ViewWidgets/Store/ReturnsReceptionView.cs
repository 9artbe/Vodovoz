﻿using System;
using System.Data.Bindings.Collections.Generic;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Orders;
using Vodovoz.Domain.Store;
using QSOrmProject;
using Vodovoz.Domain.Logistic;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Linq;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ReturnsReceptionView : Gtk.Bin
	{
		GenericObservableList<ReceptionItemNode> ReceptionReturnsList = new GenericObservableList<ReceptionItemNode>();

		public ReturnsReceptionView()
		{
			this.Build();

			ytreeReturns.ColumnsConfig = Gamma.GtkWidgets.ColumnsConfigFactory.Create<ReceptionItemNode> ()
				.AddColumn ("Номенклатура").AddTextRenderer (node => node.Name)
				.AddColumn ("Серийный номер").AddTextRenderer (node => node.Serial)
				.AddColumn ("Кол-во")
				.AddToggleRenderer (node => node.Returned, false)						
				.AddSetter ((cell, node) => cell.Visible = node.Trackable)
				.AddNumericRenderer (node => node.Amount, false)
				.Adjustment (new Gtk.Adjustment (0, 0, 9999, 1, 100, 0))
				.AddSetter ((cell, node) => cell.Editable = node.EquipmentId==0)
				.AddColumn ("")
				.Finish ();

			ytreeReturns.ItemsDataSource = ReceptionReturnsList;
		}

		private IUnitOfWork uow;

		public IUnitOfWork UoW {
			get {
				return uow;
			}
			set {
				if (uow == value)
					return;
				uow = value;
			}
		}

		Warehouse warehouse;
		public Warehouse Warehouse
		{
			get
			{
				return warehouse;
			}
			set
			{
				warehouse = value;
				FillListReturnsFromRoute();
			}
		}

		RouteList routeList;
		public RouteList RouteList
		{
			get
			{
				return routeList;
			}
			set
			{
				if (routeList == value)
					return;
				routeList = value;
				if (routeList != null)
				{
					FillListReturnsFromRoute();
				}	
				else
				{
					ReceptionReturnsList.Clear();
				}

			}
		}

		public IList<Equipment> AlreadyUnloadedEquipment;

		void FillListReturnsFromRoute(){
			ReceptionReturnsList.Clear ();

			if (Warehouse == null || RouteList == null)
				return;

			ReceptionItemNode resultAlias = null;
			Vodovoz.Domain.Orders.Order orderAlias = null;
			Equipment equipmentAlias = null;
			Nomenclature nomenclatureAlias = null;
			OrderItem orderItemsAlias = null;
			OrderEquipment orderEquipmentAlias = null;

			var returnableItems = UoW.Session.QueryOver<RouteListItem> ().Where (r => r.RouteList.Id == RouteList.Id)
				.JoinAlias (rli => rli.Order, () => orderAlias)
				.JoinAlias (() => orderAlias.OrderItems, () => orderItemsAlias)
				.JoinAlias (() => orderItemsAlias.Nomenclature, () => nomenclatureAlias)
				.Where (() => !nomenclatureAlias.Serial)	
				.Where (Restrictions.Or (
					Restrictions.On (() => nomenclatureAlias.Warehouse).IsNull,
					Restrictions.Eq (Projections.Property (() => nomenclatureAlias.Warehouse), Warehouse)
				))
				.Where (() => nomenclatureAlias.Category != NomenclatureCategory.rent
					&& nomenclatureAlias.Category != NomenclatureCategory.deposit)
				.SelectList (list => list					
					.SelectGroup (() => nomenclatureAlias.Id).WithAlias (() => resultAlias.NomenclatureId)
					.Select (() => nomenclatureAlias.Name).WithAlias (() => resultAlias.Name)
					.Select (() => false).WithAlias (() => resultAlias.Trackable)
					.Select (() => nomenclatureAlias.Category).WithAlias (() => resultAlias.NomenclatureCategory)
				)
				.TransformUsing (Transformers.AliasToBean<ReceptionItemNode> ())
				.List<ReceptionItemNode> ();

			var returnableEquipment = UoW.Session.QueryOver<RouteListItem> ().Where (r => r.RouteList.Id == RouteList.Id)
				.JoinAlias (rli => rli.Order, () => orderAlias)
				.JoinAlias (() => orderAlias.OrderEquipments, () => orderEquipmentAlias)
				.JoinAlias (() => orderEquipmentAlias.Equipment, () => equipmentAlias)
				.JoinAlias (() => equipmentAlias.Nomenclature, () => nomenclatureAlias)
				.Where(()=>orderEquipmentAlias.Direction==Vodovoz.Domain.Orders.Direction.Deliver)
				.Where (Restrictions.Or (
					Restrictions.On (() => nomenclatureAlias.Warehouse).IsNull,
					Restrictions.Eq (Projections.Property (() => nomenclatureAlias.Warehouse), Warehouse)
				))
				.Where (() => nomenclatureAlias.Category != NomenclatureCategory.rent
					&& nomenclatureAlias.Category != NomenclatureCategory.deposit)				
				.SelectList (list => list
					.Select (() => equipmentAlias.Id).WithAlias (() => resultAlias.EquipmentId)				
					.Select (() => nomenclatureAlias.Id).WithAlias (() => resultAlias.NomenclatureId)
					.Select (() => nomenclatureAlias.Name).WithAlias (() => resultAlias.Name)
					.Select (() => nomenclatureAlias.Serial).WithAlias (() => resultAlias.Trackable)
					.Select (() => nomenclatureAlias.Category).WithAlias (() => resultAlias.NomenclatureCategory)
				)
				.TransformUsing (Transformers.AliasToBean<ReceptionItemNode> ())
				.List<ReceptionItemNode> ();

			foreach (var item in returnableItems) {
				ReceptionReturnsList.Add (item);
			}
			foreach (var equipment in returnableEquipment)
			{
				if (!AlreadyUnloadedEquipment.Any(eq => eq.Id == equipment.EquipmentId))
					ReceptionReturnsList.Add(equipment);
			}

		}
	}
}

