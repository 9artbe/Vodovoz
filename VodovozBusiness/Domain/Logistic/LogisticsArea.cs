﻿using System;
using System.ComponentModel.DataAnnotations;
using GeoAPI.Geometries;
using QSOrmProject;

namespace Vodovoz.Domain.Logistic
{
	[OrmSubject (Gender = QSProjectsLib.GrammaticalGender.Masculine,
		NominativePlural = "логистические районы",
		Nominative = "логистический район")]
	public class LogisticsArea: PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		string name;

		[Required (ErrorMessage = "Название должно быть заполнено.")]
		[Display (Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value, () => Name); }
		}

		private IGeometry geometry;

		[Display(Name = "Полигон района")]
		public virtual IGeometry Geometry {
			get { return geometry; }
			set { SetField(ref geometry, value, () => Geometry); }
		}

		#endregion

		public LogisticsArea ()
		{
			Name = String.Empty;
		}
	}
}

