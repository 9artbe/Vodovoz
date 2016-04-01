﻿using System;
using QSTDI;
using System.Xml;
using Vodovoz.ExportTo1c;
using System.Linq;
using QSProjectsLib;
using QSOrmProject;
using Vodovoz.Repository;
using System.Collections.Generic;
using Vodovoz.Domain.Orders;
using Gtk;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ExportTo1cDialog : TdiTabBase
	{
		private IUnitOfWork UoW;
		private IList<Order> orders;
		private bool exportInProgress;

		public ExportTo1cDialog()
		{
			this.Build();
			UoW = UnitOfWorkFactory.CreateWithoutRoot();
			this.TabName="Выгрузка в 1с";
		}

		private ExportData exportData;

		protected void OnButtonExportClicked (object sender, EventArgs e)
		{
			var dateStart = dateperiodpicker1.StartDate;
			var dateEnd = dateperiodpicker1.EndDate;
			var exportOperation = new ExportOperation(dateStart, dateEnd);

			this.exportInProgress = true;
			UpdateExportButtonSensitivity();
			LongOperationDlg.StartOperation(exportOperation.Run, "", 1, false);
			this.exportInProgress = false;
			UpdateExportButtonSensitivity();


			exportData = exportOperation.Result;
			this.labelTotalCounterparty.Text = exportData.Objects
				.OfType<CatalogObjectNode>()
				.Count(node => node.Type == Common1cTypes.ReferenceCounterparty)
				.ToString();
			this.labelTotalNomenclature.Text = exportData.Objects
				.OfType<CatalogObjectNode>()
				.Count(node => node.Type == Common1cTypes.ReferenceNomenclature)
				.ToString();
			this.labelTotalSales.Text = exportData.Objects
				.OfType<SalesDocumentNode>()
				.Count()
				.ToString();
			this.labelTotalInvoices.Text = exportData.Objects
				.OfType<InvoiceDocumentNode>()
				.Count()
				.ToString();
			buttonSave.Sensitive = exportData != null;
		}			

		protected void OnButtonSaveClicked (object sender, EventArgs e)
		{			
			var settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				Indent = true,
				Encoding = System.Text.Encoding.UTF8,
				NewLineChars = "\r\n"
			};			
			var fileChooser = new Gtk.FileChooserDialog("Выберите файл для сохранения выгрузки",
				(Window)this.Toplevel,
				Gtk.FileChooserAction.Save,
				"Отмена",ResponseType.Cancel,
				"Сохранить",ResponseType.Accept
			);
			fileChooser.CurrentName = "Выгрузка 1с на " + exportData.EndPeriodDate.ToShortDateString()+".xml";
			var filter = new FileFilter();
			filter.AddPattern("*.xml");
			fileChooser.Filter = filter;
			if (fileChooser.Run() == (int)ResponseType.Accept)
			{
				var filename = fileChooser.Filename.EndsWith(".xml") ? fileChooser.Filename : fileChooser.Filename + ".xml";
				using (XmlWriter writer = XmlWriter.Create(filename, settings))
				{				
					exportData.ToXml().WriteTo(writer);
				}
			}
			fileChooser.Destroy();
		}

		private void UpdateExportButtonSensitivity(){
			buttonExport.Sensitive = !exportInProgress 
				&& dateperiodpicker1.EndDateOrNull != null 
				&& dateperiodpicker1.StartDateOrNull != null 
				&& dateperiodpicker1.StartDate <= dateperiodpicker1.EndDate;
		}

		protected void OnDateperiodpicker1PeriodChanged (object sender, EventArgs e)
		{
			UpdateExportButtonSensitivity();
		}
	}
}

