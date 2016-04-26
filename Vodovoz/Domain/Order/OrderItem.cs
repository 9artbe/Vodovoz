﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;
using Vodovoz.Domain.Client;

namespace Vodovoz.Domain.Orders
{
	[OrmSubject (Gender = QSProjectsLib.GrammaticalGender.Feminine,
		NominativePlural = "строки заказа",
		Nominative = "строка заказа")]
	public class OrderItem: PropertyChangedBase, IDomainObject, IValidatableObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		Order order;

		[Display (Name = "Заказ")]
		public virtual Order Order {
			get { return order; }
			set { SetField (ref order, value, () => Order); }
		}
			
		AdditionalAgreement additionalAgreement;

		[Display (Name = "Дополнительное соглашения")]
		public virtual AdditionalAgreement AdditionalAgreement {
			get { return additionalAgreement; }
			set { SetField (ref additionalAgreement, value, () => AdditionalAgreement); }
		}

		Nomenclature nomenclature;

		[Display (Name = "Номенклатура")]
		public virtual Nomenclature Nomenclature {
			get { return nomenclature; }
			set { SetField (ref nomenclature, value, () => Nomenclature); }
		}

		Equipment equipment;

		[Display (Name = "Оборудование")]
		public virtual Equipment Equipment {
			get { return equipment; }
			set { SetField (ref equipment, value, () => Equipment); }
		}

		Decimal price;

		[Display (Name = "Цена")]
		public virtual Decimal Price {
			get { return price; }
			set { 
				if(SetField (ref price, value, () => Price))
				{
					RecalculateNDS();
				}
			}
		}

		int count=-1;

		[Display (Name = "Количество")]
		public virtual int Count {
			get { return count; }
			set { 			
				if (count != -1) {	
					var oldDefaultPrice = DefaultPrice;
					var newDefaultPrice = GetDefaultPrice (value);
					if (Price == oldDefaultPrice)
						Price = newDefaultPrice;
					DefaultPrice = newDefaultPrice;
				}
				if(SetField (ref count, value, () => Count))
				{
					RecalculateNDS();
				}
			}
		}

		int actualCount;
		public virtual int ActualCount{
			get{
				return actualCount;
			}
			set{
				SetField(ref actualCount, value, () => ActualCount);
			}
		}

		Decimal includeNDS;

		[Display (Name = "Включая НДС")]
		public virtual Decimal IncludeNDS {
			get { return includeNDS; }
			set { SetField (ref includeNDS, value, () => IncludeNDS); }
		}

		#endregion

		#region Вычисляемы

		public virtual int ReturnedCount{
			get{
				return Count - ActualCount;
			}
			set{
				ActualCount = Count - value;
			}
		}

		public virtual bool IsDelivered{
			get{
				return ReturnedCount == 0;
			}
			set{
				ReturnedCount = value ? 0 : 1;
			}
		}

		protected Decimal GetDefaultPrice(int count){
			Decimal result=0;
			result = Nomenclature.GetPrice(count);
			if (Nomenclature.Category == NomenclatureCategory.water) {
				var waterSalesAgreement = AdditionalAgreement as WaterSalesAgreement;
				if (waterSalesAgreement != null && waterSalesAgreement.IsFixedPrice)
					result = waterSalesAgreement.FixedPrice;
			}
			return result;
		}

		Decimal defaultPrice=-1;

		public virtual Decimal DefaultPrice {
			get { 
				if (defaultPrice == -1) {
					defaultPrice = GetDefaultPrice (count);
				}
				return defaultPrice; 
			}
			set { SetField (ref defaultPrice, value, () => DefaultPrice); }
		}

		public virtual bool HasUserSpecifiedPrice(){
			return price != DefaultPrice;
		}

		public decimal Sum{
			get{
				return Price * Count;
			}
		}

		public virtual bool CanEditAmount {
			get { return AdditionalAgreement == null || AdditionalAgreement.Type == AgreementType.WaterSales; }
		}

		public virtual string NomenclatureString {
			get { return Nomenclature != null ? Nomenclature.Name : ""; }
		}

		public virtual string AgreementString {
			get { return AdditionalAgreement == null ? String.Empty : String.Format ("{0} №{1}", AdditionalAgreement.AgreementTypeTitle, AdditionalAgreement.AgreementNumber); }
		}

		#endregion
	
		#region IValidatableObject implementation

		public System.Collections.Generic.IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			return null;
		}

		#endregion

		#region Внутрение

		private void RecalculateNDS()
		{
			if (Nomenclature == null || Sum <= 0)
				return;

			switch (Nomenclature.VAT)
			{
				case VAT.Vat18:
					IncludeNDS = Math.Round(Sum * 0.18m, 2);
					break;
				case VAT.Vat10:
					IncludeNDS = Math.Round(Sum * 0.10m, 2);
					break;
				default:
					break;
			}
		}

		#endregion
	}
}

