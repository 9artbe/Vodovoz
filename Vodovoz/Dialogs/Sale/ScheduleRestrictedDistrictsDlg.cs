﻿using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.ColumnConfig;
using GeoAPI.Geometries;
using GMap.NET;
using GMap.NET.GtkSharp;
using GMap.NET.GtkSharp.Markers;
using Gtk;
using NetTopologySuite.Geometries;
using QS.Dialog.Gtk;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QSOrmProject;
using QSProjectsLib;
using QSValidation;
using Vodovoz.Additions.Logistic;
using Vodovoz.Dialogs.Sale;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Sale;

namespace Vodovoz.Dialogs.Logistic
{
	public partial class ScheduleRestrictedDistrictsDlg : QS.Dialog.Gtk.TdiTabBase
	{
		IUnitOfWork uow = UnitOfWorkFactory.CreateWithoutRoot();

		readonly GMapOverlay bordersOverlay = new GMapOverlay("district_borders");
		readonly GMapOverlay verticeOverlay = new GMapOverlay("district_vertice");

		IList<PointLatLng> currentBorderVertice, newBorderVertice;
		GenericObservableList<ScheduleRestrictedDistrict> observableRestrictedDistricts;

		ScheduleRestrictedDistrict currentDistrict;

		bool creatingNewBorder = false;

		GeometryFactory gf = new GeometryFactory(new PrecisionModel(), 3857);

		public ScheduleRestrictedDistrictsDlg()
		{
			this.Build();
			Configure();
		}

		void Configure()
		{
			TabName = "Районы с графиками доставки";
			ytreeDistricts.ColumnsConfig = FluentColumnsConfig<ScheduleRestrictedDistrict>.Create()
				.AddColumn("Название").AddTextRenderer(x => x.DistrictName).Editable()
				.AddColumn("Тарифная зона").AddComboRenderer(x => x.TariffZone)
					.SetDisplayFunc(x => x.Name)
					.FillItems(uow.GetAll<TariffZone>().ToList(), "Нет").Editing()
				.AddColumn("Мин. бутылей").AddNumericRenderer(x => x.MinBottles)
				.Adjustment(new Adjustment(1, 0, 1000, 1, 100, 1)).Editing()
				.AddColumn("Ценообразование").AddEnumRenderer(x => x.PriceType).Editing()
				.AddColumn("Цена воды").AddNumericRenderer(x => x.WaterPrice).Digits(2)
				.Adjustment(new Adjustment(0, 0, 100000, 1, 100, 1))
				.AddSetter((c, row) => c.Editable = row.PriceType == DistrictWaterPrice.FixForDistrict)
				.Finish();
			ytreeDistricts.SetItemsSource(ObservableRestrictedDistricts);
			ytreeDistricts.Selection.Changed += OnYTreeDistricts_SelectionChanged;

			ytreeSchedules.Selection.Mode = Gtk.SelectionMode.Single;
			ytreeSchedules.ColumnsConfig = FluentColumnsConfig<DeliverySchedule>.Create()
				.AddColumn("График").HeaderAlignment(0.5f).AddTextRenderer(x => x.Name)
				.Finish();
			ytreeSchedules.Selection.Changed += OnYTreeSchedules_SelectionChanged;

			yTreeGeographicGroups.Selection.Mode = Gtk.SelectionMode.Single;
			yTreeGeographicGroups.ColumnsConfig = FluentColumnsConfig<GeographicGroup>.Create()
				.AddColumn("Часть города").HeaderAlignment(0.5f).AddTextRenderer(x => x.Name)
				.Finish();
			yTreeGeographicGroups.Selection.Changed += (sender, e) => ControlsAccessibility();

			btnToday.TooltipText = "День в день.\nГрафик доставки при создании заказа сегодня и на сегодняшнюю дату доставки.";

			ControlsAccessibility();

			// Пока кнопочки всё равно не работают.
			buttonAddVertex.Sensitive = buttonAddVertex.Visible
				= buttonMoveVertex.Sensitive = buttonMoveVertex.Visible
				= buttonRemoveVertex.Sensitive = buttonRemoveVertex.Visible
				= false;

			yenumcomboMapType.ItemsEnum = typeof(MapProviders);
			yenumcomboMapType.SelectedItem = MapProviders.YandexMap;
			yenumcomboMapType.ChangedByUser += YenumcomboMapType_ChangedByUser;
			YenumcomboMapType_ChangedByUser(null, null);

			gmapWidget.Position = new PointLatLng(59.93900, 30.31646);
			gmapWidget.HeightRequest = 150;
			gmapWidget.HasFrame = true;
			gmapWidget.Overlays.Add(bordersOverlay);
			gmapWidget.Overlays.Add(verticeOverlay);
			ShowBorders();
			ControlsAccessibility();
		}

