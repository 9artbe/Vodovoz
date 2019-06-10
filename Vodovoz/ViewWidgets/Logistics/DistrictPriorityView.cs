﻿using System;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.ColumnConfig;
using QSOrmProject;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Sale;
using Vodovoz.Repositories.Sale;

namespace Vodovoz.ViewWidgets.Logistics
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class DistrictPriorityView : QS.Dialog.Gtk.WidgetOnDialogBase
	{
		GenericObservableList<AtWorkDriverDistrictPriority> observableDistricts;

		public GenericObservableList<AtWorkDriverDistrictPriority> Districts {
			get => observableDistricts;
			set {
				observableDistricts = value;
				ytreeviewDistricts.SetItemsSource(observableDistricts);
			}
		}

		public AtWorkDriver ListParent;

		public DistrictPriorityView()
		{
			this.Build();

			ytreeviewDistricts.ColumnsConfig = FluentColumnsConfig<AtWorkDriverDistrictPriority>.Create()
				.AddColumn("Район").AddTextRenderer(x => x.District.DistrictName)
				.AddColumn("Приоритет").AddNumericRenderer(x => x.Priority + 1)
				.Finish();
			ytreeviewDistricts.Reorderable = true;
		}

		protected void OnButtonAddDistrictClicked(object sender, EventArgs e)
		{
			var SelectDistrict = new OrmReference(
				MyOrmDialog.UoW,
				ScheduleRestrictionRepository.AreaWithGeometryQuery()
			) {
				Mode = OrmReferenceMode.MultiSelect
			};
			SelectDistrict.ObjectSelected += SelectDistrict_ObjectSelected;
			MyTab.TabParent.AddSlaveTab(MyTab, SelectDistrict);
		}

		protected void OnButtonRemoveDistrictClicked(object sender, EventArgs e)
		{
			var toRemoveDistricts = ytreeviewDistricts.GetSelectedObjects<AtWorkDriverDistrictPriority>().ToList();
			toRemoveDistricts.ForEach(x => observableDistricts.Remove(x));
		}

		void SelectDistrict_ObjectSelected(object sender, OrmReferenceObjectSectedEventArgs e)
		{
			var addDistricts = e.GetEntities<ScheduleRestrictedDistrict>();
			addDistricts.Where(x => observableDistricts.All(d => d.District.Id != x.Id))
				.Select(x => new AtWorkDriverDistrictPriority {
					Driver = ListParent,
					District = x
				})
				.ToList()
				.ForEach(x => observableDistricts.Add(x));
		}
	}
}
