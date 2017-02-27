﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;

namespace Vodovoz.Domain
{
	[OrmSubject (Gender = QSProjectsLib.GrammaticalGender.Masculine,
		NominativePlural = "шаблоны комментариев для штрафов",
		Nominative = "шаблон комментария для штрафа")]
	public class FineCommentTemplate : PropertyChangedBase, IDomainObject, IValidatableObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		string comment;

		public virtual string Comment { 
			get { return comment; } 
			set { SetField (ref comment, value, () => Comment); }
		}

		#endregion

		public FineCommentTemplate ()
		{
			Comment = String.Empty;
		}

		#region IValidatableObject implementation

		public virtual System.Collections.Generic.IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			if (String.IsNullOrEmpty (Comment))
				yield return new ValidationResult ("Текст комментария должен быть заполнен.", new [] { "Comment" });
		}

		#endregion
	}
}

