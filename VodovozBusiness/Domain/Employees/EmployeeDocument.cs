﻿using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Vodovoz.Domain.Employees
{
	[Appellative(Gender = GrammaticalGender.Masculine,
		NominativePlural = "документы",
		Nominative = "документ")]
	public class EmployeeDocument: PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		public DocumentType document;

		public enum DocumentType
		{
			[Display(Name = "Паспотр")]
			passport,
			[Display(Name = "Загранпаспорт")]
			internationalPassport,
			[Display(Name = "Свидетельство о рождении")]
			birthCertificate,
			[Display(Name = "Удостоверение офицера")]
			officerCertificate,
			[Display(Name = "Справка об освобождении из места лишения свободы ")]
			prisonReleaseCertificate,
			[Display(Name = "Паспорт морфлот")]
			navyPassport,
			[Display(Name = "Военный билет")]
			militaryID,
			[Display(Name = "Диппаспорт")]
			dippasport,
			[Display(Name = "Инпаспорт")]
			inPassport,
			[Display(Name = "Свидетельство беженца")]
			refugeeCertificate,
			[Display(Name = "Вид на жительство")]
			residence,
			[Display(Name = "Удостоверение беженца")]
			refugeeId,
			[Display(Name = "Временное удостоверение")]
			temporaryId
		}

		string passportSeria;

		[Display(Name = "Серия паспорта")]
		public virtual string PassportSeria {
			get { return passportSeria; }
			set { SetField(ref passportSeria, value, () => PassportSeria); }
		}

		string passportNumber;

		[Display(Name = "Номер паспорта")]
		public virtual string PassportNumber {
			get { return passportNumber; }
			set { SetField(ref passportNumber, value, () => PassportNumber); }
		}

		string passportIssuedOrg;

		[Display(Name = "Кем выдан паспорт")]
		public virtual string PassportIssuedOrg {
			get { return passportIssuedOrg; }
			set { SetField(ref passportIssuedOrg, value, () => PassportIssuedOrg); }
		}

		private DateTime? passportIssuedDate;

		[Display(Name = "Дата выдачи паспорта")]
		public virtual DateTime? PassportIssuedDate {
			get { return passportIssuedDate; }
			set { SetField(ref passportIssuedDate, value, () => PassportIssuedDate); }
		}
	}

	public class DocumentTypeStringType : NHibernate.Type.EnumStringType
	{
		public DocumentTypeStringType() : base(typeof(EmployeeDocument.DocumentType))
		{
		}
	}
}
