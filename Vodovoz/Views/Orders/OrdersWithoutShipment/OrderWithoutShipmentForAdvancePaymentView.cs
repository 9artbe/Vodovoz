﻿using System;
using Gamma.GtkWidgets;
using Gtk;
using QS.Project.Journal.EntitySelector;
using QS.Views.GtkUI;
using Vodovoz.Domain.Client;
using Vodovoz.Filters.ViewModels;
using Vodovoz.JournalViewModels;
using Vodovoz.ViewModels.Orders.OrdersWithoutShipment;
using Vodovoz.Domain.Orders.OrdersWithoutShipment;
using QS.Utilities;
using Vodovoz.Dialogs.Email;
using Vodovoz.Infrastructure.Converters;

namespace Vodovoz.Views.Orders.OrdersWithoutShipment
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OrderWithoutShipmentForAdvancePaymentView : TabViewBase<OrderWithoutShipmentForAdvancePaymentViewModel>
	{
		public OrderWithoutShipmentForAdvancePaymentView(OrderWithoutShipmentForAdvancePaymentViewModel viewModel) : base(viewModel)
		{
			Build();
			Configure();
		}

		private void Configure()
		{
			btnCancel.Clicked += (sender, e) => ViewModel.CancelCommand.Execute();
			buttonAddForSale.Clicked += (sender, e) => ViewModel.AddForSaleCommand.Execute();
			btnDeleteOrderItem.Clicked += (sender, e) => ViewModel.DeleteItemCommand.Execute();
			ybtnOpenBill.Clicked += (sender, e) => ViewModel.OpenBillCommand.Execute();

			ylabelOrderNum.Binding.AddBinding(ViewModel.Entity, e => e.Id, w => w.Text, new IntToStringConverter()).InitializeFromSource();
			ylabelOrderDate.Binding.AddFuncBinding(ViewModel, vm => vm.Entity.CreateDate.ToString(), w => w.Text).InitializeFromSource();
			ylabelOrderAuthor.Binding.AddFuncBinding(ViewModel, vm => vm.Entity.Author.ShortName, w => w.Text).InitializeFromSource();
			btnDeleteOrderItem.Binding.AddFuncBinding(ViewModel, vm => vm.SelectedItem != null, w => w.Sensitive).InitializeFromSource();
			yCheckBtnHideSignature.Binding.AddBinding(ViewModel.Entity, e => e.HideSignature, w => w.Active).InitializeFromSource();

			entityViewModelEntryCounterparty.SetEntityAutocompleteSelectorFactory(
				new DefaultEntityAutocompleteSelectorFactory<Counterparty, CounterpartyJournalViewModel, CounterpartyJournalFilterViewModel>(QS.Project.Services.ServicesConfig.CommonServices)
			);

			entityViewModelEntryCounterparty.Changed += ViewModel.OnEntityViewModelEntryChanged;

			entityViewModelEntryCounterparty.Binding.AddBinding(ViewModel.Entity, e => e.Client, w => w.Subject).InitializeFromSource();
			entityViewModelEntryCounterparty.Binding.AddFuncBinding(ViewModel, vm => !vm.IsDocumentSent, w => w.Sensitive).InitializeFromSource();
			entityViewModelEntryCounterparty.CanEditReference = true;
			
			var sendEmailView = new SendDocumentByEmailView(ViewModel.SendDocViewModel);
			hboxSendDocuments.Add(sendEmailView);
			sendEmailView.Show();
			
			ViewModel.OpenCounterpartyJournal += entityViewModelEntryCounterparty.OpenSelectDialog;

			ConfigureTreeItems();
		}

		private void ConfigureTreeItems()
		{
			var colorWhite = new Gdk.Color(0xff, 0xff, 0xff);
			var colorLightRed = new Gdk.Color(0xff, 0x66, 0x66);

			treeItems.ColumnsConfig = ColumnsConfigFactory.Create<OrderWithoutShipmentForAdvancePaymentItem>()
				.AddColumn("Номенклатура")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(node => node.NomenclatureString)
				.AddColumn("Кол-во")
				.SetTag("Count")
					.HeaderAlignment(0.5f)
					.AddNumericRenderer(node => node.Count)
					.Adjustment(new Adjustment(0, 0, 1000000, 1, 100, 0)).Editing().WidthChars(10)
					.AddSetter((c, node) => c.Digits = node.Nomenclature.Unit == null ? 0 : (uint)node.Nomenclature.Unit.Digits)
				.AddColumn("Аренда")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(node => node.IsRentCategory ? node.RentString : string.Empty)
				.AddColumn("Цена")
					.HeaderAlignment(0.5f)
					.AddNumericRenderer(node => node.Price).Digits(2).WidthChars(10)
					.Adjustment(new Adjustment(0, 0, 1000000, 1, 100, 0)).Editing(true)
					.AddSetter((c, node) => c.Editable = node.CanEditPrice)
				.AddTextRenderer(node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn("В т.ч. НДС")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(x => CurrencyWorks.GetShortCurrencyString(x.IncludeNDS ?? 0))
				.AddColumn("Сумма")
					.HeaderAlignment(0.5f)
					.AddTextRenderer(node => CurrencyWorks.GetShortCurrencyString(node.Sum))
				.AddColumn("Скидка")
					.HeaderAlignment(0.5f)
					.AddNumericRenderer(node => node.ManualChangingDiscount).Editing()
				.AddSetter(
						(c, n) => c.Adjustment = n.IsDiscountInMoney
									? new Adjustment(0, 0, (double)(n.Price * n.Count), 1, 100, 1)
									: new Adjustment(0, 0, 100, 1, 100, 1)
					)
					.Digits(2)
					.WidthChars(10)
					.AddTextRenderer(n => n.IsDiscountInMoney ? CurrencyWorks.CurrencyShortName : "%", false)
				.AddColumn("Скидка \nв рублях?")
					.AddToggleRenderer(x => x.IsDiscountInMoney).Editing()
				.AddColumn("Основание скидки")
					.HeaderAlignment(0.5f)
					.AddComboRenderer(node => node.DiscountReason)
					.SetDisplayFunc(x => x.Name)
					.FillItems(ViewModel.OrderRepository.GetDiscountReasons(ViewModel.UoW))
					.AddSetter((c, n) => c.Editable = n.Discount > 0)
					.AddSetter(
						(c, n) => c.BackgroundGdk = n.Discount > 0 && n.DiscountReason == null
						? colorLightRed
						: colorWhite
					)
				.RowCells()
					.XAlign(0.5f)
				.Finish();
			treeItems.ItemsDataSource = ViewModel.Entity.ObservableOrderWithoutDeliveryForAdvancePaymentItems;
			treeItems.Selection.Changed += TreeItems_Selection_Changed;
		}

		private void TreeItems_Selection_Changed(object sender, EventArgs e)
		{
			ViewModel.SelectedItem = treeItems.GetSelectedObject();
		}
		
		public override void Destroy()
		{
			entityViewModelEntryCounterparty.Changed -= ViewModel.OnEntityViewModelEntryChanged;
			
			base.Destroy();
		}
	}
}
