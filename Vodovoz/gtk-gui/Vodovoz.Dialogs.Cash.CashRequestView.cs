
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Dialogs.Cash
{
	public partial class CashRequestView
	{
		private global::Gtk.VBox vboxDialog;

		private global::Gtk.HBox hboxDialogButtons;

		private global::Gamma.GtkWidgets.yButton buttonSave;

		private global::Gamma.GtkWidgets.yButton buttonCancel;

		private global::Gamma.GtkWidgets.yLabel ylabelyourRoleIs;

		private global::Gamma.Widgets.yListComboBox comboRoleChooser;

		private global::Gamma.GtkWidgets.yLabel labelStatus;

		private global::Gamma.GtkWidgets.yLabel ylabelStatus;

		private global::Gtk.ScrolledWindow scrolledwindow1;

		private global::Gtk.Table table1;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry AuthorEntityviewmodelentry;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry ExpenseCategoryEntityviewmodelentry;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gamma.GtkWidgets.yTreeView ytreeviewSums;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gamma.GtkWidgets.yTextView yentryReasonForSendToReapproval;

		private global::Gtk.ScrolledWindow GtkScrolledWindow2;

		private global::Gamma.GtkWidgets.yTextView yentryCancelReason;

		private global::Gtk.ScrolledWindow GtkScrolledWindow3;

		private global::Gamma.GtkWidgets.yTextView yentryGround;

		private global::Gtk.HBox hbox4;

		private global::Gamma.GtkWidgets.yButton ybtnAccept;

		private global::Gamma.GtkWidgets.yButton ybtnApprove;

		private global::Gamma.GtkWidgets.yButton ybtnConveyForResults;

		private global::Gamma.GtkWidgets.yButton ybtnCancel;

		private global::Gamma.GtkWidgets.yButton ybtnReturnForRenegotiation;

		private global::Gtk.HBox hbox6;

		private global::Gtk.HBox hbox7;

		private global::Gamma.GtkWidgets.yButton ybtnAddSumm;

		private global::Gamma.GtkWidgets.yButton ybtnEditSum;

		private global::Gamma.GtkWidgets.yButton ybtnDeleteSumm;

		private global::Gamma.GtkWidgets.yButton ybtnGiveSumm;

		private global::Gtk.HBox hboxGivePartially;

		private global::Gamma.GtkWidgets.yButton ybtnGiveSummPartially;

		private global::Gamma.GtkWidgets.ySpinButton yspinGivePartially;

		private global::Gtk.HSeparator hseparator1;

		private global::Gtk.HSeparator hseparator2;

		private global::Gtk.HSeparator hseparator3;

		private global::Gamma.GtkWidgets.yLabel labelBalansOrganizations;

		private global::Gtk.Label labelCancelReason;

		private global::Gamma.GtkWidgets.yLabel labelCategoryEntityviewmodelentry;

		private global::Gamma.GtkWidgets.yLabel labelcomboOrganization;

		private global::Gtk.Label labelExplanation;

		private global::Gtk.Label labelGround;

		private global::Gtk.Label labelHaveReceipt;

		private global::Gtk.Label labelReasonForSendToReapproval;

		private global::Gtk.Label labelType5;

		private global::Gtk.Label labelType6;

		private global::QS.Widgets.GtkUI.SpecialListComboBox speccomboOrganization;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry SubdivisionEntityviewmodelentry;

		private global::Gamma.GtkWidgets.yCheckButton ycheckHaveReceipt;

		private global::Gamma.GtkWidgets.yCheckButton ycheckPossibilityNotToReconcilePayments;

		private global::Gamma.GtkWidgets.yEntry yentryExplanation;

		private global::Gamma.GtkWidgets.yLabel ylabelBalansOrganizations;

		private global::Gamma.GtkWidgets.yLabel ylabelPossibilityNotToReconcilePayments;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.Dialogs.Cash.CashRequestView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.Dialogs.Cash.CashRequestView";
			// Container child Vodovoz.Dialogs.Cash.CashRequestView.Gtk.Container+ContainerChild
			this.vboxDialog = new global::Gtk.VBox();
			this.vboxDialog.Name = "vboxDialog";
			this.vboxDialog.Spacing = 6;
			// Container child vboxDialog.Gtk.Box+BoxChild
			this.hboxDialogButtons = new global::Gtk.HBox();
			this.hboxDialogButtons.Name = "hboxDialogButtons";
			this.hboxDialogButtons.Spacing = 6;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.buttonSave = new global::Gamma.GtkWidgets.yButton();
			this.buttonSave.CanFocus = true;
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.UseUnderline = true;
			this.buttonSave.Label = global::Mono.Unix.Catalog.GetString("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-save", global::Gtk.IconSize.Menu);
			this.buttonSave.Image = w1;
			this.hboxDialogButtons.Add(this.buttonSave);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.buttonSave]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.buttonCancel = new global::Gamma.GtkWidgets.yButton();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString("Отменить");
			global::Gtk.Image w3 = new global::Gtk.Image();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w3;
			this.hboxDialogButtons.Add(this.buttonCancel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.buttonCancel]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.ylabelyourRoleIs = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelyourRoleIs.Name = "ylabelyourRoleIs";
			this.ylabelyourRoleIs.LabelProp = global::Mono.Unix.Catalog.GetString("Роль:");
			this.hboxDialogButtons.Add(this.ylabelyourRoleIs);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.ylabelyourRoleIs]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
            // Container child hboxDialogButtons.Gtk.Box+BoxChild
            this.comboRoleChooser = new Gamma.Widgets.yListComboBox();
			this.comboRoleChooser.Name = "comboRoleChooser";
			this.comboRoleChooser.AddIfNotExist = false;
			this.comboRoleChooser.DefaultFirst = false;
			this.hboxDialogButtons.Add(this.comboRoleChooser);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.comboRoleChooser]));
			w6.Position = 3;
			w6.Expand = false;
			w6.Fill = false;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.labelStatus = new global::Gamma.GtkWidgets.yLabel();
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.LabelProp = global::Mono.Unix.Catalog.GetString("Статус заявки:");
			this.hboxDialogButtons.Add(this.labelStatus);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.labelStatus]));
			w7.Position = 4;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.ylabelStatus = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelStatus.Name = "ylabelStatus";
			this.ylabelStatus.LabelProp = global::Mono.Unix.Catalog.GetString(".");
			this.hboxDialogButtons.Add(this.ylabelStatus);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.ylabelStatus]));
			w8.Position = 5;
			w8.Expand = false;
			w8.Fill = false;
			this.vboxDialog.Add(this.hboxDialogButtons);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vboxDialog[this.hboxDialogButtons]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child vboxDialog.Gtk.Box+BoxChild
			this.scrolledwindow1 = new global::Gtk.ScrolledWindow();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow1.Gtk.Container+ContainerChild
			global::Gtk.Viewport w10 = new global::Gtk.Viewport();
			w10.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.table1 = new global::Gtk.Table(((uint)(19)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.AuthorEntityviewmodelentry = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.AuthorEntityviewmodelentry.Events = ((global::Gdk.EventMask)(256));
			this.AuthorEntityviewmodelentry.Name = "AuthorEntityviewmodelentry";
			this.AuthorEntityviewmodelentry.CanEditReference = false;
			this.table1.Add(this.AuthorEntityviewmodelentry);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table1[this.AuthorEntityviewmodelentry]));
			w11.TopAttach = ((uint)(1));
			w11.BottomAttach = ((uint)(2));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ExpenseCategoryEntityviewmodelentry = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.ExpenseCategoryEntityviewmodelentry.Events = ((global::Gdk.EventMask)(256));
			this.ExpenseCategoryEntityviewmodelentry.Name = "ExpenseCategoryEntityviewmodelentry";
			this.ExpenseCategoryEntityviewmodelentry.CanEditReference = false;
			this.table1.Add(this.ExpenseCategoryEntityviewmodelentry);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1[this.ExpenseCategoryEntityviewmodelentry]));
			w12.TopAttach = ((uint)(13));
			w12.BottomAttach = ((uint)(14));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.ytreeviewSums = new global::Gamma.GtkWidgets.yTreeView();
			this.ytreeviewSums.CanFocus = true;
			this.ytreeviewSums.Name = "ytreeviewSums";
			this.GtkScrolledWindow.Add(this.ytreeviewSums);
			this.table1.Add(this.GtkScrolledWindow);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table1[this.GtkScrolledWindow]));
			w14.TopAttach = ((uint)(3));
			w14.BottomAttach = ((uint)(5));
			w14.RightAttach = ((uint)(2));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.yentryReasonForSendToReapproval = new global::Gamma.GtkWidgets.yTextView();
			this.yentryReasonForSendToReapproval.CanFocus = true;
			this.yentryReasonForSendToReapproval.Name = "yentryReasonForSendToReapproval";
			this.GtkScrolledWindow1.Add(this.yentryReasonForSendToReapproval);
			this.table1.Add(this.GtkScrolledWindow1);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table1[this.GtkScrolledWindow1]));
			w16.TopAttach = ((uint)(15));
			w16.BottomAttach = ((uint)(16));
			w16.LeftAttach = ((uint)(1));
			w16.RightAttach = ((uint)(2));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
			this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
			this.yentryCancelReason = new global::Gamma.GtkWidgets.yTextView();
			this.yentryCancelReason.CanFocus = true;
			this.yentryCancelReason.Name = "yentryCancelReason";
			this.GtkScrolledWindow2.Add(this.yentryCancelReason);
			this.table1.Add(this.GtkScrolledWindow2);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table1[this.GtkScrolledWindow2]));
			w18.TopAttach = ((uint)(16));
			w18.BottomAttach = ((uint)(17));
			w18.LeftAttach = ((uint)(1));
			w18.RightAttach = ((uint)(2));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.GtkScrolledWindow3 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow3.Name = "GtkScrolledWindow3";
			this.GtkScrolledWindow3.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow3.Gtk.Container+ContainerChild
			this.yentryGround = new global::Gamma.GtkWidgets.yTextView();
			this.yentryGround.CanFocus = true;
			this.yentryGround.Name = "yentryGround";
			this.GtkScrolledWindow3.Add(this.yentryGround);
			this.table1.Add(this.GtkScrolledWindow3);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table1[this.GtkScrolledWindow3]));
			w20.TopAttach = ((uint)(6));
			w20.BottomAttach = ((uint)(7));
			w20.LeftAttach = ((uint)(1));
			w20.RightAttach = ((uint)(2));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox4 = new global::Gtk.HBox();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.ybtnAccept = new global::Gamma.GtkWidgets.yButton();
			this.ybtnAccept.CanFocus = true;
			this.ybtnAccept.Name = "ybtnAccept";
			this.ybtnAccept.UseUnderline = true;
			this.ybtnAccept.Label = global::Mono.Unix.Catalog.GetString("Подтвердить");
			this.hbox4.Add(this.ybtnAccept);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.ybtnAccept]));
			w21.Position = 0;
			w21.Expand = false;
			w21.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.ybtnApprove = new global::Gamma.GtkWidgets.yButton();
			this.ybtnApprove.CanFocus = true;
			this.ybtnApprove.Name = "ybtnApprove";
			this.ybtnApprove.UseUnderline = true;
			this.ybtnApprove.Label = global::Mono.Unix.Catalog.GetString("Согласовать");
			this.hbox4.Add(this.ybtnApprove);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.ybtnApprove]));
			w22.Position = 1;
			w22.Expand = false;
			w22.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.ybtnConveyForResults = new global::Gamma.GtkWidgets.yButton();
			this.ybtnConveyForResults.CanFocus = true;
			this.ybtnConveyForResults.Name = "ybtnConveyForResults";
			this.ybtnConveyForResults.UseUnderline = true;
			this.ybtnConveyForResults.Label = global::Mono.Unix.Catalog.GetString("Передать на выдачу");
			this.hbox4.Add(this.ybtnConveyForResults);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.ybtnConveyForResults]));
			w23.Position = 2;
			w23.Expand = false;
			w23.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.ybtnCancel = new global::Gamma.GtkWidgets.yButton();
			this.ybtnCancel.CanFocus = true;
			this.ybtnCancel.Name = "ybtnCancel";
			this.ybtnCancel.UseUnderline = true;
			this.ybtnCancel.Label = global::Mono.Unix.Catalog.GetString("Отменить заявку");
			this.hbox4.Add(this.ybtnCancel);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.ybtnCancel]));
			w24.Position = 3;
			w24.Expand = false;
			w24.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.ybtnReturnForRenegotiation = new global::Gamma.GtkWidgets.yButton();
			this.ybtnReturnForRenegotiation.CanFocus = true;
			this.ybtnReturnForRenegotiation.Name = "ybtnReturnForRenegotiation";
			this.ybtnReturnForRenegotiation.UseUnderline = true;
			this.ybtnReturnForRenegotiation.Label = global::Mono.Unix.Catalog.GetString("Отправить на пересогласование");
			this.hbox4.Add(this.ybtnReturnForRenegotiation);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.ybtnReturnForRenegotiation]));
			w25.Position = 4;
			w25.Expand = false;
			w25.Fill = false;
			this.table1.Add(this.hbox4);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox4]));
			w26.TopAttach = ((uint)(18));
			w26.BottomAttach = ((uint)(19));
			w26.RightAttach = ((uint)(2));
			w26.XOptions = ((global::Gtk.AttachOptions)(0));
			w26.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox6 = new global::Gtk.HBox();
			this.hbox6.Name = "hbox6";
			this.hbox6.Spacing = 6;
			this.table1.Add(this.hbox6);
			global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox6]));
			w27.TopAttach = ((uint)(3));
			w27.BottomAttach = ((uint)(4));
			w27.XOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox7 = new global::Gtk.HBox();
			this.hbox7.Name = "hbox7";
			this.hbox7.Spacing = 6;
			// Container child hbox7.Gtk.Box+BoxChild
			this.ybtnAddSumm = new global::Gamma.GtkWidgets.yButton();
			this.ybtnAddSumm.CanFocus = true;
			this.ybtnAddSumm.Name = "ybtnAddSumm";
			this.ybtnAddSumm.UseUnderline = true;
			this.ybtnAddSumm.Label = global::Mono.Unix.Catalog.GetString("Добавить");
			this.hbox7.Add(this.ybtnAddSumm);
			global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.ybtnAddSumm]));
			w28.Position = 0;
			w28.Expand = false;
			w28.Fill = false;
			// Container child hbox7.Gtk.Box+BoxChild
			this.ybtnEditSum = new global::Gamma.GtkWidgets.yButton();
			this.ybtnEditSum.CanFocus = true;
			this.ybtnEditSum.Name = "ybtnEditSum";
			this.ybtnEditSum.UseUnderline = true;
			this.ybtnEditSum.Label = global::Mono.Unix.Catalog.GetString("Редактировать");
			this.hbox7.Add(this.ybtnEditSum);
			global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.ybtnEditSum]));
			w29.Position = 1;
			w29.Expand = false;
			w29.Fill = false;
			// Container child hbox7.Gtk.Box+BoxChild
			this.ybtnDeleteSumm = new global::Gamma.GtkWidgets.yButton();
			this.ybtnDeleteSumm.CanFocus = true;
			this.ybtnDeleteSumm.Name = "ybtnDeleteSumm";
			this.ybtnDeleteSumm.UseUnderline = true;
			this.ybtnDeleteSumm.Label = global::Mono.Unix.Catalog.GetString("Удалить");
			this.hbox7.Add(this.ybtnDeleteSumm);
			global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.ybtnDeleteSumm]));
			w30.Position = 2;
			w30.Expand = false;
			w30.Fill = false;
			// Container child hbox7.Gtk.Box+BoxChild
			this.ybtnGiveSumm = new global::Gamma.GtkWidgets.yButton();
			this.ybtnGiveSumm.CanFocus = true;
			this.ybtnGiveSumm.Name = "ybtnGiveSumm";
			this.ybtnGiveSumm.UseUnderline = true;
			this.ybtnGiveSumm.Label = global::Mono.Unix.Catalog.GetString("Выдать");
			this.hbox7.Add(this.ybtnGiveSumm);
			global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.ybtnGiveSumm]));
			w31.Position = 3;
			w31.Expand = false;
			w31.Fill = false;
			this.table1.Add(this.hbox7);
			global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox7]));
			w32.TopAttach = ((uint)(5));
			w32.BottomAttach = ((uint)(6));
			w32.XOptions = ((global::Gtk.AttachOptions)(4));
			w32.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hboxGivePartially = new global::Gtk.HBox();
			this.hboxGivePartially.Name = "hboxGivePartially";
			this.hboxGivePartially.Spacing = 6;
			// Container child hboxGivePartially.Gtk.Box+BoxChild
			this.ybtnGiveSummPartially = new global::Gamma.GtkWidgets.yButton();
			this.ybtnGiveSummPartially.CanFocus = true;
			this.ybtnGiveSummPartially.Name = "ybtnGiveSummPartially";
			this.ybtnGiveSummPartially.UseUnderline = true;
			this.ybtnGiveSummPartially.Label = global::Mono.Unix.Catalog.GetString("Выдать частично");
			this.hboxGivePartially.Add(this.ybtnGiveSummPartially);
			global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.hboxGivePartially[this.ybtnGiveSummPartially]));
			w33.Position = 0;
			w33.Expand = false;
			w33.Fill = false;
			// Container child hboxGivePartially.Gtk.Box+BoxChild
			this.yspinGivePartially = new global::Gamma.GtkWidgets.ySpinButton(-100000D, 100000D, 1D);
			this.yspinGivePartially.CanFocus = true;
			this.yspinGivePartially.Name = "yspinGivePartially";
			this.yspinGivePartially.Adjustment.PageIncrement = 10D;
			this.yspinGivePartially.ClimbRate = 1D;
			this.yspinGivePartially.Numeric = true;
			this.yspinGivePartially.ValueAsDecimal = 0m;
			this.yspinGivePartially.ValueAsInt = 0;
			this.hboxGivePartially.Add(this.yspinGivePartially);
			global::Gtk.Box.BoxChild w34 = ((global::Gtk.Box.BoxChild)(this.hboxGivePartially[this.yspinGivePartially]));
			w34.Position = 1;
			w34.Expand = false;
			w34.Fill = false;
			this.table1.Add(this.hboxGivePartially);
			global::Gtk.Table.TableChild w35 = ((global::Gtk.Table.TableChild)(this.table1[this.hboxGivePartially]));
			w35.TopAttach = ((uint)(5));
			w35.BottomAttach = ((uint)(6));
			w35.LeftAttach = ((uint)(1));
			w35.RightAttach = ((uint)(2));
			w35.XOptions = ((global::Gtk.AttachOptions)(4));
			w35.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hseparator1 = new global::Gtk.HSeparator();
			this.hseparator1.Name = "hseparator1";
			this.table1.Add(this.hseparator1);
			global::Gtk.Table.TableChild w36 = ((global::Gtk.Table.TableChild)(this.table1[this.hseparator1]));
			w36.TopAttach = ((uint)(10));
			w36.BottomAttach = ((uint)(11));
			w36.RightAttach = ((uint)(2));
			w36.XOptions = ((global::Gtk.AttachOptions)(4));
			w36.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hseparator2 = new global::Gtk.HSeparator();
			this.hseparator2.Name = "hseparator2";
			this.table1.Add(this.hseparator2);
			global::Gtk.Table.TableChild w37 = ((global::Gtk.Table.TableChild)(this.table1[this.hseparator2]));
			w37.TopAttach = ((uint)(14));
			w37.BottomAttach = ((uint)(15));
			w37.RightAttach = ((uint)(2));
			w37.XOptions = ((global::Gtk.AttachOptions)(4));
			w37.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hseparator3 = new global::Gtk.HSeparator();
			this.hseparator3.Name = "hseparator3";
			this.table1.Add(this.hseparator3);
			global::Gtk.Table.TableChild w38 = ((global::Gtk.Table.TableChild)(this.table1[this.hseparator3]));
			w38.TopAttach = ((uint)(17));
			w38.BottomAttach = ((uint)(18));
			w38.RightAttach = ((uint)(2));
			w38.XOptions = ((global::Gtk.AttachOptions)(4));
			w38.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelBalansOrganizations = new global::Gamma.GtkWidgets.yLabel();
			this.labelBalansOrganizations.Name = "labelBalansOrganizations";
			this.labelBalansOrganizations.Xalign = 0F;
			this.labelBalansOrganizations.LabelProp = global::Mono.Unix.Catalog.GetString("Баланс по \nнашим \nорганизациям:");
			this.table1.Add(this.labelBalansOrganizations);
			global::Gtk.Table.TableChild w39 = ((global::Gtk.Table.TableChild)(this.table1[this.labelBalansOrganizations]));
			w39.TopAttach = ((uint)(11));
			w39.BottomAttach = ((uint)(12));
			w39.XOptions = ((global::Gtk.AttachOptions)(4));
			w39.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelCancelReason = new global::Gtk.Label();
			this.labelCancelReason.Name = "labelCancelReason";
			this.labelCancelReason.Xalign = 0F;
			this.labelCancelReason.LabelProp = global::Mono.Unix.Catalog.GetString("Причина отмены:");
			this.table1.Add(this.labelCancelReason);
			global::Gtk.Table.TableChild w40 = ((global::Gtk.Table.TableChild)(this.table1[this.labelCancelReason]));
			w40.TopAttach = ((uint)(16));
			w40.BottomAttach = ((uint)(17));
			w40.XOptions = ((global::Gtk.AttachOptions)(4));
			w40.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelCategoryEntityviewmodelentry = new global::Gamma.GtkWidgets.yLabel();
			this.labelCategoryEntityviewmodelentry.Name = "labelCategoryEntityviewmodelentry";
			this.labelCategoryEntityviewmodelentry.Xalign = 0F;
			this.labelCategoryEntityviewmodelentry.LabelProp = global::Mono.Unix.Catalog.GetString("Статья расхода:");
			this.table1.Add(this.labelCategoryEntityviewmodelentry);
			global::Gtk.Table.TableChild w41 = ((global::Gtk.Table.TableChild)(this.table1[this.labelCategoryEntityviewmodelentry]));
			w41.TopAttach = ((uint)(13));
			w41.BottomAttach = ((uint)(14));
			w41.XOptions = ((global::Gtk.AttachOptions)(4));
			w41.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelcomboOrganization = new global::Gamma.GtkWidgets.yLabel();
			this.labelcomboOrganization.Name = "labelcomboOrganization";
			this.labelcomboOrganization.Xalign = 0F;
			this.labelcomboOrganization.LabelProp = global::Mono.Unix.Catalog.GetString("Наша организация:");
			this.table1.Add(this.labelcomboOrganization);
			global::Gtk.Table.TableChild w42 = ((global::Gtk.Table.TableChild)(this.table1[this.labelcomboOrganization]));
			w42.TopAttach = ((uint)(12));
			w42.BottomAttach = ((uint)(13));
			w42.XOptions = ((global::Gtk.AttachOptions)(4));
			w42.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelExplanation = new global::Gtk.Label();
			this.labelExplanation.Name = "labelExplanation";
			this.labelExplanation.Xalign = 0F;
			this.labelExplanation.LabelProp = global::Mono.Unix.Catalog.GetString("Пояснение:");
			this.table1.Add(this.labelExplanation);
			global::Gtk.Table.TableChild w43 = ((global::Gtk.Table.TableChild)(this.table1[this.labelExplanation]));
			w43.TopAttach = ((uint)(7));
			w43.BottomAttach = ((uint)(8));
			w43.XOptions = ((global::Gtk.AttachOptions)(4));
			w43.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelGround = new global::Gtk.Label();
			this.labelGround.Name = "labelGround";
			this.labelGround.Xalign = 0F;
			this.labelGround.LabelProp = global::Mono.Unix.Catalog.GetString("Основание:");
			this.table1.Add(this.labelGround);
			global::Gtk.Table.TableChild w44 = ((global::Gtk.Table.TableChild)(this.table1[this.labelGround]));
			w44.TopAttach = ((uint)(6));
			w44.BottomAttach = ((uint)(7));
			w44.XOptions = ((global::Gtk.AttachOptions)(4));
			w44.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelHaveReceipt = new global::Gtk.Label();
			this.labelHaveReceipt.Name = "labelHaveReceipt";
			this.labelHaveReceipt.Xalign = 0F;
			this.labelHaveReceipt.LabelProp = global::Mono.Unix.Catalog.GetString("Наличие чека:");
			this.table1.Add(this.labelHaveReceipt);
			global::Gtk.Table.TableChild w45 = ((global::Gtk.Table.TableChild)(this.table1[this.labelHaveReceipt]));
			w45.TopAttach = ((uint)(8));
			w45.BottomAttach = ((uint)(9));
			w45.XOptions = ((global::Gtk.AttachOptions)(4));
			w45.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelReasonForSendToReapproval = new global::Gtk.Label();
			this.labelReasonForSendToReapproval.Name = "labelReasonForSendToReapproval";
			this.labelReasonForSendToReapproval.Xalign = 0F;
			this.labelReasonForSendToReapproval.LabelProp = global::Mono.Unix.Catalog.GetString("Причина отправки на пересогласование:");
			this.labelReasonForSendToReapproval.Wrap = true;
			this.labelReasonForSendToReapproval.WidthChars = 0;
			this.labelReasonForSendToReapproval.MaxWidthChars = 20;
			this.table1.Add(this.labelReasonForSendToReapproval);
			global::Gtk.Table.TableChild w46 = ((global::Gtk.Table.TableChild)(this.table1[this.labelReasonForSendToReapproval]));
			w46.TopAttach = ((uint)(15));
			w46.BottomAttach = ((uint)(16));
			w46.XOptions = ((global::Gtk.AttachOptions)(4));
			w46.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelType5 = new global::Gtk.Label();
			this.labelType5.Name = "labelType5";
			this.labelType5.Xalign = 0F;
			this.labelType5.LabelProp = global::Mono.Unix.Catalog.GetString("Автор:");
			this.table1.Add(this.labelType5);
			global::Gtk.Table.TableChild w47 = ((global::Gtk.Table.TableChild)(this.table1[this.labelType5]));
			w47.TopAttach = ((uint)(1));
			w47.BottomAttach = ((uint)(2));
			w47.XOptions = ((global::Gtk.AttachOptions)(4));
			w47.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelType6 = new global::Gtk.Label();
			this.labelType6.Name = "labelType6";
			this.labelType6.Xalign = 0F;
			this.labelType6.LabelProp = global::Mono.Unix.Catalog.GetString("Подразделение:");
			this.table1.Add(this.labelType6);
			global::Gtk.Table.TableChild w48 = ((global::Gtk.Table.TableChild)(this.table1[this.labelType6]));
			w48.TopAttach = ((uint)(2));
			w48.BottomAttach = ((uint)(3));
			w48.XOptions = ((global::Gtk.AttachOptions)(4));
			w48.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.speccomboOrganization = new global::QS.Widgets.GtkUI.SpecialListComboBox();
			this.speccomboOrganization.Name = "speccomboOrganization";
			this.speccomboOrganization.AddIfNotExist = false;
			this.speccomboOrganization.DefaultFirst = false;
			this.speccomboOrganization.ShowSpecialStateAll = false;
			this.speccomboOrganization.ShowSpecialStateNot = false;
			this.table1.Add(this.speccomboOrganization);
			global::Gtk.Table.TableChild w49 = ((global::Gtk.Table.TableChild)(this.table1[this.speccomboOrganization]));
			w49.TopAttach = ((uint)(12));
			w49.BottomAttach = ((uint)(13));
			w49.LeftAttach = ((uint)(1));
			w49.RightAttach = ((uint)(2));
			w49.XOptions = ((global::Gtk.AttachOptions)(4));
			w49.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.SubdivisionEntityviewmodelentry = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.SubdivisionEntityviewmodelentry.Events = ((global::Gdk.EventMask)(256));
			this.SubdivisionEntityviewmodelentry.Name = "SubdivisionEntityviewmodelentry";
			this.SubdivisionEntityviewmodelentry.CanEditReference = false;
			this.table1.Add(this.SubdivisionEntityviewmodelentry);
			global::Gtk.Table.TableChild w50 = ((global::Gtk.Table.TableChild)(this.table1[this.SubdivisionEntityviewmodelentry]));
			w50.TopAttach = ((uint)(2));
			w50.BottomAttach = ((uint)(3));
			w50.LeftAttach = ((uint)(1));
			w50.RightAttach = ((uint)(2));
			w50.XOptions = ((global::Gtk.AttachOptions)(4));
			w50.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ycheckHaveReceipt = new global::Gamma.GtkWidgets.yCheckButton();
			this.ycheckHaveReceipt.CanFocus = true;
			this.ycheckHaveReceipt.Name = "ycheckHaveReceipt";
			this.ycheckHaveReceipt.Label = "";
			this.ycheckHaveReceipt.Active = true;
			this.ycheckHaveReceipt.DrawIndicator = true;
			this.ycheckHaveReceipt.UseUnderline = true;
			this.table1.Add(this.ycheckHaveReceipt);
			global::Gtk.Table.TableChild w51 = ((global::Gtk.Table.TableChild)(this.table1[this.ycheckHaveReceipt]));
			w51.TopAttach = ((uint)(8));
			w51.BottomAttach = ((uint)(9));
			w51.LeftAttach = ((uint)(1));
			w51.RightAttach = ((uint)(2));
			w51.XOptions = ((global::Gtk.AttachOptions)(4));
			w51.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ycheckPossibilityNotToReconcilePayments = new global::Gamma.GtkWidgets.yCheckButton();
			this.ycheckPossibilityNotToReconcilePayments.CanFocus = true;
			this.ycheckPossibilityNotToReconcilePayments.Name = "ycheckPossibilityNotToReconcilePayments";
			this.ycheckPossibilityNotToReconcilePayments.Label = "";
			this.ycheckPossibilityNotToReconcilePayments.DrawIndicator = true;
			this.ycheckPossibilityNotToReconcilePayments.UseUnderline = true;
			this.table1.Add(this.ycheckPossibilityNotToReconcilePayments);
			global::Gtk.Table.TableChild w52 = ((global::Gtk.Table.TableChild)(this.table1[this.ycheckPossibilityNotToReconcilePayments]));
			w52.TopAttach = ((uint)(9));
			w52.BottomAttach = ((uint)(10));
			w52.LeftAttach = ((uint)(1));
			w52.RightAttach = ((uint)(2));
			w52.XOptions = ((global::Gtk.AttachOptions)(4));
			w52.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.yentryExplanation = new global::Gamma.GtkWidgets.yEntry();
			this.yentryExplanation.CanFocus = true;
			this.yentryExplanation.Name = "yentryExplanation";
			this.yentryExplanation.IsEditable = true;
			this.yentryExplanation.InvisibleChar = '•';
			this.table1.Add(this.yentryExplanation);
			global::Gtk.Table.TableChild w53 = ((global::Gtk.Table.TableChild)(this.table1[this.yentryExplanation]));
			w53.TopAttach = ((uint)(7));
			w53.BottomAttach = ((uint)(8));
			w53.LeftAttach = ((uint)(1));
			w53.RightAttach = ((uint)(2));
			w53.XOptions = ((global::Gtk.AttachOptions)(4));
			w53.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ylabelBalansOrganizations = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelBalansOrganizations.Name = "ylabelBalansOrganizations";
			this.ylabelBalansOrganizations.LabelProp = global::Mono.Unix.Catalog.GetString(".");
			this.table1.Add(this.ylabelBalansOrganizations);
			global::Gtk.Table.TableChild w54 = ((global::Gtk.Table.TableChild)(this.table1[this.ylabelBalansOrganizations]));
			w54.TopAttach = ((uint)(11));
			w54.BottomAttach = ((uint)(12));
			w54.LeftAttach = ((uint)(1));
			w54.RightAttach = ((uint)(2));
			w54.XOptions = ((global::Gtk.AttachOptions)(4));
			w54.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ylabelPossibilityNotToReconcilePayments = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelPossibilityNotToReconcilePayments.Name = "ylabelPossibilityNotToReconcilePayments";
			this.ylabelPossibilityNotToReconcilePayments.Xalign = 0F;
			this.ylabelPossibilityNotToReconcilePayments.LabelProp = global::Mono.Unix.Catalog.GetString("Утвердить воз-ть не пересогласовывать оплаты:");
			this.table1.Add(this.ylabelPossibilityNotToReconcilePayments);
			global::Gtk.Table.TableChild w55 = ((global::Gtk.Table.TableChild)(this.table1[this.ylabelPossibilityNotToReconcilePayments]));
			w55.TopAttach = ((uint)(9));
			w55.BottomAttach = ((uint)(10));
			w55.XOptions = ((global::Gtk.AttachOptions)(4));
			w55.YOptions = ((global::Gtk.AttachOptions)(4));
			w10.Add(this.table1);
			this.scrolledwindow1.Add(w10);
			this.vboxDialog.Add(this.scrolledwindow1);
			global::Gtk.Box.BoxChild w58 = ((global::Gtk.Box.BoxChild)(this.vboxDialog[this.scrolledwindow1]));
			w58.Position = 1;
			this.Add(this.vboxDialog);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
