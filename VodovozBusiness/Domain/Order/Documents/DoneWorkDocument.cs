﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QSReport;
using Vodovoz.Domain.Service;

namespace Vodovoz.Domain.Orders.Documents
{
	public class DoneWorkDocument:OrderDocument
	{
		#region implemented abstract members of OrderDocument

		public override OrderDocumentType Type {
			get {
				return OrderDocumentType.DoneWorkReport;
			}
		}

		public override QSReport.ReportInfo GetReportInfo ()
		{
			return new QSReport.ReportInfo {
				Title = Name,
				Identifier = "Documents.DoneWorkReport",
				Parameters = new Dictionary<string, object> {
					{ "order_id",  Order.Id },
					{ "service_claim_id",ServiceClaim.Id }
				}
			};
		}

		#endregion

		ServiceClaim serviceClaim;

		[Display (Name = "Заявка на сервис")]
		public virtual ServiceClaim ServiceClaim {
			get { return serviceClaim; }
			set { SetField (ref serviceClaim, value, () => ServiceClaim); }
		}

		public override string Name {
			get { return String.Format ("Акт выполненных работ №{0}", serviceClaim.Id); }
		}

		public override DateTime? DocumentDate {
			get { return serviceClaim?.ServiceStartDate; }
		}

		public override PrinterType PrintType {
			get {
				return PrinterType.RDL;
			}
		}
	}
}

