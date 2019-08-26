﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gamma.Utilities;
using QS.DomainModel.Entity;
using QS.DomainModel.Entity.EntityPermissions;
using QS.HistoryLog;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Goods;

namespace Vodovoz.Domain.Suppliers
{
	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "строки заявок поставщику",
		Nominative = "строка заявки поставщику"
	)]
	[HistoryTrace]
	[EntityPermission]
	public class RequestToSupplierItem : PropertyChangedBase, IDomainObject, IValidatableObject, ILevelingRequestNode
	{
		#region свойства для маппинга

		public virtual int Id { get; set; }

		Nomenclature nomenclature;
		[Display(Name = "Запрашиваемая номенклатура")]
		public virtual Nomenclature Nomenclature {
			get => nomenclature;
			set => SetField(ref nomenclature, value);
		}

		int quantity;
		[Display(Name = "Количество")]
		public virtual int Quantity {
			get => quantity;
			set => SetField(ref quantity, value);
		}

		RequestToSupplier requestToSupplier;
		[Display(Name = "Заявка поставщику")]
		public virtual RequestToSupplier RequestToSupplier {
			get => requestToSupplier;
			set => SetField(ref requestToSupplier, value);
		}

		bool transfered;
		[Display(Name = "Перенесена в другой запрос")]
		public virtual bool Transfered {
			get => transfered;
			set => SetField(ref transfered, value);
		}

		RequestToSupplierItem transferedFrom;
		[Display(Name = "Строка перенесена из строки")]
		public virtual RequestToSupplierItem TransferedFromItem {
			get => transferedFrom;
			set => SetField(ref transferedFrom, value);
		}

		#endregion свойства для маппинга

		public virtual ILevelingRequestNode Parent { get; set; } = null;
		public virtual IList<ILevelingRequestNode> Children { get; set; }
		public virtual SupplierPriceItem SupplierPriceItem { get; set; }

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if(Quantity <= 0)
				yield return new ValidationResult(
					string.Format("Укажите количество ТМЦ \"{0}\"", Nomenclature.ShortOrFullName),
					new[] { this.GetPropertyName(o => o.Quantity) }
				);
			;
		}

		#region Calculatable methods



		#endregion Calculatable methods
	}
}