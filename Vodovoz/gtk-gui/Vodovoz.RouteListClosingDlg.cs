
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz
{
	public partial class RouteListClosingDlg
	{
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.HBox hbox8;
		
		private global::Gtk.Button buttonSave;
		
		private global::Gtk.Button buttonCancel;
		
		private global::Gtk.Button buttonPrint;
		
		private global::QSOrmProject.EnumMenuButton enummenuRLActions;
		
		private global::Gtk.Expander expander1;
		
		private global::Gtk.Table table1;
		
		private global::Gamma.Widgets.yDatePicker datePickerDate;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow2;
		
		private global::Gamma.GtkWidgets.yTextView ytextClosingComment;
		
		private global::Gtk.HBox hbox12;
		
		private global::Gtk.Button buttonAddTicket;
		
		private global::Gtk.Button buttonDeleteTicket;
		
		private global::Gtk.Button buttonGetDistFromTrack;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.Label label10;
		
		private global::Gtk.Label label2;
		
		private global::Gtk.Label label3;
		
		private global::Gtk.Label label4;
		
		private global::Gtk.Label label5;
		
		private global::Gtk.Label label6;
		
		private global::Gtk.Label label9;
		
		private global::Gamma.Widgets.yEntryReference referenceCar;
		
		private global::Gamma.Widgets.yEntryReference referenceDriver;
		
		private global::Gamma.Widgets.yEntryReference referenceForwarder;
		
		private global::Gamma.Widgets.yEntryReference referenceLogistican;
		
		private global::Gamma.Widgets.ySpecComboBox speccomboShift;
		
		private global::Gamma.GtkWidgets.yCheckButton ycheckHideCells;
		
		private global::Gamma.GtkWidgets.ySpinButton yspinActualDistance;
		
		private global::Gamma.GtkWidgets.yTextView ytextviewFuelInfo;
		
		private global::Gtk.Label GtkLabel17;
		
		private global::Gtk.HBox hbox6;
		
		private global::Vodovoz.RouteListClosingItemsView routeListAddressesView;
		
		private global::QSWidgetLib.RightSidePanel rightsidepanel1;
		
		private global::Gtk.VBox vboxHidenPanel;
		
		private global::Vodovoz.RouteListDiscrepancyView routelistdiscrepancyview;
		
		private global::Gtk.HBox hbox11;
		
		private global::Gtk.Button buttonReturnedRefresh;
		
		private global::Gtk.Label labelEmptyBottlesFommula;
		
		private global::Gtk.Label labelBottleDifference;
		
		private global::Gtk.CheckButton checkUseBottleFine;
		
		private global::Gtk.Button buttonBottleDelFine;
		
		private global::Gtk.Button buttonBottleAddEditFine;
		
		private global::Gtk.Label labelBottleFine;
		
		private global::Gtk.HBox hbox10;
		
		private global::Gtk.Label labelFullBottles;
		
		private global::Gtk.VSeparator vseparator3;
		
		private global::Gtk.Label labelEmptyBottles;
		
		private global::Gtk.VSeparator vseparator9;
		
		private global::Gtk.Label labelAddressCount;
		
		private global::Gtk.VSeparator vseparator8;
		
		private global::Gtk.Label labelPhone;
		
		private global::Gtk.VSeparator vseparator5;
		
		private global::Gtk.Label labelWage1;
		
		private global::Gtk.Label labelSum1;
		
		private global::Gtk.HBox hbox9;
		
		private global::Gtk.Label labelDeposits;
		
		private global::Gtk.VSeparator vseparator4;
		
		private global::Gtk.Label labelCash;
		
		private global::Gtk.VSeparator vseparator6;
		
		private global::Gtk.Label labelTotalCollected;
		
		private global::Gtk.VSeparator vseparator7;
		
		private global::Gtk.Label labelTotal;
		
		private global::Gtk.Button buttonAccept;
		
		private global::Gamma.GtkWidgets.yCheckButton ycheckConfirmDifferences;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Vodovoz.RouteListClosingDlg
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Vodovoz.RouteListClosingDlg";
			// Container child Vodovoz.RouteListClosingDlg.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			this.vbox1.BorderWidth = ((uint)(6));
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox8 = new global::Gtk.HBox ();
			this.hbox8.Name = "hbox8";
			this.hbox8.Spacing = 6;
			// Container child hbox8.Gtk.Box+BoxChild
			this.buttonSave = new global::Gtk.Button ();
			this.buttonSave.CanFocus = true;
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.UseUnderline = true;
			this.buttonSave.Label = global::Mono.Unix.Catalog.GetString ("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image ();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-save", global::Gtk.IconSize.Menu);
			this.buttonSave.Image = w1;
			this.hbox8.Add (this.buttonSave);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.buttonSave]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox8.Gtk.Box+BoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString ("Отменить");
			global::Gtk.Image w3 = new global::Gtk.Image ();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w3;
			this.hbox8.Add (this.buttonCancel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.buttonCancel]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox8.Gtk.Box+BoxChild
			this.buttonPrint = new global::Gtk.Button ();
			this.buttonPrint.CanFocus = true;
			this.buttonPrint.Name = "buttonPrint";
			this.buttonPrint.UseUnderline = true;
			this.buttonPrint.Label = global::Mono.Unix.Catalog.GetString ("Печать");
			global::Gtk.Image w5 = new global::Gtk.Image ();
			w5.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-print", global::Gtk.IconSize.Menu);
			this.buttonPrint.Image = w5;
			this.hbox8.Add (this.buttonPrint);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.buttonPrint]));
			w6.PackType = ((global::Gtk.PackType)(1));
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			// Container child hbox8.Gtk.Box+BoxChild
			this.enummenuRLActions = new global::QSOrmProject.EnumMenuButton ();
			this.enummenuRLActions.CanFocus = true;
			this.enummenuRLActions.Name = "enummenuRLActions";
			this.enummenuRLActions.UseUnderline = true;
			this.enummenuRLActions.UseMarkup = false;
			global::Gtk.Image w7 = new global::Gtk.Image ();
			w7.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-justify-fill", global::Gtk.IconSize.Menu);
			this.enummenuRLActions.Image = w7;
			this.hbox8.Add (this.enummenuRLActions);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.enummenuRLActions]));
			w8.PackType = ((global::Gtk.PackType)(1));
			w8.Position = 3;
			w8.Expand = false;
			w8.Fill = false;
			this.vbox1.Add (this.hbox8);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox8]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.expander1 = new global::Gtk.Expander (null);
			this.expander1.CanFocus = true;
			this.expander1.Name = "expander1";
			this.expander1.Expanded = true;
			// Container child expander1.Gtk.Container+ContainerChild
			this.table1 = new global::Gtk.Table (((uint)(4)), ((uint)(7)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.datePickerDate = new global::Gamma.Widgets.yDatePicker ();
			this.datePickerDate.Events = ((global::Gdk.EventMask)(256));
			this.datePickerDate.Name = "datePickerDate";
			this.datePickerDate.WithTime = false;
			this.datePickerDate.Date = new global::System.DateTime (0);
			this.datePickerDate.IsEditable = false;
			this.datePickerDate.AutoSeparation = false;
			this.table1.Add (this.datePickerDate);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1 [this.datePickerDate]));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
			this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
			this.ytextClosingComment = new global::Gamma.GtkWidgets.yTextView ();
			this.ytextClosingComment.CanFocus = true;
			this.ytextClosingComment.Name = "ytextClosingComment";
			this.GtkScrolledWindow2.Add (this.ytextClosingComment);
			this.table1.Add (this.GtkScrolledWindow2);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1 [this.GtkScrolledWindow2]));
			w12.TopAttach = ((uint)(1));
			w12.BottomAttach = ((uint)(4));
			w12.LeftAttach = ((uint)(4));
			w12.RightAttach = ((uint)(5));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox12 = new global::Gtk.HBox ();
			this.hbox12.Name = "hbox12";
			this.hbox12.Spacing = 6;
			// Container child hbox12.Gtk.Box+BoxChild
			this.buttonAddTicket = new global::Gtk.Button ();
			this.buttonAddTicket.CanFocus = true;
			this.buttonAddTicket.Name = "buttonAddTicket";
			this.buttonAddTicket.UseUnderline = true;
			this.buttonAddTicket.Label = global::Mono.Unix.Catalog.GetString ("Выдать талон");
			this.hbox12.Add (this.buttonAddTicket);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox12 [this.buttonAddTicket]));
			w13.Position = 0;
			w13.Expand = false;
			w13.Fill = false;
			// Container child hbox12.Gtk.Box+BoxChild
			this.buttonDeleteTicket = new global::Gtk.Button ();
			this.buttonDeleteTicket.CanFocus = true;
			this.buttonDeleteTicket.Name = "buttonDeleteTicket";
			this.buttonDeleteTicket.UseUnderline = true;
			this.buttonDeleteTicket.Label = global::Mono.Unix.Catalog.GetString ("Удалить");
			this.hbox12.Add (this.buttonDeleteTicket);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox12 [this.buttonDeleteTicket]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			// Container child hbox12.Gtk.Box+BoxChild
			this.buttonGetDistFromTrack = new global::Gtk.Button ();
			this.buttonGetDistFromTrack.CanFocus = true;
			this.buttonGetDistFromTrack.Name = "buttonGetDistFromTrack";
			this.buttonGetDistFromTrack.UseUnderline = true;
			this.buttonGetDistFromTrack.Label = global::Mono.Unix.Catalog.GetString ("Из трека");
			this.hbox12.Add (this.buttonGetDistFromTrack);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox12 [this.buttonGetDistFromTrack]));
			w15.Position = 2;
			w15.Expand = false;
			w15.Fill = false;
			this.table1.Add (this.hbox12);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbox12]));
			w16.TopAttach = ((uint)(3));
			w16.BottomAttach = ((uint)(4));
			w16.LeftAttach = ((uint)(5));
			w16.RightAttach = ((uint)(7));
			w16.XOptions = ((global::Gtk.AttachOptions)(0));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Смена:");
			this.table1.Add (this.label1);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
			w17.TopAttach = ((uint)(1));
			w17.BottomAttach = ((uint)(2));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label10 = new global::Gtk.Label ();
			this.label10.Name = "label10";
			this.label10.Xalign = 1F;
			this.label10.LabelProp = global::Mono.Unix.Catalog.GetString ("Логист:");
			this.table1.Add (this.label10);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table1 [this.label10]));
			w18.TopAttach = ((uint)(2));
			w18.BottomAttach = ((uint)(3));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Дата:");
			this.table1.Add (this.label2);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table1 [this.label2]));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Водитель:");
			this.table1.Add (this.label3);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table1 [this.label3]));
			w20.TopAttach = ((uint)(1));
			w20.BottomAttach = ((uint)(2));
			w20.LeftAttach = ((uint)(2));
			w20.RightAttach = ((uint)(3));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Машина:");
			this.table1.Add (this.label4);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
			w21.LeftAttach = ((uint)(2));
			w21.RightAttach = ((uint)(3));
			w21.XOptions = ((global::Gtk.AttachOptions)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Оплачиваемое расстояние:");
			this.table1.Add (this.label5);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.table1 [this.label5]));
			w22.TopAttach = ((uint)(2));
			w22.BottomAttach = ((uint)(3));
			w22.LeftAttach = ((uint)(5));
			w22.RightAttach = ((uint)(6));
			w22.XOptions = ((global::Gtk.AttachOptions)(4));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 0F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Комментарий по закрытию:</b>");
			this.label6.UseMarkup = true;
			this.table1.Add (this.label6);
			global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.table1 [this.label6]));
			w23.LeftAttach = ((uint)(4));
			w23.RightAttach = ((uint)(5));
			w23.XOptions = ((global::Gtk.AttachOptions)(4));
			w23.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label9 = new global::Gtk.Label ();
			this.label9.Name = "label9";
			this.label9.Xalign = 1F;
			this.label9.LabelProp = global::Mono.Unix.Catalog.GetString ("Экспедитор:");
			this.table1.Add (this.label9);
			global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.table1 [this.label9]));
			w24.TopAttach = ((uint)(2));
			w24.BottomAttach = ((uint)(3));
			w24.LeftAttach = ((uint)(2));
			w24.RightAttach = ((uint)(3));
			w24.XOptions = ((global::Gtk.AttachOptions)(4));
			w24.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.referenceCar = new global::Gamma.Widgets.yEntryReference ();
			this.referenceCar.Events = ((global::Gdk.EventMask)(256));
			this.referenceCar.Name = "referenceCar";
			this.table1.Add (this.referenceCar);
			global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.table1 [this.referenceCar]));
			w25.LeftAttach = ((uint)(3));
			w25.RightAttach = ((uint)(4));
			w25.XOptions = ((global::Gtk.AttachOptions)(4));
			w25.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.referenceDriver = new global::Gamma.Widgets.yEntryReference ();
			this.referenceDriver.Events = ((global::Gdk.EventMask)(256));
			this.referenceDriver.Name = "referenceDriver";
			this.table1.Add (this.referenceDriver);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.table1 [this.referenceDriver]));
			w26.TopAttach = ((uint)(1));
			w26.BottomAttach = ((uint)(2));
			w26.LeftAttach = ((uint)(3));
			w26.RightAttach = ((uint)(4));
			w26.XOptions = ((global::Gtk.AttachOptions)(4));
			w26.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.referenceForwarder = new global::Gamma.Widgets.yEntryReference ();
			this.referenceForwarder.Events = ((global::Gdk.EventMask)(256));
			this.referenceForwarder.Name = "referenceForwarder";
			this.table1.Add (this.referenceForwarder);
			global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.table1 [this.referenceForwarder]));
			w27.TopAttach = ((uint)(2));
			w27.BottomAttach = ((uint)(3));
			w27.LeftAttach = ((uint)(3));
			w27.RightAttach = ((uint)(4));
			w27.XOptions = ((global::Gtk.AttachOptions)(4));
			w27.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.referenceLogistican = new global::Gamma.Widgets.yEntryReference ();
			this.referenceLogistican.Events = ((global::Gdk.EventMask)(256));
			this.referenceLogistican.Name = "referenceLogistican";
			this.table1.Add (this.referenceLogistican);
			global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.table1 [this.referenceLogistican]));
			w28.TopAttach = ((uint)(2));
			w28.BottomAttach = ((uint)(3));
			w28.LeftAttach = ((uint)(1));
			w28.RightAttach = ((uint)(2));
			w28.XOptions = ((global::Gtk.AttachOptions)(4));
			w28.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.speccomboShift = new global::Gamma.Widgets.ySpecComboBox ();
			this.speccomboShift.Name = "speccomboShift";
			this.speccomboShift.AddIfNotExist = false;
			this.speccomboShift.DefaultFirst = false;
			this.speccomboShift.ShowSpecialStateAll = false;
			this.speccomboShift.ShowSpecialStateNot = false;
			this.table1.Add (this.speccomboShift);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table1 [this.speccomboShift]));
			w29.TopAttach = ((uint)(1));
			w29.BottomAttach = ((uint)(2));
			w29.LeftAttach = ((uint)(1));
			w29.RightAttach = ((uint)(2));
			w29.XOptions = ((global::Gtk.AttachOptions)(4));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ycheckHideCells = new global::Gamma.GtkWidgets.yCheckButton ();
			this.ycheckHideCells.CanFocus = true;
			this.ycheckHideCells.Name = "ycheckHideCells";
			this.ycheckHideCells.Label = global::Mono.Unix.Catalog.GetString ("Скрыть столбцы");
			this.ycheckHideCells.Active = true;
			this.ycheckHideCells.DrawIndicator = true;
			this.ycheckHideCells.UseUnderline = true;
			this.table1.Add (this.ycheckHideCells);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table1 [this.ycheckHideCells]));
			w30.TopAttach = ((uint)(3));
			w30.BottomAttach = ((uint)(4));
			w30.RightAttach = ((uint)(2));
			w30.XOptions = ((global::Gtk.AttachOptions)(4));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.yspinActualDistance = new global::Gamma.GtkWidgets.ySpinButton (0, 10000, 1);
			this.yspinActualDistance.CanFocus = true;
			this.yspinActualDistance.Name = "yspinActualDistance";
			this.yspinActualDistance.Adjustment.PageIncrement = 10;
			this.yspinActualDistance.ClimbRate = 1;
			this.yspinActualDistance.Numeric = true;
			this.yspinActualDistance.ValueAsDecimal = 0m;
			this.yspinActualDistance.ValueAsInt = 0;
			this.table1.Add (this.yspinActualDistance);
			global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.table1 [this.yspinActualDistance]));
			w31.TopAttach = ((uint)(2));
			w31.BottomAttach = ((uint)(3));
			w31.LeftAttach = ((uint)(6));
			w31.RightAttach = ((uint)(7));
			w31.XOptions = ((global::Gtk.AttachOptions)(4));
			w31.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ytextviewFuelInfo = new global::Gamma.GtkWidgets.yTextView ();
			this.ytextviewFuelInfo.CanFocus = true;
			this.ytextviewFuelInfo.Name = "ytextviewFuelInfo";
			this.ytextviewFuelInfo.Editable = false;
			this.table1.Add (this.ytextviewFuelInfo);
			global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table1 [this.ytextviewFuelInfo]));
			w32.BottomAttach = ((uint)(2));
			w32.LeftAttach = ((uint)(5));
			w32.RightAttach = ((uint)(7));
			w32.XOptions = ((global::Gtk.AttachOptions)(4));
			w32.YOptions = ((global::Gtk.AttachOptions)(4));
			this.expander1.Add (this.table1);
			this.GtkLabel17 = new global::Gtk.Label ();
			this.GtkLabel17.Name = "GtkLabel17";
			this.GtkLabel17.LabelProp = global::Mono.Unix.Catalog.GetString ("Информация о маршрутном листе");
			this.GtkLabel17.UseUnderline = true;
			this.expander1.LabelWidget = this.GtkLabel17;
			this.vbox1.Add (this.expander1);
			global::Gtk.Box.BoxChild w34 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.expander1]));
			w34.Position = 1;
			w34.Expand = false;
			w34.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox6 = new global::Gtk.HBox ();
			this.hbox6.Name = "hbox6";
			this.hbox6.Spacing = 6;
			// Container child hbox6.Gtk.Box+BoxChild
			this.routeListAddressesView = new global::Vodovoz.RouteListClosingItemsView ();
			this.routeListAddressesView.Events = ((global::Gdk.EventMask)(256));
			this.routeListAddressesView.Name = "routeListAddressesView";
			this.routeListAddressesView.ColumsVisibility = false;
			this.hbox6.Add (this.routeListAddressesView);
			global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.routeListAddressesView]));
			w35.Position = 0;
			// Container child hbox6.Gtk.Box+BoxChild
			this.rightsidepanel1 = new global::QSWidgetLib.RightSidePanel ();
			this.rightsidepanel1.Events = ((global::Gdk.EventMask)(256));
			this.rightsidepanel1.Name = "rightsidepanel1";
			this.rightsidepanel1.Title = "Возвраты/Недвозы";
			this.rightsidepanel1.IsHided = false;
			this.hbox6.Add (this.rightsidepanel1);
			global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.rightsidepanel1]));
			w36.Position = 1;
			w36.Expand = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.vboxHidenPanel = new global::Gtk.VBox ();
			this.vboxHidenPanel.Name = "vboxHidenPanel";
			this.vboxHidenPanel.Spacing = 6;
			// Container child vboxHidenPanel.Gtk.Box+BoxChild
			this.routelistdiscrepancyview = new global::Vodovoz.RouteListDiscrepancyView ();
			this.routelistdiscrepancyview.Events = ((global::Gdk.EventMask)(256));
			this.routelistdiscrepancyview.Name = "routelistdiscrepancyview";
			this.vboxHidenPanel.Add (this.routelistdiscrepancyview);
			global::Gtk.Box.BoxChild w37 = ((global::Gtk.Box.BoxChild)(this.vboxHidenPanel [this.routelistdiscrepancyview]));
			w37.Position = 0;
			// Container child vboxHidenPanel.Gtk.Box+BoxChild
			this.hbox11 = new global::Gtk.HBox ();
			this.hbox11.Name = "hbox11";
			this.hbox11.Spacing = 6;
			// Container child hbox11.Gtk.Box+BoxChild
			this.buttonReturnedRefresh = new global::Gtk.Button ();
			this.buttonReturnedRefresh.TooltipMarkup = "Обновить информацию по возврату на склад";
			this.buttonReturnedRefresh.CanFocus = true;
			this.buttonReturnedRefresh.Name = "buttonReturnedRefresh";
			this.buttonReturnedRefresh.UseUnderline = true;
			global::Gtk.Image w38 = new global::Gtk.Image ();
			w38.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-refresh", global::Gtk.IconSize.Menu);
			this.buttonReturnedRefresh.Image = w38;
			this.hbox11.Add (this.buttonReturnedRefresh);
			global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.hbox11 [this.buttonReturnedRefresh]));
			w39.Position = 0;
			w39.Expand = false;
			w39.Fill = false;
			// Container child hbox11.Gtk.Box+BoxChild
			this.labelEmptyBottlesFommula = new global::Gtk.Label ();
			this.labelEmptyBottlesFommula.Name = "labelEmptyBottlesFommula";
			this.labelEmptyBottlesFommula.LabelProp = global::Mono.Unix.Catalog.GetString ("Тара");
			this.labelEmptyBottlesFommula.UseMarkup = true;
			this.hbox11.Add (this.labelEmptyBottlesFommula);
			global::Gtk.Box.BoxChild w40 = ((global::Gtk.Box.BoxChild)(this.hbox11 [this.labelEmptyBottlesFommula]));
			w40.Position = 1;
			w40.Expand = false;
			w40.Fill = false;
			// Container child hbox11.Gtk.Box+BoxChild
			this.labelBottleDifference = new global::Gtk.Label ();
			this.labelBottleDifference.Name = "labelBottleDifference";
			this.labelBottleDifference.LabelProp = global::Mono.Unix.Catalog.GetString ("(разница)");
			this.labelBottleDifference.UseMarkup = true;
			this.hbox11.Add (this.labelBottleDifference);
			global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.hbox11 [this.labelBottleDifference]));
			w41.Position = 2;
			w41.Expand = false;
			w41.Fill = false;
			// Container child hbox11.Gtk.Box+BoxChild
			this.checkUseBottleFine = new global::Gtk.CheckButton ();
			this.checkUseBottleFine.TooltipMarkup = "Штраф за бутыли";
			this.checkUseBottleFine.CanFocus = true;
			this.checkUseBottleFine.Name = "checkUseBottleFine";
			this.checkUseBottleFine.Label = global::Mono.Unix.Catalog.GetString ("checkbutton1");
			this.checkUseBottleFine.DrawIndicator = true;
			this.checkUseBottleFine.UseUnderline = true;
			this.hbox11.Add (this.checkUseBottleFine);
			global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(this.hbox11 [this.checkUseBottleFine]));
			w42.Position = 3;
			// Container child hbox11.Gtk.Box+BoxChild
			this.buttonBottleDelFine = new global::Gtk.Button ();
			this.buttonBottleDelFine.TooltipMarkup = "Убрать штраф";
			this.buttonBottleDelFine.CanFocus = true;
			this.buttonBottleDelFine.Name = "buttonBottleDelFine";
			this.buttonBottleDelFine.UseUnderline = true;
			this.buttonBottleDelFine.Relief = ((global::Gtk.ReliefStyle)(2));
			global::Gtk.Image w43 = new global::Gtk.Image ();
			w43.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Vodovoz.icons.buttons.remove.png");
			this.buttonBottleDelFine.Image = w43;
			this.hbox11.Add (this.buttonBottleDelFine);
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.hbox11 [this.buttonBottleDelFine]));
			w44.PackType = ((global::Gtk.PackType)(1));
			w44.Position = 4;
			w44.Expand = false;
			w44.Fill = false;
			// Container child hbox11.Gtk.Box+BoxChild
			this.buttonBottleAddEditFine = new global::Gtk.Button ();
			this.buttonBottleAddEditFine.TooltipMarkup = "Добавить изменить штраф.";
			this.buttonBottleAddEditFine.CanFocus = true;
			this.buttonBottleAddEditFine.Name = "buttonBottleAddEditFine";
			this.buttonBottleAddEditFine.UseUnderline = true;
			this.buttonBottleAddEditFine.Relief = ((global::Gtk.ReliefStyle)(2));
			global::Gtk.Image w45 = new global::Gtk.Image ();
			w45.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Vodovoz.icons.buttons.add.png");
			this.buttonBottleAddEditFine.Image = w45;
			this.hbox11.Add (this.buttonBottleAddEditFine);
			global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.hbox11 [this.buttonBottleAddEditFine]));
			w46.PackType = ((global::Gtk.PackType)(1));
			w46.Position = 5;
			w46.Expand = false;
			w46.Fill = false;
			// Container child hbox11.Gtk.Box+BoxChild
			this.labelBottleFine = new global::Gtk.Label ();
			this.labelBottleFine.Name = "labelBottleFine";
			this.labelBottleFine.LabelProp = global::Mono.Unix.Catalog.GetString ("Штраф:");
			this.hbox11.Add (this.labelBottleFine);
			global::Gtk.Box.BoxChild w47 = ((global::Gtk.Box.BoxChild)(this.hbox11 [this.labelBottleFine]));
			w47.PackType = ((global::Gtk.PackType)(1));
			w47.Position = 6;
			w47.Expand = false;
			w47.Fill = false;
			this.vboxHidenPanel.Add (this.hbox11);
			global::Gtk.Box.BoxChild w48 = ((global::Gtk.Box.BoxChild)(this.vboxHidenPanel [this.hbox11]));
			w48.Position = 1;
			w48.Expand = false;
			w48.Fill = false;
			this.hbox6.Add (this.vboxHidenPanel);
			global::Gtk.Box.BoxChild w49 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.vboxHidenPanel]));
			w49.Position = 2;
			w49.Expand = false;
			w49.Fill = false;
			this.vbox1.Add (this.hbox6);
			global::Gtk.Box.BoxChild w50 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox6]));
			w50.Position = 2;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox10 = new global::Gtk.HBox ();
			this.hbox10.Name = "hbox10";
			this.hbox10.Spacing = 6;
			// Container child hbox10.Gtk.Box+BoxChild
			this.labelFullBottles = new global::Gtk.Label ();
			this.labelFullBottles.Name = "labelFullBottles";
			this.labelFullBottles.LabelProp = global::Mono.Unix.Catalog.GetString ("Полн. бутылей");
			this.hbox10.Add (this.labelFullBottles);
			global::Gtk.Box.BoxChild w51 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.labelFullBottles]));
			w51.Position = 0;
			w51.Expand = false;
			w51.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.vseparator3 = new global::Gtk.VSeparator ();
			this.vseparator3.Name = "vseparator3";
			this.hbox10.Add (this.vseparator3);
			global::Gtk.Box.BoxChild w52 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.vseparator3]));
			w52.Position = 1;
			w52.Expand = false;
			w52.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.labelEmptyBottles = new global::Gtk.Label ();
			this.labelEmptyBottles.Name = "labelEmptyBottles";
			this.labelEmptyBottles.LabelProp = global::Mono.Unix.Catalog.GetString ("Пустых бутылей");
			this.hbox10.Add (this.labelEmptyBottles);
			global::Gtk.Box.BoxChild w53 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.labelEmptyBottles]));
			w53.Position = 2;
			w53.Expand = false;
			w53.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.vseparator9 = new global::Gtk.VSeparator ();
			this.vseparator9.Name = "vseparator9";
			this.hbox10.Add (this.vseparator9);
			global::Gtk.Box.BoxChild w54 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.vseparator9]));
			w54.Position = 3;
			w54.Expand = false;
			w54.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.labelAddressCount = new global::Gtk.Label ();
			this.labelAddressCount.Name = "labelAddressCount";
			this.labelAddressCount.LabelProp = global::Mono.Unix.Catalog.GetString ("Адресов");
			this.hbox10.Add (this.labelAddressCount);
			global::Gtk.Box.BoxChild w55 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.labelAddressCount]));
			w55.Position = 4;
			w55.Expand = false;
			w55.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.vseparator8 = new global::Gtk.VSeparator ();
			this.vseparator8.Name = "vseparator8";
			this.hbox10.Add (this.vseparator8);
			global::Gtk.Box.BoxChild w56 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.vseparator8]));
			w56.Position = 5;
			w56.Expand = false;
			w56.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.labelPhone = new global::Gtk.Label ();
			this.labelPhone.Name = "labelPhone";
			this.labelPhone.LabelProp = global::Mono.Unix.Catalog.GetString ("Сот. связь");
			this.hbox10.Add (this.labelPhone);
			global::Gtk.Box.BoxChild w57 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.labelPhone]));
			w57.Position = 6;
			w57.Expand = false;
			w57.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.vseparator5 = new global::Gtk.VSeparator ();
			this.vseparator5.Name = "vseparator5";
			this.hbox10.Add (this.vseparator5);
			global::Gtk.Box.BoxChild w58 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.vseparator5]));
			w58.Position = 7;
			w58.Expand = false;
			w58.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.labelWage1 = new global::Gtk.Label ();
			this.labelWage1.Name = "labelWage1";
			this.labelWage1.LabelProp = global::Mono.Unix.Catalog.GetString ("Зарплата:");
			this.hbox10.Add (this.labelWage1);
			global::Gtk.Box.BoxChild w59 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.labelWage1]));
			w59.Position = 8;
			w59.Expand = false;
			w59.Fill = false;
			// Container child hbox10.Gtk.Box+BoxChild
			this.labelSum1 = new global::Gtk.Label ();
			this.labelSum1.Name = "labelSum1";
			this.labelSum1.Xalign = 1F;
			this.hbox10.Add (this.labelSum1);
			global::Gtk.Box.BoxChild w60 = ((global::Gtk.Box.BoxChild)(this.hbox10 [this.labelSum1]));
			w60.PackType = ((global::Gtk.PackType)(1));
			w60.Position = 9;
			w60.Expand = false;
			w60.Fill = false;
			this.vbox1.Add (this.hbox10);
			global::Gtk.Box.BoxChild w61 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox10]));
			w61.Position = 3;
			w61.Expand = false;
			w61.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox9 = new global::Gtk.HBox ();
			this.hbox9.Name = "hbox9";
			this.hbox9.Spacing = 6;
			// Container child hbox9.Gtk.Box+BoxChild
			this.labelDeposits = new global::Gtk.Label ();
			this.labelDeposits.Name = "labelDeposits";
			this.labelDeposits.LabelProp = global::Mono.Unix.Catalog.GetString ("Залогов");
			this.hbox9.Add (this.labelDeposits);
			global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.labelDeposits]));
			w62.Position = 0;
			w62.Expand = false;
			w62.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.vseparator4 = new global::Gtk.VSeparator ();
			this.vseparator4.Name = "vseparator4";
			this.hbox9.Add (this.vseparator4);
			global::Gtk.Box.BoxChild w63 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.vseparator4]));
			w63.Position = 1;
			w63.Expand = false;
			w63.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.labelCash = new global::Gtk.Label ();
			this.labelCash.Name = "labelCash";
			this.labelCash.LabelProp = global::Mono.Unix.Catalog.GetString ("Итого(нал)");
			this.hbox9.Add (this.labelCash);
			global::Gtk.Box.BoxChild w64 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.labelCash]));
			w64.Position = 2;
			w64.Expand = false;
			w64.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.vseparator6 = new global::Gtk.VSeparator ();
			this.vseparator6.Name = "vseparator6";
			this.hbox9.Add (this.vseparator6);
			global::Gtk.Box.BoxChild w65 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.vseparator6]));
			w65.Position = 3;
			w65.Expand = false;
			w65.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.labelTotalCollected = new global::Gtk.Label ();
			this.labelTotalCollected.Name = "labelTotalCollected";
			this.labelTotalCollected.LabelProp = global::Mono.Unix.Catalog.GetString ("Итого сдано");
			this.hbox9.Add (this.labelTotalCollected);
			global::Gtk.Box.BoxChild w66 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.labelTotalCollected]));
			w66.Position = 4;
			w66.Expand = false;
			w66.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.vseparator7 = new global::Gtk.VSeparator ();
			this.vseparator7.Name = "vseparator7";
			this.hbox9.Add (this.vseparator7);
			global::Gtk.Box.BoxChild w67 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.vseparator7]));
			w67.Position = 5;
			w67.Expand = false;
			w67.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.labelTotal = new global::Gtk.Label ();
			this.labelTotal.Name = "labelTotal";
			this.labelTotal.LabelProp = global::Mono.Unix.Catalog.GetString ("Итого");
			this.hbox9.Add (this.labelTotal);
			global::Gtk.Box.BoxChild w68 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.labelTotal]));
			w68.Position = 6;
			w68.Expand = false;
			w68.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.buttonAccept = new global::Gtk.Button ();
			this.buttonAccept.CanFocus = true;
			this.buttonAccept.Name = "buttonAccept";
			this.buttonAccept.UseUnderline = true;
			this.buttonAccept.Label = global::Mono.Unix.Catalog.GetString ("Подтвердить");
			global::Gtk.Image w69 = new global::Gtk.Image ();
			w69.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-apply", global::Gtk.IconSize.Menu);
			this.buttonAccept.Image = w69;
			this.hbox9.Add (this.buttonAccept);
			global::Gtk.Box.BoxChild w70 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.buttonAccept]));
			w70.PackType = ((global::Gtk.PackType)(1));
			w70.Position = 8;
			w70.Expand = false;
			w70.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.ycheckConfirmDifferences = new global::Gamma.GtkWidgets.yCheckButton ();
			this.ycheckConfirmDifferences.CanFocus = true;
			this.ycheckConfirmDifferences.Name = "ycheckConfirmDifferences";
			this.ycheckConfirmDifferences.Label = global::Mono.Unix.Catalog.GetString ("Расхождения подтверждаю");
			this.ycheckConfirmDifferences.DrawIndicator = true;
			this.ycheckConfirmDifferences.UseUnderline = true;
			this.hbox9.Add (this.ycheckConfirmDifferences);
			global::Gtk.Box.BoxChild w71 = ((global::Gtk.Box.BoxChild)(this.hbox9 [this.ycheckConfirmDifferences]));
			w71.PackType = ((global::Gtk.PackType)(1));
			w71.Position = 9;
			w71.Expand = false;
			w71.Fill = false;
			this.vbox1.Add (this.hbox9);
			global::Gtk.Box.BoxChild w72 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox9]));
			w72.Position = 4;
			w72.Expand = false;
			w72.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.buttonSave.Clicked += new global::System.EventHandler (this.OnButtonSaveClicked);
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.buttonPrint.Clicked += new global::System.EventHandler (this.OnButtonPrintClicked);
			this.yspinActualDistance.ValueChanged += new global::System.EventHandler (this.OnYspinActualDistanceValueChanged);
			this.ycheckHideCells.Toggled += new global::System.EventHandler (this.OnYcheckHideCellsToggled);
			this.buttonAddTicket.Clicked += new global::System.EventHandler (this.OnButtonAddTicketClicked);
			this.buttonDeleteTicket.Clicked += new global::System.EventHandler (this.OnButtonDeleteTicketClicked);
			this.buttonGetDistFromTrack.Clicked += new global::System.EventHandler (this.OnButtonGetDistFromTrackClicked);
			this.buttonReturnedRefresh.Clicked += new global::System.EventHandler (this.OnButtonReturnedRefreshClicked);
			this.checkUseBottleFine.Toggled += new global::System.EventHandler (this.OnCheckUseBottleFineToggled);
			this.buttonBottleAddEditFine.Clicked += new global::System.EventHandler (this.OnButtonBottleAddEditFineClicked);
			this.buttonBottleDelFine.Clicked += new global::System.EventHandler (this.OnButtonBottleDelFineClicked);
			this.ycheckConfirmDifferences.Toggled += new global::System.EventHandler (this.OnYcheckConfirmDifferencesToggled);
			this.buttonAccept.Clicked += new global::System.EventHandler (this.OnButtonAcceptClicked);
		}
	}
}
