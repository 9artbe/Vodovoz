﻿using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.ColumnConfig;
using Gamma.Utilities;
using Gamma.Widgets;
using QS.Banks.Domain;
using QS.Dialog;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.Project.DB;
using QS.Project.Services;
using QS.Views.GtkUI;
using QS.Widgets.GtkUI;
using QSOrmProject;
using Vodovoz.Dialogs.Employees;
using Vodovoz.Domain.Contacts;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Logistic;
using Vodovoz.ViewModels.ViewModels.Employees;

namespace Vodovoz.Views.Employees
{
	public partial class EmployeeView : TabViewBase<EmployeeViewModel>, IEntityDialog
	{
		public EmployeeView(EmployeeViewModel viewModel) : base(viewModel)
		{
			Build();
			ConfigureDlg();
		}
		
		public IUnitOfWork UoW => ViewModel.UoW;
		public object EntityObject => ViewModel.UoWGeneric.RootObject;

		private void ConfigureDlg()
		{
			notebookMain.Page = 0;
			notebookMain.ShowTabs = false;

			buttonSave.Clicked += (sender, args) => ViewModel.SaveAndClose();
			buttonCancel.Clicked += (sender, args) => ViewModel.Close(false, CloseSource.Cancel);
			
			ConfigureRadioButtons();
			
			//Вкладка Информация
			yenumcomboStatus.ItemsEnum = typeof(EmployeeStatus);
			yenumcomboStatus.Binding.AddBinding(ViewModel.Entity, e => e.Status, w => w.SelectedItem).InitializeFromSource();
			
			dataentryLastName.Binding.AddBinding(ViewModel.Entity, e => e.LastName, w => w.Text).InitializeFromSource();
			dataentryName.Binding.AddBinding(ViewModel.Entity, e => e.Name, w => w.Text).InitializeFromSource();
			dataentryPatronymic.Binding.AddBinding(ViewModel.Entity, e => e.Patronymic, w => w.Text).InitializeFromSource();
			
			photoviewEmployee.Binding.AddBinding(ViewModel.Entity, e => e.Photo, w => w.ImageFile).InitializeFromSource();
			photoviewEmployee.GetSaveFileName = () => ViewModel.Entity.FullName;
			
			entryEmployeePost.SetEntityAutocompleteSelectorFactory(
				ViewModel.EmployeePostsJournalFactory.CreateEmployeePostsAutocompleteSelectorFactory());
			entryEmployeePost.Binding.AddBinding(ViewModel.Entity, e => e.Post, w => w.Subject).InitializeFromSource();
			
			comboSkillLevel.ItemsList = ViewModel.Entity.GetSkillLevels();
			comboSkillLevel.Binding
				.AddBinding(
					ViewModel.Entity,
					e => e.SkillLevel,
					w => w.ActiveText,
					new Gamma.Binding.Converters.NumbersToStringConverter()
				).InitializeFromSource();
			comboSkillLevel.SelectedItem = ViewModel.Entity.SkillLevel;
			
			ConfigureCategory();
			
			cmbDriverOf.ItemsEnum = typeof(CarTypeOfUse);
			cmbDriverOf.Binding
				.AddBinding(ViewModel.Entity, e => e.DriverOf, w => w.SelectedItemOrNull)
				.InitializeFromSource();
			
			checkVisitingMaster.Binding
				.AddBinding(ViewModel.Entity, e => e.VisitingMaster, w => w.Active)
				.InitializeFromSource();
			chkDriverForOneDay.Binding
				.AddBinding(ViewModel.Entity, e => e.IsDriverForOneDay, w => w.Active)
				.InitializeFromSource();
			checkChainStoreDriver.Binding
				.AddBinding(ViewModel.Entity, e => e.IsChainStoreDriver, w => w.Active)
				.InitializeFromSource();
			
			referenceNationality.SubjectType = typeof(Nationality);
			referenceNationality.Binding
				.AddBinding(ViewModel.Entity, e => e.Nationality, w => w.Subject)
				.InitializeFromSource();
			
			GenderComboBox.ItemsEnum = typeof(Gender);
			GenderComboBox.Binding
				.AddBinding(ViewModel.Entity, e => e.Gender, w => w.SelectedItemOrNull)
				.InitializeFromSource();
			
			ConfigureSubdivision();
			
			referenceUser.SubjectType = typeof(User);
			referenceUser.CanEditReference = false;
			referenceUser.Binding.AddBinding(ViewModel.Entity, e => e.User, w => w.Subject).InitializeFromSource();
			referenceUser.Sensitive = ViewModel.CanManageUsers;
			
			ylblUserLogin.TooltipText =
				"При сохранении сотрудника создаёт нового пользователя с введённым логином " +
				"и отправляет сотруднику SMS с сгенерированным паролем";
			yentryUserLogin.Binding.AddBinding(ViewModel.Entity, e => e.LoginForNewUser, w => w.Text);
			yentryUserLogin.Sensitive = ViewModel.CanCreateNewUser;
			
			birthdatePicker.Binding
				.AddBinding(ViewModel.Entity, e => e.BirthdayDate, w => w.DateOrNull)
				.InitializeFromSource();
			
			dataentryInnerPhone.Binding
				.AddBinding(
					ViewModel.Entity,
					e => e.InnerPhone,
					w => w.Text,
					new Gamma.Binding.Converters.NumbersToStringConverter()
				).InitializeFromSource();
			
			checkbuttonRussianCitizen.Binding
				.AddBinding(ViewModel.Entity, e => e.IsRussianCitizen, w => w.Active)
				.InitializeFromSource();
			OnRussianCitizenToggled(null, EventArgs.Empty);
			
			referenceCitizenship.SubjectType = typeof(Citizenship);
			referenceCitizenship.Binding
				.AddBinding(ViewModel.Entity, e => e.Citizenship, w => w.Subject)
				.InitializeFromSource();
			
			dataentryDrivingNumber.MaxLength = 20;
			dataentryDrivingNumber.Binding
				.AddBinding(ViewModel.Entity, e => e.DrivingLicense, w => w.Text)
				.InitializeFromSource();
			
			registrationTypeCmb.ItemsEnum = typeof(RegistrationType);
			registrationTypeCmb.Binding
				.AddBinding(ViewModel.Entity, e => e.Registration, w => w.SelectedItemOrNull)
				.InitializeFromSource();
			
			phonesView.ViewModel = ViewModel.PhonesViewModel;
			phonesView.ViewModel.PhonesList = new GenericObservableList<Phone>(ViewModel.Entity.Phones);

			entryAddressCurrent.Binding
				.AddBinding(ViewModel.Entity, e => e.AddressCurrent, w => w.Text)
				.InitializeFromSource();
			entryAddressRegistration.Binding
				.AddBinding(ViewModel.Entity, e => e.AddressRegistration, w => w.Text)
				.InitializeFromSource();
			yentryEmailAddress.Binding.AddBinding(ViewModel.Entity, e => e.Email, w => w.Text).InitializeFromSource();

			ydateFirstWorkDay.Binding
				.AddBinding(ViewModel.Entity, e => e.FirstWorkDay, w => w.DateOrNull)
				.InitializeFromSource();
			dateFired.Binding.AddBinding(ViewModel.Entity, e => e.DateFired, w => w.DateOrNull).InitializeFromSource();
			dateHired.Binding.AddBinding(ViewModel.Entity, e => e.DateHired, w => w.DateOrNull).InitializeFromSource();
			dateCalculated.Binding
				.AddBinding(ViewModel.Entity, e => e.DateCalculated, w => w.DateOrNull)
				.InitializeFromSource();

			//Вкладка Логистика
			dataentryAndroidLogin.Binding
				.AddBinding(ViewModel.Entity, e => e.AndroidLogin, w => w.Text)
				.InitializeFromSource();
			dataentryAndroidLogin.Binding
				.AddBinding(ViewModel, vm => vm.CanRegisterMobileUser, w => w.Sensitive)
				.InitializeFromSource();

			dataentryAndroidPassword.Binding
				.AddBinding(ViewModel.Entity, e => e.AndroidPassword, w => w.Text)
				.InitializeFromSource();
			dataentryAndroidPassword.Binding
				.AddBinding(ViewModel, vm => vm.CanRegisterMobileUser, w => w.Sensitive)
				.InitializeFromSource();

			yMobileLoginInfo.Binding
				.AddBinding(ViewModel, vm => vm.AddMobileLoginInfo, w => w.LabelProp)
				.InitializeFromSource();

			yAddMobileLogin.Binding
				.AddBinding(ViewModel, vm => vm.IsValidNewMobileUser, w => w.Sensitive)
				.InitializeFromSource();

			defaultForwarderEntry.SetEntityAutocompleteSelectorFactory(
				ViewModel.EmployeeJournalFactory.CreateWorkingForwarderEmployeeAutocompleteSelectorFactory());
			defaultForwarderEntry.Binding
				.AddBinding(ViewModel.Entity, e => e.DefaultForwarder, w => w.Subject)
				.InitializeFromSource();
			
			yspinTripsPriority.Binding
				.AddBinding(ViewModel.Entity, e => e.TripPriority, w => w.ValueAsShort)
				.InitializeFromSource();
			yspinDriverSpeed.Binding
				.AddBinding(ViewModel.Entity, e => e.DriverSpeed, w => w.Value, new MultiplierToPercentConverter())
				.InitializeFromSource();
			
			minAddressesSpin.Binding
				.AddBinding(ViewModel.Entity, e => e.MinRouteAddresses, w => w.ValueAsInt)
				.InitializeFromSource();
			maxAddressesSpin.Binding
				.AddBinding(ViewModel.Entity, e => e.MaxRouteAddresses, w => w.ValueAsInt)
				.InitializeFromSource();
			
			comboDriverType.ItemsEnum = typeof(DriverType);
			comboDriverType.Binding
				.AddBinding(ViewModel.Entity, e => e.DriverType, w => w.SelectedItemOrNull)
				.InitializeFromSource();
			
			ConfigureWorkSchedules();
			ConfigureDistrictPriorities();
			
			//Вкладка Реквизиты
			entryInn.Binding.AddBinding(ViewModel.Entity, e => e.INN, w => w.Text).InitializeFromSource();

			accountsView.ParentReference = new ParentReferenceGeneric<Employee, Account>(ViewModel.UoWGeneric, o => o.Accounts);
			accountsView.SetTitle("Банковские счета сотрудника");
			
			//Вкладка Файлы
			attachmentFiles.AttachToTable = OrmConfig.GetDBTableName(typeof(Employee));
			
			if(ViewModel.Entity.Id != 0)
			{
				attachmentFiles.ItemId =  ViewModel.Entity.Id;
				attachmentFiles.UpdateFileList();
			}

			ViewModel.SaveAttachmentFilesChangesAction += SaveAttachmentFilesChanges;
			ViewModel.HasAttachmentFilesChangesFunc += HasAttachmentsFilesChanges;

			//Вкладка Документы
			if(radioTabEmployeeDocument.Sensitive = ViewModel.CanReadEmployeeDocuments)
			{
				ConfigureDocumentsTabButtons();
				ConfigureTreeEmployeeDocuments();
			}
			
			//Вкладка Договора
			ConfigureContractsTabButtons();
			ConfigureTreeEmployeeContracts();

			//Вкладка Зарплата
			specialListCmbOrganisation.ItemsList = ViewModel.organizations;
			specialListCmbOrganisation.Binding
				.AddBinding(ViewModel.Entity, e => e.OrganisationForSalary, w => w.SelectedItem)
				.InitializeFromSource();
			specialListCmbOrganisation.Sensitive = ViewModel.CanEditOrganisationForSalary;

			wageParametersView.ViewModel = 
				ViewModel.EmployeeWageParametersFactory.CreateEmployeeWageParametersViewModel(ViewModel.Entity, ViewModel, ViewModel.UoW);
		}

