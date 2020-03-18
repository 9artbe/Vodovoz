﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QSOrmProject;
using QSProjectsLib;
using QS.Project.Repositories;
using QS.Validation;
using Vodovoz.Domain.Cash;
using Vodovoz.Domain.Logistic;
using Vodovoz.Repositories.HumanResources;
using Vodovoz.Repository.Cash;
using Vodovoz.Filters.ViewModels;
using Vodovoz.EntityRepositories.Employees;
using QS.EntityRepositories;
using QS.Services;
using Vodovoz.EntityRepositories;
using QS.Project.Services;

namespace Vodovoz
{
	public partial class CashIncomeDlg : QS.Dialog.Gtk.EntityDialogBase<Income>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();

		private bool canEdit = true;

		List<Selectable<Expense>> selectableAdvances;

		public CashIncomeDlg (IPermissionService permissionService)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Income>();
			Entity.Casher = EmployeeSingletonRepository.GetInstance().GetEmployeeForCurrentUser (UoW);
			if(Entity.Casher == null)
			{
				MessageDialogHelper.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете создавать кассовые документы, так как некого указывать в качестве кассира.");
				FailInitialize = true;
				return;
			}

			var userPermission = permissionService.ValidateUserPermission(typeof(Income), UserSingletonRepository.GetInstance().GetCurrentUser(UoW).Id);
			if(!userPermission.CanCreate) 
			{
				MessageDialogHelper.RunErrorDialog("Отсутствуют права на создание приходного ордера");
				FailInitialize = true;
				return;
			}
			if(!accessfilteredsubdivisionselectorwidget.Configure(UoW, false, typeof(Income))) {

				MessageDialogHelper.RunErrorDialog(accessfilteredsubdivisionselectorwidget.ValidationErrorMessage);
				FailInitialize = true;
				return;
			}

