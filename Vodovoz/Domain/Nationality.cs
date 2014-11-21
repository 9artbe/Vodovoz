﻿using System;
using QSOrmProject;

namespace Vodovoz
{
	[OrmSubjectAttributes("Национальности")]
	public class Nationality : IDomainObject
	{
		#region Свойства
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		#endregion


		public Nationality()
		{
			Name = String.Empty;
		}
	}
}

