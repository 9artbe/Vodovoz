﻿using System;
using QSOrmProject;
using System.ComponentModel.DataAnnotations;

namespace Vodovoz
{
	[OrmSubject (JournalName = "Значимость контрагента", ObjectName = "значимость контрагента")]
	public class Significance : PropertyChangedBase, IDomainObject
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

		public Significance ()
		{
			Name = String.Empty;
		}
	}
}

