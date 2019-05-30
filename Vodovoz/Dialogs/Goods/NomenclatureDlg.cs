﻿using System;
using System.Linq;
using Gamma.GtkWidgets;
using Gtk;
using NLog;
using QS.DomainModel.UoW;
using QS.Helpers;
using QS.Project.Dialogs;
using QSBusinessCommon.Domain;
using QSOrmProject;
using QS.Project.Repositories;
using QSValidation;
using QSWidgetLib;
using Vodovoz.Additions.Store;
using Vodovoz.Domain;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Store;
using Vodovoz.Repositories.HumanResources;
using Vodovoz.Repository;
using Vodovoz.ServiceDialogs.Database;
using Vodovoz.ViewModel;
using Vodovoz.Domain.Logistic;

namespace Vodovoz
{
	public partial class NomenclatureDlg : QS.Dialog.Gtk.EntityDialogBase<Nomenclature>
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		Warehouse selectedWarehouse;

		public NomenclatureDlg()
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Nomenclature>();
			TabName = "Новая номенклатура";
			ConfigureDlg();
		}

		public NomenclatureDlg(int id)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Nomenclature>(id);
			ConfigureDlg();
		}

		public NomenclatureDlg(Nomenclature sub) : this(sub.Id) { }

		private void ConfigureDlg()
		{
			notebook1.ShowTabs = false;
			spinWeight.Sensitive = false;
			spinVolume.Sensitive = false;
			lblPercentForMaster.Visible = spinPercentForMaster.Visible = false;
			entryName.IsEditable = true;
			radioInfo.Active = true;

			ylabelCreationDate.Binding.AddFuncBinding(Entity, s => s.CreateDate.HasValue ? s.CreateDate.Value.ToString("dd.MM.yyyy HH:mm") : "", w => w.LabelProp).InitializeFromSource();
			ylabelCreatedBy.Binding.AddFuncBinding<Nomenclature>(Entity, s => GetUserEmployeeName(s.CreatedBy), w => w.LabelProp).InitializeFromSource();

			enumVAT.ItemsEnum = typeof(VAT);
			enumVAT.Binding.AddBinding(Entity, e => e.VAT, w => w.SelectedItem).InitializeFromSource();

			enumType.ItemsEnum = typeof(NomenclatureCategory);
			enumType.Binding.AddBinding(Entity, e => e.Category, w => w.SelectedItem).InitializeFromSource();

			enumTareVolume.ItemsEnum = typeof(TareVolume);
			enumTareVolume.Binding.AddBinding(Entity, e => e.TareVolume, w => w.SelectedItemOrNull).InitializeFromSource();
			ycheckDisposableTare.Binding.AddBinding(Entity, e => e.IsDisposableTare, w => w.Active).InitializeFromSource();

			enumEquipmentSubtype.Visible = Entity.Category == NomenclatureCategory.equipment;
			enumEquipmentSubtype.ItemsEnum = typeof(SubtypeOfEquipmentCategory);
			enumEquipmentSubtype.Binding.AddBinding(Entity, e => e.SubTypeOfEquipmentCategory, w => w.SelectedItem).InitializeFromSource();

			enumDepositType.Visible = Entity.Category == NomenclatureCategory.deposit;
			enumDepositType.ItemsEnum = typeof(TypeOfDepositCategory);
			enumDepositType.Binding.AddBinding(Entity, e => e.TypeOfDepositCategory, w => w.SelectedItemOrNull).InitializeFromSource();

			comboMobileCatalog.ItemsEnum = typeof(MobileCatalog);
			comboMobileCatalog.Binding.AddBinding(Entity, e => e.MobileCatalog, w => w.SelectedItem).InitializeFromSource();

			labelSubType.Visible = (Entity.Category == NomenclatureCategory.deposit || Entity.Category == NomenclatureCategory.equipment);

			entryName.Binding.AddBinding(Entity, e => e.Name, w => w.Text).InitializeFromSource();
			yentryOfficialName.Binding.AddBinding(Entity, e => e.OfficialName, w => w.Text).InitializeFromSource();
			var parallel = new ParallelEditing(yentryOfficialName);
			parallel.SubscribeOnChanges(entryName);
			parallel.GetParallelTextFunc = GenerateOfficialName;

			ycheckRentPriority.Binding.AddBinding(Entity, e => e.RentPriority, w => w.Active).InitializeFromSource();
			checkNotReserve.Binding.AddBinding(Entity, e => e.DoNotReserve, w => w.Active).InitializeFromSource();
			checkcanPrintPrice.Binding.AddBinding(Entity, e => e.CanPrintPrice, w => w.Active).InitializeFromSource();
			labelCanPrintPrice.Visible = checkcanPrintPrice.Visible = Entity.Category == NomenclatureCategory.water && !Entity.IsDisposableTare;
			checkHide.Binding.AddBinding(Entity, e => e.Hide, w => w.Active).InitializeFromSource();
			entryCode1c.Binding.AddBinding(Entity, e => e.Code1c, w => w.Text).InitializeFromSource();
			yspinSumOfDamage.Binding.AddBinding(Entity, e => e.SumOfDamage, w => w.ValueAsDecimal).InitializeFromSource();
			spinWeight.Binding.AddBinding(Entity, e => e.Weight, w => w.Value).InitializeFromSource();
			spinVolume.Binding.AddBinding(Entity, e => e.Volume, w => w.Value).InitializeFromSource();
			spinPercentForMaster.Binding.AddBinding(Entity, e => e.PercentForMaster, w => w.Value).InitializeFromSource();
			checkSerial.Binding.AddBinding(Entity, e => e.IsSerial, w => w.Active).InitializeFromSource();
			ycheckNewBottle.Binding.AddBinding(Entity, e => e.IsNewBottle, w => w.Active).InitializeFromSource();
			ycheckDefectiveBottle.Binding.AddBinding(Entity, e => e.IsDefectiveBottle, w => w.Active).InitializeFromSource();
			chkIsDiler.Binding.AddBinding(Entity, e => e.IsDiler, w => w.Active).InitializeFromSource();
			spinMinStockCount.Binding.AddBinding(Entity, e => e.MinStockCount, w => w.ValueAsDecimal).InitializeFromSource();

			ycomboFuelTypes.SetRenderTextFunc<FuelType>(x => x.Name);
			ycomboFuelTypes.ItemsList = UoW.GetAll<FuelType>();
			ycomboFuelTypes.Binding.AddBinding(Entity, e => e.FuelType, w => w.SelectedItem).InitializeFromSource();
			ycomboFuelTypes.Visible = Entity.Category == NomenclatureCategory.fuel;

			yentryFolder1c.SubjectType = typeof(Folder1c);
			yentryFolder1c.Binding.AddBinding(Entity, e => e.Folder1C, w => w.Subject).InitializeFromSource();
			yentryProductGroup.SubjectType = typeof(ProductGroup);
			yentryProductGroup.Binding.AddBinding(Entity, e => e.ProductGroup, w => w.Subject).InitializeFromSource();
			referenceUnit.SubjectType = typeof(MeasurementUnits);
			referenceUnit.Binding.AddBinding(Entity, n => n.Unit, w => w.Subject).InitializeFromSource();
			yentryrefEqupmentType.SubjectType = typeof(EquipmentType);
			yentryrefEqupmentType.Binding.AddBinding(Entity, e => e.Type, w => w.Subject).InitializeFromSource();
			referenceColor.SubjectType = typeof(EquipmentColors);
			referenceColor.Binding.AddBinding(Entity, e => e.EquipmentColor, w => w.Subject).InitializeFromSource();
			referenceRouteColumn.SubjectType = typeof(Domain.Logistic.RouteColumn);
			referenceRouteColumn.Binding.AddBinding(Entity, n => n.RouteListColumn, w => w.Subject).InitializeFromSource();
			referenceManufacturer.SubjectType = typeof(Manufacturer);
			referenceManufacturer.Binding.AddBinding(Entity, e => e.Manufacturer, w => w.Subject).InitializeFromSource();
			checkNoDeliver.Binding.AddBinding(Entity, e => e.NoDelivey, w => w.Active).InitializeFromSource();

			yentryShortName.Binding.AddBinding(Entity, e => e.ShortName, w => w.Text, new NullToEmptyStringConverter()).InitializeFromSource();
			yentryShortName.MaxLength = 220;
			checkIsArchive.Binding.AddBinding(Entity, e => e.IsArchive, w => w.Active).InitializeFromSource();
			checkIsArchive.Sensitive = UserPermissionRepository.CurrentUserPresetPermissions["can_create_and_arc_nomenclatures"];

			#region Вкладка "Склады отгрузки"

			//repTreeViewWarehouses.RepresentationModel = new WarehousesVM(UoW);
			repTreeViewWarehouses.ColumnsConfig = ColumnsConfigFactory.Create<Warehouse>()
				.AddColumn("Название").AddTextRenderer(node => node.Name)
				.AddColumn("Код").AddTextRenderer(node => node.Id.ToString())
				.Finish();
			repTreeViewWarehouses.SetItemsSource(Entity.ObservableWarehouses);
			repTreeViewWarehouses.Selection.Changed += (sender, e) => {
				selectedWarehouse = repTreeViewWarehouses.GetSelectedObject<Warehouse>();
				btnRemoveWarehouse.Sensitive = selectedWarehouse != null;
			};
			btnRemoveWarehouse.Sensitive = selectedWarehouse != null;

			#endregion

			#region Вкладка характиристики

			ytextDescription.Binding.AddBinding(Entity, e => e.Description, w => w.Buffer.Text).InitializeFromSource();
			nomenclaturecharacteristicsview1.Uow = UoWGeneric;

			#endregion

			int currNomenclatureOfDependence = (Entity.DependsOnNomenclature == null ? 0 : Entity.DependsOnNomenclature.Id);

			dependsOnNomenclature.RepresentationModel = new NomenclatureDependsFromVM(Entity);
			dependsOnNomenclature.Binding.AddBinding(Entity, e => e.DependsOnNomenclature, w => w.Subject).InitializeFromSource();

			ConfigureInputs(Entity.Category);

			pricesView.UoWGeneric = UoWGeneric;
			pricesView.Sensitive = UserPermissionRepository.CurrentUserPresetPermissions["can_create_and_arc_nomenclatures"];

			Imageslist.ImageButtonPressEvent += Imageslist_ImageButtonPressEvent;

			Entity.PropertyChanged += Entity_PropertyChanged;

			//make actions menu
			var menu = new Gtk.Menu();
			var menuItem = new Gtk.MenuItem("Заменить все ссылки на номенклатуру...");
			menuItem.Activated += MenuItem_ReplaceLinks_Activated; ;
			menu.Add(menuItem);
			menuActions.Menu = menu;
			menu.ShowAll();
			menuActions.Sensitive = !UoWGeneric.IsNew;
		}

		void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(e.PropertyName == nameof(Entity.ProductGroup))
				nomenclaturecharacteristicsview1.RefreshWidgets();
		}


		void MenuItem_ReplaceLinks_Activated(object sender, EventArgs e)
		{
			var replaceDlg = new ReplaceEntityLinksDlg(Entity);
			OpenTab(replaceDlg);
		}

		private string GetUserEmployeeName(User s)
		{
			if(Entity.CreatedBy == null) {
				return "";
			}
			var employee = EmployeeRepository.GetEmployeesForUser(UoW, s.Id).FirstOrDefault();
			if(employee == null) {
				return Entity.CreatedBy.Name;
			} else {
				return employee.ShortName;
			}
		}

		string GenerateOfficialName(object arg)
		{
			var widget = arg as Gtk.Entry;
			return widget.Text;
		}

		public override bool Save()
		{
			if(String.IsNullOrWhiteSpace(Entity.Code1c)) {
				Entity.Code1c = NomenclatureRepository.GetNextCode1c(UoW);
			}

			var valid = new QSValidator<Nomenclature>(UoWGeneric.Root);
			if(valid.RunDlgIfNotValid((Gtk.Window)this.Toplevel))
				return false;
			logger.Info("Сохраняем номенклатуру...");
			Entity.SetNomenclatureCreationInfo();
			pricesView.SaveChanges();
			UoWGeneric.Save();
			return true;
		}

		protected void OnEnumTypeChanged(object sender, EventArgs e)
		{
			ConfigureInputs(Entity.Category);

			if(Entity.Category != NomenclatureCategory.deposit) {
				Entity.TypeOfDepositCategory = null;
			}
		}

		protected void ConfigureInputs(NomenclatureCategory selected)
		{
			enumDepositType.Visible = selected == NomenclatureCategory.deposit;
			enumEquipmentSubtype.Visible = radioEuqpment.Sensitive = selected == NomenclatureCategory.equipment;
			labelSubType.Visible = (selected == NomenclatureCategory.deposit || selected == NomenclatureCategory.equipment);
			spinWeight.Sensitive = !(selected == NomenclatureCategory.service || selected == NomenclatureCategory.rent || selected == NomenclatureCategory.deposit);
			spinVolume.Sensitive = !(selected == NomenclatureCategory.service || selected == NomenclatureCategory.rent || selected == NomenclatureCategory.deposit);
			lblPercentForMaster.Visible = spinPercentForMaster.Visible = (selected == NomenclatureCategory.master);
			labelManufacturer.Sensitive = referenceManufacturer.Sensitive = (selected == NomenclatureCategory.equipment);
			labelColor.Sensitive = referenceColor.Sensitive = (selected == NomenclatureCategory.equipment);
			labelClass.Sensitive = yentryrefEqupmentType.Sensitive = (selected == NomenclatureCategory.equipment);
			labelModel.Sensitive = entryModel.Sensitive = (selected == NomenclatureCategory.equipment);
			labelSerial.Sensitive = checkSerial.Sensitive = (selected == NomenclatureCategory.equipment);
			labelRentPriority.Sensitive = ycheckRentPriority.Sensitive = (selected == NomenclatureCategory.equipment);
			labelReserve.Sensitive = checkNotReserve.Sensitive = !(selected == NomenclatureCategory.service || selected == NomenclatureCategory.rent || selected == NomenclatureCategory.deposit);
			labelCanPrintPrice.Visible = checkcanPrintPrice.Visible = Entity.Category == NomenclatureCategory.water && !Entity.IsDisposableTare;

			labelTypeTare.Visible = hboxTare.Visible = selected == NomenclatureCategory.water;
			labelBottle.Sensitive = ycheckNewBottle.Sensitive = ycheckDefectiveBottle.Sensitive = selected == NomenclatureCategory.bottle;
			lblFuelType.Visible = ycomboFuelTypes.Visible = selected == NomenclatureCategory.fuel;
			//FIXME запуск оборудования - временный фикс
			//if (Entity.Category == NomenclatureCategory.equipment)
			//Entity.Serial = true;
		}

		#region Переключение вкладок

		protected void OnRadioInfoToggled(object sender, EventArgs e)
		{
			if(radioInfo.Active)
				notebook1.CurrentPage = 0;
		}

		protected void OnRadioWarehousesToggled(object sender, EventArgs e)
		{
			if(radioWarehouses.Active)
				notebook1.CurrentPage = 1;
		}

		protected void OnRadioEuqpmentToggled(object sender, EventArgs e)
		{
			if(radioEuqpment.Active)
				notebook1.CurrentPage = 2;
		}

		protected void OnRadioCharacteristicsToggled(object sender, EventArgs e)
		{
			if(radioCharacteristics.Active)
				notebook1.CurrentPage = 3;
		}

		protected void OnRadioImagesToggled(object sender, EventArgs e)
		{
			if(radioImages.Active) {
				notebook1.CurrentPage = 4;
				ImageTabOpen();
			}
		}

		protected void OnRadioPriceToggled(object sender, EventArgs e)
		{
			if(radioPrice.Active)
				notebook1.CurrentPage = 5;
		}

		#endregion

		#region Вкладка изображений

		bool imageLoaded = false;
		NomenclatureImage popupMenuOn;

		private void ImageTabOpen()
		{
			if(!imageLoaded) {
				ReloadImages();
				imageLoaded = true;
			}
		}

		private void ReloadImages()
		{
			Imageslist.Images.Clear();
			foreach(var imageSource in Entity.Images) {
				Imageslist.AddImage(new Gdk.Pixbuf(imageSource.Image), imageSource);
			}
			Imageslist.UpdateList();
		}

		protected void OnButtonAddImageClicked(object sender, EventArgs e)
		{
			FileChooserDialog Chooser = new FileChooserDialog("Выберите изображение...",
				(Window)this.Toplevel,
				FileChooserAction.Open,
				"Отмена", ResponseType.Cancel,
				"Загрузить", ResponseType.Accept);

			FileFilter Filter = new FileFilter();
			Filter.AddPixbufFormats();
			Filter.Name = "Все изображения";
			Chooser.AddFilter(Filter);

			if((ResponseType)Chooser.Run() == ResponseType.Accept) {
				Chooser.Hide();
				logger.Info("Загрузка изображения...");

				var imageFile = ImageHelper.LoadImageToJpgBytes(Chooser.Filename);
				Entity.Images.Add(new NomenclatureImage(Entity, imageFile));
				ReloadImages();

				logger.Info("Ok");
			}
			Chooser.Destroy();
		}

		void Imageslist_ImageButtonPressEvent(object sender, ImageButtonPressEventArgs e)
		{
			if((int)e.eventArgs.Event.Button == 3) {
				popupMenuOn = (NomenclatureImage)e.Tag;
				Gtk.Menu jBox = new Gtk.Menu();
				Gtk.MenuItem MenuItem1 = new MenuItem("Удалить");
				MenuItem1.Activated += DeleteImage_Activated; ;
				jBox.Add(MenuItem1);
				jBox.ShowAll();
				jBox.Popup();
			}
		}

		void DeleteImage_Activated(object sender, EventArgs e)
		{
			Entity.Images.Remove(popupMenuOn);
			popupMenuOn = null;
			ReloadImages();
		}

		#endregion

		protected void OnDependsOnNomenclatureChanged(object sender, EventArgs e)
		{
			radioPrice.Sensitive = Entity.DependsOnNomenclature == null;
		}

		protected void OnBtnAddWarehouseClicked(object sender, EventArgs e)
		{
			var refWin = new OrmReference(StoreDocumentHelper.GetWarehouseQuery()) {
				ButtonMode = ReferenceButtonMode.None,
				Mode = OrmReferenceMode.MultiSelect
			};
			refWin.ObjectSelected += RefWin_ObjectSelected;
			TabParent.AddSlaveTab(this, refWin);
		}

		void RefWin_ObjectSelected(object sender, OrmReferenceObjectSectedEventArgs e)
		{
			var warehouses = e.Subjects.OfType<Warehouse>();
			foreach(var w in warehouses) {
				if(w != null && !Entity.ObservableWarehouses.Any(x => x.Id == w.Id))
					Entity.ObservableWarehouses.Add(w);
			}
		}

		protected void OnBtnRemoveWarehouseClicked(object sender, EventArgs e)
		{
			if(selectedWarehouse != null)
				Entity.ObservableWarehouses.Remove(selectedWarehouse);
		}
	}
}