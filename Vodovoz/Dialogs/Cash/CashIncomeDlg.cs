﻿using System;
using QSOrmProject;
using QSProjectsLib;
using QSValidation;
using Vodovoz.Domain.Cash;

namespace Vodovoz
{
	public partial class CashIncomeDlg : OrmGtkDialogBase<Income>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();

		public CashIncomeDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Income>();
			Entity.Casher = Repository.EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			if(Entity.Casher == null)
			{
				MessageDialogWorks.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете создавать кассовые документы, так как некого указывать в качестве кассира.");
				FailInitialize = true;
				return;
			}
			Entity.Date = DateTime.Today;
			ConfigureDlg ();
		}

		public CashIncomeDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Income> (id);
			ConfigureDlg ();
		}

		public CashIncomeDlg (Income sub) : this (sub.Id) {}

		void ConfigureDlg()
		{
			enumcomboOperation.ItemsEnum = typeof(IncomeType);
			enumcomboOperation.Binding.AddBinding (Entity, s => s.TypeOperation, w => w.SelectedItem).InitializeFromSource ();

			yentryCasher.ItemsQuery = Repository.EmployeeRepository.ActiveEmployeeQuery ();
			yentryCasher.Binding.AddBinding (Entity, s => s.Casher, w => w.Subject).InitializeFromSource ();

			yentryEmploeey.ItemsQuery = Repository.EmployeeRepository.ActiveEmployeeQuery ();
			yentryEmploeey.Binding.AddBinding (Entity, s => s.Employee, w => w.Subject).InitializeFromSource ();

			ydateDocument.Binding.AddBinding (Entity, s => s.Date, w => w.Date).InitializeFromSource ();

			comboCategory.ItemsList = Repository.Cash.CategoryRepository.IncomeCategories (UoW);
			comboCategory.Binding.AddBinding (Entity, s => s.IncomeCategory, w => w.SelectedItem).InitializeFromSource ();

			yspinMoney.Binding.AddBinding (Entity, s => s.Money, w => w.ValueAsDecimal).InitializeFromSource ();

			ytextviewDescription.Binding.AddBinding (Entity, s => s.Description, w => w.Buffer.Text).InitializeFromSource ();
		}

		public override bool Save ()
		{
			var valid = new QSValidator<Income> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем Приходный ордер...");
			try {
				UoWGeneric.Save();
			} catch (Exception ex) {
				logger.Error (ex, "Не удалось записать приходный ордер.");
				QSProjectsLib.QSMain.ErrorMessage ((Gtk.Window)this.Toplevel, ex);
				return false;
			}
			logger.Info ("Ok");
			return true;

		}
			
	}
}

