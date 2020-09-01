﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Gamma.Utilities;
using NHibernate.Criterion;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;
using QS.HistoryLog;

namespace Vodovoz.Domain.Client
{
	[Appellative (Gender = GrammaticalGender.Neuter,
		NominativePlural = "дополнительные соглашения",
		Nominative = "дополнительное соглашение",
		Accusative = "дополнительное соглашение",
		Genitive = "дополнительного соглашения"
	)]
	public class AdditionalAgreement : PropertyChangedBase, IDomainObject, IValidatableObject
	{
		/// <summary>
		/// Используется для возможности приведения общего типа к конкретному, если
		/// напрямую привести не удается. 
		/// AdditionalAgreement a = entity.self;
		/// (a as WaterSalesAgreement).IsFixedPrice
		/// где IsFixedPrice доступно только для WaterSalesAgreement
		/// </summary> 
		public virtual AdditionalAgreement Self => this;

		public virtual int Id { get; set; }

		int agreementNumber;

		[Display (Name = "Номер")]
		[PropertyChangedAlso("FullNumberText")]
		public virtual int AgreementNumber {
			get => agreementNumber;
			set => SetField(ref agreementNumber, value, () => AgreementNumber);
		}

		DocTemplate agreeemntTemplate;

		[Display (Name = "Шаблон договора")]
		public virtual DocTemplate DocumentTemplate {
			get => agreeemntTemplate;
			protected set => SetField(ref agreeemntTemplate, value, () => DocumentTemplate);
		}

		byte[] changedTemplateFile;

		[Display (Name = "Измененное соглашение")]
		//[PropertyChangedAlso("FileSize")]
		public virtual byte[] ChangedTemplateFile {
			get => changedTemplateFile;
			set => SetField(ref changedTemplateFile, value, () => ChangedTemplateFile);
		}

		[Display (Name = "Тип доп. соглашения")]
		public virtual AgreementType Type {
			get {
				if (this is NonfreeRentAgreement)
					return AgreementType.NonfreeRent;
				if (this is WaterSalesAgreement)
					return AgreementType.WaterSales;
				throw new NotImplementedException();
			}		
		}

		[Required (ErrorMessage = "Договор должен быть указан.")]
		[Display (Name = "Договор")]
		[PropertyChangedAlso("FullNumberText")]
		public virtual CounterpartyContract Contract { get; set; }

		[Required (ErrorMessage = "Дата создания должна быть указана.")]
		[Display (Name = "Дата подписания")]
		[HistoryDateOnly]
		public virtual DateTime IssueDate { get; set; }

		[Required (ErrorMessage = "Дата начала действия должна быть указана.")]
		[Display (Name = "Дата начала")]
		[HistoryDateOnly]
		public virtual DateTime StartDate { get; set; }

		[Display (Name = "Точка доставки")]
		public virtual DeliveryPoint DeliveryPoint { get; set; }

		[Display (Name = "Закрыто")]
		public virtual bool IsCancelled { get; set; }

		#region Вычисляемые

		public virtual string AgreementDeliveryPoint => DeliveryPoint != null ? DeliveryPoint.CompiledAddress : "Не указана";

		public virtual string AgreementTypeTitle => Type.GetEnumTitle();

		public virtual string Title => string.Format("Доп. соглашение №{0} от {1}", FullNumberText, StartDate.ToShortDateString());

		[Display(Name = "Полный номер")]
		public virtual string FullNumberText => string.Format("{0}-{1}/{2}{3}", Contract.Counterparty.VodovozInternalId, Contract.ContractSubNumber, GetTypePrefix(Type), AgreementNumber);

		#endregion

		public AdditionalAgreement ()
		{
			IssueDate = StartDate = DateTime.Today;
		}

		public virtual IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			int count = 0;
			foreach (AdditionalAgreement agreement in Contract.AdditionalAgreements)
				if (agreement.AgreementNumber == this.AgreementNumber && agreement.Type == this.Type)
					count++;
			if (count > 1)
				yield return new ValidationResult ("Доп. соглашение с таким номером уже существует.", new[] { "AgreementNumber" });
		}

