﻿using System;
using System.Collections.Generic;
using QS.DomainModel.UoW;
using QS.Dialog;
using QS.Report;
using QSReport;
using QS.Dialog.GtkUI;

namespace Vodovoz.ReportsParameters
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class DriversToDistrictsAssignmentReport : SingleUoWWidgetBase, IParametersWidget
    {
        public DriversToDistrictsAssignmentReport()
        {
            this.Build();
            UoW = UnitOfWorkFactory.CreateWithoutRoot();
        }

        #region IParametersWidget implementation

        public event EventHandler<LoadReportEventArgs> LoadReport;

        public string Title => "Отчет по распределению водителей на районы";

        #endregion

        private ReportInfo GetReportInfo()
        {
            return new ReportInfo
            {
                Identifier = "Logistic.DriversToDistrictsAssignmentReport",
                UseUserVariables = true,
                Parameters = new Dictionary<string, object>
                {
                    { "start_date", dateperiodpicker.StartDateOrNull },
                    { "end_date", dateperiodpicker.EndDate.AddDays(1).AddTicks(-1) },
                    { "only_different_districts", onlyDifferentDistricts.Active }
                }
            };
        }

        protected void OnButtonCreateReportClicked(object sender, EventArgs e)
        {
            OnUpdate(true);
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
                (dateperiodpicker.EndDateOrNull != null && dateperiodpicker.StartDateOrNull != null);
        }

        protected void OnDateperiodpickerPeriodChanged(object sender, EventArgs e)
        {
            CanRun();
        }
    }
}
