﻿using System;
using System.Linq;
using Gamma.GtkWidgets;
using Gamma.Utilities;
using QS.DomainModel.UoW;
using QSOrmProject;
using QSProjectsLib;
using QSTDI;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Orders;
using Vodovoz.Repository;
using Vodovoz.Repository.Operations;
using Vodovoz.SidePanel.InfoProviders;

namespace Vodovoz.SidePanel.InfoViews
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class CounterpartyPanelView : Gtk.Bin, IPanelView
	{		
		Counterparty Counterparty{get;set;}

		public CounterpartyPanelView()
		{
			this.Build();
			Configure();
		}

		void Configure()
		{
			labelName.LineWrapMode = Pango.WrapMode.WordChar;
			labelLatestOrderDate.LineWrapMode = Pango.WrapMode.WordChar;
			ytreeCurrentOrders.ColumnsConfig = ColumnsConfigFactory.Create<Order>()
				.AddColumn("Номер")
					.AddNumericRenderer(node => node.Id)
				.AddColumn("Дата")
				.AddTextRenderer(node => node.DeliveryDate.HasValue ? node.DeliveryDate.Value.ToShortDateString() : String.Empty)
				.AddColumn("Статус")
					.AddTextRenderer(node => node.OrderStatus.GetEnumTitle())
				.Finish();			
		}

		#region IPanelView implementation
		public IInfoProvider InfoProvider{ get; set; }

		public void Refresh()
		{
			Counterparty = (InfoProvider as ICounterpartyInfoProvider)?.Counterparty;
			if(Counterparty == null) {
				buttonSaveComment.Sensitive = false;
				return;
			}
			buttonSaveComment.Sensitive = true;
			labelName.Text = Counterparty.FullName;
			textviewComment.Buffer.Text = Counterparty.Comment;

			var debt = MoneyRepository.GetCounterpartyDebt(InfoProvider.UoW, Counterparty);
			string labelDebtFormat 		 = "<span {0}>{1}</span>";
			string backgroundDebtColor 	 = "";
			if (debt > 0)
			{
				backgroundDebtColor 	 = "background=\"red\"";
				ylabelDebtInfo.LabelProp = "Долг:";
			}
			if (debt < 0)
			{
				backgroundDebtColor 	 = "background=\"lightgreen\"";
				ylabelDebtInfo.LabelProp = "Баланс:";
				debt 	= -debt;
			}
			labelDebt.Markup = string.Format(labelDebtFormat, backgroundDebtColor, CurrencyWorks.GetShortCurrencyString(debt));

			var latestOrder = OrderRepository.GetLatestCompleteOrderForCounterparty(InfoProvider.UoW, Counterparty);
			if (latestOrder != null)
			{
				var daysFromLastOrder = (DateTime.Today - latestOrder.DeliveryDate.Value).Days;
				labelLatestOrderDate.Text = String.Format(
					"{0} ({1} {2} назад)",
					latestOrder.DeliveryDate.Value.ToShortDateString(),
					daysFromLastOrder,
					RusNumber.Case(daysFromLastOrder, "день", "дня", "дней")
				);
			}
			else
			{
				labelLatestOrderDate.Text = "(Выполненных заказов нет)";
			}
			var currentOrders = OrderRepository.GetCurrentOrders(InfoProvider.UoW, Counterparty);
			ytreeCurrentOrders.SetItemsSource<Order>(currentOrders);
			vboxCurrentOrders.Visible = currentOrders.Count > 0;

			labelPhone.LabelProp = String.Join(";\n", Counterparty.Phones.Select(ph => ph.LongText));
			if(Counterparty.Phones.Count <= 0)
				labelPhone.Text = "[+] чтоб добавить -->";
		}

		public bool VisibleOnPanel
		{
			get
			{
				return Counterparty != null;
			}
		}
			
		public void OnCurrentObjectChanged(object changedObject)
		{			
			if (changedObject is Counterparty)
			{
				Refresh();
			}
		}

		protected void OnButtonSaveCommentClicked(object sender, EventArgs e)
		{
			using(var uow = UnitOfWorkFactory.CreateForRoot<Counterparty>(Counterparty.Id))
			{
				uow.Root.Comment = textviewComment.Buffer.Text;
				uow.Save();
			}
		}

		protected void OnBtnAddPhoneClicked(object sender, EventArgs e)
		{
			var dlg = new CounterpartyDlg(Counterparty.Id);
			dlg.ActivateContactsTab();
			TDIMain.MainNotebook.OpenTab(
				OrmMain.GenerateDialogHashName<Counterparty>(Counterparty.Id),
				() => dlg
			);
		}
		#endregion
	}
}

