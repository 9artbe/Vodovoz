﻿using System;
using FluentNHibernate.Mapping;
using Vodovoz.Domain;

namespace Vodovoz.HMap
{
	public class NomenclatureMap : ClassMap<Nomenclature>
	{
		public NomenclatureMap ()
		{
			Table("nomenclature");

			Id(x => x.Id).Column ("id").GeneratedBy.Native();
			Map(x => x.Name).Column ("name");
			Map (x => x.Model).Column ("model");
			Map (x => x.Weight).Column ("weight");
			Map (x => x.VAT).Column ("vat").CustomType<VATStringType> ();
			Map (x => x.DoNotReserve).Column ("reserve");
			Map (x => x.Serial).Column ("serial");
			Map (x => x.Category).Column ("category").CustomType<NomenclatureCategoryStringType> ();
			References (x => x.Unit).Column ("unit_id");
			References (x => x.Color).Column ("color_id");
			References (x => x.Type).Column ("type_id");
			References (x => x.Manufacturer).Column ("manufacturer_id");
			References (x => x.RouteListColumn).Column ("route_column_id");
			HasMany (x => x.NomenclaturePrice).Cascade.AllDeleteOrphan ().LazyLoad ().KeyColumn ("nomenclature_id");
		}
	}
}

