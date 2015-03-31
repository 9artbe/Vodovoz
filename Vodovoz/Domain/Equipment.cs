﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;
using NHibernate.Criterion;
using System.Collections.Generic;
using NHibernate;

namespace Vodovoz
{
	[OrmSubject ("Оборудование")]
	public class Equipment: PropertyChangedBase, IDomainObject, IValidatableObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		bool onDuty;

		public virtual bool OnDuty {
			get { return onDuty; }
			set { SetField (ref onDuty, value, () => OnDuty); }
		}

		string serial;

		public virtual string Serial {
			get { return serial; }
			set { SetField (ref serial, value, () => Serial); }
		}

		string comment;

		public virtual string Comment {
			get { return comment; }
			set { SetField (ref comment, value, () => Comment); }
		}

		Nomenclature nomenclature;

		public virtual Nomenclature Nomenclature {
			get { return nomenclature; }
			set { SetField (ref nomenclature, value, () => Nomenclature); }
		}

		DateTime lastServiceDate;

		public virtual DateTime LastServiceDate {
			get { return lastServiceDate; }
			set { SetField (ref lastServiceDate, value, () => LastServiceDate); }
		}

		DateTime warrantyEndDate;

		public virtual DateTime WarrantyEndDate {
			get { return warrantyEndDate; }
			set { SetField (ref warrantyEndDate, value, () => WarrantyEndDate); }
		}

		#endregion

		public virtual string Type { get { return Nomenclature == null ? String.Empty : Nomenclature.Type.Name; } }

		public virtual string NomenclatureName { get { return Nomenclature == null ? String.Empty : Nomenclature.Name; } }


		public virtual string LastServiceDateString { get { return LastServiceDate.ToShortDateString (); } }

		public Equipment ()
		{
			Serial = Comment = String.Empty;
		}

		#region IValidatableObject implementation

		public System.Collections.Generic.IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			if (LastServiceDate > DateTime.Now)
				yield return new ValidationResult ("Дата последней санитарной обработки не может быть в будущем.");
			if (Serial == String.Empty)
				yield return new ValidationResult ("Серийный номер должен быть заполнен.");
			if (Nomenclature == null)
				yield return new ValidationResult ("Должна быть указана номенклатура.");
		}

		#endregion
	}

	public static class EquipmentWorks
	{
		public static ICriterion FilterUsedEquipment (ISession session)
		{
			var fAgreements = session.CreateCriteria<FreeRentAgreement> ().List<FreeRentAgreement> ();
			var nAgreements = session.CreateCriteria<NonfreeRentAgreement> ().List<NonfreeRentAgreement> ();
			var IDs = new List<int> ();
			foreach (FreeRentAgreement fr in fAgreements)
				foreach (FreeRentEquipment eq in fr.Equipment)
					IDs.Add (eq.Equipment.Id);
			foreach (NonfreeRentAgreement nfr in nAgreements)
				foreach (PaidRentEquipment eq in nfr.Equipment)
					IDs.Add (eq.Equipment.Id);
			int[] arr = new int[IDs.Count];
			IDs.CopyTo (arr, 0);

			return Restrictions.Not (Restrictions.In ("Id", arr)); 
		}
	}
}

