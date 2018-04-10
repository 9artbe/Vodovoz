﻿using System;
using QSOrmProject;
using QSOrmProject.RepresentationModel;
using Vodovoz.Domain.Cash;
using Vodovoz.Domain.Employees;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class UnclosedAdvancesFilter : Gtk.Bin, IRepresentationFilter
	{
		IUnitOfWork uow;

		public IUnitOfWork UoW {
			get {
				return uow;
			}
			set {
				uow = value;
			}
		}

		public UnclosedAdvancesFilter (IUnitOfWork uow) : this()
		{
			UoW = uow;
		}

		public UnclosedAdvancesFilter ()
		{
			this.Build ();

			yentryExpense.ItemsQuery = Repository.Cash.CategoryRepository.ExpenseCategoriesQuery ();
			var filter = new EmployeeFilter(UoW);
			filter.RestrictFired = false;
			yentryAccountable.RepresentationModel = new ViewModel.EmployeesVM(filter);
		}

		#region IReferenceFilter implementation

		public event EventHandler Refiltered;

		void OnRefiltered ()
		{
			if (Refiltered != null)
				Refiltered (this, new EventArgs ());
		}

		#endregion

		public ExpenseCategory RestrictExpenseCategory {
			get { return yentryExpense.Subject as ExpenseCategory;}
			set { yentryExpense.Subject = value;
				yentryExpense.Sensitive = false;
			}
		}

		public Employee RestrictAccountable {
			get { return yentryAccountable.Subject as Employee;}
			set { yentryAccountable.Subject = value;
				yentryAccountable.Sensitive = false;
			}
		}

		protected void OnYentryAccountableChanged (object sender, EventArgs e)
		{
			OnRefiltered ();
		}

		protected void OnYentryExpenseChanged (object sender, EventArgs e)
		{
			OnRefiltered ();
		}
	}
}

