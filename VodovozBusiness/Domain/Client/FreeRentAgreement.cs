﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using QS.DomainModel.Entity;
using QS.DomainModel.Entity.EntityPermissions;
using QS.DomainModel.UoW;

namespace Vodovoz.Domain.Client
{

	[Appellative (Gender = GrammaticalGender.Neuter,
		NominativePlural = "доп. соглашения бесплатной аренды",
		Nominative = "доп. соглашение бесплатной аренды")]
	[EntityPermission]
	public class FreeRentAgreement : AdditionalAgreement
	{
		IList<FreeRentEquipment> equipment = new List<FreeRentEquipment> ();

		[Display (Name = "Список оборудования")]
		public virtual IList<FreeRentEquipment> Equipment {
			get { return equipment; }
			set { SetField (ref equipment, value, () => Equipment); }
		}

		GenericObservableList<FreeRentEquipment> observableEquipment;
		//FIXME Костыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<FreeRentEquipment> ObservableEquipment {
			get {
				if (observableEquipment == null)
					observableEquipment = new GenericObservableList<FreeRentEquipment> (Equipment);
				return observableEquipment;
			}
		}

		public override IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			foreach (ValidationResult result in base.Validate (validationContext))
				yield return result;
			
			if (!Equipment.Any())
				yield return new ValidationResult("Необходимо добавить в список оборудование", new[] { "Equipment" });
		}

		public static IUnitOfWorkGeneric<FreeRentAgreement> Create (CounterpartyContract contract)
		{
			var uow = UnitOfWorkFactory.CreateWithNewRoot<FreeRentAgreement> ($"Создание нового доп. соглашения бесплатной аренды для договора {contract.Number}.");
			uow.Root.Contract = uow.GetById<CounterpartyContract>(contract.Id);
			uow.Root.AgreementNumber = AdditionalAgreement.GetNumberWithType (uow.Root.Contract, AgreementType.FreeRent);
			return uow;
		}

		public virtual void RemoveEquipment(FreeRentEquipment freeEquipment)
		{
			foreach (FreeRentEquipment eq in this.ObservableEquipment.CreateList())
			{
				if (eq == freeEquipment)
				{
					ObservableEquipment.Remove(eq);
				}
			}
		}
	}
	
}
