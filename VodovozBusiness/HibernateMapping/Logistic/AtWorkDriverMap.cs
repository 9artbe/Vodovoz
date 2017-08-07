﻿using System;
using Vodovoz.Domain.Logistic;
using FluentNHibernate.Mapping;
using DataAccess.NhibernateFixes;

namespace Vodovoz.HibernateMapping
{
	public class AtWorkDriverMap : ClassMap<AtWorkDriver>
	{
		public AtWorkDriverMap ()
		{
			Table("at_work_drivers");

			Id(x => x.Id).Column ("id").GeneratedBy.Native();
			Map(x => x.Date).Column("date");
			Map(x => x.Trips).Column("trips");
			Map(x => x.PriorityAtDay).Column("piority_at_day");
			Map(x => x.EndOfDay).Column("end_of_day").CustomType<TimeAsTimeSpanTypeClone>();

			References(x => x.Employee).Column("employee_id");
			References(x => x.Car).Column("car_id");

			HasMany(x => x.Districts).Cascade.AllDeleteOrphan().Inverse()
									 .KeyColumn("at_work_driver_id")
									 .AsList(x => x.Column("priority"));
		}
	}
}