		private void SaveAttachmentFilesChanges()
		{
			if(ViewModel.UoWGeneric.IsNew)
			{
				attachmentFiles.ItemId = ViewModel.Entity.Id;
			}
			
			attachmentFiles.SaveChanges();
		}

		private bool HasAttachmentsFilesChanges() => attachmentFiles.HasChanges;
		
		private void ConfigureRadioButtons()
		{
			radioTabInfo.Clicked += OnRadioTabInfoToggled;
			radioTabLogistic.Clicked += OnRadioTabLogisticToggled;
			radioTabContracts.Clicked += OnRadioTabContractsToggled;
			radioTabAccounting.Clicked += OnRadioTabAccountingToggled;
			radioTabFiles.Clicked += OnRadioTabFilesToggled;
			radioWageParameters.Clicked += OnRadioWageParametersClicked;
			radioTabEmployeeDocument.Clicked += OnRadioTabEmployeeDocumentToggled;
		}

		#region Вкладка Документы

		private void ConfigureDocumentsTabButtons()
		{
			btnAddDocument.Clicked += OnButtonAddDocumentClicked;
			btnEditDocument.Clicked += OnButtonEditDocumentClicked;
			btnRemoveDocument.Clicked += (s, e) => ViewModel.RemoveEmployeeDocumentsCommand.Execute();

			btnAddDocument.Sensitive = ViewModel.CanAddEmployeeDocument;
			btnEditDocument.Binding
				.AddBinding(ViewModel, vm => vm.CanEditEmployeeDocument, w => w.Sensitive).InitializeFromSource();
			btnRemoveDocument.Binding
				.AddBinding(ViewModel, vm => vm.CanRemoveEmployeeDocument, w => w.Sensitive).InitializeFromSource();
		}
		
