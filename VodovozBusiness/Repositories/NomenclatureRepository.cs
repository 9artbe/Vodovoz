﻿using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using QS.DomainModel.UoW;
using Vodovoz.Domain;
using Vodovoz.Domain.Goods;
using Vodovoz.Parameters;
using NewNomenclatureRepository = Vodovoz.EntityRepositories.Goods.NomenclatureRepository;

namespace Vodovoz.Repositories
{
	[Obsolete("Используйте одноимённый класс из Vodovoz.EntityRepositories.Goods")]
	public static class NomenclatureRepository
	{
		private static readonly NomenclatureParametersProvider nomenclatureParametersProvider = new NomenclatureParametersProvider();
		
		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureForProductMaterialsQuery() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureForProductMaterialsQuery();

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureEquipmentsQuery() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureEquipmentsQuery();

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureForSaleQuery() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureForSaleQuery();

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureByCategory(NomenclatureCategory category) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureByCategory(category);

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureOfGoodsOnlyQuery() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureOfGoodsOnlyQuery();

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureOfGoodsWithoutEmptyBottlesQuery() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureOfGoodsWithoutEmptyBottlesQuery();

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureWaterOnlyQuery() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureWaterOnlyQuery();

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureEquipOnlyQuery() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureEquipOnlyQuery();

		[Obsolete]
		public static Nomenclature GetDefaultBottle(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetDefaultBottle(uow);

		[Obsolete]
		public static IList<Nomenclature> GetDependedNomenclatures(IUnitOfWork uow, Nomenclature influentialNomenclature) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetDependedNomenclatures(uow, influentialNomenclature);

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureOfItemsForService() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureOfItemsForService();

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureOfPartsForService() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureOfPartsForService();

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureOfServices() => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureOfServices();

		[Obsolete]
		public static IList<Nomenclature> NomenclatureOfDefectiveGoods(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetNomenclatureOfDefectiveGoods(uow);

		[Obsolete]
		public static string GetNextCode1c(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetNextCode1c(uow);

		[Obsolete]
		public static QueryOver<Nomenclature> NomenclatureInGroupsQuery(int[] groupsIds) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).NomenclatureInGroupsQuery(groupsIds);

		[Obsolete]
		public static Nomenclature GetNomenclatureToAddWithMaster(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetNomenclatureToAddWithMaster(uow);

		[Obsolete]
		public static Nomenclature GetForfeitNomenclature(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetForfeitNomenclature(uow);

		[Obsolete]
		public static Nomenclature GetSanitisationNomenclature(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetSanitisationNomenclature(uow);

		[Obsolete]
		public static IList<Nomenclature> GetNomenclatureWithPriceForMobileApp(IUnitOfWork uow, params MobileCatalog[] catalogs) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetNomenclatureWithPriceForMobileApp(uow, catalogs);

		[Obsolete]
		public static Dictionary<Nomenclature, IList<Certificate>> GetDictionaryWithCertificatesForNomenclatures(IUnitOfWork uow, Nomenclature[] nomenclatures) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetDictionaryWithCertificatesForNomenclatures(uow, nomenclatures);

		[Obsolete]
		public static Dictionary<int, int[]> GetNomenclatureImagesIds(IUnitOfWork uow, params int[] nomenclatureIds) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetNomenclatureImagesIds(uow, nomenclatureIds);

		[Obsolete]
		public static Nomenclature GetWaterSemiozerie(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetWaterSemiozerie(uow);

		[Obsolete]
		public static Nomenclature GetWaterKislorodnaya(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetWaterKislorodnaya(uow);

		[Obsolete]
		public static Nomenclature GetWaterSnyatogorskaya(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetWaterSnyatogorskaya(uow);

		[Obsolete]
		public static Nomenclature GetWaterKislorodnayaDeluxe(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetWaterKislorodnayaDeluxe(uow);

		[Obsolete]
		public static Nomenclature GetWaterStroika(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetWaterStroika(uow);

		[Obsolete]
		public static Nomenclature GetWaterRuchki(IUnitOfWork uow) => 
			new NewNomenclatureRepository(nomenclatureParametersProvider).GetWaterRuchki(uow);
	}
}