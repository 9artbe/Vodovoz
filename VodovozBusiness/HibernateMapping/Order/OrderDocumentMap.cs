﻿using FluentNHibernate.Mapping;
using Vodovoz.Domain.Orders.Documents;
using Vodovoz.Domain.Orders.Documents.AssemblyList;
using Vodovoz.Domain.Orders.Documents.Bill;
using Vodovoz.Domain.Orders.Documents.Certificate;
using Vodovoz.Domain.Orders.Documents.DoneWork;
using Vodovoz.Domain.Orders.Documents.DriverTicket;
using Vodovoz.Domain.Orders.Documents.Equipment;
using Vodovoz.Domain.Orders.Documents.Invoice;
using Vodovoz.Domain.Orders.Documents.OrderContract;
using Vodovoz.Domain.Orders.Documents.OrderM2Proxy;
using Vodovoz.Domain.Orders.Documents.ShetFactura;
using Vodovoz.Domain.Orders.Documents.Torg12;
using Vodovoz.Domain.Orders.Documents.Torg2;
using Vodovoz.Domain.Orders.Documents.TransportInvoice;
using Vodovoz.Domain.Orders.Documents.UPD;

namespace Vodovoz.HibernateMapping
{
	public class OrderDocumentMap : ClassMap<OrderDocument>
	{
		public OrderDocumentMap ()
		{
			Table ("order_documents");
			Not.LazyLoad ();
			
			Id (x => x.Id).Column ("id").GeneratedBy.Native ();
			DiscriminateSubClassesOnColumn ("type");
			References (x => x.Order).Column ("order_id");
			References (x => x.AttachedToOrder).Column ("attached_to_order_id");
		}
	}

	public class OrderM2ProxyMap : SubclassMap<OrderM2Proxy>
	{
		public OrderM2ProxyMap()
		{
			DiscriminatorValue("M2Proxy");
			References(x => x.M2Proxy).Column("m2proxy_id").Cascade.SaveUpdate();
		}
	}

	public class OrderAgreementMap : SubclassMap<OrderAgreement>
	{
		public OrderAgreementMap ()
		{
			DiscriminatorValue ("AdditionalAgreement");
			References (x => x.AdditionalAgreement).Column ("agreement_id");
		}
	}

	public class OrderContractMap : SubclassMap<OrderContract>
	{
		public OrderContractMap ()
		{
			DiscriminatorValue ("Contract");
			References (x => x.Contract).Column ("contract_id");
		}
	}

	public class BillDocumentMap : SubclassMap<BillDocument>
	{
		public BillDocumentMap()
		{
			DiscriminatorValue ("Bill");

			Map(x => x.HideSignature).Column("hide_signature");
		}
	}

	public class SpecialBillDocumentMap : SubclassMap<SpecialBillDocument>
	{
		public SpecialBillDocumentMap()
		{
			DiscriminatorValue("SpecialBill");

			Map(x => x.HideSignature).Column("hide_signature");
		}
	}

	public class CoolerWarrantyDocumentMap:SubclassMap<CoolerWarrantyDocument>
	{
		public CoolerWarrantyDocumentMap()
		{
			DiscriminatorValue ("CoolerWarranty");
			Map(x => x.WarrantyNumber).Column("warranty_number");
			References(x => x.Contract).Column("contract_id");
			References(x => x.AdditionalAgreement).Column("agreement_id");
		}
	}

	public class DoneWorkDocumentMap:SubclassMap<DoneWorkDocument>
	{
		public DoneWorkDocumentMap()
		{
			DiscriminatorValue ("DoneWorkReport");
		}
	}

	public class EquipmentTransferDocumentMap:SubclassMap<EquipmentTransferDocument>
	{
		public EquipmentTransferDocumentMap()
		{
			DiscriminatorValue ("EquipmentTransfer");
		}
	}

	public class EquipmentReturnDocumentMap : SubclassMap<EquipmentReturnDocument>
	{
		public EquipmentReturnDocumentMap()
		{
			DiscriminatorValue("EquipmentReturn");
		}
	}


