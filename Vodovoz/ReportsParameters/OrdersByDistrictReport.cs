﻿using System;
using QSOrmProject;
using QSReport;
using System.Collections.Generic;
using Vodovoz.Domain.Logistic;
using QSProjectsLib;

namespace Vodovoz.ReportsParameters
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class OrdersByDistrictReport : Gtk.Bin, IOrmDialog, IParametersWidget
	{
		public OrdersByDistrictReport ()
		{
			this.Build ();
			UoW = UnitOfWorkFactory.CreateWithoutRoot ();
			referenceLogisticArea.SubjectType = typeof (LogisticsArea);
			//referenceLogisticArea.Subject = Repository.Logistics.
		}

		#region IOrmDialog implementation

		public IUnitOfWork UoW { get; private set; }

		public object EntityObject { get { return null; } }

		#endregion

		#region IParametersWidget implementation

		public event EventHandler<LoadReportEventArgs> LoadReport;

		public string Title {
			get {
				return "Отчет по районам";
			}
		}

		#endregion

		private ReportInfo GetReportInfo ()
		{
			return new ReportInfo {
				Identifier = "Orders.OrdersByDistrict",
				UseUserVariables = true,
				Parameters = new Dictionary<string, object>
				{
					{ "start_date", dateperiodpicker.StartDate },
					{ "end_date", dateperiodpicker.EndDate.AddHours(23).AddMinutes(59).AddSeconds(59) },
					{ "id", ((LogisticsArea)referenceLogisticArea.Subject).Id }
				} 
			};
		} 

		protected void OnButtonCreateReportClicked (object sender, EventArgs e)
		{
			string errorString = string.Empty;
			//if (referenceLogisticArea.Subject == null)
				//errorString += "   \n";
			if (dateperiodpicker.StartDateOrNull == null)
				errorString += "Не заполнена дата\n";
			if (!string.IsNullOrWhiteSpace (errorString)) {
				MessageDialogWorks.RunErrorDialog (errorString);
				return;
			}
			OnUpdate (true);
		}

		void OnUpdate (bool hide = false)
		{
			if (LoadReport != null) {
				LoadReport (this, new LoadReportEventArgs (GetReportInfo (), hide));
			}
		}

		void CanRun ()
		{
			buttonCreateReport.Sensitive =
				(dateperiodpicker.EndDateOrNull != null && dateperiodpicker.StartDateOrNull != null);
		}

		protected void OnDateperiodpickerPeriodChanged (object sender, EventArgs e)
		{
			CanRun ();
		}

	}
}