			Entity.Date = DateTime.Now;
			ConfigureDlg ();
		}

		public CashIncomeDlg (int id, IPermissionService permissionService)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Income> (id);

			if(!accessfilteredsubdivisionselectorwidget.Configure(UoW, false, typeof(Income))) {

				MessageDialogHelper.RunErrorDialog(accessfilteredsubdivisionselectorwidget.ValidationErrorMessage);
				FailInitialize = true;
				return;
			}

			var userPermission = permissionService.ValidateUserPermission(typeof(Income), UserSingletonRepository.GetInstance().GetCurrentUser(UoW).Id);
			if(!userPermission.CanRead) {
				MessageDialogHelper.RunErrorDialog("Отсутствуют права на просмотр приходного ордера");
				FailInitialize = true;
				return;
			}
			canEdit = userPermission.CanUpdate;

			ConfigureDlg ();
		}

		public CashIncomeDlg (Expense advance, IPermissionService permissionService) : this (permissionService) 
		{
			if(advance.Employee == null)
			{
				logger.Error("Аванс без сотрудника. Для него нельзя открыть диалог возврата.");
				base.FailInitialize = true;
				return;
			}

			Entity.TypeOperation = IncomeType.Return;
			Entity.ExpenseCategory = advance.ExpenseCategory;
			Entity.Employee = advance.Employee;
			selectableAdvances.Find(x => x.Value.Id == advance.Id).Selected = true;
		}

		public CashIncomeDlg (Income sub, IPermissionService permissionService) : this (sub.Id, permissionService) {}

		void ConfigureDlg()
		{
			accessfilteredsubdivisionselectorwidget.OnSelected += Accessfilteredsubdivisionselectorwidget_OnSelected;
			if(Entity.RelatedToSubdivision != null) {
				accessfilteredsubdivisionselectorwidget.SelectIfPossible(Entity.RelatedToSubdivision);
			}

			enumcomboOperation.ItemsEnum = typeof(IncomeType);
			enumcomboOperation.Binding.AddBinding (Entity, s => s.TypeOperation, w => w.SelectedItem).InitializeFromSource ();

			var filterCasher = new EmployeeFilterViewModel();
			filterCasher.Status = Domain.Employees.EmployeeStatus.IsWorking;
			yentryCasher.RepresentationModel = new ViewModel.EmployeesVM(filterCasher);
			yentryCasher.Binding.AddBinding(Entity, s => s.Casher, w => w.Subject).InitializeFromSource();

			var filter = new EmployeeFilterViewModel();
			filter.Status = Domain.Employees.EmployeeStatus.IsWorking;
			yentryEmployee.RepresentationModel = new ViewModel.EmployeesVM(filter);
			yentryEmployee.Binding.AddBinding(Entity, s => s.Employee, w => w.Subject).InitializeFromSource();

			var filterRL = new RouteListsFilter(UoW);
			filterRL.OnlyStatuses = new RouteListStatus[] { RouteListStatus.EnRoute, RouteListStatus.OnClosing };
			yEntryRouteList.RepresentationModel = new ViewModel.RouteListsVM(filterRL);
			yEntryRouteList.Binding.AddBinding(Entity, s => s.RouteListClosing, w => w.Subject).InitializeFromSource();
			yEntryRouteList.CanEditReference = ServicesConfig.CommonServices.CurrentPermissionService.ValidatePresetPermission("can_delete");

			yEntryRouteList.Hidden += YEntryRouteList_ValueOrVisibilityChanged;
			yEntryRouteList.Shown += YEntryRouteList_ValueOrVisibilityChanged;
			yEntryRouteList.ChangedByUser += YEntryRouteList_ValueOrVisibilityChanged;

			yentryClient.ItemsQuery = Repository.CounterpartyRepository.ActiveClientsQuery ();
			yentryClient.Binding.AddBinding (Entity, s => s.Customer, w => w.Subject).InitializeFromSource ();

			ydateDocument.Binding.AddBinding (Entity, s => s.Date, w => w.Date).InitializeFromSource ();

			OrmMain.GetObjectDescription<ExpenseCategory> ().ObjectUpdated += OnExpenseCategoryUpdated;
			OnExpenseCategoryUpdated (null, null);
			comboExpense.Binding.AddBinding (Entity, s => s.ExpenseCategory, w => w.SelectedItem).InitializeFromSource ();

			OrmMain.GetObjectDescription<IncomeCategory> ().ObjectUpdated += OnIncomeCategoryUpdated;
			OnIncomeCategoryUpdated (null, null);
			comboCategory.Binding.AddBinding (Entity, s => s.IncomeCategory, w => w.SelectedItem).InitializeFromSource ();

			checkNoClose.Binding.AddBinding(Entity, e => e.NoFullCloseMode, w => w.Active);

			yspinMoney.Binding.AddBinding (Entity, s => s.Money, w => w.ValueAsDecimal).InitializeFromSource ();

			ytextviewDescription.Binding.AddBinding (Entity, s => s.Description, w => w.Buffer.Text).InitializeFromSource ();

			ytreeviewDebts.ColumnsConfig = ColumnsConfigFactory.Create<Selectable<Expense>> ()
				.AddColumn ("Закрыть").AddToggleRenderer (a => a.Selected).Editing ()
				.AddColumn ("Дата").AddTextRenderer (a => a.Value.Date.ToString ())
				.AddColumn ("Получено").AddTextRenderer (a => a.Value.Money.ToString ("C"))
				.AddColumn ("Непогашено").AddTextRenderer (a => a.Value.UnclosedMoney.ToString ("C"))
				.AddColumn ("Статья").AddTextRenderer (a => a.Value.ExpenseCategory.Name)
				.AddColumn ("Основание").AddTextRenderer (a => a.Value.Description)
				.Finish ();
			UpdateSubdivision();

			table1.Sensitive = canEdit;
			ytreeviewDebts.Sensitive = canEdit;
			ytextviewDescription.Sensitive = canEdit;
			buttonSave.Sensitive = canEdit;
			accessfilteredsubdivisionselectorwidget.Sensitive &= canEdit;
		}

		public void FillForRoutelist(int routelistId)
		{
			var cashier = EmployeeSingletonRepository.GetInstance().GetEmployeeForCurrentUser(UoW);
			if(cashier == null) {
				MessageDialogHelper.RunErrorDialog("Ваш пользователь не привязан к действующему сотруднику, вы не можете закрыть МЛ, так как некого указывать в качестве кассира.");
				return;
			}

			var rl = UoW.GetById<RouteList>(routelistId);

			Entity.IncomeCategory = CategoryRepository.RouteListClosingIncomeCategory(UoW);
			Entity.TypeOperation = IncomeType.DriverReport;
			Entity.Date = DateTime.Now;
			Entity.Casher = cashier;
			Entity.Employee = rl.Driver;
			Entity.Description = $"Закрытие МЛ №{rl.Id} от {rl.Date:d}";
			Entity.RouteListClosing = rl;
			Entity.RelatedToSubdivision = rl.ClosingSubdivision;
		}

		void OnIncomeCategoryUpdated (object sender, QSOrmProject.UpdateNotification.OrmObjectUpdatedEventArgs e)
		{
			comboCategory.ItemsList = Repository.Cash.CategoryRepository.IncomeCategories (UoW);
		}

		void OnExpenseCategoryUpdated (object sender, QSOrmProject.UpdateNotification.OrmObjectUpdatedEventArgs e)
		{
			comboExpense.ItemsList = Repository.Cash.CategoryRepository.ExpenseCategories (UoW);
		}

		void Accessfilteredsubdivisionselectorwidget_OnSelected(object sender, EventArgs e)
		{
			UpdateSubdivision();
		}

		private void UpdateSubdivision()
		{
			if(accessfilteredsubdivisionselectorwidget.SelectedSubdivision != null && accessfilteredsubdivisionselectorwidget.NeedChooseSubdivision) {
				Entity.RelatedToSubdivision = accessfilteredsubdivisionselectorwidget.SelectedSubdivision;
			}
		}


		public override bool Save ()
		{
			if (Entity.TypeOperation == IncomeType.Return && UoW.IsNew && selectableAdvances != null)
				Entity.PrepareCloseAdvance(selectableAdvances.Where(x => x.Selected).Select(x => x.Value).ToList());

			var valid = new QSValidator<Income> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем Приходный ордер..."); 
			if (Entity.TypeOperation == IncomeType.Return && UoW.IsNew) {
				logger.Info ("Закрываем авансы...");
				Entity.CloseAdvances(UoW);
			}
			UoWGeneric.Save();
			logger.Info ("Ok");
			return true;
		}
			
		protected void OnButtonPrintClicked (object sender, EventArgs e)
		{
			if (UoWGeneric.HasChanges && CommonDialogs.SaveBeforePrint (typeof(Expense), "квитанции"))
				Save ();

			var reportInfo = new QS.Report.ReportInfo {
				Title = String.Format ("Квитанция №{0} от {1:d}", Entity.Id, Entity.Date),
				Identifier = "Cash.ReturnTicket",
				Parameters = new Dictionary<string, object> {
					{ "id",  Entity.Id }
				}
			};

			var report = new QSReport.ReportViewDlg (reportInfo);
			TabParent.AddTab (report, this, false);
		}

		protected void OnEnumcomboOperationEnumItemSelected (object sender, Gamma.Widgets.ItemSelectedEventArgs e)
		{
			buttonPrint.Sensitive = Entity.TypeOperation == IncomeType.Return;
			labelExpenseTitle.Visible = comboExpense.Visible = Entity.TypeOperation == IncomeType.Return;
			labelIncomeTitle.Visible = comboCategory.Visible = Entity.TypeOperation != IncomeType.Return;

			labelClientTitle.Visible = yentryClient.Visible = Entity.TypeOperation == IncomeType.Payment;

			vboxDebts.Visible = checkNoClose.Visible = Entity.TypeOperation == IncomeType.Return && UoW.IsNew;
			yspinMoney.Sensitive = Entity.TypeOperation != IncomeType.Return;
			yspinMoney.ValueAsDecimal = 0;

			FillDebts ();
			CheckOperation((IncomeType)e.SelectedItem);
		}

		void CheckOperation(IncomeType incomeType){
			lblRouteList.Visible = yEntryRouteList.Visible
				= incomeType == IncomeType.DriverReport;

			if(incomeType == IncomeType.DriverReport){
				Entity.IncomeCategory = UoW.GetById<IncomeCategory>(1);
			}
		}

		protected void OnYentryEmployeeChanged (object sender, EventArgs e)
		{			
			FillDebts ();
		}

		protected void OnComboExpenseItemSelected (object sender, Gamma.Widgets.ItemSelectedEventArgs e)
		{
			FillDebts ();
		}

		protected void FillDebts(){
			if (Entity.TypeOperation == IncomeType.Return && Entity.Employee != null) {
				var advances = Repository.Cash.AccountableDebtsRepository
					.UnclosedAdvance (UoW, Entity.Employee, Entity.ExpenseCategory);
				selectableAdvances = advances.Select (advance => new Selectable<Expense> (advance))
				.ToList ();
				selectableAdvances.ForEach (advance => advance.SelectChanged += OnAdvanceSelectionChanged);
				ytreeviewDebts.ItemsDataSource = selectableAdvances;
			}
		}

		protected void OnAdvanceSelectionChanged(object sender, EventArgs args){
			if(checkNoClose.Active && (sender as Selectable<Expense>).Selected)
			{
				selectableAdvances.Where(x => x != sender).ToList().ForEach(x => x.SilentUnselect());
			}

			if (checkNoClose.Active)
				return;

			Entity.Money = selectableAdvances.
				Where(expense=>expense.Selected)
				.Sum (selectedExpense => selectedExpense.Value.UnclosedMoney);
		}
			
		protected void OnCheckNoCloseToggled(object sender, EventArgs e)
		{
			if (selectableAdvances == null)
				return;
			if(checkNoClose.Active && selectableAdvances.Count(x => x.Selected) > 1)
			{
				MessageDialogHelper.RunWarningDialog("Частично вернуть можно только один аванс.");
				checkNoClose.Active = false;
				return;
			}
			yspinMoney.Sensitive = checkNoClose.Active;
			if(!checkNoClose.Active)
			{
				yspinMoney.ValueAsDecimal = selectableAdvances.Where(x => x.Selected).Sum(x => x.Value.UnclosedMoney);
			}
		}

		void YEntryRouteList_ValueOrVisibilityChanged(object sender, EventArgs e)
		{
			if(yEntryRouteList.Visible && Entity.RouteListClosing != null){
				Entity.Description = $"Приход по МЛ №{Entity.RouteListClosing.Id} от {Entity.RouteListClosing.Date:d}";
				Entity.Employee = Entity.RouteListClosing.Driver;
			} else {
				Entity.Description = "";
				Entity.RouteListClosing = null;
				Entity.Employee = null;
			}
		}
	}

	public class Selectable<T> {

		private bool selected;

		public bool Selected {
			get { return selected;}
			set{ selected = value;
				if (SelectChanged != null)
					SelectChanged (this, EventArgs.Empty);
			}
		}

		public event EventHandler SelectChanged;

		public void SilentUnselect()
		{
			selected = false;
		}

		public T Value { get; set;}

		public Selectable(T obj)
		{
			Value = obj;
			Selected = false;
		}
	}
}

