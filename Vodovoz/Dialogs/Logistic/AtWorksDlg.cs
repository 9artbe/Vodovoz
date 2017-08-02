﻿using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.ColumnConfig;
using QSOrmProject;
using QSTDI;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Logistic;
using Vodovoz.Repository.Logistics;

namespace Vodovoz.Dialogs.Logistic
{
	public partial class AtWorksDlg : TdiTabBase, ITdiDialog
	{
		static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		private IUnitOfWork uow = UnitOfWorkFactory.CreateWithoutRoot();

		IList<AtWorkDriver> driversAtDay;

		GenericObservableList<AtWorkDriver> observableDriversAtDay;

		private DateTime DialogAtDate
		{
			get { return ydateAtWorks.Date; }
		}

		private IList<AtWorkDriver> DriversAtDay
		{
			set
			{
				driversAtDay = value;
				observableDriversAtDay = new GenericObservableList<AtWorkDriver>(driversAtDay);
				ytreeviewAtWorkDrivers.SetItemsSource(observableDriversAtDay);
			}
			get
			{
				return driversAtDay;
			}
		}

		public override string TabName
		{
			get
			{
				return String.Format("Работают {0:d}", ydateAtWorks.Date);
			}
			protected set
			{
				throw new InvalidOperationException("Установка протеворечит логике работы.");
			}
		}

		Gdk.Pixbuf vodovozCarIcon = Gdk.Pixbuf.LoadFromResource("Vodovoz.icons.buttons.vodovoz-logo.png");

		public AtWorksDlg()
		{
			this.Build();

			ytreeviewAtWorkDrivers.ColumnsConfig = FluentColumnsConfig<AtWorkDriver>.Create()
				.AddColumn("Приоритет").AddNumericRenderer(x => x.PriorityAtDay).Editing(new Gtk.Adjustment(6, 1, 10,1,1,1))
				.AddColumn("Водитель").AddTextRenderer(x => x.Employee.ShortName)
				.AddColumn("Поездок").AddNumericRenderer(x => x.Trips).Editing(new Gtk.Adjustment(1, 0, 10, 1, 1, 1))
				.AddColumn("Автомобиль")
					.AddPixbufRenderer(x => x.Car != null && x.Car.IsCompanyHavings ? vodovozCarIcon : null)
					.AddTextRenderer(x => x.Car != null ? x.Car.RegistrationNumber : "нет")
				.AddColumn("")
				.Finish();
			ytreeviewAtWorkDrivers.Selection.Mode = Gtk.SelectionMode.Multiple;

			ytreeviewAtWorkDrivers.Selection.Changed += YtreeviewDrivers_Selection_Changed;

			ydateAtWorks.Date = DateTime.Today;
		}

		void FillDialogAtDay()
		{
			MainClass.MainWin.ProgressStart(1);
			uow.Session.Clear();

			logger.Info("Загружаем водителей на {0:d}...", DialogAtDate);
			DriversAtDay = Repository.Logistics.AtWorkRepository.GetDriversAtDay(uow, DialogAtDate);

			MainClass.MainWin.ProgressClose();
		}

		protected void OnYdateAtWorksDateChanged(object sender, EventArgs e)
		{
			FillDialogAtDay();
			OnTabNameChanged();
		}

		protected void OnButtonSaveChangesClicked(object sender, EventArgs e)
		{
			Save();
		}

		protected void OnButtonCancelChangesClicked(object sender, EventArgs e)
		{
			uow.Session.Clear();
			FillDialogAtDay();
		}

		public event EventHandler<EntitySavedEventArgs> EntitySaved;

		public bool Save()
		{
			DriversAtDay.ToList().ForEach(x => uow.Save(x));
			uow.Commit();
			FillDialogAtDay();
			return true;
		}

		public void SaveAndClose()
		{
			throw new NotImplementedException();
		}

		public bool HasChanges
		{
			get
			{
				return uow.HasChanges;
			}
		}

		protected void OnButtonAddDriverClicked(object sender, EventArgs e)
		{
			var SelectDrivers = new OrmReference(
				uow,
				Repository.EmployeeRepository.ActiveDriversOrderedQuery()
			);
			SelectDrivers.Mode = OrmReferenceMode.MultiSelect;
			SelectDrivers.ObjectSelected += SelectDrivers_ObjectSelected;
			TabParent.AddSlaveTab(this, SelectDrivers);
		}

		void SelectDrivers_ObjectSelected(object sender, OrmReferenceObjectSectedEventArgs e)
		{
			var addDrivers = e.GetEntities<Employee>().ToList();
			logger.Info("Получаем авто для водителей...");
			MainClass.MainWin.ProgressStart(2);
			var onlyNew = addDrivers.Where(x => driversAtDay.All(y => y.Employee.Id != x.Id)).ToList();
			var allCars = CarRepository.GetCarsbyDrivers(uow, onlyNew.Select(x => x.Id).ToArray());
			MainClass.MainWin.ProgressAdd();

			foreach (var driver in addDrivers)
			{
				driversAtDay.Add(new AtWorkDriver
				{
					Date = DialogAtDate,
					Employee = driver,
					Car = allCars.FirstOrDefault(x => x.Driver.Id == driver.Id),
					Trips = 1
				});
			}
			MainClass.MainWin.ProgressAdd();
			DriversAtDay = driversAtDay.OrderBy(x => x.Employee.ShortName).ToList();
			logger.Info("Ок");
			MainClass.MainWin.ProgressClose();
		}

		protected void OnButtonRemoveDriverClicked(object sender, EventArgs e)
		{
			var toDel = ytreeviewAtWorkDrivers.GetSelectedObjects<AtWorkDriver>();
			foreach (var driver in toDel)
			{
				if (driver.Id > 0)
					uow.Delete(driver);
				observableDriversAtDay.Remove(driver);
			}
		}

		protected void OnButtonDriverSelectAutoClicked(object sender, EventArgs e)
		{
			var SelectDriverCar = new OrmReference(
				uow,
				Repository.Logistics.CarRepository.ActiveCompanyCarsQuery()
			);
			var driver = ytreeviewAtWorkDrivers.GetSelectedObjects<AtWorkDriver>().First();
			SelectDriverCar.Tag = driver;
			SelectDriverCar.Mode = OrmReferenceMode.Select;
			SelectDriverCar.ObjectSelected += SelectDriverCar_ObjectSelected;
			TabParent.AddSlaveTab(this, SelectDriverCar);
		}

		void SelectDriverCar_ObjectSelected(object sender, OrmReferenceObjectSectedEventArgs e)
		{
			var driver = e.Tag as AtWorkDriver;
			var car = e.Subject as Car;
			driversAtDay.Where(x => x.Car != null && x.Car.Id == car.Id).ToList().ForEach(x => x.Car = null);
			driver.Car = car;
		}

		void YtreeviewDrivers_Selection_Changed(object sender, EventArgs e)
		{
			buttonRemoveDriver.Sensitive = buttonDriverSelectAuto.Sensitive = ytreeviewAtWorkDrivers.Selection.CountSelectedRows() > 0;
		}
	}
}
