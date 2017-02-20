﻿using System;
using QSOrmProject;
using QSReport;
using System.Collections.Generic;
using Gamma.GtkWidgets;
using Vodovoz.Domain.Employees;
using Gamma.ColumnConfig;
using System.Linq;
using NHibernate.Transform;
using QSProjectsLib;

namespace Vodovoz.Reports
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class DriversWageBalanceReport : Gtk.Bin, IOrmDialog, IParametersWidget
	{
		private class DriverNode
		{
			public int 	  Id 		 { get; set; }
			public string Name 		 { get; set; }
			public string LastName 	 { get; set; }
			public string FullName 	 { get {return LastName + " " + Name;} }
			public bool   IsSelected { get; set; } = false;
		}

		IColumnsConfig columnsConfig = ColumnsConfigFactory.Create<DriverNode> ()
			.AddColumn("Имя").AddTextRenderer(d => d.FullName)
			.AddColumn("Выбрать").AddToggleRenderer(d => d.IsSelected)
			.Finish();

		IList<DriverNode> driversList = new List<DriverNode>();

		public DriversWageBalanceReport()
		{
			this.Build();
			UoW = UnitOfWorkFactory.CreateWithoutRoot();
			ConfigureWgt();
		}

		#region IOrmDialog implementation

		public IUnitOfWork UoW { get; private set; }

		public object EntityObject { get {return null;} }

		#endregion

		#region IParametersWidget implementation

		public event EventHandler<LoadReportEventArgs> LoadReport;

		public string Title {
			get	{
				return "Отчет по балансу водителей";
			}
		}

		#endregion

		private void ConfigureWgt()
		{
			FillDrivers();

			ytreeviewDrivers.ColumnsConfig = columnsConfig;
			ytreeviewDrivers.SetItemsSource(driversList);

			ydateBalanceBefore.Date = DateTime.Today;
		}

		void OnUpdate(bool hide = false)
		{
			if (LoadReport != null)
			{
				LoadReport(this, new LoadReportEventArgs(GetReportInfo(), hide));
			}
		}

		private ReportInfo GetReportInfo()
		{			
			return new ReportInfo
			{
				Identifier = "Employees.DriversWageBalance",
				Parameters = new Dictionary<string, object>
				{ 
					{ "date", ydateBalanceBefore.Date.Date.AddDays(1).AddTicks(-1) },
					{ "drivers", driversList.Where(d => d.IsSelected).Select(d => d.Id) }
				}
			};
		}

		private void FillDrivers()
		{
			DriverNode resultAlias = null;
			Employee employeeAlias = null;

			driversList = UoW.Session.QueryOver<Employee>(() => employeeAlias)
				.Where(() => employeeAlias.Category == EmployeeCategory.driver)
				.SelectList(list => list
					.Select(() => employeeAlias.Id).WithAlias(() => resultAlias.Id)
					.Select(() => employeeAlias.Name).WithAlias(() => resultAlias.Name)
					.Select(() => employeeAlias.LastName).WithAlias(() => resultAlias.LastName))
				.OrderBy(e => e.LastName).Asc
				.TransformUsing(Transformers.AliasToBean<DriverNode>())
				.List<DriverNode>();
		}

		protected void OnButtonCreateReportClicked (object sender, EventArgs e)
		{
			if (driversList.Where(d => d.IsSelected).Select(d => d.Id).Count() <= 0)
			{
				MessageDialogWorks.RunErrorDialog("Необходимо выбрать хотя бы одного водителя");
				return;
			}

			OnUpdate(true);
		}

		protected void OnButtonSelectAllClicked (object sender, EventArgs e)
		{
			foreach (var item in driversList)
				item.IsSelected = true;
			ytreeviewDrivers.SetItemsSource(driversList);
		}

		protected void OnButtonUnselectAllClicked (object sender, EventArgs e)
		{
			foreach (var item in driversList)
				item.IsSelected = false;
			ytreeviewDrivers.SetItemsSource(driversList);
		}

	}
}