		private void ConfigureTreeEmployeeDocuments()
		{
			ytreeviewEmployeeDocument.ColumnsConfig = FluentColumnsConfig<EmployeeDocument>.Create()
				.AddColumn("Документ").AddTextRenderer(x => x.Document.GetEnumTitle())
				.AddColumn("Доп. название").AddTextRenderer(x => x.Name)
				.Finish();
			
			ytreeviewEmployeeDocument.SetItemsSource(ViewModel.Entity.ObservableDocuments);
			ytreeviewEmployeeDocument.Selection.Changed += TreeEmployeeDocumentsSelectionOnChanged;
			ytreeviewEmployeeDocument.RowActivated += OnEmployeeDocumentRowActivated;
		}

		private void TreeEmployeeDocumentsSelectionOnChanged(object sender, EventArgs e)
		{
			ViewModel.SelectedEmployeeDocuments = ytreeviewEmployeeDocument.GetSelectedObjects<EmployeeDocument>();
		}
		
		private void OnButtonAddDocumentClicked(object sender, EventArgs e)
		{
			var dlg = new EmployeeDocDlg(
				ViewModel.UoW,
				ViewModel.Entity.IsRussianCitizen ? ViewModel.HiddenForRussianDocument : ViewModel.HiddenForForeignCitizen,
				ServicesConfig.CommonServices);
			dlg.Save += (s, args) => ViewModel.Entity.ObservableDocuments.Add(dlg.Entity);
			ViewModel.TabParent.AddSlaveTab(ViewModel, dlg);
		}