		private void ObservableItemsField_ListContentChanged(object sender, EventArgs e)
		{
			ytreeSchedules.QueueDraw();
		}

		void YenumcomboMapType_ChangedByUser(object sender, EventArgs e)
		{
			gmapWidget.MapProvider = MapProvidersHelper.GetPovider((MapProviders)yenumcomboMapType.SelectedItem);
		}

		protected void OnYTreeDistricts_SelectionChanged(object sender, EventArgs e)
		{
			UpdateCurrentDistrict();
		}

		void UpdateCurrentDistrict()
		{
			currentDistrict = ytreeDistricts.GetSelectedObject() as ScheduleRestrictedDistrict;

			if(currentDistrict != null) {
				btnToday.Click();
				yTreeGeographicGroups.ItemsDataSource = currentDistrict.ObservableGeographicGroups;
			}

			if(currentDistrict != null && currentDistrict.DistrictBorder != null)
				currentBorderVertice = GetCurrentBorderVertice();
			else
				currentBorderVertice = new List<PointLatLng>();

			ShowBorderVertice(currentBorderVertice);
			ControlsAccessibility();
		}


		protected void OnYTreeSchedules_SelectionChanged(object sender, EventArgs e)
		{
			ControlsAccessibility();
		}

		protected void OnButtonAddDistrictClicked(object sender, EventArgs e)
		{
			var district = new ScheduleRestrictedDistrict();
			observableRestrictedDistricts.Add(district);
			UpdateCurrentDistrict();
		}

		protected void OnBtnEditDistrictClicked(object sender, EventArgs e)
		{
			if(currentDistrict.Id == 0
			&& MessageDialogHelper.RunQuestionDialog("Для продолжения необходимо сохранить район, сохранить и продолжить?")) {
				uow.Save(currentDistrict);
				uow.Commit();
			}
			TabParent.OpenTab(
				DialogHelper.GenerateDialogHashName<ScheduleRestrictedDistrict>(currentDistrict.Id),
				() => new ScheduleRestrictedDistrictDlg(currentDistrict)
			);
		}

		protected void OnButtonDeleteDistrictClicked(object sender, EventArgs e)
		{
			if(currentDistrict.Id != 0 && QSMain.User.Admin)
				OrmMain.DeleteObject(typeof(ScheduleRestrictedDistrict), currentDistrict.Id);
			else if(currentDistrict.Id == 0) {
				var mapPolygon = bordersOverlay.Polygons.FirstOrDefault(p => (p.Tag as ScheduleRestrictedDistrict) == currentDistrict);
				if(mapPolygon != null)
					mapPolygon.IsVisible = false;
				observableRestrictedDistricts.Remove(currentDistrict);
				UpdateCurrentDistrict();
			}
		}

		protected void OnButtonAddScheduleClicked(object sender, EventArgs e)
		{
			var SelectSchedules = new OrmReference(typeof(DeliverySchedule), uow) {
				Mode = OrmReferenceMode.MultiSelect
			};
			SelectSchedules.ObjectSelected += SelectSchedules_ObjectSelected;
			TabParent.AddSlaveTab(this, SelectSchedules);
		}

		void SelectSchedules_ObjectSelected(object sender, OrmReferenceObjectSectedEventArgs e)
		{
			if(ytreeSchedules.ItemsDataSource is GenericObservableList<DeliverySchedule> scheduleList)
				foreach(var item in e.Subjects) {
					if(item is DeliverySchedule schedule && !scheduleList.Any(x => x.Id == schedule.Id))
						scheduleList.Add(schedule);
				}
		}

		protected void OnButtonDeleteScheduleClicked(object sender, EventArgs e)
		{
			var scheduleList = ytreeSchedules.ItemsDataSource as GenericObservableList<DeliverySchedule>;
			if(ytreeSchedules.GetSelectedObject() is DeliverySchedule selectedObj && scheduleList != null)
				scheduleList.Remove(selectedObj);
		}

		public virtual GenericObservableList<ScheduleRestrictedDistrict> ObservableRestrictedDistricts {
			get {
				if(observableRestrictedDistricts == null) {
					observableRestrictedDistricts = new GenericObservableList<ScheduleRestrictedDistrict>(GetAllDistricts());
				}
				return observableRestrictedDistricts;
			}
		}

		void OnObservableRestrictedDistricts_ElementAdded(object sender, int[] aIdx)
		{
			ytreeDistricts.SetItemsSource(ObservableRestrictedDistricts);
		}

