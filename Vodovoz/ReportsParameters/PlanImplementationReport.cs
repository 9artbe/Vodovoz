﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gamma.Utilities;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QS.Report;
using QSReport;
using Vodovoz.Domain.Employees;
using Vodovoz.Filters.ViewModels;
using Vodovoz.ViewModel;

namespace Vodovoz.ReportsParameters
{
	public partial class PlanImplementationReport : SingleUoWWidgetBase, IParametersWidget
	{
		EmployeeFilterViewModel filter;
		public PlanImplementationReport(bool orderById = false)
		{
			this.Build();
			ConfigureDlg();
		}

		void ConfigureDlg()
		{
			UoW = UnitOfWorkFactory.CreateWithoutRoot();
			dateperiodpicker.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			dateperiodpicker.EndDate = dateperiodpicker.StartDate.AddMonths(1).AddTicks(-1);

			filter = new EmployeeFilterViewModel(ServicesConfig.CommonServices);

			var availablePlansToUse = new[] { WageCalculationType.salesPlan };
			lstCmbPlanType.SetRenderTextFunc<WageCalculationType>(t => t.GetEnumTitle());
			lstCmbPlanType.ItemsList = availablePlansToUse;
			lstCmbPlanType.SelectedItem = availablePlansToUse.FirstOrDefault();
			lstCmbPlanType.Changed += LstCmbPlanType_Changed;
			LstCmbPlanType_Changed(this, new EventArgs());
			yEntRefEmployee.RepresentationModel = new EmployeesVM(filter);
			yEntRefEmployee.ChangedByUser += (sender, e) => {
				lblEmployeePlan.Markup = (yEntRefEmployee.Subject as Employee)?.WageCalculationParameter?.Title;
			};
		}

		void LstCmbPlanType_Changed(object sender, EventArgs e)
		{
			filter.SetAndRefilterAtOnce(
				x => x.RestrictCategory = EmployeeCategory.office,
				x => x.ShowFired = false,
				x => x.RestrictWageType = lstCmbPlanType.SelectedItem as WageCalculationType?
			);
		}

		#region IParametersWidget implementation

		public event EventHandler<LoadReportEventArgs> LoadReport;

		public string Title => "Отчёт о выполнении плана";

		#endregion

		void OnUpdate(bool hide = false)
		{
			LoadReport?.Invoke(this, new LoadReportEventArgs(GetReportInfo(), hide));
		}

		private ReportInfo GetReportInfo()
		{
			int employeeId = (yEntRefEmployee.Subject as Employee)?.Id ?? 0;
			return new ReportInfo {
				Identifier = employeeId > 0 ? "Sales.PlanImplementationByEmployeeReport" : "Sales.PlanImplementationFullReport",
				Parameters = new Dictionary<string, object>
				{
					{ "start_date", dateperiodpicker.StartDateOrNull },
					{ "end_date", dateperiodpicker.EndDateOrNull },
					{ "employee_id", employeeId }
				}
			};
		}

		protected void OnButtonCreateReportClicked(object sender, EventArgs e)
		{
			if(dateperiodpicker.StartDateOrNull == null) {
				MessageDialogHelper.RunErrorDialog("Необходимо выбрать дату");
				return;
			}
			OnUpdate(true);
		}
	}
}
