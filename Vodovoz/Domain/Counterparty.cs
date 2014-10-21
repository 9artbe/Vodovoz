﻿using System;
using QSOrmProject;
using System.Data.Bindings;

namespace Vodovoz
{
	[OrmSubjectAttibutes("Контрагенты")]
	public class Counterparty : IDomainObject
	{
		#region Свойства
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string FullName { get; set; }
		public virtual Payment PaymentMethod { get; set; }
		public virtual PersonType PersonType { get; set; } 
		#endregion

		public Counterparty ()
		{
			Name = String.Empty;
			FullName = String.Empty;
		}
	}

	public enum PersonType{
		[ItemTitleAttribute("Физическая")]
		natural,
		[ItemTitleAttribute("Юридическая")]
		legal
	}

	public class PersonTypeStringType : NHibernate.Type.EnumStringType 
	{
		public PersonTypeStringType() : base(typeof(PersonType))
		{}
	}

	public enum Payment{
		[ItemTitleAttribute("Наличная")]
		cash,
		[ItemTitleAttribute("Безналичная")]
		cashless
	}

	public class PaymentStringType : NHibernate.Type.EnumStringType 
	{
		public PaymentStringType() : base(typeof(Payment))
		{}
	}
}

