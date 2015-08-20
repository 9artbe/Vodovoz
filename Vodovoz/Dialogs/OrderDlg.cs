﻿using System;
using QSOrmProject;
using Vodovoz.Domain;
using NLog;
using QSValidation;
using QSTDI;
using Vodovoz.Repository;
using QSProjectsLib;
using Gtk;
using Gtk.DataBindings;
using System.Linq;
using Vodovoz.Domain.Orders;
using System.Collections.Generic;
using System.Data.Bindings;
using Vodovoz.Domain.Service;
using System.Data.Bindings.Collections.Generic;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class OrderDlg : OrmGtkDialogBase<Order>
	{
		static Logger logger = LogManager.GetCurrentClassLogger ();

		public OrderDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Order> ();
			UoWGeneric.Root.OrderStatus = OrderStatus.NewOrder;
			TabName = "Новый заказ";
			ConfigureDlg ();
		}

		public OrderDlg (int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Order> (id);
			ConfigureDlg ();
		}

		public OrderDlg (Order sub) : this (sub.Id)
		{
		}

		public void ConfigureDlg ()
		{
			if (UoWGeneric.Root.PreviousOrder != null) {
				labelPreviousOrder.Text = "Посмотреть предыдущий заказ";
//TODO Make it clickable.
			} else
				labelPreviousOrder.Visible = false;
			buttonAccept.Visible = (UoWGeneric.Root.OrderStatus == OrderStatus.NewOrder || UoWGeneric.Root.OrderStatus == OrderStatus.Accepted);
			if (UoWGeneric.Root.OrderStatus == OrderStatus.Accepted) {
				var icon = new Image ();
				icon.Pixbuf = Stetic.IconLoader.LoadIcon (this, "gtk-edit", IconSize.Menu);
				buttonAccept.Image = icon;
				buttonAccept.Label = "Редактировать";
			}
			subjectAdaptor.Target = UoWGeneric.Root;

			treeDocuments.ItemsDataSource = UoWGeneric.Root.ObservableOrderDocuments;
			treeItems.ItemsDataSource = UoWGeneric.Root.ObservableOrderItems;
			treeEquipment.ItemsDataSource = UoWGeneric.Root.ObservableOrderEquipments;
			treeDepositRefundItems.ItemsDataSource = UoWGeneric.Root.ObservableOrderDepositRefundItem;
			treeServiceClaim.ItemsDataSource = UoWGeneric.Root.ObservableInitialOrderService;
			//TODO FIXME Добавить в таблицу закрывающие заказы.

			enumSignatureType.DataSource = subjectAdaptor;
			enumPaymentType.DataSource = subjectAdaptor;
			datatable1.DataSource = subjectAdaptor;
			datatable2.DataSource = subjectAdaptor;
			enumStatus.DataSource = subjectAdaptor;

			referenceDeliveryPoint.Sensitive = (UoWGeneric.Root.Client != null);
			buttonViewDocument.Sensitive = false;
			buttonDelete.Sensitive = false;
			enumStatus.Sensitive = false;
			notebook1.ShowTabs = false;
			notebook1.Page = 0;

			referenceClient.SubjectType = typeof(Counterparty);
			referenceDeliveryPoint.SubjectType = typeof(DeliveryPoint);
			referenceDeliverySchedule.SubjectType = typeof(DeliverySchedule);

			#region Events
			treeDocuments.Selection.Changed += (sender, e) => {
				buttonViewDocument.Sensitive = treeDocuments.Selection.CountSelectedRows () > 0;
			};

			treeDocuments.RowActivated += (o, args) => buttonViewDocument.Click ();

			enumAddRentButton.EnumItemClicked += (sender, e) => AddRentAgreement ((OrderAgreementType)e.ItemEnum);
				
			checkSelfDelivery.Toggled += (sender, e) => {
				referenceDeliverySchedule.Sensitive = labelDeliverySchedule.Sensitive = !checkSelfDelivery.Active;
			};

			UoWGeneric.Root.ObservableOrderItems.ElementChanged += (aList, aIdx) => {
				FixPrice (aIdx [0]);
				enumPaymentType.Sensitive = UoWGeneric.Root.CanChangePaymentType ();
			};

			UoWGeneric.Root.ObservableOrderItems.ElementAdded += (aList, aIdx) => { 
				FixPrice (aIdx [0]); 			
				enumPaymentType.Sensitive = UoWGeneric.Root.CanChangePaymentType ();
			};

			UoWGeneric.Root.ObservableOrderItems.ListContentChanged += (sender, e) => {
				UpdateSum ();
				enumPaymentType.Sensitive = UoWGeneric.Root.CanChangePaymentType ();
			};

			UoWGeneric.Root.ObservableOrderDepositRefundItem.ListContentChanged += (sender, e) => {
				UpdateSum ();
				enumPaymentType.Sensitive = UoWGeneric.Root.CanChangePaymentType ();
			};

			UoWGeneric.Root.ObservableFinalOrderService.ElementAdded += (aList, aIdx) => {
				enumPaymentType.Sensitive = UoWGeneric.Root.CanChangePaymentType ();
				UpdateSum ();
			};
			
			UoWGeneric.Root.ObservableOrderDepositRefundItem.ElementAdded += (aList, aIdx) => {
				enumPaymentType.Sensitive = UoWGeneric.Root.CanChangePaymentType ();
				UpdateSum ();
			};

			treeItems.Selection.Changed += TreeItems_Selection_Changed;
			#endregion

			dataSumDifferenceReason.Completion = new EntryCompletion ();
			dataSumDifferenceReason.Completion.Model = OrderRepository.GetListStoreSumDifferenceReasons (UoWGeneric);
			dataSumDifferenceReason.Completion.TextColumn = 0;

			spinSumDifference.Value = (double)(UoWGeneric.Root.SumToReceive - UoWGeneric.Root.TotalSum);

			treeItems.ColumnMappingConfig = FluentMappingConfig<OrderItem>.Create ()
				.AddColumn ("Номенклатура").SetDataProperty (node => node.NomenclatureString)
				.AddColumn ("Кол-во").AddNumericRenderer (node => node.Count)
				.Adjustment (new Adjustment (0, 0, 1000000, 1, 100, 0))
				.AddSetter ((c, node) => c.Digits = node.Nomenclature.Unit == null ? 0 : (uint)node.Nomenclature.Unit.Digits)
				.AddSetter ((c, node) => c.Editable = node.CanEditAmount)
				.WidthChars (10)
				.AddTextRenderer (node => node.Nomenclature.Unit == null ? String.Empty : node.Nomenclature.Unit.Name, false)
				.AddColumn ("Цена").AddNumericRenderer (node => node.Price).Digits (2)
				.AddTextRenderer (node => CurrencyWorks.CurrencyShortName, false)
				.AddColumn ("Сумма").AddTextRenderer (node => CurrencyWorks.GetShortCurrencyString (node.Price * node.Count))
				.AddColumn ("Доп. соглашение").SetDataProperty (node => node.AgreementString)
				.Finish ();

			treeEquipment.ColumnMappingConfig = FluentMappingConfig<OrderEquipment>.Create ()
				.AddColumn ("Наименование").SetDataProperty (node => node.NameString)
				.AddColumn ("Направление").SetDataProperty (node => node.DirectionString)
				.AddColumn ("Причина").SetDataProperty (node => node.ReasonString)
				.Finish ();

			treeDocuments.ColumnMappingConfig = FluentMappingConfig<OrderDocument>.Create ()
				.AddColumn ("Документ").SetDataProperty (node => node.Name)
				.AddColumn ("Дата").SetDataProperty (node => node.DocumentDate)
				.Finish ();

			treeDepositRefundItems.ColumnMappingConfig = FluentMappingConfig<OrderDepositRefundItem>.Create ()
				.AddColumn ("Тип").SetDataProperty (node => node.DepositTypeString)
				.AddColumn ("Сумма").AddNumericRenderer (node => node.RefundDeposit)
				.Finish ();

			treeServiceClaim.ColumnMappingConfig = FluentMappingConfig<ServiceClaim>.Create ()
				.AddColumn ("Статус заявки").SetDataProperty (node => node.Status.GetEnumTitle ())
				.AddColumn ("Номенклатура оборудования").SetDataProperty (node => node.Nomenclature != null ? node.Nomenclature.Name : "-")
				.AddColumn ("Серийный номер").SetDataProperty (node => node.Equipment != null ? node.Equipment.Serial : "-")
				.AddColumn ("Причина").SetDataProperty (node => node.Reason)
				.Finish ();
			
			UpdateSum ();

			enumPaymentType.Sensitive = UoWGeneric.Root.CanChangePaymentType ();

			if (UoWGeneric.Root.OrderStatus != OrderStatus.NewOrder)
				IsEditable ();
		}

		void FixPrice (int id)
		{
			OrderItem item = UoWGeneric.Root.ObservableOrderItems [id];
			if (item.Nomenclature.Category == NomenclatureCategory.water) {
				UoWGeneric.Root.RecalcBottlesDeposits (UoWGeneric);
				if ((item.AdditionalAgreement as WaterSalesAgreement).IsFixedPrice) {
					return;
				}
			}
			item.Price = item.Nomenclature.GetPrice (item.Count);
		}

		void TreeItems_Selection_Changed (object sender, EventArgs e)
		{
			object[] items = treeItems.GetSelectedObjects ();

			buttonDelete.Sensitive = items.Length > 0 && ((items [0] as OrderItem).AdditionalAgreement == null || (items [0] as OrderItem).Nomenclature.Category == NomenclatureCategory.water);
		}

		public override bool Save ()
		{
			var valid = new QSValidator<Order> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем заказ...");
			UoWGeneric.Save ();
			logger.Info ("Ok.");
			return true;
		}

		#region Toggle buttons

		protected void OnToggleInformationToggled (object sender, EventArgs e)
		{
			if (toggleInformation.Active)
				notebook1.CurrentPage = 0;
		}

		protected void OnToggleGoodsToggled (object sender, EventArgs e)
		{
			if (toggleGoods.Active)
				notebook1.CurrentPage = 1;
		}

		protected void OnToggleEquipmentToggled (object sender, EventArgs e)
		{
			if (toggleEquipment.Active)
				notebook1.CurrentPage = 2;
		}

		protected void OnToggleServiceToggled (object sender, EventArgs e)
		{
			if (toggleService.Active)
				notebook1.CurrentPage = 3;
		}

		protected void OnToggleDocumentsToggled (object sender, EventArgs e)
		{
			if (toggleDocuments.Active)
				notebook1.CurrentPage = 4;
		}

		#endregion

		protected void OnReferenceClientChanged (object sender, EventArgs e)
		{
			if (UoWGeneric.Root.Client != null) {
				referenceDeliveryPoint.RepresentationModel = new ViewModel.DeliveryPointsVM (UoW, Entity.Client);
				referenceDeliveryPoint.Sensitive = UoWGeneric.Root.OrderStatus == OrderStatus.NewOrder;
				enumSignatureType.Visible = checkDelivered.Visible = labelSignatureType.Visible = 
					(UoWGeneric.Root.Client.PersonType == PersonType.legal);
			} else {
				referenceDeliveryPoint.Sensitive = false;
			}
			UpdateProxyInfo ();
		}

		private void IsEditable (bool val = false)
		{
			referenceDeliverySchedule.Sensitive = referenceDeliveryPoint.Sensitive = 
				referenceClient.Sensitive = spinBottlesReturn.Sensitive = val;
			spinSumDifference.Sensitive = textComments.Sensitive = 
				checkDelivered.Sensitive = checkSelfDelivery.Sensitive = val;
			enumAddRentButton.Sensitive = buttonAddForSale.Sensitive = 
				enumSignatureType.Sensitive = enumStatus.Sensitive = val;
			enumPaymentType.Sensitive = pickerDeliveryDate.Sensitive = buttonFillComment.Sensitive =
				dataSumDifferenceReason.Sensitive = val;
			treeItems.Sensitive = val;
		}

		protected void OnButtonDeleteClicked (object sender, EventArgs e)
		{
			Entity.RemoveItem (treeItems.GetSelectedObject () as OrderItem);
		}

		protected void OnButtonAddForSaleClicked (object sender, EventArgs e)
		{
			OrmReference SelectDialog = new OrmReference (typeof(Nomenclature), UoWGeneric, 
				                            NomenclatureRepository.NomenclatureForSaleQuery ()
				.GetExecutableQueryOver (UoWGeneric.Session).RootCriteria);
			SelectDialog.Mode = OrmReferenceMode.Select;
			SelectDialog.ButtonMode = ReferenceButtonMode.CanAdd;
			SelectDialog.ObjectSelected += NomenclatureSelected;
			TabParent.AddSlaveTab (this, SelectDialog);
		}

		void NomenclatureSelected (object sender, OrmReferenceObjectSectedEventArgs e)
		{
			Nomenclature nomenclature = e.Subject as Nomenclature;
			if (nomenclature.Category == NomenclatureCategory.additional) {
				UoWGeneric.Root.AddAdditionalNomenclatureForSale (nomenclature);
			} else if (nomenclature.Category == NomenclatureCategory.equipment) {
				UoWGeneric.Root.AddEquipmentNomenclatureForSale (nomenclature, UoWGeneric);
			} else if (nomenclature.Category == NomenclatureCategory.water) {
				CounterpartyContract contract = CounterpartyContractRepository.
					GetCounterpartyContractByPaymentType (UoWGeneric, UoWGeneric.Root.Client, UoWGeneric.Root.PaymentType);
				if (contract == null) {
					RunContractCreateDialog ();
					return;
				}
				WaterSalesAgreement wsa = contract.GetWaterSalesAgreement (UoWGeneric.Root.DeliveryPoint);
				if (wsa == null) {	
					//Если нет доп. соглашения продажи воды.
					if (MessageDialogWorks.RunQuestionDialog ("Отсутствует доп. соглашение с клиентом для продажи воды. Создать?")) {
						ITdiDialog dlg = new AdditionalAgreementWater (CounterpartyContractRepository.GetCounterpartyContractByPaymentType (UoWGeneric, UoWGeneric.Root.Client, UoWGeneric.Root.PaymentType), UoWGeneric.Root.DeliveryDate);
						(dlg as IAgreementSaved).AgreementSaved += AgreementSaved;
						TabParent.AddSlaveTab (this, dlg);
					} else
						return;
				} else {
					UoWGeneric.Root.AddWaterForSale (nomenclature, wsa);
					UoWGeneric.Root.RecalcBottlesDeposits (UoWGeneric);
				}
			}
			UpdateSum ();
		}

		private void AddRentAgreement (OrderAgreementType type)
		{
			ITdiDialog dlg;

			if (UoWGeneric.Root.Client == null || UoWGeneric.Root.DeliveryPoint == null) {
				MessageDialogWorks.RunWarningDialog ("Для добавления оборудования должна быть выбрана точка доставки.");
				return;
			}
			CounterpartyContract contract = CounterpartyContractRepository.GetCounterpartyContractByPaymentType (UoWGeneric, UoWGeneric.Root.Client, UoWGeneric.Root.PaymentType);
			if (contract == null) {
				RunContractCreateDialog ();
				return;
			} 
			switch (type) {
			case OrderAgreementType.NonfreeRent:
				dlg = new AdditionalAgreementNonFreeRent (contract, UoWGeneric.Root.DeliveryPoint, UoWGeneric.Root.DeliveryDate);
				break;
			case OrderAgreementType.DailyRent:
				dlg = new AdditionalAgreementDailyRent (contract, UoWGeneric.Root.DeliveryPoint, UoWGeneric.Root.DeliveryDate);
				break;
			default: 
				dlg = new AdditionalAgreementFreeRent (contract, UoWGeneric.Root.DeliveryPoint, UoWGeneric.Root.DeliveryDate);
				break;
			}
			(dlg as IAgreementSaved).AgreementSaved += AgreementSaved;
			TabParent.AddSlaveTab (this, dlg);
		}

		void AgreementSaved (object sender, AgreementSavedEventArgs e)
		{
			UoWGeneric.Root.ObservableOrderDocuments.Add (new OrderAgreement { 
				Order = UoWGeneric.Root,
				AdditionalAgreement = e.Agreement
			});
			UoWGeneric.Root.FillItemsFromAgreement (e.Agreement);
			UpdateSum ();
			CounterpartyContractRepository.GetCounterpartyContractByPaymentType (UoWGeneric, UoWGeneric.Root.Client, UoWGeneric.Root.PaymentType).AdditionalAgreements.Add (e.Agreement);
		}

		void UpdateSum ()
		{
			Decimal sum = UoWGeneric.Root.TotalSum;
			labelSum.Text = CurrencyWorks.GetShortCurrencyString (sum);
			UoWGeneric.Root.SumToReceive = sum + (Decimal)spinSumDifference.Value;
			labelSumTotal.Text = CurrencyWorks.GetShortCurrencyString (UoWGeneric.Root.SumToReceive);
		}

		protected void OnButtonViewDocumentClicked (object sender, EventArgs e)
		{
			if (treeDocuments.GetSelectedObjects ().GetLength (0) > 0) {
				ITdiDialog dlg = null;
				if (treeDocuments.GetSelectedObjects () [0] is OrderAgreement) {
					var agreement = (treeDocuments.GetSelectedObjects () [0] as OrderAgreement).AdditionalAgreement;
					dlg = OrmMain.CreateObjectDialog (agreement);
				} else if (treeDocuments.GetSelectedObjects () [0] is OrderContract) {
					var contract = (treeDocuments.GetSelectedObjects () [0] as OrderContract).Contract;
					dlg = OrmMain.CreateObjectDialog (contract);
				}

				if (dlg != null) {
					(dlg as IEditableDialog).IsEditable = false;
					TabParent.AddSlaveTab (this, dlg);
				}
			}
		}

		protected void OnButtonFillCommentClicked (object sender, EventArgs e)
		{
			OrmReference SelectDialog = new OrmReference (typeof(CommentTemplate), UoWGeneric);
			SelectDialog.Mode = OrmReferenceMode.Select;
			SelectDialog.ButtonMode = ReferenceButtonMode.CanAdd;
			SelectDialog.ObjectSelected += (s, ea) => {
				if (ea.Subject != null) {
					UoWGeneric.Root.Comment = (ea.Subject as CommentTemplate).Comment;
				}
			};
			TabParent.AddSlaveTab (this, SelectDialog);
		}

		protected void OnSpinSumDifferenceValueChanged (object sender, EventArgs e)
		{
			UpdateSum ();
			string text;
			if (spinSumDifference.Value > 0)
				text = "Сумма <b>преплаты</b>/недоплаты:";
			else if (spinSumDifference.Value < 0)
				text = "Сумма преплаты/<b>недоплаты</b>:";
			else
				text = "Сумма преплаты/недоплаты:";
			labelSumDifference.Markup = text;
		}

		void RunContractCreateDialog ()
		{
			ITdiTab dlg;
			string question = "Отсутствует договор с клиентом для " +
			                  (UoWGeneric.Root.PaymentType == PaymentType.cash ? "наличной" : "безналичной") +
			                  " формы оплаты. Создать?";
			if (MessageDialogWorks.RunQuestionDialog (question)) {
				dlg = new CounterpartyContractDlg (UoWGeneric.Root.Client, 
					(UoWGeneric.Root.PaymentType == PaymentType.cash ?
						OrganizationRepository.GetCashOrganization (UoWGeneric) :
						OrganizationRepository.GetCashlessOrganization (UoWGeneric)));
				(dlg as IContractSaved).ContractSaved += (sender, e) => {
					UoWGeneric.Root.ObservableOrderDocuments.Add (new OrderContract { 
						Order = UoWGeneric.Root,
						Contract = e.Contract
					});
				};
				TabParent.AddSlaveTab (this, dlg);
			}
		}

		protected void OnSpinBottlesReturnValueChanged (object sender, EventArgs e)
		{
			UoWGeneric.Root.RecalcBottlesDeposits (UoWGeneric);
		}

		protected void OnButtonAcceptClicked (object sender, EventArgs e)
		{
			if (UoWGeneric.Root.OrderStatus == OrderStatus.NewOrder) {
				var valid = new QSValidator<Order> (UoWGeneric.Root, 
					            new Dictionary<object, object> {
						{ "NewStatus", OrderStatus.Accepted }
					});
				if (valid.RunDlgIfNotValid ((Window)this.Toplevel))
					return;

				if (UoWGeneric.Root.BottlesReturn == 0 && Entity.OrderItems.Any (i => i.Nomenclature.Category == NomenclatureCategory.water)) {
					if (!MessageDialogWorks.RunQuestionDialog ("Указано нулевое количество бутылей на возврат. Вы действительно хотите продолжить?"))
						return;
				}
				
				UoWGeneric.Root.OrderStatus = OrderStatus.Accepted;
				IsEditable ();
				var icon = new Image ();
				icon.Pixbuf = Stetic.IconLoader.LoadIcon (this, "gtk-edit", IconSize.Menu);
				buttonAccept.Image = icon;
				buttonAccept.Label = "Редактировать";
				return;
			}
			if (UoWGeneric.Root.OrderStatus == OrderStatus.Accepted) {
				UoWGeneric.Root.OrderStatus = OrderStatus.NewOrder;
				IsEditable (true);
				var icon = new Image ();
				icon.Pixbuf = Stetic.IconLoader.LoadIcon (this, "gtk-edit", IconSize.Menu);
				buttonAccept.Image = icon;
				buttonAccept.Label = "Подтвердить";
				return;
			}
		}

		protected void OnEnumSignatureTypeChanged (object sender, EventArgs e)
		{
			UpdateProxyInfo ();
		}

		void UpdateProxyInfo ()
		{
			labelProxyInfo.Visible = Entity.SignatureType == OrderSignatureType.ByProxy;
			if (Entity.SignatureType != OrderSignatureType.ByProxy)
				return;
			DBWorks.SQLHelper text = new DBWorks.SQLHelper ("");
			if (Entity.Client != null) {
				var proxies = Entity.Client.Proxies.Where (p => p.IsActiveProxy (Entity.DeliveryDate) && (p.DeliveryPoint == null || p.DeliveryPoint == Entity.DeliveryPoint));
				foreach (var proxy in proxies) {
					if (!String.IsNullOrWhiteSpace (text.Text))
						text.Add ("\n");
					text.Add (String.Format ("Доверенность{2} №{0} от {1:d}", proxy.Number, proxy.IssueDate, 
						proxy.DeliveryPoint == null ? "(общая)" : ""));
					text.StartNewList (": ");
					foreach (var pers in proxy.Persons) {
						text.AddAsList (pers.NameWithInitials);
					}
				}
			}
			if (String.IsNullOrWhiteSpace (text.Text))
				labelProxyInfo.Markup = "<span foreground=\"red\">Нет активной доверенности</span>";
			else
				labelProxyInfo.LabelProp = text.Text;
		}

		protected void OnReferenceDeliveryPointChanged (object sender, EventArgs e)
		{
			UpdateProxyInfo ();
		}

		protected void OnButtonPrintEnumItemClicked (object sender, EnumItemClickedEventArgs e)
		{
			QSReport.ReportViewDlg report = null;
			QSReport.ReportInfo reportInfo = null;
			PrintDocuments selected = (PrintDocuments)e.ItemEnum;

			switch (selected) {
			case PrintDocuments.Bill:
				reportInfo = new QSReport.ReportInfo {
					Title = String.Format ("Счет №{0} от {1:d}", Entity.Id, Entity.DeliveryDate),
					Identifier = "Bill",
					Parameters = new Dictionary<string, object> {
						{ "order_id",  Entity.Id },
						{ "organization_id", OrganizationRepository.CashlessOrganizationId },
						{ "hide_signature", false }
					}
				};
				break;
			case PrintDocuments.BillWithoutSignature:
				reportInfo = new QSReport.ReportInfo {
					Title = String.Format ("Счет №{0} от {1:d} (без печати и подписи)", Entity.Id, Entity.DeliveryDate),
					Identifier = "Bill",
					Parameters = new Dictionary<string, object> {
						{ "order_id",  Entity.Id },
						{ "organization_id", OrganizationRepository.CashlessOrganizationId },
						{ "hide_signature", true }
					}
				};
				break;
			case PrintDocuments.DoneWorkReport:
				throw new InvalidOperationException (String.Format ("Тип документа еще не поддерживается: {0}", selected));
			case PrintDocuments.EquipmentTransfer:
				throw new InvalidOperationException (String.Format ("Тип документа еще не поддерживается: {0}", selected));
			case PrintDocuments.Invoice:
				reportInfo = new QSReport.ReportInfo {
					Title = String.Format ("Накладная №{0} от {1:d}", Entity.Id, Entity.DeliveryDate),
					Identifier = "Invoice",
					Parameters = new Dictionary<string, object> {
						{ "order_id",  Entity.Id }
					}
				};
				break;
			case PrintDocuments.InvoiceBarter:
				reportInfo = new QSReport.ReportInfo {
					Title = String.Format ("Накладная №{0} от {1:d} (безденежно)", Entity.Id, Entity.DeliveryDate),
					Identifier = "InvoiceBarter",
					Parameters = new Dictionary<string, object> {
						{ "order_id",  Entity.Id }
					}
				};
				break;
			case PrintDocuments.UPD:
				reportInfo = new QSReport.ReportInfo {
					Title = String.Format ("УПД {0} от {1:d}", Entity.Id, Entity.DeliveryDate),
					Identifier = "UPD",
					Parameters = new Dictionary<string, object> {
						{ "order_id", Entity.Id }
					}
				};
				break;
			default:
				throw new InvalidOperationException (String.Format ("Тип документа еще не поддерживается: {0}", selected));
			}
			if (UoWGeneric.HasChanges && CommonDialogs.SaveBeforePrint (typeof(Order), selected.GetEnumTitle ()))
				UoWGeneric.Save ();
			
			if (reportInfo != null) {
				report = new QSReport.ReportViewDlg (reportInfo);
				TabParent.AddTab (report, this, false);
			}
		}

		protected void OnButtonAddServiceClaimClicked (object sender, EventArgs e)
		{
			var dlg = new ServiceClaimDlg (UoWGeneric.Root);
			TabParent.AddSlaveTab (this, dlg);
		}

		protected void OnTreeServiceClaimRowActivated (object o, RowActivatedArgs args)
		{
			ITdiTab mytab = TdiHelper.FindMyTab (this);
			if (mytab == null)
				return;

			ServiceClaimDlg dlg = new ServiceClaimDlg ((treeServiceClaim.GetSelectedObjects () [0] as ServiceClaim).Id);
			mytab.TabParent.AddSlaveTab (mytab, dlg);
		}

		protected void OnButtonAddDoneServiceClicked (object sender, EventArgs e)
		{
			OrmReference SelectDialog = new OrmReference (typeof(ServiceClaim), UoWGeneric, 
				                            ServiceClaimRepository.GetDoneClaimsForClient (UoWGeneric.Root.Client)
				.GetExecutableQueryOver (UoWGeneric.Session).RootCriteria);
			SelectDialog.Mode = OrmReferenceMode.Select;
			SelectDialog.ButtonMode = ReferenceButtonMode.CanAdd;
			SelectDialog.ObjectSelected += DoneServiceSelected;
			;
			TabParent.AddSlaveTab (this, SelectDialog);
		}

		void DoneServiceSelected (object sender, OrmReferenceObjectSectedEventArgs e)
		{
			ServiceClaim selected = (e.Subject as ServiceClaim);
			selected.FinalOrder = UoWGeneric.Root;
			UoWGeneric.Root.ObservableFinalOrderService.Add (selected);
		}
	}

	public enum PrintDocuments
	{
		[ItemTitleAttribute ("Счет")]
		Bill,
		[ItemTitleAttribute ("Счет (Без печати и подписи)")]
		BillWithoutSignature,
		[ItemTitleAttribute ("Акт выполненных работ")]
		DoneWorkReport,
		[ItemTitleAttribute ("Акт приема-передачи оборудования")]
		EquipmentTransfer,
		[ItemTitleAttribute ("Накладная (нал.)")]
		Invoice,
		[ItemTitleAttribute ("Накладная (безденежно)")]
		InvoiceBarter,
		[ItemTitleAttribute ("УПД")]
		UPD
	}
}