﻿using System;
using System.ComponentModel.DataAnnotations;
using Gamma.Utilities;
using QS.DomainModel.Entity;

namespace Vodovoz.ViewModels.Logistic
{
	public enum AddressType
	{
		[Display(Name = "Доставка")]
		Delivery,
		[Display(Name = "Сервисное обслуживание")]
		Service,
		[Display(Name = "Сетевой магазин")]
		ChainStore,
		[Display(Name = "Складская логика")]
		StorageLogic
	}

	public class AddressTypeNode : PropertyChangedBase
	{
		private bool selected;
		public virtual bool Selected {
			get => selected;
			set => SetField(ref selected, value);
		}

		public AddressType AddressType { get; }

		public string Title => AddressType.GetEnumTitle();

		public AddressTypeNode(AddressType addressType)
		{
			AddressType = addressType;
		}
	}
}