		private void OnButtonEditDocumentClicked(object sender, EventArgs e)
		{
			var dlg = new EmployeeDocDlg(ViewModel.SelectedEmployeeDocuments.ElementAt(0).Id, ViewModel.UoW, ServicesConfig.CommonServices);
			ViewModel.TabParent.AddSlaveTab(ViewModel, dlg);
		}

		private void OnEmployeeDocumentRowActivated(object o, Gtk.RowActivatedArgs args)
		{
			if(ViewModel.CanEditEmployeeDocument)
			{
				btnEditDocument.Click();
			}
		}

		#endregion

		#region Вкладка Договора
		
		private void ConfigureContractsTabButtons()
		{
			btnAddContract.Clicked += OnAddContractButtonClicked;
			btnEditContract.Clicked += OnButtonEditContractClicked;
			btnRemoveContract.Clicked += (s, e) => ViewModel.RemoveEmployeeContractsCommand.Execute();
			
			btnEditContract.Binding
				.AddBinding(ViewModel, vm => vm.CanEditEmployeeContract, w => w.Sensitive).InitializeFromSource();
			btnRemoveContract.Binding
				.AddBinding(ViewModel, vm => vm.CanRemoveEmployeeContract, w => w.Sensitive).InitializeFromSource();
		}
		
