﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;
using QSBanks;
using Vodovoz.Domain.Cash;

namespace Vodovoz.Domain.Accounting
{
	[OrmSubject (Gender = QSProjectsLib.GrammaticalGender.Masculine,
		NominativePlural = "операции прихода",
		Nominative = "операция прихода")]
	public class AccountIncome: PropertyChangedBase, IDomainObject, IValidatableObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		int number;

		public virtual int Number {
			get { return number; }
			set { SetField (ref number, value, () => Number); }
		}

		private DateTime date;

		[Display (Name = "Дата")]
		public DateTime Date {
			get { return date; }
			set { SetField (ref date, value, () => Date); }
		}

		decimal total;

		public virtual decimal Total {
			get { return total; }
			set { SetField (ref total, value, () => Total); }
		}

		string description;

		public virtual string Description {
			get { return description; }
			set { SetField (ref description, value, () => Description); }
		}

		Organization organization;

		public virtual Organization Organization {
			get { return organization; }
			set { SetField (ref organization, value, () => Organization); }
		}

		Account organizationAccount;

		public virtual Account OrganizationAccount {
			get { return organizationAccount; }
			set { SetField (ref organizationAccount, value, () => OrganizationAccount); }
		}

		Counterparty counterparty;

		public virtual Counterparty Counterparty { 
			get { return counterparty; }
			set { SetField (ref counterparty, value, () => Counterparty); }
		}

		Account counterpartyAccount;

		public virtual Account CounterpartyAccount {
			get { return counterpartyAccount; }
			set { SetField (ref counterpartyAccount, value, () => CounterpartyAccount); }
		}

		IncomeCategory category;

		public virtual IncomeCategory Category {
			get { return category; }
			set { SetField (ref category, value, () => Category); }
		}

		#endregion

		public AccountIncome ()
		{
		}

		#region IValidatableObject implementation

		public System.Collections.Generic.IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}


