﻿using NHibernate.Criterion;
using Vodovoz.Domain;
using System.Collections.Generic;
using QSOrmProject;
using System.Linq;
using Vodovoz.Domain.Operations;
using Vodovoz.Domain.Orders;
using Vodovoz.Domain.Store;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Documents;

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

		public static QueryOver<Equipment> GetEquipmentAtDeliveryPointQuery(Counterparty client, DeliveryPoint deliveryPoint)
		{
			Equipment equipmentAlias=null;
			CounterpartyMovementOperation operationAlias = null;
			CounterpartyMovementOperation subsequentOperationAlias = null;

			var subsequentOperationsSubquery = QueryOver.Of<CounterpartyMovementOperation> (() => subsequentOperationAlias)
				.Where (() => operationAlias.Id < subsequentOperationAlias.Id && operationAlias.Equipment == subsequentOperationAlias.Equipment)
				.Select (op=>op.Id);
			
			var availableEquipmentIDsSubquery = QueryOver.Of<CounterpartyMovementOperation>(() => operationAlias)
				.WithSubquery.WhereNotExists(subsequentOperationsSubquery)
				.Where (() => operationAlias.IncomingCounterparty.Id == client.Id);
			if (deliveryPoint != null)
				availableEquipmentIDsSubquery
					.Where(() => operationAlias.IncomingDeliveryPoint.Id == deliveryPoint.Id);
			availableEquipmentIDsSubquery
				.Select(op=>op.Equipment.Id);
			return QueryOver.Of<Equipment> (() => equipmentAlias).WithSubquery.WhereProperty (() => equipmentAlias.Id).In (availableEquipmentIDsSubquery);
		}

		public static IList<Equipment> GetEquipmentAtDeliveryPoint(IUnitOfWork uow, DeliveryPoint deliveryPoint)
		{
			return GetEquipmentAtDeliveryPointQuery(deliveryPoint.Counterparty, deliveryPoint)
				.GetExecutableQueryOver(uow.Session)
				.List();
		}

		public static IList<Equipment> GetEquipmentForClient(IUnitOfWork uow, Counterparty counterparty)
		{
			return GetEquipmentAtDeliveryPointQuery(counterparty, null)
				.GetExecutableQueryOver(uow.Session)
				.List();
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
	}
}

