﻿using System.Linq;
using Gamma.Binding;
using Gamma.GtkWidgets;
using NLog;
using QS.DomainModel.UoW;
using QS.Project.Domain;
using QS.Validation;
using QSOrmProject;
using QSProjectsLib;
using Vodovoz.Domain.Sale;
using Vodovoz.Representations;
using Vodovoz.ViewModel;
using System;
using QS.Project.Journal.EntitySelector;
using Vodovoz.Domain.WageCalculation;
using Vodovoz.ViewWidgets.Permissions;
using Vodovoz.ViewModels.Permissions;
using Vodovoz.EntityRepositories.Permissions;
using Vodovoz.Journals.JournalViewModels.WageCalculation;
using QS.Project.Services;
using Vodovoz.TempAdapters;
using QS.Project.Journal;
using Vodovoz.EntityRepositories.Subdivisions;
using Vodovoz.Parameters;

namespace Vodovoz
{
	[Obsolete("Использовать Vodovoz.Views.Organization.SubdivisionView")]
	public partial class SubdivisionDlg : QS.Dialog.Gtk.EntityDialogBase<Subdivision>
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private readonly ISubdivisionRepository _subdivisionRepository = new SubdivisionRepository(new ParametersProvider());
		SubdivisionsVM subdivisionsVM;
		PresetSubdivisionPermissionsViewModel presetPermissionVM;

		[Obsolete("Использовать Vodovoz.Views.Organization.SubdivisionView")]
		public SubdivisionDlg()
		{
			this.Build();
			TabName = "Новое подразделение";
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Subdivision>();
			ConfigureDlg();
		}

		[Obsolete("Использовать Vodovoz.Views.Organization.SubdivisionView")]
		public SubdivisionDlg(int id)
		{
			this.Build();
			logger.Info("Загрузка информации о подразделении...");
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Subdivision>(id);
			ConfigureDlg();
		}

		[Obsolete("Использовать Vodovoz.Views.Organization.SubdivisionView")]
		public SubdivisionDlg(Subdivision sub) : this(sub.Id) { }

