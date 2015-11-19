﻿using System;
using Vodovoz.Domain.Operations;
using QSOrmProject;
using System.Data.Bindings;

namespace Vodovoz.Domain.Orders
{
	public class OrderDepositItem : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		Order order;

		public virtual Order Order {
			get { return order; }
			set { SetField (ref order, value, () => Order); }
		}

		int count;

		public virtual int Count {
			get { return count; }
			set { SetField (ref count, value, () => Count); }
		}

		PaymentDirection paymentDirection;

		public virtual PaymentDirection PaymentDirection {
			get { return paymentDirection; }
			set { SetField (ref paymentDirection, value, () => PaymentDirection); }
		}

		DepositOperation depositOperation;

		public virtual DepositOperation DepositOperation {
			get { return depositOperation; }
			set { SetField (ref depositOperation, value, () => DepositOperation); }
		}

		PaidRentEquipment paidRentItem;

		public virtual PaidRentEquipment PaidRentItem {
			get { return paidRentItem; }
			set { SetField (ref paidRentItem, value, () => PaidRentItem); }
		}

		FreeRentEquipment freeRentItem;

		public virtual FreeRentEquipment FreeRentItem {
			get { return freeRentItem; }
			set { SetField (ref freeRentItem, value, () => FreeRentItem); }
		}

		DepositType depositType;

		public virtual DepositType DepositType {
			get { return depositType; }
			set { SetField (ref depositType, value, () => DepositType); }
		}

		public virtual string DepositTypeString {
			get { 
				switch (DepositType) {
				case DepositType.Bottles:
					if (PaymentDirection == PaymentDirection.FromClient)
						return "Залог за бутыли";
					return "Возврат залога за бутыли";
				case DepositType.Equipment:
					if (PaymentDirection == PaymentDirection.FromClient)
						return "Залог за оборудование";
					return "Возврат залога за оборудования";
				default:
					return "Не определено";
				}
			} 
		}

		Decimal deposit;

		public virtual Decimal Deposit {
			get { return deposit; }
			set { SetField (ref deposit, value, () => Deposit); }
		}

		public virtual Decimal Total { get { return Deposit * Count; } }
	}

	public enum PaymentDirection
	{
		[ItemTitleAttribute ("Клиенту")]ToClient,
		[ItemTitleAttribute ("От клиента")]FromClient
	}

	public class PaymentDirectionStringType : NHibernate.Type.EnumStringType
	{
		public PaymentDirectionStringType () : base (typeof(PaymentDirection))
		{
		}
	}
}

