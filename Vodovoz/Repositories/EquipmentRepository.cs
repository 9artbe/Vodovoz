﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using QSOrmProject;
using Vodovoz.Domain;
using Vodovoz.Domain.Documents;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Operations;
using Vodovoz.Domain.Orders;
using Vodovoz.Domain.Store;

namespace Vodovoz.Repository
{
	public static class EquipmentRepository
	{
		public static QueryOver<Equipment> GetEquipmentWithTypesQuery (List<EquipmentType> types)
		{
			Nomenclature nomenclatureAlias = null;
			var Query = QueryOver.Of<Equipment> ()
				.JoinAlias (e => e.Nomenclature, () => nomenclatureAlias)
				.Where (() => nomenclatureAlias.Type.IsIn (types));
			return Query;
		}

		public static Equipment GetEquipmentForSaleByNomenclature (IUnitOfWork uow, Nomenclature nomenclature)
		{
			return AvailableEquipmentQuery ().GetExecutableQueryOver (uow.Session)
				.Where (eq => eq.Nomenclature.Id == nomenclature.Id)
				.Where (eq => !eq.OnDuty)
				.Take (1)
				.List ().First ();
		}
			
		public static IList<Equipment> GetEquipmentForSaleByNomenclature(IUnitOfWork uow, Nomenclature nomenclature, int count=0, int[] exceptIDs=null){
			if(exceptIDs==null) exceptIDs=new int[0];
			return (count > 0) ? AvailableEquipmentQuery().GetExecutableQueryOver(uow.Session)
				.Where(eq => eq.Nomenclature.Id == nomenclature.Id)
				.Where(eq => !eq.OnDuty)
				.Where(eq => !eq.Id.IsIn(exceptIDs))
				.Take(count)
				.List()
					: new List<Equipment>();
		}

		public static QueryOver<Equipment> AvailableOnDutyEquipmentQuery(){
			return AvailableEquipmentQuery ().Where (equipment => equipment.OnDuty);
		}

		public static QueryOver<Equipment,Equipment> AvailableEquipmentQuery(){
			Vodovoz.Domain.Orders.Order orderAlias = null;
			Equipment equipmentAlias = null;
			WarehouseMovementOperation operationAddAlias = null;
			OrderEquipment orderEquipmentAlias = null;

			var equipmentInStockCriterion = Subqueries.IsNotNull (
				                                QueryOver.Of<WarehouseMovementOperation> (() => operationAddAlias)
				.OrderBy (() => operationAddAlias.OperationTime).Desc
				.Where (() => equipmentAlias.Id == operationAddAlias.Equipment.Id)
				.Select (op => op.IncomingWarehouse)
				.Take (1).DetachedCriteria
			                                );

			var subqueryAllReservedEquipment = QueryOver.Of<Vodovoz.Domain.Orders.Order> (() => orderAlias)
				.Where (() => orderAlias.OrderStatus == OrderStatus.Accepted
					|| orderAlias.OrderStatus == OrderStatus.InTravelList)
				.JoinAlias (() => orderAlias.OrderEquipments, () => orderEquipmentAlias)
				.Where (() => orderEquipmentAlias.Direction == Direction.Deliver)
				.Select (Projections.Property(()=>orderEquipmentAlias.Equipment.Id));

			return QueryOver.Of<Equipment> (() => equipmentAlias)
				.Where (equipmentInStockCriterion)
				.WithSubquery.WhereProperty (()=>equipmentAlias.Id).NotIn (subqueryAllReservedEquipment);
		}

		public static QueryOver<Equipment> GetEquipmentByNomenclature (Nomenclature nomenclature)
		{
			Nomenclature nomenclatureAlias = null;

			return QueryOver.Of<Equipment> ()
				.JoinAlias (e => e.Nomenclature, () => nomenclatureAlias)
				.Where (() => nomenclatureAlias.Id == nomenclature.Id);
		}