		private void ConfigureTreeEmployeeContracts()
		{
			ytreeviewEmployeeContract.ColumnsConfig = FluentColumnsConfig<EmployeeContract>.Create()
				.AddColumn("Договор").AddTextRenderer(x => x.EmployeeContractTemplate.TemplateType.GetEnumTitle())
				.AddColumn("Название").AddTextRenderer(x => x.Name)
				.AddColumn("Дата начала").AddTextRenderer(x => x.FirstDay.ToString("dd/MM/yyyy"))
				.AddColumn("Дата конца").AddTextRenderer(x => x.LastDay.ToString("dd/MM/yyyy"))
				.Finish();
			
			ytreeviewEmployeeContract.SetItemsSource(ViewModel.Entity.ObservableContracts);
			ytreeviewEmployeeContract.Selection.Changed += TreeEmployeeContractsSelectionOnChanged;
			ytreeviewEmployeeContract.RowActivated += OnEmployeeContractRowActivated;
		}

		private void TreeEmployeeContractsSelectionOnChanged(object sender, EventArgs e)
		{
			ViewModel.SelectedEmployeeContracts = ytreeviewEmployeeContract.GetSelectedObjects<EmployeeContract>();
		}

		private void OnAddContractButtonClicked(object sender, EventArgs e)
		{
			List<EmployeeDocument> doc = ViewModel.Entity.GetMainDocuments();
			
			if(!doc.Any())
			{
				MessageDialogHelper.RunInfoDialog("Отсутствует главный документ");
				return;
			}
			
			if(ViewModel.Entity.Registration != RegistrationType.Contract)
			{
				MessageDialogHelper.RunInfoDialog("Должен быть указан тип регистрации: 'ГПК' ");
				return;
			}
			
			var dlg = new EmployeeContractDlg(doc[0], ViewModel.Entity, ViewModel.UoW);
			dlg.Save += (s, args) => ViewModel.Entity.ObservableContracts.Add(dlg.Entity);
			ViewModel.TabParent.AddSlaveTab(ViewModel, dlg);
		}

		private void OnButtonEditContractClicked(object sender, EventArgs e)
		{
			var dlg = new EmployeeContractDlg(ViewModel.SelectedEmployeeDocuments.ElementAt(0).Id, ViewModel.UoW);
			ViewModel.TabParent.AddSlaveTab(ViewModel, dlg);
		}

		private void OnEmployeeContractRowActivated(object o, Gtk.RowActivatedArgs args)
		{
			if(ViewModel.CanEditEmployeeContract)
			{
				btnEditContract.Click();
			}
		}

		#endregion
		
		#region Вкладка Логистика
		
		#region DriverDistrictPriorities

