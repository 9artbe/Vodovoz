﻿using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using Gtk;
using QSProjectsLib;
using QSReport;
using QSTDI;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Orders;
using Vodovoz.Domain.Orders.Documents;

namespace Vodovoz.Dialogs
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OrderDocumentsPrinterDlg : TdiTabBase
	{
		Order currentOrder;
		MultipleDocumentPrinter multipleDocumentPrinter = new MultipleDocumentPrinter();
		List<SelectablePrintDocument> printDocuments = new List<SelectablePrintDocument>();

		public OrderDocumentsPrinterDlg(Order order)
		{
			this.Build();

			TabName = "Печать документов заказа";

			currentOrder = order;
			foreach(var item in currentOrder.OrderDocuments) {
				////var contract = item as OrderContract;
				//bool b1 = item is IHavingTemplateDocument;
				//var d = (item as IHavingTemplateDocument);
				//bool b2 = d.DocumentTemplate == null;

				//var document = ((OrderContract)item);
				//switch(item.Type) {
				//	case OrderDocumentType.AdditionalAgreement:
						
				//	case OrderDocumentType.Contract:
						
				//		break;
				//	default:
				//		break;
				//}

				//if(item is IHavingTemplateDocument && ((IHavingTemplateDocument)item).DocumentTemplate == null){
				//	MessageDialogWorks.RunWarningDialog(
				//		String.Format("Документ '{0}' в комплект печати добавлен не был, т.к. " +
				//		              "для него не установлен шаблон документа"), item.Name
				//	);
				//	continue;
				//}
					printDocuments.Add(new SelectablePrintDocument(item, DefaultCopies(item.Type)) { Selected = true });
			}

			Configure();
		}

		int DefaultCopies(OrderDocumentType orderDocType)
		{
			switch(orderDocType) {
				case OrderDocumentType.Bill:
				case OrderDocumentType.DriverTicket:
				case OrderDocumentType.M2Proxy:
					return 1;
				case OrderDocumentType.UPD:
				case OrderDocumentType.Torg12:
				case OrderDocumentType.ShetFactura:
					return currentOrder.DocumentType == Domain.Client.DefaultDocumentType.torg12 ? 1 : 2;
				default:
					return 2;
			}
		}

		void Configure()
		{
			multipleDocumentPrinter.PrintableDocuments = new GenericObservableList<SelectablePrintDocument>(printDocuments);

			ytreeviewDocuments.ColumnsConfig = ColumnsConfigFactory.Create<SelectablePrintDocument>()
				.AddColumn("✓").AddToggleRenderer(x => x.Selected)
				.AddColumn("Документ").AddTextRenderer(x => x.Document.Name)
				.AddColumn("Копий").AddNumericRenderer(x => x.Copies).Editing()
					   .Adjustment(new Adjustment(0, 0, 10000, 1, 100, 0))
				.RowCells()
				.Finish();

			ytreeviewDocuments.ItemsDataSource = multipleDocumentPrinter.PrintableDocuments;

			DefaultPreviewDocument();
		}

		protected void DefaultPreviewDocument()
		{
			var documents = printDocuments.Where(x => x.Document is OrderDocument)
										  .Where(x => (x.Document as OrderDocument).Order.Id == currentOrder.Id);

			var driverTicket = documents.Where(x => x.Document is DriverTicketDocument).FirstOrDefault();
			var invoiceDocument = documents.Where(x => x.Document is InvoiceDocument).FirstOrDefault();
			if(driverTicket != null && currentOrder.PaymentType == Domain.Client.PaymentType.cashless) {
				PreviewDocument(driverTicket);
			} else if(invoiceDocument != null) {
				PreviewDocument(invoiceDocument);
			}
		}

		void PreviewDocument(SelectablePrintDocument selectedDocument)
		{
			if(selectedDocument == null)
				return;

			if(selectedDocument.Document.PrintType != PrinterType.RDL) {

				return;
			}
			var reportInfo = selectedDocument.Document.GetReportInfo();
			reportviewer.LoadReport(reportInfo.GetReportUri(), reportInfo.GetParametersString(), reportInfo.ConnectionString, true);
		}

		protected void OnButtonPrintAllClicked(object sender, EventArgs e)
		{
			multipleDocumentPrinter.PrintSelectedDocuments();
		}

		protected void OnButtonPrintClicked(object sender, EventArgs e)
		{
			var selectedDocument = ytreeviewDocuments.GetSelectedObject() as SelectablePrintDocument;
			if(selectedDocument == null) {
				return;
			}
			multipleDocumentPrinter.PrintDocument(selectedDocument);
		}

		protected void OnYtreeviewDocumentsRowActivated(object o, RowActivatedArgs args)
		{
			PreviewDocument(ytreeviewDocuments.GetSelectedObject() as SelectablePrintDocument);
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			OnCloseTab(false);
		}
	}
}
