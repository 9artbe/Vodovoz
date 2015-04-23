﻿using System;
using QSOrmProject;
using System.Collections.Generic;

namespace Vodovoz
{
	[OrmSubject ("Договоры контрагента")]
	public class CounterpartyContract : PropertyChangedBase, IDomainObject, IAdditionalAgreementOwner, ISpecialRowsRender
	{
		private IList<AdditionalAgreement> agreements { get; set; }

		#region IAdditionalAgreementOwner implementation

		public virtual IList<AdditionalAgreement> AdditionalAgreements {
			get { return agreements; }
			set { agreements = value; }
		}

		#endregion

		public virtual int Id { get; set; }

		int maxDelay;

		public virtual int MaxDelay {
			get { return maxDelay; }
			set { SetField (ref maxDelay, value, () => MaxDelay); }
		}

		bool isNew;

		public virtual bool IsNew {
			get { return isNew; }
			set { SetField (ref isNew, value, () => IsNew); }
		}

		bool isArchive;

		public virtual bool IsArchive {
			get { return isArchive; }
			set { SetField (ref isArchive, value, () => IsArchive); }
		}

		bool onCancellation;

		public virtual bool OnCancellation {
			get { return onCancellation; }
			set { SetField (ref onCancellation, value, () => OnCancellation); }
		}

		public virtual string Number { get { return Id > 0 ? Id.ToString () : "Не определен"; } set { } }

		DateTime issueDate;

		public virtual DateTime IssueDate {
			get { return issueDate; }
			set { SetField (ref issueDate, value, () => IssueDate); }
		}

		Organization organization;

		public virtual Organization Organization {
			get { return organization; }
			set { SetField (ref organization, value, () => Organization); }
		}

		Counterparty counterparty;

		public virtual Counterparty Counterparty {
			get { return counterparty; }
			set { SetField (ref counterparty, value, () => Counterparty); }
		}

		#region ISpecialRowsRender implementation

		public string TextColor {
			get {
				if (IsArchive)
					return "grey";
				if (OnCancellation)
					return "blue";
				return "black";
					
			}
		}

		#endregion

		public virtual string Title { 
			get { return String.Format ("№{0} от {1}", Id, IssueDate.ToShortDateString ()); }
		}

		public virtual string OrganizationTitle { 
			get { return Organization != null ? Organization.FullName : "Не указана"; }
		}

		public virtual string AdditionalAgreementsCount { 
			get { return AdditionalAgreements != null ? AdditionalAgreements.Count.ToString () : "0"; }
		}
	}

	public interface IAdditionalAgreementOwner
	{
		IList<AdditionalAgreement> AdditionalAgreements { get; set; }
	}
}

