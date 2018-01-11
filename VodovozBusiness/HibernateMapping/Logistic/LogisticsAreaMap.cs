﻿using FluentNHibernate.Mapping;
using NHibernate.Spatial.Type;
using Vodovoz.Domain.Logistic;

namespace Vodovoz.HibernateMapping
{
	public class LogisticsAreaMap : ClassMap<LogisticsArea>
	{
		public LogisticsAreaMap ()
		{
			Table("logistics_area");

			Id(x => x.Id).Column ("id").GeneratedBy.Native();
			Map(x => x.Name).Column ("name");
			Map(x => x.IsCity).Column("is_city");
			Map(x => x.Geometry).Column("logistics_district").CustomType<GeometryType>();
		}
	}
}

