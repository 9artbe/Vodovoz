﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Transform;
using QS.BusinessCommon.Domain;
using QS.DomainModel.UoW;
using Vodovoz.Domain;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Operations;
using Vodovoz.Domain.Orders;
using Vodovoz.EntityRepositories.Nodes;
using Vodovoz.Services;

namespace Vodovoz.EntityRepositories.Goods
{
	public class NomenclatureRepository : INomenclatureRepository {
		
		private readonly INomenclatureParametersProvider nomenclatureParametersProvider;

		public NomenclatureRepository(INomenclatureParametersProvider nomenclatureParametersProvider) {
			this.nomenclatureParametersProvider = nomenclatureParametersProvider ?? 
				throw new ArgumentNullException(nameof(nomenclatureParametersProvider));
		}
		
		public QueryOver<Nomenclature> NomenclatureForProductMaterialsQuery()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category.IsIn(Nomenclature.GetCategoriesForProductMaterial()))
							.Where(n => !n.IsArchive);
		}

		public QueryOver<Nomenclature> NomenclatureEquipmentsQuery()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category == NomenclatureCategory.equipment)
							.Where(n => !n.IsArchive);
		}

		public QueryOver<Nomenclature> NomenclatureForSaleQuery()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category.IsIn(Nomenclature.GetCategoriesForSale()))
							.Where(n => !n.IsArchive);
		}

		public QueryOver<Nomenclature> NomenclatureByCategory(NomenclatureCategory category)
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category == category)
							.Where(n => !n.IsArchive);
		}

		/// <summary>
		/// Запрос номенклатур которые можно использовать на складе
		/// </summary>
		public QueryOver<Nomenclature> NomenclatureOfGoodsOnlyQuery()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category.IsIn(Nomenclature.GetCategoriesForGoods()))
							.Where(n => !n.IsArchive);
		}

		public QueryOver<Nomenclature> NomenclatureOfGoodsWithoutEmptyBottlesQuery()
		{
			return QueryOver.Of<Nomenclature>()
							.Where(n => n.Category.IsIn(Nomenclature.GetCategoriesForGoodsWithoutEmptyBottles()))
							.Where(n => !n.IsArchive);
		}

		public QueryOver<Nomenclature> NomenclatureWaterOnlyQuery()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category == NomenclatureCategory.water)
							.Where(n => !n.IsArchive);
		}

		public QueryOver<Nomenclature> NomenclatureEquipOnlyQuery()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category == NomenclatureCategory.equipment)
							.Where(n => !n.IsArchive);
		}

		public Nomenclature GetDefaultBottleNomenclature(IUnitOfWork uow) =>
			nomenclatureParametersProvider.GetDefaultBottleNomenclature(uow);

		/// <summary>
		/// Возвращает список номенклатур, которые зависят от передаваемой номенклатуры.
		/// </summary>
		/// <returns>Список зависимых номенклатур.</returns>
		/// <param name="uow">uow - Unit of work</param>
		/// <param name="influentialNomenclature">influentialNomenclature - вляющая номенклатура</param>
		public IList<Nomenclature> GetDependedNomenclatures(IUnitOfWork uow, Nomenclature influentialNomenclature)
		{
			return uow.Session.QueryOver<Nomenclature>()
					  .Where(n => n.DependsOnNomenclature.Id == influentialNomenclature.Id)
					  .List();
		}

		public QueryOver<Nomenclature> NomenclatureOfItemsForService()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category == NomenclatureCategory.equipment)
							.Where(n => !n.IsArchive);
		}

		public QueryOver<Nomenclature> NomenclatureOfPartsForService()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category == NomenclatureCategory.spare_parts)
							.Where(n => !n.IsArchive);
		}

		public QueryOver<Nomenclature> NomenclatureOfServices()
		{
			return QueryOver.Of<Nomenclature>()
				.Where(n => n.Category == NomenclatureCategory.service)
							.Where(n => !n.IsArchive);
		}

		public IList<Nomenclature> GetNomenclatureOfDefectiveGoods(IUnitOfWork uow)
		{
			return uow.Session.QueryOver<Nomenclature>()
				.Where(n => n.IsDefectiveBottle).List();
		}

		public string GetNextCode1c(IUnitOfWork uow)
		{
			var lastCode1c = uow.Query<Nomenclature>()
								.Where(n => n.Code1c.IsLike(Nomenclature.PrefixOfCode1c, MatchMode.Start))
								.OrderBy(n => n.Code1c).Desc
								.Select(n => n.Code1c)
								.Take(1)
								.SingleOrDefault<string>();
			int id = 0;
			if(!String.IsNullOrEmpty(lastCode1c)) {
				id = int.Parse(lastCode1c.Replace(Nomenclature.PrefixOfCode1c, ""));//Тут специально падаем в эксепшен если не смогли распарсить, подума 5 раз, пережде чем заменить на TryParse
			}
			id++;
			string format = new String('0', Nomenclature.LengthOfCode1c - Nomenclature.PrefixOfCode1c.Length);
			return Nomenclature.PrefixOfCode1c + id.ToString(format);
		}

		public QueryOver<Nomenclature> NomenclatureInGroupsQuery(int[] groupsIds)
		{
			return QueryOver.Of<Nomenclature>()
							.Where(n => n.ProductGroup.Id.IsIn(groupsIds));
		}

		public Nomenclature GetNomenclatureToAddWithMaster(IUnitOfWork uow) =>
			nomenclatureParametersProvider.GetNomenclatureToAddWithMaster(uow);
		
		public Nomenclature GetForfeitNomenclature(IUnitOfWork uow) => nomenclatureParametersProvider.GetForfeitNomenclature(uow);
		
		public Nomenclature GetSanitisationNomenclature(IUnitOfWork uow) => nomenclatureParametersProvider.GetSanitisationNomenclature(uow);
		
		public IList<Nomenclature> GetNomenclatureWithPriceForMobileApp(IUnitOfWork uow, params MobileCatalog[] catalogs)
		{
			return uow.Session.QueryOver<Nomenclature>()
							  .Where(n => !n.IsArchive)
							  .Where(n => n.MobileCatalog.IsIn(catalogs))
							  .List();
		}

		#region Rent

		/// <summary>
		/// Возвращает доступное оборудование указанного типа для аренды
		/// </summary>
		public Nomenclature GetAvailableNonSerialEquipmentForRent(IUnitOfWork uow, EquipmentKind kind, IEnumerable<int> excludeNomenclatures)
		{
			Nomenclature nomenclatureAlias = null;

			var nomenclatureList = GetAllNonSerialEquipmentForRent(uow, kind);
			
			//Выбираются только доступные на складе и еще не выбранные в диалоге
			var availableNomenclature = nomenclatureList.Where(x => x.Available > 0)
				.Where(x => !excludeNomenclatures.Contains(x.Id))
				.ToList();

			//Если есть дежурное оборудование выбираем сначала его
			var duty = uow.Session.QueryOver(() => nomenclatureAlias)
				.Where(() => nomenclatureAlias.IsDuty)
				.WhereRestrictionOn(() => nomenclatureAlias.Id)
				.IsIn(availableNomenclature.Select(x => x.Id).ToArray()).List();
			
			if(duty.Any()) 
			{
				return duty.First();
			}

			//Иначе если есть приоритетное оборудование выбираем его
			var priority = uow.Session.QueryOver(() => nomenclatureAlias)
				.Where(() => nomenclatureAlias.RentPriority)
				.WhereRestrictionOn(() => nomenclatureAlias.Id)
				.IsIn(availableNomenclature.Select(x => x.Id).ToArray()).List();
			
			if(priority.Any()) 
			{
				return priority.First();
			}

			//Выбираем любое доступное оборудование
			var any = uow.Session.QueryOver(() => nomenclatureAlias)
				.WhereRestrictionOn(() => nomenclatureAlias.Id)
				.IsIn(availableNomenclature.Select(x => x.Id).ToArray()).List();
			
			return any.FirstOrDefault();
		}
		
		/// <summary>
		/// Возвращает список всего оборудования определенного типа для аренды
		/// </summary>
		public IList<NomenclatureForRentNode> GetAllNonSerialEquipmentForRent(IUnitOfWork uow, EquipmentKind kind)
		{
			return QueryAvailableNonSerialEquipmentForRent(kind)
				.GetExecutableQueryOver(uow.Session)
				.List<NomenclatureForRentNode>();
		}
		
		/// <summary>
		/// Запрос выбирающий количество добавленное на склад, отгруженное со склада 
		/// и зарезервированное в заказах каждой номенклатуры по выбранному типу оборудования
		/// </summary>
		public QueryOver<Nomenclature, Nomenclature> QueryAvailableNonSerialEquipmentForRent(EquipmentKind kind)
		{
			Nomenclature nomenclatureAlias = null;
			WarehouseMovementOperation operationAddAlias = null;

			//Подзапрос выбирающий по номенклатуре количество добавленное на склад
			var subqueryAdded = QueryOver.Of(() => operationAddAlias)
				.Where(() => operationAddAlias.Nomenclature.Id == nomenclatureAlias.Id)
				.Where(Restrictions.IsNotNull(Projections.Property<WarehouseMovementOperation>(o => o.IncomingWarehouse)))
				.Select(Projections.Sum<WarehouseMovementOperation>(o => o.Amount));
			
			//Подзапрос выбирающий по номенклатуре количество отгруженное со склада
			var subqueryRemoved = QueryOver.Of(() => operationAddAlias)
				.Where(() => operationAddAlias.Nomenclature.Id == nomenclatureAlias.Id)
				.Where(Restrictions.IsNotNull(Projections.Property<WarehouseMovementOperation>(o => o.WriteoffWarehouse)))
				.Select(Projections.Sum<WarehouseMovementOperation>(o => o.Amount));

			//Подзапрос выбирающий по номенклатуре количество зарезервированное в заказах до отгрузки со склада
			Vodovoz.Domain.Orders.Order localOrderAlias = null;
			OrderEquipment localOrderEquipmentAlias = null;
			Equipment localEquipmentAlias = null;
			
			var subqueryReserved = QueryOver.Of(() => localOrderAlias)
				.JoinAlias(() => localOrderAlias.OrderEquipments, () => localOrderEquipmentAlias)
				.JoinAlias(() => localOrderEquipmentAlias.Equipment, () => localEquipmentAlias)
				.Where(() => localEquipmentAlias.Nomenclature.Id == nomenclatureAlias.Id)
				.Where(() => localOrderEquipmentAlias.Direction == Direction.Deliver)
				.Where(() => localOrderAlias.OrderStatus == OrderStatus.Accepted
					   || localOrderAlias.OrderStatus == OrderStatus.InTravelList
					   || localOrderAlias.OrderStatus == OrderStatus.OnLoading)
				.Select(Projections.Sum(() => localOrderEquipmentAlias.Count));

			NomenclatureForRentNode resultAlias = null;
			MeasurementUnits unitAlias = null;
			EquipmentKind equipmentKindAlias = null;

			//Запрос выбирающий количество добавленное на склад, отгруженное со склада 
			//и зарезервированное в заказах каждой номенклатуры по выбранному типу оборудования
			var query = QueryOver.Of(() => nomenclatureAlias)
							 .JoinAlias(() => nomenclatureAlias.Unit, () => unitAlias)
							 .JoinAlias(() => nomenclatureAlias.Kind, () => equipmentKindAlias);
			
			if(kind != null)
			{
				query = query.Where(() => equipmentKindAlias.Id == kind.Id);
			}

			query = query.SelectList(
				list => list
					.SelectGroup(() => nomenclatureAlias.Id).WithAlias(() => resultAlias.Id)
		            .Select(() => nomenclatureAlias.Name).WithAlias(() => resultAlias.NomenclatureName)
		            .Select(() => equipmentKindAlias.Id).WithAlias(() => resultAlias.TypeId)
		            .Select(() => equipmentKindAlias.Name).WithAlias(() => resultAlias.EquipmentKindName)
					.Select(() => unitAlias.Name).WithAlias(() => resultAlias.UnitName)
					.Select(() => unitAlias.Digits).WithAlias(() => resultAlias.UnitDigits)
					.SelectSubQuery(subqueryAdded).WithAlias(() => resultAlias.Added)
					.SelectSubQuery(subqueryRemoved).WithAlias(() => resultAlias.Removed)
					.SelectSubQuery(subqueryReserved).WithAlias(() => resultAlias.Reserved))
				.OrderBy(x => x.Name).Asc
				.TransformUsing(Transformers.AliasToBean<NomenclatureForRentNode>());
			return query;
		}

		#endregion

		/// <summary>
		/// Возврат словаря сертификатов для передаваемых номенклатур
		/// </summary>
		/// <returns>Словарь сертификатов</returns>
		/// <param name="uow">IUnitOfWork</param>
		/// <param name="nomenclatures">Список номенклатур</param>
		public Dictionary<Nomenclature, IList<Certificate>> GetDictionaryWithCertificatesForNomenclatures(IUnitOfWork uow, Nomenclature[] nomenclatures)
		{
			Dictionary<Nomenclature, IList<Certificate>> dict = new Dictionary<Nomenclature, IList<Certificate>>();
			foreach(var n in nomenclatures) {
				Nomenclature nomenclatureAlias = null;
				var certificates = uow.Session.QueryOver<Certificate>()
									   .Left.JoinAlias(c => c.Nomenclatures, () => nomenclatureAlias)
									   .Where(() => nomenclatureAlias.Id == n.Id)
									   .List()
									   ;
				if(certificates.Any()) {
					if(!dict.ContainsKey(n))
						dict.Add(n, certificates);
					else {
						foreach(Certificate certificate in certificates)
							dict[n].Add(certificate);
					}
				}
			}

			return dict;
		}

		/// <summary>
		/// Возвращает Dictionary где: 
		/// key - id номенклатуры
		/// value - массив id картинок
		/// </summary>
		/// <returns>The nomenclature images identifiers.</returns>
		public Dictionary<int, int[]> GetNomenclatureImagesIds(IUnitOfWork uow, params int[] nomenclatureIds)
		{
			return uow.Session.QueryOver<NomenclatureImage>()
						 .Where(n => n.Nomenclature.Id.IsIn(nomenclatureIds))
						 .SelectList(list => list
						 	.Select(i => i.Id)
							.Select(i => i.Nomenclature.Id)
						 )
						 .List<object[]>()
						 .GroupBy(x => (int)x[1])
						 .ToDictionary(g => g.Key, g => g.Select(x => (int)x[0]).ToArray());
		}

		#region Получение номенклатур воды

		public Nomenclature GetWaterSemiozerie(IUnitOfWork uow) => nomenclatureParametersProvider.GetWaterSemiozerie(uow);

		public Nomenclature GetWaterKislorodnaya(IUnitOfWork uow) => nomenclatureParametersProvider.GetWaterKislorodnaya(uow);

		public Nomenclature GetWaterSnyatogorskaya(IUnitOfWork uow) => nomenclatureParametersProvider.GetWaterSnyatogorskaya(uow);

		public Nomenclature GetWaterKislorodnayaDeluxe(IUnitOfWork uow) => nomenclatureParametersProvider.GetWaterKislorodnayaDeluxe(uow);

		public Nomenclature GetWaterStroika(IUnitOfWork uow) => nomenclatureParametersProvider.GetWaterStroika(uow);

		public Nomenclature GetWaterRuchki(IUnitOfWork uow) => nomenclatureParametersProvider.GetWaterRuchki(uow);

		#endregion

		public decimal GetWaterPriceIncrement => nomenclatureParametersProvider.GetWaterPriceIncrement;

		public int GetIdentifierOfOnlineShopGroup() => nomenclatureParametersProvider.GetIdentifierOfOnlineShopGroup();
	}
}