		void OnObservableRestrictedDistricts_ElementRemoved(object aList, int[] aIdx, object aObject)
		{
			ytreeDistricts.SetItemsSource(ObservableRestrictedDistricts);
		}

		void OnObservableRestrictions_ElementAdded(object sender, int[] aIdx)
		{
			UpdateCurrentDistrict();
		}

		void OnObservableRestrictions_ElementRemoved(object aList, int[] aIdx, object aObject)
		{
			UpdateCurrentDistrict();
		}

		void ControlsAccessibility()
		{
			buttonDeleteDistrict.Sensitive = ytreeDistricts.Selection.CountSelectedRows() == 1 && (currentDistrict.Id == 0 || QSMain.User.Admin);
			buttonCreateBorder.Sensitive = ytreeDistricts.Selection.CountSelectedRows() == 1;
			buttonRemoveBorder.Sensitive = ytreeDistricts.Selection.CountSelectedRows() == 1 && currentDistrict != null && currentDistrict.DistrictBorder != null;
			btnEditDistrict.Sensitive = buttonAddSchedule.Sensitive = currentDistrict != null;
			vboxGeographicGroups.Visible = vboxSchedules.Visible = currentDistrict != null;
			btnAddGeographicGroup.Sensitive = currentDistrict != null && !currentDistrict.GeographicGroups.Any();
		}

		IList<ScheduleRestrictedDistrict> GetAllDistricts()
		{
			var srdQuery = uow.Session.QueryOver<ScheduleRestrictedDistrict>()
							  .List<ScheduleRestrictedDistrict>();

			return srdQuery;
		}

		protected void OnButtonSaveClicked(object sender, EventArgs e)
		{
			foreach(ScheduleRestrictedDistrict district in observableRestrictedDistricts) {
				var valid = new QSValidator<ScheduleRestrictedDistrict>(district);
				if(valid.RunDlgIfNotValid((Gtk.Window)this.Toplevel))
					return;
			}

			foreach(ScheduleRestrictedDistrict district in observableRestrictedDistricts)
				district.Save(uow);

			uow.Commit();
		}

		protected void OnButtonCreateBorderClicked(object sender, EventArgs e)
		{
			if(!creatingNewBorder) {
				creatingNewBorder = true;
				newBorderVertice = new List<PointLatLng>();
			} else {
				if(MessageDialogHelper.RunQuestionDialog("Завершить задание границ района?")) {
					if(MessageDialogHelper.RunQuestionDialog("Сохранить новые границы района?")) {
						var closingPoint = newBorderVertice[0];
						newBorderVertice.Add(closingPoint);
						currentBorderVertice = newBorderVertice;
						currentDistrict.DistrictBorder = gf.CreatePolygon(GetCoordinatesFromPoints());
					}
					creatingNewBorder = false;
					ShowBorders();
					ShowBorderVertice(currentBorderVertice);
				}
			}

			ControlsAccessibility();
		}

		protected void OnButtonRemoveBorderClicked(object sender, EventArgs e)
		{
			currentDistrict.DistrictBorder = null;
			ShowBorders();
			ShowBorderVertice(GetCurrentBorderVertice());
			ControlsAccessibility();
		}

		protected void OnButtonAddVertexClicked(object sender, EventArgs e) { }

		protected void OnButtonMoveVertexClicked(object sender, EventArgs e) { }

		protected void OnButtonRemoveVertexClicked(object sender, EventArgs e) { }

		IList<PointLatLng> GetCurrentBorderVertice()
		{
			if(currentDistrict.DistrictBorder == null) {
				return null;
			}

			var coords = currentDistrict.DistrictBorder.Coordinates;
			var vertice = new List<PointLatLng>();

			foreach(Coordinate coord in coords) {
				vertice.Add(new PointLatLng() {
					Lat = coord.X,
					Lng = coord.Y
				});
			}

			return vertice;
		}

		void ShowBorders()
		{
			bordersOverlay.Clear();

			foreach(ScheduleRestrictedDistrict district in observableRestrictedDistricts) {
				if(district.DistrictBorder != null) {
					var border = new GMapPolygon(district.DistrictBorder.Coordinates.Select(p => new PointLatLng(p.X, p.Y)).ToList(), district.DistrictName) {
						Tag = district
					};
					bordersOverlay.Polygons.Add(border);
				}
			}
		}

		void ShowBorderVertice(IList<PointLatLng> vertice, bool newBorder = false)
		{
			verticeOverlay.Clear();

			if(vertice == null) {
				return;
			}

			foreach(PointLatLng vertex in vertice) {
				GMapMarker point = new GMarkerGoogle(vertex, newBorder ? GMarkerGoogleType.red : GMarkerGoogleType.blue);

				verticeOverlay.Markers.Add(point);
			}
		}

