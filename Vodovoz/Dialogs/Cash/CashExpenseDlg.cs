﻿using System;
using QSOrmProject;
using Vodovoz.Domain.Cash;
using QSValidation;

namespace Vodovoz
{
	public partial class CashExpenseDlg : OrmGtkDialogBase<Expense>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();

		public CashExpenseDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Expense>();
			Entity.Casher = Repository.EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			Entity.Date = DateTime.Today;
			ConfigureDlg ();
		}

		public CashExpenseDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Expense> (id);
			ConfigureDlg ();
		}

		public CashExpenseDlg (Expense sub) : this (sub.Id) {}

		void ConfigureDlg()
		{
			enumcomboOperation.ItemsEnum = typeof(ExpenseType);
			enumcomboOperation.Binding.AddBinding (Entity, s => s.TypeOperation, w => w.SelectedItem).InitializeFromSource ();

			yentryCasher.ItemsQuery = Repository.EmployeeRepository.ActiveEmployeeQuery ();
			yentryCasher.Binding.AddBinding (Entity, s => s.Casher, w => w.Subject).InitializeFromSource ();

			yentryEmploeey.ItemsQuery = Repository.EmployeeRepository.ActiveEmployeeQuery ();
			yentryEmploeey.Binding.AddBinding (Entity, s => s.Employee, w => w.Subject).InitializeFromSource ();

			ydateDocument.Binding.AddBinding (Entity, s => s.Date, w => w.Date).InitializeFromSource ();

			comboCategory.ItemsList = Repository.Cash.CategoryRepository.ExpenseCategories (UoW);
			comboCategory.Binding.AddBinding (Entity, s => s.ExpenseCategory, w => w.SelectedItem).InitializeFromSource ();

			yspinMoney.Binding.AddBinding (Entity, s => s.Money, w => w.ValueAsDecimal).InitializeFromSource ();

			ytextviewDescription.Binding.AddBinding (Entity, s => s.Description, w => w.Buffer.Text).InitializeFromSource ();
		}

		public override bool Save ()
		{
			var valid = new QSValidator<Expense> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем расходный ордер...");
			try {
				UoWGeneric.Save();
			} catch (Exception ex) {
				logger.Error (ex, "Не удалось записать расходный ордер.");
				QSProjectsLib.QSMain.ErrorMessage ((Gtk.Window)this.Toplevel, ex);
				return false;
			}
			logger.Info ("Ok");
			return true;

		}

	}
}

