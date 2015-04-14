﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings;

namespace Vodovoz
{
	public class Operation: PropertyChangedBase, IDomainObject, IValidatableObject
	{
		public virtual int Id { get; set; }

		DateTime operationTime;

		public virtual DateTime OperationTime {
			get { return operationTime; }
			set { SetField (ref operationTime, value, () => OperationTime); }
		}

		Order order;

		public virtual Order Order {
			get { return order; }
			set { SetField (ref order, value, () => Order); }
		}

		#region IValidatableObject implementation

		public System.Collections.Generic.IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			return null;
		}

		#endregion
	}

	public enum PaymentType
	{
		[ItemTitleAttribute ("Наличная оплата")] Cash,
		[ItemTitleAttribute ("Безналичная оплата")] Clearing
	}

	public enum DepositType
	{
		[ItemTitleAttribute ("Отсутствует")] None,
		[ItemTitleAttribute ("Тара")] Bottles,
		[ItemTitleAttribute ("Оборудование")] Equipment
	}
}

