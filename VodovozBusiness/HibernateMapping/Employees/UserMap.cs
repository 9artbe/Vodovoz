﻿using FluentNHibernate.Mapping;
using Vodovoz.Domain.Employees;

namespace Vodovoz.HibernateMapping
{
	public class UserMap : ClassMap<User>
	{
		public UserMap ()
		{
			Table ("users");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();
			Map(x => x.Name).Column("name");
			Map(x => x.Login).Column("login");
			Map(x => x.Deactivated).Column("deactivated");
			Map(x => x.IsAdmin).Column("admin");
			Map(x => x.Email).Column("email");

			Map(x => x.WarehouseAccess).Column("warehouse_access").LazyLoad();
		}
	}
}