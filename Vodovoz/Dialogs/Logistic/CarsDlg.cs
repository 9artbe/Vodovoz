﻿using System;
using NLog;
using QSOrmProject;
using QSValidation;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Logistic;

namespace Vodovoz
{
	public partial class CarsDlg : OrmGtkDialogBase<Car>
	{
		private static Logger logger = LogManager.GetCurrentClassLogger ();

		public override bool HasChanges { 
			get { return UoWGeneric.HasChanges || attachmentFiles.HasChanges; }
		}

		public CarsDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Car>();
			TabName = "Новый автомобиль";
			ConfigureDlg ();
		}

		public CarsDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Car> (id);
			ConfigureDlg ();
		}

		public CarsDlg (Car sub) : this (sub.Id) {}

		private void ConfigureDlg ()
		{
			notebook1.Page = 0;
			notebook1.ShowTabs = false;

			dataentryModel.Binding.AddBinding(Entity, e => e.Model, w => w.Text).InitializeFromSource();
			dataentryRegNumber.Binding.AddBinding(Entity, e => e.RegistrationNumber, w => w.Text).InitializeFromSource();

			dataentryreferenceDriver.SubjectType = typeof(Employee);
			dataentryreferenceDriver.Binding.AddBinding(Entity, e => e.Driver, w => w.Subject).InitializeFromSource();

			dataentryFuelType.SubjectType = typeof(FuelType);
			dataentryFuelType.Binding.AddBinding(Entity, e => e.FuelType, w => w.Subject).InitializeFromSource();
			radiobuttonMain.Active = true;

			dataspinbutton1.Binding.AddBinding(Entity, e => e.FuelConsumption, w => w.Value).InitializeFromSource();

			photoviewCar.Binding.AddBinding(Entity, e => e.Photo, w => w.ImageFile).InitializeFromSource();
			photoviewCar.GetSaveFileName = () => String.Format("{0}({1})", Entity.Model, Entity.RegistrationNumber);

			checkIsCompanyHavings.Binding.AddBinding(Entity, e => e.IsCompanyHavings, w => w.Active).InitializeFromSource();
			checkIsArchive.Binding.AddBinding(Entity, e => e.IsArchive, w => w.Active).InitializeFromSource();
			checkIsTruck.Binding.AddBinding(Entity, e => e.IsTruck, w => w.Active).InitializeFromSource();

			attachmentFiles.AttachToTable = OrmMain.GetDBTableName (typeof(Car));
			if (!UoWGeneric.IsNew) {
				attachmentFiles.ItemId = UoWGeneric.Root.Id;
				attachmentFiles.UpdateFileList ();
			}
			OnDataentryreferenceDriverChanged (null, null);
			textDriverInfo.Selectable = true;
		}

		public override bool Save ()
		{
			var valid = new QSValidator<Car> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем автомобиль...");
			try {
				UoWGeneric.Save();
				if (UoWGeneric.IsNew) {
					attachmentFiles.ItemId = UoWGeneric.Root.Id;
				}
				attachmentFiles.SaveChanges ();
			} catch (Exception ex) {
				logger.Error (ex, "Не удалось записать Автомобиль.");
				QSProjectsLib.QSMain.ErrorMessage ((Gtk.Window)this.Toplevel, ex);
				return false;
			}
			logger.Info ("Ok");
			return true;

		}

		protected void OnRadiobuttonFilesToggled (object sender, EventArgs e)
		{
			if (radiobuttonFiles.Active)
				notebook1.CurrentPage = 1;
		}

		protected void OnRadiobuttonMainToggled (object sender, EventArgs e)
		{
			if (radiobuttonMain.Active)
				notebook1.CurrentPage = 0;
		}

		protected void OnDataentryreferenceDriverChanged (object sender, EventArgs e)
		{
			if (UoWGeneric.Root.Driver != null)
				textDriverInfo.Text = "\tПаспорт: " + UoWGeneric.Root.Driver.PassportSeria + " № " + UoWGeneric.Root.Driver.PassportNumber +
					"\n\tАдрес регистрации: " + UoWGeneric.Root.Driver.AddressRegistration;
		}

		protected void OnCheckIsCompanyHavingsToggled (object sender, EventArgs e)
		{
			Entity.IsCompanyHavings = checkIsCompanyHavings.Active;
			dataentryreferenceDriver.Sensitive = !Entity.IsCompanyHavings;

			if (Entity.IsCompanyHavings)
				Entity.Driver = null;
		}
	}
}