		public static QueryOver<Equipment> GetEquipmentAtDeliveryPoint(Counterparty client, DeliveryPoint deliveryPoint)
		{
			Equipment equipmentAlias=null;
			CounterpartyMovementOperation operationAlias = null;
			CounterpartyMovementOperation subsequentOperationAlias = null;

			var subsequentOperationsSubquery = QueryOver.Of<CounterpartyMovementOperation> (() => subsequentOperationAlias)
				.Where (() => operationAlias.Id < subsequentOperationAlias.Id && operationAlias.Equipment == subsequentOperationAlias.Equipment)
				.Select (op=>op.Id);
			
			var availableEquipmentIDsSubquery = QueryOver.Of<CounterpartyMovementOperation> (() => operationAlias)
				.WithSubquery.WhereNotExists(subsequentOperationsSubquery)
				.Where (() => operationAlias.IncomingCounterparty.Id == client.Id)
				.Where (() => operationAlias.IncomingDeliveryPoint.Id == deliveryPoint.Id).Select(op=>op.Equipment.Id);
			return QueryOver.Of<Equipment> (() => equipmentAlias).WithSubquery.WhereProperty (() => equipmentAlias.Id).In (availableEquipmentIDsSubquery);
		}

		public static IList<Equipment> GetEquipmentUnloadedTo(IUnitOfWork uow, Warehouse warehouse, RouteList routeList){
			CarUnloadDocumentItem unloadItemAlias = null;
			WarehouseMovementOperation operationAlias = null;
			Equipment equipmentAlias = null;
			var unloadedEquipmentIdsQuery = QueryOver.Of<CarUnloadDocument>().Where(doc => doc.RouteList.Id == routeList.Id)
				.JoinAlias(doc => doc.Items, () => unloadItemAlias)
				.JoinAlias(() => unloadItemAlias.MovementOperation, () => operationAlias)
				.JoinAlias(() => operationAlias.Equipment, () => equipmentAlias)
				.Select(op => equipmentAlias.Id);
			return uow.Session.QueryOver<Equipment>(()=>equipmentAlias).WithSubquery.WhereProperty(() => equipmentAlias.Id).In(unloadedEquipmentIdsQuery).List();
		}

		public static EquipmentLocation GetLocation(IUnitOfWork uow, Equipment equ)
		{
			var result = new EquipmentLocation();
			var lastWarehouseOp = uow.Session.QueryOver<WarehouseMovementOperation>()
				.Where(o => o.Equipment == equ)
				.OrderBy(o => o.OperationTime).Desc
				.Take(1)
				.SingleOrDefault();
			var lastCouterpartyOp = uow.Session.QueryOver<CounterpartyMovementOperation>()
				.Where(o => o.Equipment == equ)
				.OrderBy(o => o.OperationTime).Desc
				.Take(1)
				.SingleOrDefault();

			if (lastWarehouseOp == null && lastCouterpartyOp == null)
				result.Type = EquipmentLocation.LocationType.NoMovements;
			else if (lastWarehouseOp != null && lastWarehouseOp.IncomingWarehouse != null 
				&& (lastCouterpartyOp == null || lastCouterpartyOp.OperationTime < lastWarehouseOp.OperationTime))
			{
				result.Type = EquipmentLocation.LocationType.Warehouse;
				result.Warehouse = lastWarehouseOp.IncomingWarehouse;
				result.Operation = lastWarehouseOp;
			}
			else if( lastCouterpartyOp != null && lastCouterpartyOp.IncomingCounterparty != null 
				&& (lastWarehouseOp == null || lastWarehouseOp.OperationTime < lastCouterpartyOp.OperationTime))
			{
				result.Type = EquipmentLocation.LocationType.Couterparty;
				result.Operation = lastCouterpartyOp;
				result.Counterparty = lastCouterpartyOp.IncomingCounterparty;
				result.DeliveryPoint = lastCouterpartyOp.IncomingDeliveryPoint;
			}
			else
			{
				result.Type = EquipmentLocation.LocationType.Superposition;
			}

			return result;
		}

		public class EquipmentLocation{

			public LocationType Type { get; set;}

			public OperationBase Operation { get; set;}

			public Warehouse Warehouse { get; set;}

			public Counterparty Counterparty { get; set;}

			public DeliveryPoint DeliveryPoint { get; set;}

			public enum LocationType
			{
				NoMovements,
				Warehouse,
				Couterparty,
				Superposition //Like Quantum
			}

			public string Title{
				get{
					switch (Type)
					{
						case LocationType.NoMovements:
							return "Нет движений в БД";
						case LocationType.Warehouse:
							return String.Format("На складе: {0}", Warehouse.Name);
						case LocationType.Couterparty:
							return String.Format("У {0}{1}", Counterparty.Name,
								DeliveryPoint != null ? " на адресе " + DeliveryPoint.Title : String.Empty);
						case LocationType.Superposition:
							return "В состоянии суперпозиции (как кот Шрёдингера)";
					}
					return null;
				}
			}
		}
	}
}

