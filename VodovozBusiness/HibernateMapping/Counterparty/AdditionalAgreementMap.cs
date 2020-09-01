﻿﻿using FluentNHibernate.Mapping;
 using Vodovoz.Domain.Client;

 namespace Vodovoz.HibernateMapping
{
	public class AdditionalAgreementMap : ClassMap<AdditionalAgreement>
	{
		public AdditionalAgreementMap ()
		{
			Table ("additional_agreements");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();
			DiscriminateSubClassesOnColumn ("type");
			Map(x => x.AgreementNumber).Column("number");
			Map(x => x.StartDate).Column("start_date");
			Map(x => x.IssueDate).Column("issue_date");
			Map(x => x.IsCancelled).Column("is_cancelled");
			Map(x => x.ChangedTemplateFile).Column("template_file").LazyLoad();

			References (x => x.Contract).Column ("counterparty_contract_id");
			References (x => x.DeliveryPoint).Column ("delivery_point_id");
			References(x => x.DocumentTemplate).Column("template_id");
		}
	}

	public class NonfreeRentAgreementMap : SubclassMap<NonfreeRentAgreement>
	{
		public NonfreeRentAgreementMap ()
		{
			DiscriminatorValue ("NonfreeRent");
			Map(x => x.RentMonths).Column("rent_months");
			HasMany (x => x.PaidRentEquipments).Cascade.AllDeleteOrphan ().LazyLoad ()
				.KeyColumn ("additional_agreement_id");
		}
	}

	public class WaterSalesAgreementMap : SubclassMap<WaterSalesAgreement>
	{
		public WaterSalesAgreementMap()
		{
			DiscriminatorValue ("WaterSales");
			HasMany(x => x.FixedPrices).KeyColumn("agreement_id").Inverse().Cascade.AllDeleteOrphan().LazyLoad();
		}
	}
}