		private void ConfigureDistrictPriorities()
		{
			ytreeDistrictPrioritySets.ColumnsConfig = FluentColumnsConfig<DriverDistrictPrioritySet>.Create()
				.AddColumn("Код")
					.HeaderAlignment(0.5f)
					.MinWidth(75)
					.AddTextRenderer(x => x.Id == 0 ? "Новый" : x.Id.ToString())
					.XAlign(0.5f)
				.AddColumn("Активен")
					.HeaderAlignment(0.5f)
					.AddToggleRenderer(x => x.IsActive)
					.XAlign(0.5f)
					.Editing(false)
				.AddColumn("Дата\nсоздания")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.DateCreated.ToString("g"))
				.AddColumn("Дата\nпоследнего изменения")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.DateLastChanged.ToString("g"))
				.AddColumn("Дата\nактивации")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.DateActivated != null ? x.DateActivated.Value.ToString("g") : "")
				.AddColumn("Дата\nдеактивации")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.DateDeactivated != null ? x.DateDeactivated.Value.ToString("g") : "")
				.AddColumn("Автор")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.Author != null ? x.Author.ShortName : "-")
					.XAlign(0.5f)
				.AddColumn("Изменил")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.LastEditor != null ? x.LastEditor.ShortName : "-")
					.XAlign(0.5f)
				.AddColumn("Создан\nавтоматически")
					.HeaderAlignment(0.5f)
					.AddToggleRenderer(x => x.IsCreatedAutomatically)
					.XAlign(0.5f)
					.Editing(false)
				.AddColumn("")
				.Finish();

			ytreeDistrictPrioritySets.RowActivated += (o, args) => 
			{
				if(ViewModel.CanEditDistrictPrioritySet)
				{
					ViewModel.OpenDistrictPrioritySetEditWindowCommand.Execute();
				}
			};
			ytreeDistrictPrioritySets.ItemsDataSource = ViewModel.Entity.ObservableDriverDistrictPrioritySets;
			ytreeDistrictPrioritySets.Selection.Changed += SelectionDistrictPrioritySetsOnChanged;
			
			ybuttonCopyDistrictPrioritySet.Clicked += (sender, args) => ViewModel.CopyDistrictPrioritySetCommand.Execute();
			ybuttonCopyDistrictPrioritySet.Binding
				.AddBinding(ViewModel, vm => vm.CanCopyDistrictPrioritySet, w => w.Sensitive).InitializeFromSource();
			
			ybuttonEditDistrictPrioritySet.Clicked += (sender, args) => ViewModel.OpenDistrictPrioritySetEditWindowCommand.Execute();
			ybuttonEditDistrictPrioritySet.Binding
				.AddBinding(ViewModel, vm => vm.CanEditDistrictPrioritySet, w => w.Sensitive).InitializeFromSource();
			
			ybuttonActivateDistrictPrioritySet.Clicked += (sender, args) => ViewModel.ActivateDistrictPrioritySetCommand.Execute();
			ybuttonActivateDistrictPrioritySet.Binding
				.AddBinding(ViewModel, x => x.CanActivateDistrictPrioritySet, w => w.Sensitive).InitializeFromSource();

			ybuttonCreateDistrictPrioritySet.Clicked += (sender, args) => ViewModel.OpenDistrictPrioritySetCreateWindowCommand.Execute();
			ybuttonCreateDistrictPrioritySet.Sensitive = ViewModel.DriverDistrictPrioritySetPermission.CanCreate;
		}

		private void SelectionDistrictPrioritySetsOnChanged(object sender, EventArgs e)
		{
			ViewModel.SelectedDistrictPrioritySet = ytreeDistrictPrioritySets.GetSelectedObject<DriverDistrictPrioritySet>();
		}

		#endregion

		#region DriverWorkSchedules

		private void ConfigureWorkSchedules()
		{
			ytreeDriverScheduleSets.ColumnsConfig = FluentColumnsConfig<DriverWorkScheduleSet>.Create()
				.AddColumn("Код")
					.HeaderAlignment(0.5f)
					.MinWidth(75)
					.AddTextRenderer(x => x.Id == 0 ? "Новый" : x.Id.ToString())
					.XAlign(0.5f)
				.AddColumn("Активен")
					.HeaderAlignment(0.5f)
					.AddToggleRenderer(x => x.IsActive)
					.XAlign(0.5f)
					.Editing(false)
				.AddColumn("Дата\nактивации")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.DateActivated.ToString("g"))
				.AddColumn("Дата\nдеактивации")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.DateDeactivated != null ? x.DateDeactivated.Value.ToString("g") : "")
				.AddColumn("Автор")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.Author != null ? x.Author.ShortName : "-")
					.XAlign(0.5f)
				.AddColumn("Изменил")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => x.LastEditor != null ? x.LastEditor.ShortName : "-")
					.XAlign(0.5f)
				.AddColumn("Создан\nавтоматически")
					.HeaderAlignment(0.5f)
					.AddToggleRenderer(x => x.IsCreatedAutomatically)
					.XAlign(0.5f)
					.Editing(false)
				.AddColumn("")
				.Finish();

			ytreeDriverScheduleSets.RowActivated += (o, args) => 
			{
				if(ViewModel.CanEditDriverScheduleSet)
				{
					ViewModel.OpenDriverWorkScheduleSetEditWindowCommand.Execute();
				}
			};
			ytreeDriverScheduleSets.ItemsDataSource = ViewModel.Entity.ObservableDriverWorkScheduleSets;
			ytreeDriverScheduleSets.Selection.Changed += SelectionDriverScheduleSetOnChanged;

			ybuttonCopyScheduleSet.Clicked += (sender, args) => ViewModel.CopyDriverWorkScheduleSetCommand.Execute();
			ybuttonCopyScheduleSet.Binding
				.AddBinding(ViewModel, vm => vm.CanCopyDriverScheduleSet, w => w.Sensitive).InitializeFromSource();

			ybuttonEditScheduleSet.Clicked += (sender, args) => ViewModel.OpenDriverWorkScheduleSetEditWindowCommand.Execute();
			ybuttonEditScheduleSet.Binding.
				AddBinding(ViewModel, vm => vm.CanEditDriverScheduleSet, w => w.Sensitive).InitializeFromSource();

			ybuttonCreateScheduleSet.Clicked += (sender, args) => ViewModel.OpenDriverWorkScheduleSetCreateWindowCommand.Execute();
			ybuttonCreateScheduleSet.Sensitive = ViewModel.DriverWorkScheduleSetPermission.CanCreate;
		}

		private void SelectionDriverScheduleSetOnChanged(object sender, EventArgs e)
		{
			ViewModel.SelectedDriverScheduleSet = ytreeDriverScheduleSets.GetSelectedObject<DriverWorkScheduleSet>();
		}
		
		#endregion

		#endregion

		private void ConfigureCategory()
		{
			comboCategory.EnumItemSelected += OnComboCategoryEnumItemSelected;
			comboCategory.ItemsEnum = typeof(EmployeeCategory);
			comboCategory.Binding.AddSource(ViewModel.Entity)
				.AddBinding(e => e.Category, w => w.SelectedItem)
				.InitializeFromSource();
			comboCategory.Binding.AddSource(ViewModel)
				.AddBinding(vm => vm.CanEditEmployeeCategory, w => w.Sensitive)
				.InitializeFromSource();

			if(ViewModel.HiddenCategories != null && ViewModel.HiddenCategories.Any())
			{
				comboCategory.AddEnumToHideList(ViewModel.HiddenCategories.OfType<object>().ToArray());
			}
			
			comboCategory.ChangedByUser += (sender, e) => 
			{
				if(ViewModel.Entity.Category != EmployeeCategory.driver)
				{
					cmbDriverOf.SelectedItemOrNull = null;
				}
			};
		}

		private void ConfigureSubdivision()
		{
			if(ViewModel.CanManageDriversAndForwarders && !ViewModel.CanManageOfficeWorkers)
			{
				var entityentrySubdivision = new EntityViewModelEntry();
				entityentrySubdivision.SetEntityAutocompleteSelectorFactory(
					ViewModel.SubdivisionJournalFactory.CreateLogisticSubdivisionAutocompleteSelectorFactory(
						ViewModel.EmployeeJournalFactory.CreateEmployeeAutocompleteSelectorFactory()));
				entityentrySubdivision.Binding
					.AddBinding(ViewModel.Entity, e => e.Subdivision, w => w.Subject)
					.InitializeFromSource();
				hboxSubdivision.Add(entityentrySubdivision);
				hboxSubdivision.ShowAll();
				return;
			}

			var entrySubdivision = new yEntryReference();
			entrySubdivision.SubjectType = typeof(Subdivision);
			entrySubdivision.Binding.AddBinding(ViewModel.Entity, e => e.Subdivision, w => w.Subject).InitializeFromSource();
			hboxSubdivision.Add(entrySubdivision);
			hboxSubdivision.ShowAll();

			if(!ViewModel.CanManageOfficeWorkers && !ViewModel.CanManageDriversAndForwarders)
			{
				entrySubdivision.Sensitive = false;
			}
		}

		private void OnRussianCitizenToggled(object sender, EventArgs e)
		{
			if(ViewModel.Entity.IsRussianCitizen == false)
			{
				labelCitizenship.Visible = true;
				referenceCitizenship.Visible = true;
			}
			else
			{
				labelCitizenship.Visible = false;
				referenceCitizenship.Visible = false;
				ViewModel.Entity.Citizenship = null;
			}
		}

		#region RadioTabToggled
		
		private void OnRadioTabInfoToggled(object sender, EventArgs e)
		{
			if(radioTabInfo.Active)
			{
				notebookMain.CurrentPage = 0;
			}
		}

		private void OnRadioTabFilesToggled(object sender, EventArgs e)
		{
			if(radioTabFiles.Active)
			{
				notebookMain.CurrentPage = 3;
			}
		}

		private void OnRadioTabAccountingToggled(object sender, EventArgs e)
		{
			if(radioTabAccounting.Active)
			{
				notebookMain.CurrentPage = 2;
			}
		}

		private void OnRadioTabLogisticToggled(object sender, EventArgs e)
		{
			if(terminalmanagementview1.ViewModel == null)
			{
				terminalmanagementview1.ViewModel = ViewModel.TerminalManagementViewModel;
			}
			if(radioTabLogistic.Active)
			{
				notebookMain.CurrentPage = 1;
			}
		}

		private void OnRadioTabEmployeeDocumentToggled(object sender, EventArgs e)
		{
			if(radioTabEmployeeDocument.Active)
			{
				notebookMain.CurrentPage = 5;
			}
		}

		private void OnRadioTabContractsToggled(object sender, EventArgs e)
		{
			if(radioTabContracts.Active)
			{
				notebookMain.CurrentPage = 4;
			}
		}
		
		#endregion
		
		#region Driver & forwarder

		private void OnComboCategoryEnumItemSelected(object sender, ItemSelectedEventArgs e)
		{
			radioTabLogistic.Visible
				= lblDriverOf.Visible
				= hboxDriversParameters.Visible
				= (EmployeeCategory)e.SelectedItem == EmployeeCategory.driver;

			wageParametersView.Sensitive = ViewModel.CanEditWage;
		}

		private void OnRadioWageParametersClicked(object sender, EventArgs e)
		{
			if(radioWageParameters.Active)
			{
				notebookMain.CurrentPage = 6;
			}
		}

		#endregion

		public override void Destroy()
		{
			ViewModel.SaveAttachmentFilesChangesAction -= SaveAttachmentFilesChanges;
			ViewModel.HasAttachmentFilesChangesFunc -= HasAttachmentsFilesChanges;
			
			base.Destroy();
		}

		protected void OnYAddMobileLoginClicked(object sender, EventArgs e)
		{
			ViewModel.RegisterDriverModileUserCommand.Execute();
		}
	}
}
