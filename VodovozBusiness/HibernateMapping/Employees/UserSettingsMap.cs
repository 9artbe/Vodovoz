﻿using FluentNHibernate.Mapping;
using Vodovoz.Domain.Employees;

namespace Vodovoz.HibernateMapping
{
	public class UserSettingsMap : ClassMap<UserSettings>
	{
		public UserSettingsMap()
		{
			Table("user_settings");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.ToolbarStyle).Column("toolbar_style").CustomType<ToolbarStyleStringType>();
			Map(x => x.ToolBarIconsSize).Column("toolbar_icons_size").CustomType<ToolBarIconsSizeStringType>();
			Map(x => x.JournalDaysToAft).Column("journal_days_to_aft");
			Map(x => x.JournalDaysToFwd).Column("journal_days_to_fwd");
			References(x => x.User).Column("user_id");
			References(x => x.DefaultWarehouse).Column("default_warehouse_id");
		}
	}
}