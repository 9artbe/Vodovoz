﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;
using QS.DomainModel.Entity.EntityPermissions;
using QS.HistoryLog;

namespace Vodovoz.Domain.Cash
{
	[Appellative (Gender = GrammaticalGender.Feminine,
		NominativePlural = "статьи расхода",
		Nominative = "статья расхода")]
	[EntityPermission]
	[HistoryTrace]
	public class ExpenseCategory : DomainTreeNodeBase<ExpenseCategory>, IDomainObject, IValidatableObject
	{
		#region Свойства

		string name;

		[Required (ErrorMessage = "Название статьи должно быть заполнено.")]
		[Display (Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value, () => Name); }
		}

		ExpenseInvoiceDocumentType expenseDocumentType;
		/// <summary>
		/// Тип расходного ордера для которого возможно будет выбрать эту категорию
		/// </summary>
		[Required(ErrorMessage = "Должно быть заполнен тип расходного ордера.")]
		[Display(Name = "Тип расходного ордера")]
		public virtual ExpenseInvoiceDocumentType ExpenseDocumentType {
			get { return expenseDocumentType; }
			set { SetField(ref expenseDocumentType, value, () => ExpenseDocumentType); }
		}

		#endregion

		public ExpenseCategory ()
		{
			Name = String.Empty;
		}


		#region IValidatableObject implementation

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			yield break;
		}

		#endregion IValidatableObject implementation
	}
}