		private void ConfigureDlg()
		{
			subdivisionentitypermissionwidget.ConfigureDlg(UoW, Entity);
			yentryName.Binding.AddBinding(Entity, e => e.Name, w => w.Text).InitializeFromSource();
			yentryShortName.Binding.AddBinding(Entity, e => e.ShortName, w => w.Text).InitializeFromSource();
			yentryrefParentSubdivision.SubjectType = typeof(Subdivision);
			yentryrefParentSubdivision.Binding.AddBinding(Entity, e => e.ParentSubdivision, w => w.Subject).InitializeFromSource();
			yentryreferenceChief.RepresentationModel = new EmployeesVM();
			yentryreferenceChief.Binding.AddBinding(Entity, e => e.Chief, w => w.Subject).InitializeFromSource();

			yenumcomboType.ItemsEnum = typeof(SubdivisionType);
			yenumcomboType.Binding.AddBinding(Entity, e => e.SubdivisionType, w => w.SelectedItem).InitializeFromSource();
			yenumcomboType.Sensitive = false;

			subdivisionsVM = new SubdivisionsVM(UoW, Entity);
			repTreeChildSubdivisions.RepresentationModel = subdivisionsVM;
			repTreeChildSubdivisions.YTreeModel = new RecursiveTreeModel<SubdivisionVMNode>(subdivisionsVM.Result, x => x.Parent, x => x.Children);

			ySpecCmbGeographicGroup.ItemsList = UoW.Session.QueryOver<GeographicGroup>().List();
			ySpecCmbGeographicGroup.Binding.AddBinding(Entity, e => e.GeographicGroup, w => w.SelectedItem).InitializeFromSource();
			ySpecCmbGeographicGroup.ItemSelected += YSpecCmbGeographicGroup_ItemSelected;
			SetControlsAccessibility();

			ytreeviewDocuments.ColumnsConfig = ColumnsConfigFactory.Create<TypeOfEntity>()
				.AddColumn("Документ").AddTextRenderer(x => x.CustomName)
				.Finish();
			ytreeviewDocuments.ItemsDataSource = Entity.ObservableDocumentTypes;

			lblWarehouses.LineWrapMode = Pango.WrapMode.Word;
			if(Entity.Id > 0)
				lblWarehouses.Text = Entity.GetWarehousesNames(UoW, _subdivisionRepository);
			else
				frmWarehoses.Visible = false;
			vboxDocuments.Visible = QSMain.User.Admin;

			presetPermissionVM = new PresetSubdivisionPermissionsViewModel(UoW, new PermissionRepository(), Entity);
			vboxPresetPermissions.Add(new PresetPermissionsView(presetPermissionVM));
			vboxPresetPermissions.ShowAll();
			vboxPresetPermissions.Visible = QSMain.User.Admin;

			presetPermissionVM.ObservablePermissionsList.ListContentChanged += (sender, e) => HasChanges = true;
			Entity.ObservableDocumentTypes.ListContentChanged += (sender, e) => HasChanges = true;
			subdivisionentitypermissionwidget.ViewModel.ObservableTypeOfEntitiesList.ListContentChanged += (sender, e) => HasChanges = true;

			entryDefaultSalesPlan.SetEntityAutocompleteSelectorFactory(
				new EntityAutocompleteSelectorFactory<SalesPlanJournalViewModel>(typeof(SalesPlan),
					() => new SalesPlanJournalViewModel(
						UnitOfWorkFactory.GetDefaultFactory,
						ServicesConfig.CommonServices,
						new NomenclatureSelectorFactory())
					{
						SelectionMode = JournalSelectionMode.Single
					}
			));
			entryDefaultSalesPlan.Binding.AddBinding(Entity, s => s.DefaultSalesPlan, w => w.Subject).InitializeFromSource();
			entryDefaultSalesPlan.CanEditReference = false;
		}

		void YSpecCmbGeographicGroup_ItemSelected(object sender, Gamma.Widgets.ItemSelectedEventArgs e)
		{
			SetControlsAccessibility();
			if(Entity.ParentSubdivision != null || Entity.ChildSubdivisions.Any())
				foreach(var s in Entity.ChildSubdivisions) {
					s.GeographicGroup = Entity.GeographicGroup;
				}
		}

		#region implemented abstract members of OrmGtkDialogBase

		public override bool Save()
		{
			var valid = new QSValidator<Subdivision>(UoWGeneric.Root);
			if(valid.RunDlgIfNotValid((Gtk.Window)this.Toplevel))
				return false;

			UoWGeneric.Save();
			subdivisionentitypermissionwidget.ViewModel.SavePermissions(UoW);
			presetPermissionVM.SaveCommand.Execute();
			UoW.Commit();
			return true;
		}

		#endregion

		protected void OnButtonAddDocumentClicked(object sender, System.EventArgs e)
		{
			var docTypesJournal = new OrmReference(typeof(TypeOfEntity), UoW) {
				Mode = OrmReferenceMode.Select
			};
			docTypesJournal.ObjectSelected += DocTypesJournal_ObjectSelected;
			TabParent.AddSlaveTab(this, docTypesJournal);
		}

		protected void OnButtonDeleteDocumentClicked(object sender, System.EventArgs e)
		{
			if(ytreeviewDocuments.GetSelectedObject() is TypeOfEntity selectedObject)
				Entity.ObservableDocumentTypes.Remove(selectedObject);
		}

		void DocTypesJournal_ObjectSelected(object sender, OrmReferenceObjectSectedEventArgs e)
		{
			if(e.Subject is TypeOfEntity selectedObject)
				Entity.ObservableDocumentTypes.Add(selectedObject);
		}

		void SetControlsAccessibility()
		{
			lblGeographicGroup.Visible = ySpecCmbGeographicGroup.Visible
				= Entity.ParentSubdivision != null && Entity.ChildSubdivisions.Any();
		}
	}
}