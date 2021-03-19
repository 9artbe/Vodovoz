﻿using System;
using System.Collections.Generic;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QS.Project.Journal.EntitySelector;
using QS.Report;
using QSReport;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Retail;

namespace Vodovoz.ReportsParameters.Retail
{
    public partial class QualityReport : SingleUoWWidgetBase, IParametersWidget
    {
        public QualityReport(IEntityAutocompleteSelectorFactory counterpartySelectorFactory,
            IEntityAutocompleteSelectorFactory salesChannelSelectorFactory,
            IEntityAutocompleteSelectorFactory employeeSelectorFactory)
        {
            this.Build();
            UoW = UnitOfWorkFactory.CreateWithoutRoot();
            Configure(counterpartySelectorFactory, salesChannelSelectorFactory, employeeSelectorFactory);
        }

        private void Configure(IEntityAutocompleteSelectorFactory counterpartySelectorFactory,
            IEntityAutocompleteSelectorFactory salesChannelSelectorFactory,
            IEntityAutocompleteSelectorFactory employeeSelectorFactory)
        {
            buttonCreateReport.Clicked += (sender, e) => Validate();
            yEntityCounterParty.SetEntityAutocompleteSelectorFactory(counterpartySelectorFactory);
            yEntitySalesChannel.SetEntityAutocompleteSelectorFactory(salesChannelSelectorFactory);
            yEntityMainContact.SetEntityAutocompleteSelectorFactory(employeeSelectorFactory);
        }

        private ReportInfo GetReportInfo()
        {
            var parameters = new Dictionary<string, object> {
                { "create_date", ydateperiodpickerCreate.StartDateOrNull },
                { "end_date", ydateperiodpickerCreate.EndDateOrNull },
                { "shipping_date", ydateperiodpickerShippind.StartDateOrNull },
                { "shipping_end_date", ydateperiodpickerShippind.EndDateOrNull },
                { "counterparty_id", ((Counterparty)yEntityCounterParty.Subject)?.Id ?? 0 },
                { "sales_channel_id", ((SalesChannel)yEntitySalesChannel.Subject)?.Id ?? 0},
                { "main_contact_id", ((Employee)yEntityMainContact.Subject)?.Id ?? 0}
            };

            return new ReportInfo
            {
                Identifier = "Retail.QualityReport",
                Parameters = parameters
            };
        }

        public string Title => $"Качественный отчет";

        public event EventHandler<LoadReportEventArgs> LoadReport;

        void OnUpdate(bool hide = false) => LoadReport?.Invoke(this, new LoadReportEventArgs(GetReportInfo(), hide));

        void Validate()
        {
            string errorString = string.Empty;
            if (!(ydateperiodpickerCreate.StartDateOrNull.HasValue &&
                ydateperiodpickerCreate.EndDateOrNull.HasValue &&
                ydateperiodpickerShippind.StartDateOrNull.HasValue &&
                ydateperiodpickerShippind.EndDateOrNull.HasValue))
            {
                errorString = "Не выбран ни один из фильтров дат";
                MessageDialogHelper.RunErrorDialog(errorString);
                return;
            }
            OnUpdate(true);
        }
    }
}