		/// <summary>
		/// Updates template for the additional agreement.
		/// </summary>
		/// <returns><c>true</c>, in case of successful update, <c>false</c> if template for the additional agreement was not found.</returns>
		/// <param name="uow">Unit of Work.</param>
		public virtual bool UpdateContractTemplate(IUnitOfWork uow)
		{
			if (Contract == null)
			{
				DocumentTemplate = null;
				ChangedTemplateFile = null;
			}
			else
			{
				var newTemplate = Repository.Client.DocTemplateRepository.GetTemplate(uow, GetTemplateType(Type), Contract.Organization, Contract.ContractType);
				if(newTemplate == null) {
					DocumentTemplate = null;
					ChangedTemplateFile = null;
					return false;
				}
				if (!DomainHelper.EqualDomainObjects(newTemplate, DocumentTemplate))
				{
					DocumentTemplate = newTemplate;
					ChangedTemplateFile = null;
					return true;
				}
			}
			return false;
		}
		#region Статические

		public static int GetNumberWithType(CounterpartyContract contract, AgreementType type)
		{
			//Вычисляем номер для нового соглашения.
			var additionalAgreements = contract.AdditionalAgreements;
			var numbers = additionalAgreements.Where(x => x.Type == type).Select(x => x.AgreementNumber).ToList();
			numbers.Sort();
			return numbers.Any() ? numbers.Last() + 1 : 1;
		}

		public static int GetNumberWithTypeFromDB<TAgreement>(CounterpartyContract contract) 
			where TAgreement : AdditionalAgreement
		{
			using(var uow = UnitOfWorkFactory.CreateWithoutRoot()) {
				var maxNumber = uow.Session.QueryOver<TAgreement>()
								   .Where(x => x.Contract.Id == contract.Id)
				                   .Select(Projections.Max<TAgreement>(y => y.AgreementNumber))
				                   .SingleOrDefault<int>();
				return maxNumber + 1;
			}
		}

		public static string GetTypePrefix(AgreementType type)
		{
			switch (type)
			{
				case AgreementType.NonfreeRent:
					return "АМ";
				case AgreementType.WaterSales:
					return "В";
				default:
					throw new InvalidOperationException(string.Format("Тип {0} не поддерживается.", type));
			}
		}
			

		public static TemplateType GetTemplateType(AgreementType type)
		{
			switch (type)
			{
				case AgreementType.NonfreeRent:
					return TemplateType.AgLongRent;
				case AgreementType.WaterSales:
					return TemplateType.AgWater;
				default:
					throw new InvalidOperationException(string.Format("Тип {0} не поддерживается.", type));
			}
		}

		#endregion

		/// <summary>
		/// Возвращает типы доп соглашений которые создаются на 
		/// каждое новое создание аренды в заказе, и могут хранится 
		/// в неограниченном количестве в договоре
		/// </summary>
		public static AgreementType[] GetOrderBasedAgreementTypes()
		{
			return new AgreementType[] {
				AgreementType.NonfreeRent
			};
		}
	}

	public enum AgreementType
	{
		[Display (Name = "Долгосрочная аренда")]
		NonfreeRent,
		[Display (Name = "Продажа воды")]
		WaterSales,
	}

	public class AgreementTypeStringType : NHibernate.Type.EnumStringType
	{
		public AgreementTypeStringType () : base (typeof(AgreementType))
		{
		}
	}

	public enum OrderAgreementType
	{
		[Display (Name = "Долгосрочная аренда")]
		NonfreeRent,
		[Display (Name = "Бесплатная аренда")]
		FreeRent
	}

	public interface IAgreementSaved
	{
		event EventHandler<AgreementSavedEventArgs> AgreementSaved;
	}

	public class AgreementSavedEventArgs : EventArgs
	{
		public AdditionalAgreement Agreement { get; private set; }

		public AgreementSavedEventArgs (AdditionalAgreement agreement)
		{
			Agreement = agreement;
		}
	}
}