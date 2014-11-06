﻿using System;
using QSOrmProject;
using System.Data.Bindings;
using System.Collections.Generic;
using QSContacts;

namespace Vodovoz
{
	[OrmSubjectAttibutes("Контрагенты")]
	public class Counterparty : QSContacts.ContactOwnBase
	{
		#region Свойства
		public virtual int Id { get; set; }
		public virtual decimal MaxCredit { get; set; }
		public virtual string Name { get; set; }
		public virtual string FullName { get; set; }
		public virtual string Comment { get; set; }
		public virtual string WaybillComment { get; set; }
		public virtual Payment PaymentMethod { get; set; }
		public virtual PersonType PersonType { get; set; }
		public virtual Significance Significance { get; set; }
		public virtual Counterparty MainCounterparty { get; set; }
		public virtual CounterpartyType CounterpartyType { get; set; }
		public virtual CounterpartyStatus Status { get; set; }
		public virtual IList<QSContacts.Phone> Phones { get; set; }
		public virtual IList<QSContacts.Email> Emails { get; set; }
		public virtual Employee Accountant { get; set; }
		public virtual Employee SalesManager { get; set; }
		public virtual Employee BottlesManager { get; set; }
		public virtual Contact MainContact { get; set; }
		public virtual Contact FinancialContact { get; set; }

		#endregion

		public Counterparty ()
		{
			Name = String.Empty;
			FullName = String.Empty;
			Comment = String.Empty;
			WaybillComment = String.Empty;
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

	public enum CounterpartyType{
		[ItemTitleAttribute("Покупатель")]
		customer,
		[ItemTitleAttribute("Поставщик")]
		supplier,
		[ItemTitleAttribute("Партнер")]
		partner,
	}

	public class CounterpartyTypeStringType : NHibernate.Type.EnumStringType 
	{
		public CounterpartyTypeStringType() : base(typeof(CounterpartyType))
		{}
	}
}

