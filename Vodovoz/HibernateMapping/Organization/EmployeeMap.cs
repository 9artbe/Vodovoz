﻿using FluentNHibernate.Mapping;
using Vodovoz.Domain;

namespace Vodovoz
{
	public class EmployeeMap : ClassMap<Employee>
	{
		public EmployeeMap ()
		{
			Table ("employees");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();
			Map (x => x.Name).Column ("name");
			Map (x => x.LastName).Column ("last_name");
			Map (x => x.Patronymic).Column ("patronymic");
			Map (x => x.Category).Column ("category").CustomType<EmployeeCategoryStringType> ();
			Map (x => x.PassportSeria).Column ("passport_seria");
			Map (x => x.PassportNumber).Column ("passport_number");
			Map (x => x.DrivingNumber).Column ("driving_number");
			Map (x => x.Photo).Column ("photo").CustomSqlType ("BinaryBlob");
			Map (x => x.AddressRegistration).Column ("address_registration");
			Map (x => x.AddressCurrent).Column ("address_current");
			Map (x => x.IsFired).Column ("is_fired");
			Map (x => x.INN).Column ("inn");
			References (x => x.Nationality).Column ("nationality_id");
			References (x => x.User).Column ("user_id");
			References (x => x.DefaultAccount).Column ("default_account_id");
			HasMany (x => x.Accounts).Cascade.AllDeleteOrphan ().LazyLoad ().KeyColumn ("employee_id");
			HasMany (x => x.Phones).Cascade.AllDeleteOrphan ().LazyLoad ().KeyColumn ("employee_id");
		}
	}
}
