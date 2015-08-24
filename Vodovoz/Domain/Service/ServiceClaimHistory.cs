﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;

namespace Vodovoz.Domain.Service
{
	[OrmSubject (Gender = QSProjectsLib.GrammaticalGender.Feminine,
		NominativePlural = "строки истории заявки на обслуживание",
		Nominative = "строка истории заявки на обслуживание")]
	public class ServiceClaimHistory: PropertyChangedBase, IDomainObject, IValidatableObject
	{
		public virtual int Id { get; set; }

		DateTime date;

		public virtual DateTime Date { 
			get { return date; } 
			set { SetField (ref date, value, () => Date); }
		}

		Employee employee;

		public virtual Employee Employee {
			get { return employee; }
			set { SetField (ref employee, value, () => Employee); }
		}

		ServiceClaimStatus status;

		public virtual ServiceClaimStatus Status {
			get { return status; }
			set { SetField (ref status, value, () => Status); }
		}

		string comment;

		public virtual string Comment {
			get { return comment; }
			set { SetField (ref comment, value, () => Comment); }
		}

		public System.Collections.Generic.IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			if (Comment.Length > 200)
				yield return new ValidationResult ("Комментарий не может быть длиннее 200 символов.",
					new[] { this.GetPropertyName (o => o.Comment) });
		}

	}
}

