﻿using System;
using QSOrmProject;
using QSReport;
using System.Collections.Generic;
using Vodovoz.Domain.Employees;

namespace Vodovoz.Reports
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ForwarderWageReport : Gtk.Bin, IOrmDialog, IParametersWidget
	{
		public ForwarderWageReport()
		{
			this.Build();
			UoW = UnitOfWorkFactory.CreateWithoutRoot ();
			yentryreferenceForwarder.SubjectType = typeof(Employee);
			yentryreferenceForwarder.ItemsQuery = Repository.EmployeeRepository.ForwarderQuery();
		}

		#region IOrmDialog implementation

		public IUnitOfWork UoW { get; private set; }

		public object EntityObject {
			get {
				return null;
			}
		}

		#endregion

		#region IParametersWidget implementation

		public event EventHandler<LoadReportEventArgs> LoadReport;

		public string Title {
			get	{
				return "Отчет по зарплате экспедитора";
			}
		}

		#endregion

		private ReportInfo GetReportInfo()
		{			
			return new ReportInfo
			{
				Identifier = "Employees.ForwarderWage",
				Parameters = new Dictionary<string, object>
				{ 
					{ "start_date", dateperiodpicker.StartDateOrNull },
					{ "end_date", dateperiodpicker.EndDateOrNull },
					{ "forwarder_id", (yentryreferenceForwarder.Subject as Employee)?.Id }
				}
			};
		}

		void OnUpdate(bool hide = false)
		{
			if (LoadReport != null)
			{
				LoadReport(this, new LoadReportEventArgs(GetReportInfo(), hide));
			}
		}

		void CanRun()
		{
			buttonCreateReport.Sensitive = 
				(dateperiodpicker.EndDateOrNull != null && dateperiodpicker.StartDateOrNull != null
					&& yentryreferenceForwarder.Subject != null);
		}

		protected void OnButtonCreateReportClicked (object sender, EventArgs e)
		{
			OnUpdate(true);
		}

		protected void OnDateperiodpickerPeriodChanged (object sender, EventArgs e)
		{
			CanRun();
		}

		protected void OnYentryreferenceForwarderChanged (object sender, EventArgs e)
		{
			CanRun();
		}
	}
}

