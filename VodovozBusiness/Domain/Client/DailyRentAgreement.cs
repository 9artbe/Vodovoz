﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using QS.DomainModel.Entity;
using QS.DomainModel.Entity.EntityPermissions;
using QS.DomainModel.UoW;

namespace Vodovoz.Domain.Client
{

	[Appellative (Gender = GrammaticalGender.Neuter,
		NominativePlural = "доп. соглашения посуточной аренды",
		Nominative = "доп. соглашение посуточной аренды")]
	[EntityPermission]
	public class DailyRentAgreement : AdditionalAgreement
	{
		[Display (Name = "Количество дней аренды")]
		public virtual int RentDays { get; set; }

		IList<PaidRentEquipment> equipment = new List<PaidRentEquipment> ();

		[Display (Name = "Список оборудования")]
		public virtual IList<PaidRentEquipment> Equipment {
			get { return equipment; }
			set { SetField (ref equipment, value, () => Equipment); }
		}

		GenericObservableList<PaidRentEquipment> observableEquipment;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<PaidRentEquipment> ObservableEquipment {
			get {
				if (observableEquipment == null)
					observableEquipment = new GenericObservableList<PaidRentEquipment> (Equipment);
				return observableEquipment;
			}
		}

		public virtual DateTime EndDate{
			get{
				return base.StartDate.AddDays(RentDays);
			}
			set {; }
		}

		public override IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			foreach (ValidationResult result in base.Validate (validationContext))
				yield return result;
			
			if (RentDays < 1)
				yield return new ValidationResult ("Срок аренды не может быть меньше одного дня.", new[] { "RentDays" });

			if (!Equipment.Any())
				yield return new ValidationResult("Необходимо добавить в список оборудование.", new[] { "Equipment" });
		}

		public static IUnitOfWorkGeneric<DailyRentAgreement> Create (CounterpartyContract contract)
		{
			var uow = UnitOfWorkFactory.CreateWithNewRoot<DailyRentAgreement> ($"Создание нового доп. соглашения посуточной аренды для договора {contract.Number}.");
			uow.Root.Contract = uow.GetById<CounterpartyContract>(contract.Id);
			uow.Root.AgreementNumber = AdditionalAgreement.GetNumberWithType (uow.Root.Contract, AgreementType.DailyRent);
			return uow;
		}

		public virtual void RemoveEquipment(PaidRentEquipment paidEquipment)
		{
			foreach(PaidRentEquipment eq in this.ObservableEquipment.CreateList())
			{
				if(eq == paidEquipment)
				{
					ObservableEquipment.Remove(eq);
				}
			}
		}
	}
}