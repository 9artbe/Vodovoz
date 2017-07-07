﻿using System;
using Vodovoz.Domain.Logistic;
using FluentNHibernate.Mapping;

namespace Vodovoz.HibernateMapping
{
	public class FuelTypeMap : ClassMap<FuelType>
	{
		public FuelTypeMap ()
		{
			Table("fuel_types");
			Not.LazyLoad ();

			Id(x => x.Id).Column ("id").GeneratedBy.Native();
			Map(x => x.Name).Column ("name");
			Map(x => x.Cost).Column ("cost");
		}
	}
}

