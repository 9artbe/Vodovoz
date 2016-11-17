
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz
{
	public partial class RoutesAtDayDlg
	{
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.Table table1;
		
		private global::Gtk.HBox hbox4;
		
		private global::Gtk.Button buttonSaveChanges;
		
		private global::Gtk.Button buttonCancelChanges;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.Label label2;
		
		private global::Gtk.ProgressBar progressOrders;
		
		private global::Gtk.TextView textOrdersInfo;
		
		private global::Gamma.Widgets.yDatePicker ydateForRoutes;
		
		private global::Gtk.HBox hbox2;
		
		private global::Gtk.VBox vbox3;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::Gamma.GtkWidgets.yTreeView ytreeRoutes;
		
		private global::Gtk.HBox hbox5;
		
		private global::Gtk.Button buttonOpen;
		
		private global::Gtk.Button buttonRemoveAddress;
		
		private global::Gtk.VBox vbox2;
		
		private global::Gtk.HBox hbox3;
		
		private global::Gtk.Label labelSelected;
		
		private global::QSWidgetLib.MenuButton menuAddToRL;
		
		private global::Gamma.Widgets.yEnumComboBox yenumcomboMapType;
		
		private global::GMap.NET.GtkSharp.GMapControl gmapWidget;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Vodovoz.RoutesAtDayDlg
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Vodovoz.RoutesAtDayDlg";
			// Container child Vodovoz.RoutesAtDayDlg.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(2)), ((uint)(5)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.buttonSaveChanges = new global::Gtk.Button ();
			this.buttonSaveChanges.CanFocus = true;
			this.buttonSaveChanges.Name = "buttonSaveChanges";
			this.buttonSaveChanges.UseUnderline = true;
			this.buttonSaveChanges.Label = global::Mono.Unix.Catalog.GetString ("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image ();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-floppy", global::Gtk.IconSize.Menu);
			this.buttonSaveChanges.Image = w1;
			this.hbox4.Add (this.buttonSaveChanges);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.buttonSaveChanges]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.buttonCancelChanges = new global::Gtk.Button ();
			this.buttonCancelChanges.CanFocus = true;
			this.buttonCancelChanges.Name = "buttonCancelChanges";
			this.buttonCancelChanges.UseUnderline = true;
			this.buttonCancelChanges.Label = global::Mono.Unix.Catalog.GetString ("Отменить");
			global::Gtk.Image w3 = new global::Gtk.Image ();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			this.buttonCancelChanges.Image = w3;
			this.hbox4.Add (this.buttonCancelChanges);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.buttonCancelChanges]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			this.table1.Add (this.hbox4);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbox4]));
			w5.LeftAttach = ((uint)(2));
			w5.RightAttach = ((uint)(3));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Дата:");
			this.table1.Add (this.label1);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.Yalign = 0F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Оперативная сводка:");
			this.table1.Add (this.label2);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.label2]));
			w7.LeftAttach = ((uint)(3));
			w7.RightAttach = ((uint)(4));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.progressOrders = new global::Gtk.ProgressBar ();
			this.progressOrders.Name = "progressOrders";
			this.table1.Add (this.progressOrders);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.progressOrders]));
			w8.TopAttach = ((uint)(1));
			w8.BottomAttach = ((uint)(2));
			w8.RightAttach = ((uint)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.textOrdersInfo = new global::Gtk.TextView ();
			this.textOrdersInfo.CanFocus = true;
			this.textOrdersInfo.Name = "textOrdersInfo";
			this.table1.Add (this.textOrdersInfo);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1 [this.textOrdersInfo]));
			w9.BottomAttach = ((uint)(2));
			w9.LeftAttach = ((uint)(4));
			w9.RightAttach = ((uint)(5));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ydateForRoutes = new global::Gamma.Widgets.yDatePicker ();
			this.ydateForRoutes.Events = ((global::Gdk.EventMask)(256));
			this.ydateForRoutes.Name = "ydateForRoutes";
			this.ydateForRoutes.WithTime = false;
			this.ydateForRoutes.Date = new global::System.DateTime (0);
			this.ydateForRoutes.IsEditable = true;
			this.ydateForRoutes.AutoSeparation = true;
			this.table1.Add (this.ydateForRoutes);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1 [this.ydateForRoutes]));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox1.Add (this.table1);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.table1]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.ytreeRoutes = new global::Gamma.GtkWidgets.yTreeView ();
			this.ytreeRoutes.CanFocus = true;
			this.ytreeRoutes.Name = "ytreeRoutes";
			this.GtkScrolledWindow.Add (this.ytreeRoutes);
			this.vbox3.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.GtkScrolledWindow]));
			w13.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox5 = new global::Gtk.HBox ();
			this.hbox5.Name = "hbox5";
			this.hbox5.Spacing = 6;
			// Container child hbox5.Gtk.Box+BoxChild
			this.buttonOpen = new global::Gtk.Button ();
			this.buttonOpen.CanFocus = true;
			this.buttonOpen.Name = "buttonOpen";
			this.buttonOpen.UseUnderline = true;
			this.buttonOpen.Label = global::Mono.Unix.Catalog.GetString ("Открыть");
			this.hbox5.Add (this.buttonOpen);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.buttonOpen]));
			w14.Position = 0;
			w14.Expand = false;
			w14.Fill = false;
			// Container child hbox5.Gtk.Box+BoxChild
			this.buttonRemoveAddress = new global::Gtk.Button ();
			this.buttonRemoveAddress.CanFocus = true;
			this.buttonRemoveAddress.Name = "buttonRemoveAddress";
			this.buttonRemoveAddress.UseUnderline = true;
			this.buttonRemoveAddress.Label = global::Mono.Unix.Catalog.GetString ("Убрать адрес");
			global::Gtk.Image w15 = new global::Gtk.Image ();
			w15.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-remove", global::Gtk.IconSize.Menu);
			this.buttonRemoveAddress.Image = w15;
			this.hbox5.Add (this.buttonRemoveAddress);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.buttonRemoveAddress]));
			w16.Position = 2;
			w16.Expand = false;
			w16.Fill = false;
			this.vbox3.Add (this.hbox5);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox5]));
			w17.Position = 1;
			w17.Expand = false;
			w17.Fill = false;
			this.hbox2.Add (this.vbox3);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox3]));
			w18.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.labelSelected = new global::Gtk.Label ();
			this.labelSelected.Name = "labelSelected";
			this.labelSelected.LabelProp = global::Mono.Unix.Catalog.GetString ("Адресов не выбрано");
			this.hbox3.Add (this.labelSelected);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.labelSelected]));
			w19.Position = 0;
			w19.Expand = false;
			w19.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.menuAddToRL = new global::QSWidgetLib.MenuButton ();
			this.menuAddToRL.Sensitive = false;
			this.menuAddToRL.CanFocus = true;
			this.menuAddToRL.Name = "menuAddToRL";
			this.menuAddToRL.UseUnderline = true;
			this.menuAddToRL.UseMarkup = false;
			this.menuAddToRL.Label = global::Mono.Unix.Catalog.GetString ("В маршрутный лист");
			global::Gtk.Image w20 = new global::Gtk.Image ();
			w20.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			this.menuAddToRL.Image = w20;
			this.hbox3.Add (this.menuAddToRL);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.menuAddToRL]));
			w21.Position = 1;
			w21.Expand = false;
			w21.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.yenumcomboMapType = new global::Gamma.Widgets.yEnumComboBox ();
			this.yenumcomboMapType.Name = "yenumcomboMapType";
			this.yenumcomboMapType.ShowSpecialStateAll = false;
			this.yenumcomboMapType.ShowSpecialStateNot = false;
			this.yenumcomboMapType.UseShortTitle = false;
			this.yenumcomboMapType.DefaultFirst = true;
			this.hbox3.Add (this.yenumcomboMapType);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.yenumcomboMapType]));
			w22.PackType = ((global::Gtk.PackType)(1));
			w22.Position = 2;
			w22.Expand = false;
			w22.Fill = false;
			this.vbox2.Add (this.hbox3);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox3]));
			w23.Position = 0;
			w23.Expand = false;
			w23.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.gmapWidget = new global::GMap.NET.GtkSharp.GMapControl ();
			this.gmapWidget.Name = "gmapWidget";
			this.gmapWidget.MaxZoom = 24;
			this.gmapWidget.MinZoom = 0;
			this.gmapWidget.MouseWheelZoomEnabled = true;
			this.gmapWidget.ShowTileGridLines = false;
			this.gmapWidget.GrayScaleMode = false;
			this.gmapWidget.NegativeMode = false;
			this.gmapWidget.HasFrame = false;
			this.gmapWidget.Bearing = 0F;
			this.gmapWidget.Zoom = 9;
			this.gmapWidget.RoutesEnabled = false;
			this.gmapWidget.PolygonsEnabled = false;
			this.gmapWidget.MarkersEnabled = true;
			this.gmapWidget.CanDragMap = true;
			this.vbox2.Add (this.gmapWidget);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.gmapWidget]));
			w24.Position = 1;
			this.hbox2.Add (this.vbox2);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox2]));
			w25.Position = 1;
			this.vbox1.Add (this.hbox2);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox2]));
			w26.Position = 1;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.ydateForRoutes.DateChanged += new global::System.EventHandler (this.OnYdateForRoutesDateChanged);
			this.buttonSaveChanges.Clicked += new global::System.EventHandler (this.OnButtonSaveChangesClicked);
			this.buttonCancelChanges.Clicked += new global::System.EventHandler (this.OnButtonCancelChangesClicked);
			this.buttonRemoveAddress.Clicked += new global::System.EventHandler (this.OnButtonRemoveAddressClicked);
			this.yenumcomboMapType.ChangedByUser += new global::System.EventHandler (this.OnYenumcomboMapTypeChangedByUser);
		}
	}
}