		protected void OnGmapWidgetButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if(args.Event.Button == 1) {
				if(creatingNewBorder) {
					var point = gmapWidget.FromLocalToLatLng((int)args.Event.X, (int)args.Event.Y);
					newBorderVertice.Add(point);
					ShowBorderVertice(newBorderVertice, true);
				}
			}
		}

		Coordinate[] GetCoordinatesFromPoints()
		{
			IList<Coordinate> coords = new List<Coordinate>();

			foreach(PointLatLng point in currentBorderVertice) {
				coords.Add(new Coordinate {
					X = point.Lat,
					Y = point.Lng
				});
			}

			return coords.ToArray();
		}

		protected void OnBtnTodayClicked(object sender, EventArgs e)
		{
			currentDistrict.CreateScheduleRestriction(WeekDayName.today);
			ytreeSchedules.ItemsDataSource = currentDistrict.ScheduleRestrictionToday.ObservableSchedules;
			ytreeSchedules.QueueDraw();
		}

		protected void OnBtnMondayClicked(object sender, EventArgs e)
		{
			currentDistrict.CreateScheduleRestriction(WeekDayName.monday);
			ytreeSchedules.ItemsDataSource = currentDistrict.ScheduleRestrictionMonday.ObservableSchedules;
			ytreeSchedules.QueueDraw();
		}

		protected void OnBtnTuesdayClicked(object sender, EventArgs e)
		{
			currentDistrict.CreateScheduleRestriction(WeekDayName.tuesday);
			ytreeSchedules.ItemsDataSource = currentDistrict.ScheduleRestrictionTuesday.ObservableSchedules;
			ytreeSchedules.QueueDraw();
		}

		protected void OnBtnWednesdayClicked(object sender, EventArgs e)
		{
			currentDistrict.CreateScheduleRestriction(WeekDayName.wednesday);
			ytreeSchedules.ItemsDataSource = currentDistrict.ScheduleRestrictionWednesday.ObservableSchedules;
			ytreeSchedules.QueueDraw();
		}

		protected void OnBtnThursdayClicked(object sender, EventArgs e)
		{
			currentDistrict.CreateScheduleRestriction(WeekDayName.thursday);
			ytreeSchedules.ItemsDataSource = currentDistrict.ScheduleRestrictionThursday.ObservableSchedules;
			ytreeSchedules.QueueDraw();
		}

		protected void OnBtnFridayClicked(object sender, EventArgs e)
		{
			currentDistrict.CreateScheduleRestriction(WeekDayName.friday);
			ytreeSchedules.ItemsDataSource = currentDistrict.ScheduleRestrictionFriday.ObservableSchedules;
			ytreeSchedules.QueueDraw();
		}

		protected void OnBtnSaturdayClicked(object sender, EventArgs e)
		{
			currentDistrict.CreateScheduleRestriction(WeekDayName.saturday);
			ytreeSchedules.ItemsDataSource = currentDistrict.ScheduleRestrictionSaturday.ObservableSchedules;
			ytreeSchedules.QueueDraw();
		}

		protected void OnBtnSundayClicked(object sender, EventArgs e)
		{
			currentDistrict.CreateScheduleRestriction(WeekDayName.sunday);
			ytreeSchedules.ItemsDataSource = currentDistrict.ScheduleRestrictionSunday.ObservableSchedules;
			ytreeSchedules.QueueDraw();
		}

		protected void OnBtnAddGeographicGroupClicked(object sender, EventArgs e)
		{
			var selectGeographicGroups = new OrmReference(typeof(GeographicGroup), uow) {
				Mode = OrmReferenceMode.MultiSelect
			};
			selectGeographicGroups.ObjectSelected += SelectGeographicGroups_ObjectSelected;
			TabParent.AddSlaveTab(this, selectGeographicGroups);
		}

		void SelectGeographicGroups_ObjectSelected(object sender, OrmReferenceObjectSectedEventArgs e)
		{
			if(yTreeGeographicGroups.ItemsDataSource is GenericObservableList<GeographicGroup> ggList)
				foreach(var item in e.Subjects) {
					if(item is GeographicGroup group && !ggList.Any(x => x.Id == group.Id))
						ggList.Add(group);
				}
		}

		protected void OnBtnRemoveGeographicGroupClicked(object sender, EventArgs e)
		{
			var ggList = yTreeGeographicGroups.ItemsDataSource as GenericObservableList<GeographicGroup>;
			if(yTreeGeographicGroups.GetSelectedObject() is GeographicGroup selectedObj && ggList != null)
				ggList.Remove(selectedObj);
		}
	}
}