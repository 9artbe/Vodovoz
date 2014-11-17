﻿using System;
using QSOrmProject;
using System.Data.Bindings;

namespace Vodovoz
{
	[OrmSubjectAttibutes ("Дополнительные соглашения")]
	public class AdditionalAgreement : IDomainObject
	{
		#region Свойства
		public virtual int Id { get; set; }
		public virtual string AgreementNumber { get; set; }
		public virtual AgreementType Type { get; set; }
		public virtual DateTime IssueDate { get; set; }
		public virtual DateTime StartDate { get; set; }
		//public virtual DeliveryPoint Point { get; set; }
		#endregion

		public AdditionalAgreement ()
		{
			AgreementNumber = String.Empty;
		}
		public virtual string AgreementType { get { return Type.GetEnumTitle(); } }

	}

	public class NonfreeRentAgreement : AdditionalAgreement
	{
		#region Свойства

		#endregion
	}
	public class FreeRentAgreement : AdditionalAgreement
	{
		#region Свойства

		#endregion
	}
	public class WaterSalesAgreement : AdditionalAgreement
	{
		#region Свойства

		#endregion
	}
	public class RepairAgreement : AdditionalAgreement
	{
		#region Свойства

		#endregion
	}

	public enum AgreementType {
		[ItemTitleAttribute("Платная аренда")]
		NonfreeRent,
		[ItemTitleAttribute("Бесплатная аренда")]
		FreeRent,
		[ItemTitleAttribute("Продажа воды")]
		WaterSales,
		[ItemTitleAttribute("Продажа оборудования")]
		EquipmentSales,
		[ItemTitleAttribute("Ремонт")]
		Repair
	}

	public class AgreementTypeStringType : NHibernate.Type.EnumStringType 
	{
		public AgreementTypeStringType() : base(typeof(AgreementType))
		{}
	}
}

