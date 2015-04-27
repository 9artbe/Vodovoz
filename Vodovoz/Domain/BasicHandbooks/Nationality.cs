﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;

namespace Vodovoz
{
	[OrmSubject (JournalName = "Национальности", ObjectName = " национальность")]
	public class Nationality : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		string name;
		[Required (ErrorMessage = "Название должно быть заполнено.")]
		[Display(Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value, () => Name); }
		}

		#endregion


		public Nationality ()
		{
			Name = String.Empty;
		}
	}
}

