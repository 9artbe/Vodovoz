﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Vodovoz
{
	[OrmSubject ("Оборудование для платной аренды")]
	public class PaidRentEquipment : PropertyChangedBase, IDomainObject, IValidatableObject
	{
		public virtual int Id { get; set; }

		PaidRentPackage paidRentPackage;

		public virtual PaidRentPackage PaidRentPackage {
			get { return paidRentPackage; }
			set { SetField (ref paidRentPackage, value, () => PaidRentPackage); }
		}

		Equipment equipment;

		public virtual Equipment Equipment {
			get { return equipment; }
			set { SetField (ref equipment, value, () => Equipment); }
		}

		int rentPeriod;

		public virtual int RentPeriod {
			get { return rentPeriod; }
			set { SetField (ref rentPeriod, value, () => RentPeriod); }
		}

		Decimal price;

		public virtual Decimal Price {
			get { return price; }
			set { SetField (ref price, value, () => Price); }
		}

		bool isNew;

		public virtual bool IsNew {
			get { return isNew; }
			set { SetField (ref isNew, value, () => IsNew); }
		}

		public virtual string PackageName { get { return PaidRentPackage != null ? PaidRentPackage.Name : ""; } }

		public virtual string EquipmentName { get { return Equipment != null ? Equipment.NomenclatureName : ""; } }

		public virtual string EquipmentSerial { get { return Equipment != null ? Equipment.Serial : ""; } }

		public virtual string PriceString { get { return String.Format ("{0} р.", Price); } }

		#region IValidatableObject implementation

		public IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
		{
			if (PaidRentPackage == null)
				yield return new ValidationResult ("Не выбран пакет платной аренды.", new[] { "PaidRentPackage" });

			if (Equipment == null)
				yield return new ValidationResult ("Не выбрано оборудование.", new[] { "Equipment" });
		}

		#endregion

		public PaidRentEquipment ()
		{
		}
	}
}

