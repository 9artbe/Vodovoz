﻿using System;
using NHibernate.Criterion;
using QS.Tools;
using Vodovoz.Domain.Cash;
using QS.DomainModel.Entity;
using Vodovoz.Domain.Employees;

namespace Vodovoz.Filters
{
	public sealed class IncomeFilter : PropertyChangedBase, IQueryFilter
	{
		private DateTime startDate;
		public DateTime StartDate {
			get => startDate;
			set => SetField(ref startDate, value, () => StartDate);
		}

		private DateTime endDate;
		public DateTime EndDate {
			get => endDate;
			set => SetField(ref endDate, value, () => EndDate);
		}


		private Employee employee;
		public Employee Employee {
			get => employee;
			set => SetField(ref employee, value, () => Employee);
		}

		private IncomeCategory incomeCategory;
		public IncomeCategory IncomeCategory {
			get => incomeCategory;
			set => SetField(ref incomeCategory, value, () => IncomeCategory);
		}

		private ExpenseCategory expenseCategory;
		public ExpenseCategory ExpenseCategory {
			get => expenseCategory;
			set => SetField(ref expenseCategory, value, () => ExpenseCategory);
		}

		public IncomeFilter()
		{
			SetDefaults();
		}

		public void SetDefaults()
		{
			StartDate = DateTime.Today.AddDays(-7);
			EndDate = DateTime.Today;
		}

		public ICriterion GetFilter()
		{
			ICriterion result = Restrictions.Where<Income>(x => x.Date >= StartDate && x.Date <= EndDate);

			if(Employee != null) {
				result = Restrictions.And(result, Restrictions.Where<Income>(x => x.Employee == Employee));
			}

			if(IncomeCategory != null) {
				result = Restrictions.And(result, Restrictions.Where<Income>(x => x.IncomeCategory == IncomeCategory));
			}

			if(ExpenseCategory != null) {
				result = Restrictions.And(result, Restrictions.Where<Income>(x => x.ExpenseCategory == ExpenseCategory));
			}

			return result;
		}
	}
}
