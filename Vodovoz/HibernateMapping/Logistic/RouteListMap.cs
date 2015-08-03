﻿using System;
using Vodovoz.Domain.Logistic;
using FluentNHibernate.Mapping;

namespace Vodovoz.HMap
{
	public class RouteListMap : ClassMap<RouteList>
	{
		public RouteListMap ()
		{
			Table("route_lists");
			Not.LazyLoad ();

			Id(x => x.Id).Column ("id").GeneratedBy.Native();
			Map(x => x.PlannedDistance).Column ("planned_distance");
			Map(x => x.ActualDistance).Column ("actual_distance");
			Map(x => x.Date).Column ("date");
			Map(x => x.Status).Column ("status").CustomType<RouteListStatusStringType> ();
			References (x => x.Car).Column ("car_id");
			References (x => x.Driver).Column ("driver_id");
			References (x => x.Forwarder).Column ("forwarder_id");
			HasMany (x => x.Addresses).Inverse ().Cascade.AllDeleteOrphan ()
				.KeyColumn ("route_list_id")
				.AsList (x => x.Column ("order_in_route"));
		}
	}
}

