﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using Gamma.Utilities;
using QS.DomainModel.Entity;
using QS.DomainModel.Entity.EntityPermissions;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Sale;
using Vodovoz.Domain.Store;
using Vodovoz.Repositories.HumanResources;
using QS.DomainModel.UoW;
using System.Linq;
using System.Text;

namespace Vodovoz
{
	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "подразделения",
		Nominative = "подразделение")]
	[EntityPermission]
	public class Subdivision : PropertyChangedBase, IDomainObject, IValidatableObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display(Name = "Название подразделения")]
		[Required(ErrorMessage = "Название подразделения должно быть заполнено.")]
		public virtual string Name {
			get => name;
			set => SetField(ref name, value, () => Name);
		}

		private Employee chief;

		[Display(Name = "Начальник подразделения")]
		public virtual Employee Chief {
			get => chief;
			set => SetField(ref chief, value, () => Chief);
		}

		private Subdivision parentSubdivision;

		[Display(Name = "Вышестоящее подразделение")]
		public virtual Subdivision ParentSubdivision {
			get => parentSubdivision;
			set => SetField(ref parentSubdivision, value, () => ParentSubdivision);
		}

		IList<Subdivision> childSubdivisions = new List<Subdivision>();

		[Display(Name = "Дочерние подразделения")]
		public virtual IList<Subdivision> ChildSubdivisions {
			get => childSubdivisions;
			set => SetField(ref childSubdivisions, value, () => ChildSubdivisions);
		}

		GenericObservableList<Subdivision> observableChildSubdivisions;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<Subdivision> ObservableChildSubdivisions {
			get {
				if(observableChildSubdivisions == null)
					observableChildSubdivisions = new GenericObservableList<Subdivision>(ChildSubdivisions);
				return observableChildSubdivisions;
			}
		}

		IList<ScheduleRestrictedDistrict> servicingDistricts = new List<ScheduleRestrictedDistrict>();

		[Display(Name = "Обслуживаемые районы")]
		public virtual IList<ScheduleRestrictedDistrict> ServicingDistricts {
			get => servicingDistricts;
			set => SetField(ref servicingDistricts, value, () => ServicingDistricts);
		}

		GenericObservableList<ScheduleRestrictedDistrict> observableServicingDistricts;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<ScheduleRestrictedDistrict> ObservableServicingDistricts {
			get {
				if(observableServicingDistricts == null)
					observableServicingDistricts = new GenericObservableList<ScheduleRestrictedDistrict>(ServicingDistricts);
				return observableServicingDistricts;
			}
		}

		#endregion

		#region Геттеры и методы

		/// <summary>
		/// Уровень в иерархии
		/// </summary>
		public virtual int GetLevel => ParentSubdivision == null ? 0 : ParentSubdivision.GetLevel + 1;

		/// <summary>
		/// Является ли подразделение ребёнком другого подразделения?
		/// </summary>
		/// <returns><c>true</c>, если является, <c>false</c> если не является.</returns>
		/// <param name="subdivision">Предпологаемый родитель.</param>
		public virtual bool IsChildOf(Subdivision subdivision)
		{
			if(this == subdivision)
				return false;
			Subdivision parent = ParentSubdivision;
			while(parent != null) {
				if(parent == subdivision)
					return true;
				parent = parent.ParentSubdivision;
			}
			return false;
		}

		public virtual string GetWarehousesNames(IUnitOfWork uow)
		{
			string result = string.Empty;
			if(Id != 0) {
				var whs = SubdivisionsRepository.GetWarehouses(uow, this).Select(w => w.Name);
				result = string.Join(", ", whs);
			}
			return result;
		}

		#endregion

		public Subdivision() { }

		#region IValidatableObject implementation

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if(string.IsNullOrWhiteSpace(Name))
				yield return new ValidationResult("Название подразделения должно быть заполнено.",
					new[] { this.GetPropertyName(o => o.Name) });

			if(ParentSubdivision != null && ParentSubdivision.IsChildOf(this))
				yield return new ValidationResult(
					"Нельзя указывать 'Дочернее подразделение' в качестве родительского.",
					new[] { this.GetPropertyName(o => o.ParentSubdivision) }
				);
		}

		#endregion
	}
}

