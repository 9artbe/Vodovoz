﻿using FluentNHibernate.Mapping;
using Vodovoz.Domain;
using Vodovoz.Domain.Goods;

namespace Vodovoz.HibernateMapping
{
	public class NomenclatureMap : ClassMap<Nomenclature>
	{
		public NomenclatureMap()
		{
			Table("nomenclature");
			Not.LazyLoad();

			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.CreateDate).Column("create_date");
			Map(x => x.IsArchive).Column("is_archive");
			Map(x => x.CanPrintPrice).Column("can_print_price");
			Map(x => x.Name).Column("name");
			Map(x => x.OfficialName).Column("official_name");
			Map(x => x.Model).Column("model");
			Map(x => x.Weight).Column("weight");
			Map(x => x.Volume).Column("volume");
			Map(x => x.VAT).Column("vat").CustomType<VATStringType>();
			Map(x => x.DoNotReserve).Column("reserve");
			Map(x => x.RentPriority).Column("rent_priority");
			Map(x => x.IsDuty).Column("is_duty");
			Map(x => x.IsSerial).Column("serial");
			Map(x => x.Category).Column("category").CustomType<NomenclatureCategoryStringType>();
			Map(x => x.Code1c).Column("code_1c");
			Map(x => x.SumOfDamage).Column("sum_of_damage");
			Map(x => x.ShortName).Column("short_name");
			Map(x => x.Hide).Column("hide");
			Map(x => x.NoDelivey).Column("no_delivery");
			Map(x => x.IsNewBottle).Column("is_new_bottle");
			Map(x => x.IsDefectiveBottle).Column("is_defective_bottle");
			Map(x => x.IsDiler).Column("is_diler");
			Map(x => x.PercentForMaster).Column("percent_for_master");
			Map(x => x.TypeOfDepositCategory).Column("type_of_deposit").CustomType<TypeOfDepositCategoryStringType>();
			Map(x => x.SubTypeOfEquipmentCategory).Column("subtype_of_equipment").CustomType<SubtypeOfEquipmentCategoryStringType>();
			Map(x => x.OnlineStoreGuid).Column("online_store_guid");
			Map(x => x.MinStockCount).Column("min_stock_count");
			Map(x => x.MobileCatalog).Column("mobile_catalog").CustomType<MobileCatalogStringType>();

			References(x => x.CreatedBy).Column("created_by");
			References(x => x.DependsOnNomenclature).Column("depends_on_nomenclature");
			References(x => x.Unit).Column("unit_id").Not.LazyLoad();
			References(x => x.EquipmentColor).Column("color_id");
			References(x => x.Type).Column("type_id");
			References(x => x.Manufacturer).Column("manufacturer_id");
			References(x => x.RouteListColumn).Column("route_column_id");
			References(x => x.Warehouse).Column("warehouse_id");
			References(x => x.Folder1C).Column("folder_1c_id");
			References(x => x.ProductGroup).Column("group_id");

			HasMany(x => x.NomenclaturePrice).Inverse().Cascade.AllDeleteOrphan().LazyLoad().KeyColumn("nomenclature_id");
			HasMany(x => x.Images).Inverse().Cascade.AllDeleteOrphan().LazyLoad().KeyColumn("nomenclature_id");
		}
	}
}