	public class InvoiceBarterDocumentMap:SubclassMap<InvoiceBarterDocument>
	{
		public InvoiceBarterDocumentMap()
		{
			DiscriminatorValue ("InvoiceBarter");
		}
	}

	public class InvoiceContractDocMap : SubclassMap<InvoiceContractDocument>
	{
		public InvoiceContractDocMap()
		{
			DiscriminatorValue("InvoiceContractDoc");

			Map(x => x.WithoutAdvertising).Column("without_advertising");
			Map(x => x.HideSignature).Column("hide_signature");
		}
	}

	public class InvoiceDocumentMap:SubclassMap<InvoiceDocument>
	{
		public InvoiceDocumentMap()
		{
			DiscriminatorValue ("Invoice");

			Map(x => x.WithoutAdvertising).Column("without_advertising");
			Map(x => x.HideSignature).Column("hide_signature");
		}
	}

	public class PumpWarrantyDocumentMap:SubclassMap<PumpWarrantyDocument>
	{
		public PumpWarrantyDocumentMap()
		{
			DiscriminatorValue ("PumpWarranty");
			Map(x => x.WarrantyNumber).Column("warranty_number");
			References(x => x.Contract).Column("contract_id");
			References(x => x.AdditionalAgreement).Column("agreement_id");
		}
	}

	public class UPDDocumentMap:SubclassMap<UPDDocument>
	{
		public UPDDocumentMap()
		{
			DiscriminatorValue ("UPD");
		}
	}

	public class SpecialUPDDocumentMap : SubclassMap<SpecialUPDDocument>
	{
		public SpecialUPDDocumentMap()
		{
			DiscriminatorValue("SpecialUPD");
		}
	}

	public class DriverTicketDocumentMap:SubclassMap<DriverTicketDocument>
	{
		public DriverTicketDocumentMap()
		{
			DiscriminatorValue ("DriverTicket");
		}
	}

	public class Torg12DocumentMap:SubclassMap<Torg12Document>
	{
		public Torg12DocumentMap()
		{
			DiscriminatorValue("Torg12");
		}
	}

	public class ShetFacturaDocumentMap:SubclassMap<ShetFacturaDocument>
	{
		public ShetFacturaDocumentMap()
		{
			DiscriminatorValue("ShetFactura");
		}
	}

	public class RefundBottleDepositDocumentMap : SubclassMap<RefundBottleDepositDocument>
	{
		public RefundBottleDepositDocumentMap()
		{
			DiscriminatorValue("RefundBottleDeposit");
		}
	}

	public class RefundEquipmentDepositDocumentMap : SubclassMap<RefundEquipmentDepositDocument>
	{
		public RefundEquipmentDepositDocumentMap()
		{
			DiscriminatorValue("RefundEquipmentDeposit");
		}
	}

	public class BottleTransferDocumentMap : SubclassMap<BottleTransferDocument>
	{
		public BottleTransferDocumentMap()
		{
			DiscriminatorValue("BottleTransfer");
		}
	}

	public class NomenclatureCertificateDocumentMap : SubclassMap<NomenclatureCertificateDocument>
	{
		public NomenclatureCertificateDocumentMap()
		{
			DiscriminatorValue("ProductCertificate");
			References(x => x.Certificate).Column("certificate_id");
		}
	}

	public class TransportInvoiceDocumentMap : SubclassMap<TransportInvoiceDocument>
	{
		public TransportInvoiceDocumentMap()
		{
			DiscriminatorValue("TransportInvoice");
		}
	}

	public class Torg2DocumentMap : SubclassMap<Torg2Document>
	{
		public Torg2DocumentMap()
		{
			DiscriminatorValue("Torg2");
		}
	}

	public class AssemblyListDocumentMap : SubclassMap<AssemblyListDocument>
	{
		public AssemblyListDocumentMap()
		{
			DiscriminatorValue("AssemblyList");
		}
	}
}