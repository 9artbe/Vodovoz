﻿using System;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using Vodovoz.Additions.Store;
using Vodovoz.Core.Permissions;
using Vodovoz.Domain.Documents;
using Vodovoz.Repositories.HumanResources;

namespace Vodovoz
{
	public partial class RegradingOfGoodsDocumentDlg : QS.Dialog.Gtk.EntityDialogBase<RegradingOfGoodsDocument>
	{
		static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();

		public RegradingOfGoodsDocumentDlg()
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<RegradingOfGoodsDocument> ();
			Entity.Author = EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			if(Entity.Author == null)
			{
				MessageDialogHelper.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете создавать складские документы, так как некого указывать в качестве кладовщика.");
				FailInitialize = true;
				return;
			}
			Entity.Warehouse = StoreDocumentHelper.GetDefaultWarehouse(UoW, WarehousePermissions.RegradingOfGoodsEdit);

			ConfigureDlg();
		}

		public RegradingOfGoodsDocumentDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<RegradingOfGoodsDocument> (id);
			ConfigureDlg ();
		}

		public RegradingOfGoodsDocumentDlg (RegradingOfGoodsDocument sub) : this (sub.Id)
		{
		}

		void ConfigureDlg ()
		{
			if(StoreDocumentHelper.CheckAllPermissions(UoW.IsNew, WarehousePermissions.RegradingOfGoodsEdit, Entity.Warehouse)) {
				FailInitialize = true;
				return;
			}

			var editing = StoreDocumentHelper.CanEditDocument(WarehousePermissions.RegradingOfGoodsEdit, Entity.Warehouse);
			yentryrefWarehouse.IsEditable = ytextviewCommnet.Editable = editing;
			regradingofgoodsitemsview.Sensitive = editing;

			ylabelDate.Binding.AddFuncBinding(Entity, e => e.TimeStamp.ToString("g"), w => w.LabelProp).InitializeFromSource();
			yentryrefWarehouse.ItemsQuery = StoreDocumentHelper.GetRestrictedWarehouseQuery(WarehousePermissions.RegradingOfGoodsEdit);
			yentryrefWarehouse.Binding.AddBinding(Entity, e => e.Warehouse, w => w.Subject).InitializeFromSource();
			ytextviewCommnet.Binding.AddBinding(Entity, e => e.Comment, w => w.Buffer.Text).InitializeFromSource();

			regradingofgoodsitemsview.DocumentUoW = UoWGeneric;
			if (Entity.Items.Count > 0)
				yentryrefWarehouse.Sensitive = false;
		}

		public override bool Save ()
		{
			var valid = new QS.Validation.GtkUI.QSValidator<RegradingOfGoodsDocument> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			Entity.LastEditor = EmployeeRepository.GetEmployeeForCurrentUser (UoW);
			Entity.LastEditedTime = DateTime.Now;
			if(Entity.LastEditor == null)
			{
				MessageDialogHelper.RunErrorDialog ("Ваш пользователь не привязан к действующему сотруднику, вы не можете изменять складские документы, так как некого указывать в качестве кладовщика.");
				return false;
			}

			logger.Info ("Сохраняем документ пересортицы...");
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}
	}
}

