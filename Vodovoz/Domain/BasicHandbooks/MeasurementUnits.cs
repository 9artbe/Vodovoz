﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;

namespace Vodovoz.Domain
{
	[OrmSubject (JournalName = "Единицы измерения", ObjectName = "единица измерения")]
	public class MeasurementUnits : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		string name;
		[Required (ErrorMessage = "Название должно быть заполнено.")]
		[StringLength(10)]
		[Display(Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value, () => Name); }
		}

		short digits;

		[Display(Name = "Знаков после запятой")]
		public virtual short Digits {
			get { return digits; }
			set { SetField (ref digits, value, () => Digits); }
		}

		#endregion

		public MeasurementUnits ()
		{
			Name = String.Empty;
		}
	}
}

