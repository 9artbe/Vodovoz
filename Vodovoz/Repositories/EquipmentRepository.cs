﻿using NHibernate.Criterion;
using Vodovoz.Domain;
using System.Collections.Generic;
using QSOrmProject;
using System.Linq;
using Vodovoz.Domain.Operations;
using Vodovoz.Domain.Orders;

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

		public static QueryOver<Equipment> AvailableEquipmentQuery(){
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
					|| orderAlias.OrderStatus == OrderStatus.NewOrder
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
	}
}

