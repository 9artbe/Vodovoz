﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Sale;

namespace Vodovoz.Domain.Logistic
{
	public class AtWorkDriver : AtWorkBase
	{
		private Car car;

		[Display(Name = "Автомобиль")]
		public virtual Car Car {
			get => car;
			set => SetField(ref car, value, () => Car);
		}

		private short priorityAtDay;

		[Display(Name = "Приоритет для текущего дня")]
		public virtual short PriorityAtDay {
			get => priorityAtDay;
			set => SetField(ref priorityAtDay, value, () => PriorityAtDay);
		}

		private TimeSpan? endOfDay;

		[Display(Name = "Конец рабочего дня")]
		public virtual TimeSpan? EndOfDay {
			get => endOfDay;
			set => SetField(ref endOfDay, value, () => EndOfDay);
		}

		public virtual string EndOfDayText {
			get => EndOfDay?.ToString("hh\\:mm");
			set {
				if(String.IsNullOrWhiteSpace(value)) {
					EndOfDay = null;
					return;
				}
				TimeSpan temp;
				if(TimeSpan.TryParse(value, out temp))
					EndOfDay = temp;
			}
		}

		private DeliveryDaySchedule daySchedule;

		[Display(Name = "График работы")]
		public virtual DeliveryDaySchedule DaySchedule {
			get => daySchedule;
			set => SetField(ref daySchedule, value, () => DaySchedule);
		}

		private AtWorkForwarder withForwarder;

		[Display(Name = "С экспедитором")]
		public virtual AtWorkForwarder WithForwarder {
			get => withForwarder;
			set => SetField(ref withForwarder, value, () => WithForwarder);
		}

		GeographicGroup geographicGroup;
		[Display(Name = "База")]
		public virtual GeographicGroup GeographicGroup {
			get => geographicGroup;
			set => SetField(ref geographicGroup, value, () => GeographicGroup);
		}

		private IList<AtWorkDriverDistrictPriority> districts = new List<AtWorkDriverDistrictPriority>();

		[Display(Name = "Районы")]
		public virtual IList<AtWorkDriverDistrictPriority> Districts {
			get => districts;
			set => SetField(ref districts, value, () => Districts);
		}

		GenericObservableList<AtWorkDriverDistrictPriority> observableDistricts;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<AtWorkDriverDistrictPriority> ObservableDistricts {
			get {
				if(observableDistricts == null) {
					observableDistricts = new GenericObservableList<AtWorkDriverDistrictPriority>(districts);
					observableDistricts.ElementAdded += ObservableDistricts_ElementAdded;
					observableDistricts.ElementRemoved += ObservableDistricts_ElementRemoved;
				}
				return observableDistricts;
			}
		}

		protected AtWorkDriver() { }

		public AtWorkDriver(Employee driver, DateTime date, Car car, DeliveryDaySchedule daySchedule = null)
		{
			Date = date;
			Employee = driver;
			priorityAtDay = driver.TripPriority;
			this.car = car;
			DaySchedule = daySchedule;

			districts = new List<AtWorkDriverDistrictPriority>(driver.Districts.Select(x => x.CreateAtDay(this)));
			if(car?.GeographicGroups.Count() == 1)
				this.GeographicGroup = car.GeographicGroups[0];
		}

		#region Функции

		private void CheckDistrictsPriorities()
		{
			for(int i = 0; i < Districts.Count; i++) {
				if(Districts[i] == null) {
					Districts.RemoveAt(i);
					i--;
					continue;
				}

				if(Districts[i].Priority != i)
					Districts[i].Priority = i;
			}
		}

		#endregion

		void ObservableDistricts_ElementAdded(object aList, int[] aIdx)
		{
			CheckDistrictsPriorities();
		}

		void ObservableDistricts_ElementRemoved(object aList, int[] aIdx, object aObject)
		{
			CheckDistrictsPriorities();
		}
	}
}
