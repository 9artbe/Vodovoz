﻿using System;
using FluentNHibernate.Mapping;
using Vodovoz.Domain.Cash;

namespace Vodovoz.HMap
{
	public class IncomeMap : ClassMap<Income>
	{
		public IncomeMap ()
		{
			Table("cash_income");
			Not.LazyLoad ();

			Id(x => x.Id).Column ("id").GeneratedBy.Native();
			Map(x => x.TypeOperation).Column ("type").CustomType<IncomeTypeStringType> ();
			Map (x => x.Date).Column ("date");
			References (x => x.Casher).Column ("casher_employee_id");
			References (x => x.Employee).Column ("employee_id");
			References (x => x.IncomeCategory).Column ("cash_income_category_id");
			Map (x => x.Money).Column ("money");
			Map (x => x.Description).Column ("description");
		}
	}
}

