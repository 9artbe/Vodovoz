
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Dialogs
{
	public partial class HistoryView
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.Table tblSettings;

		private global::Gtk.Button buttonSearch;

		private global::Gamma.Widgets.yEnumComboBox comboAction;

		private global::Vodovoz.JournalViewers.NodeViewModelEntry entryObject3;

		private global::Vodovoz.JournalViewers.NodeViewModelEntry entryProperty;

		private global::Gtk.Entry entrySearchEntity;

		private global::Gtk.Entry entrySearchValue;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry entryUser;

		private global::Gtk.Entry entSearchId;

		private global::Gtk.Label label10;

		private global::Gtk.Label label3;

		private global::Gtk.Label label4;

		private global::Gtk.Label label5;

		private global::Gtk.Label label6;

		private global::Gtk.Label label8;

		private global::Gtk.Label label9;

		private global::QSWidgetLib.SelectPeriod selectperiod;

		private global::Gtk.Button btnFilter;

		private global::Gtk.VPaned vpaned1;

		private global::Gtk.ScrolledWindow GtkScrolledWindowChangesets;

		private global::Gamma.GtkWidgets.yTreeView datatreeChangesets;

		private global::Gtk.VBox vbox3;

		private global::Gtk.Label label7;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gamma.GtkWidgets.yTreeView datatreeChanges;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.Dialogs.HistoryView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.Dialogs.HistoryView";
			// Container child Vodovoz.Dialogs.HistoryView.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.tblSettings = new global::Gtk.Table(((uint)(6)), ((uint)(5)), false);
			this.tblSettings.Name = "tblSettings";
			this.tblSettings.RowSpacing = ((uint)(6));
			this.tblSettings.ColumnSpacing = ((uint)(6));
			// Container child tblSettings.Gtk.Table+TableChild
			this.buttonSearch = new global::Gtk.Button();
			this.buttonSearch.CanFocus = true;
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.UseUnderline = true;
			global::Gtk.Image w1 = new global::Gtk.Image();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-find", global::Gtk.IconSize.Menu);
			this.buttonSearch.Image = w1;
			this.tblSettings.Add(this.buttonSearch);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.buttonSearch]));
			w2.TopAttach = ((uint)(3));
			w2.BottomAttach = ((uint)(6));
			w2.LeftAttach = ((uint)(4));
			w2.RightAttach = ((uint)(5));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.comboAction = new global::Gamma.Widgets.yEnumComboBox();
			this.comboAction.Name = "comboAction";
			this.comboAction.ShowSpecialStateAll = true;
			this.comboAction.ShowSpecialStateNot = false;
			this.comboAction.UseShortTitle = false;
			this.comboAction.DefaultFirst = false;
			this.tblSettings.Add(this.comboAction);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.comboAction]));
			w3.TopAttach = ((uint)(2));
			w3.BottomAttach = ((uint)(3));
			w3.LeftAttach = ((uint)(1));
			w3.RightAttach = ((uint)(2));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.entryObject3 = new global::Vodovoz.JournalViewers.NodeViewModelEntry();
			this.entryObject3.Events = ((global::Gdk.EventMask)(256));
			this.entryObject3.Name = "entryObject3";
			this.entryObject3.CanEditReference = false;
			this.tblSettings.Add(this.entryObject3);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.entryObject3]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.LeftAttach = ((uint)(1));
			w4.RightAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.entryProperty = new global::Vodovoz.JournalViewers.NodeViewModelEntry();
			this.entryProperty.Events = ((global::Gdk.EventMask)(256));
			this.entryProperty.Name = "entryProperty";
			this.entryProperty.CanEditReference = false;
			this.tblSettings.Add(this.entryProperty);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.entryProperty]));
			w5.TopAttach = ((uint)(3));
			w5.BottomAttach = ((uint)(4));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.entrySearchEntity = new global::Gtk.Entry();
			this.entrySearchEntity.CanFocus = true;
			this.entrySearchEntity.Name = "entrySearchEntity";
			this.entrySearchEntity.IsEditable = true;
			this.entrySearchEntity.InvisibleChar = '●';
			this.tblSettings.Add(this.entrySearchEntity);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.entrySearchEntity]));
			w6.TopAttach = ((uint)(3));
			w6.BottomAttach = ((uint)(4));
			w6.LeftAttach = ((uint)(3));
			w6.RightAttach = ((uint)(4));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.entrySearchValue = new global::Gtk.Entry();
			this.entrySearchValue.CanFocus = true;
			this.entrySearchValue.Name = "entrySearchValue";
			this.entrySearchValue.IsEditable = true;
			this.entrySearchValue.InvisibleChar = '●';
			this.tblSettings.Add(this.entrySearchValue);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.entrySearchValue]));
			w7.TopAttach = ((uint)(5));
			w7.BottomAttach = ((uint)(6));
			w7.LeftAttach = ((uint)(3));
			w7.RightAttach = ((uint)(4));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.entryUser = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.entryUser.Events = ((global::Gdk.EventMask)(256));
			this.entryUser.Name = "entryUser";
			this.entryUser.CanEditReference = false;
			this.tblSettings.Add(this.entryUser);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.entryUser]));
			w8.LeftAttach = ((uint)(1));
			w8.RightAttach = ((uint)(2));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.entSearchId = new global::Gtk.Entry();
			this.entSearchId.CanFocus = true;
			this.entSearchId.Name = "entSearchId";
			this.entSearchId.IsEditable = true;
			this.entSearchId.InvisibleChar = '●';
			this.tblSettings.Add(this.entSearchId);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.entSearchId]));
			w9.TopAttach = ((uint)(4));
			w9.BottomAttach = ((uint)(5));
			w9.LeftAttach = ((uint)(3));
			w9.RightAttach = ((uint)(4));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.label10 = new global::Gtk.Label();
			this.label10.Name = "label10";
			this.label10.Xalign = 1F;
			this.label10.LabelProp = global::Mono.Unix.Catalog.GetString("Поиск по коду объекта:");
			this.tblSettings.Add(this.label10);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.label10]));
			w10.TopAttach = ((uint)(4));
			w10.BottomAttach = ((uint)(5));
			w10.LeftAttach = ((uint)(2));
			w10.RightAttach = ((uint)(3));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Пользователь:");
			this.tblSettings.Add(this.label3);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.label3]));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("Действие с объектом:");
			this.tblSettings.Add(this.label4);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.label4]));
			w12.TopAttach = ((uint)(2));
			w12.BottomAttach = ((uint)(3));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString("Поиск в изменениях:");
			this.tblSettings.Add(this.label5);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.label5]));
			w13.TopAttach = ((uint)(5));
			w13.BottomAttach = ((uint)(6));
			w13.LeftAttach = ((uint)(2));
			w13.RightAttach = ((uint)(3));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString("Объект изменений:");
			this.tblSettings.Add(this.label6);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.label6]));
			w14.TopAttach = ((uint)(1));
			w14.BottomAttach = ((uint)(2));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = global::Mono.Unix.Catalog.GetString("Реквизит объекта:");
			this.tblSettings.Add(this.label8);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.label8]));
			w15.TopAttach = ((uint)(3));
			w15.BottomAttach = ((uint)(4));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.label9 = new global::Gtk.Label();
			this.label9.Name = "label9";
			this.label9.Xalign = 1F;
			this.label9.LabelProp = global::Mono.Unix.Catalog.GetString("Поиск в имени объекта:");
			this.tblSettings.Add(this.label9);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.label9]));
			w16.TopAttach = ((uint)(3));
			w16.BottomAttach = ((uint)(4));
			w16.LeftAttach = ((uint)(2));
			w16.RightAttach = ((uint)(3));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tblSettings.Gtk.Table+TableChild
			this.selectperiod = new global::QSWidgetLib.SelectPeriod();
			this.selectperiod.Events = ((global::Gdk.EventMask)(256));
			this.selectperiod.Name = "selectperiod";
			this.selectperiod.DateBegin = new global::System.DateTime(0);
			this.selectperiod.DateEnd = new global::System.DateTime(0);
			this.selectperiod.AutoDateSeparation = true;
			this.selectperiod.ShowToday = true;
			this.selectperiod.ShowWeek = true;
			this.selectperiod.ShowMonth = true;
			this.selectperiod.Show3Month = true;
			this.selectperiod.Show6Month = false;
			this.selectperiod.ShowYear = false;
			this.selectperiod.ShowAllTime = false;
			this.selectperiod.ShowCurWeek = false;
			this.selectperiod.ShowCurMonth = false;
			this.selectperiod.ShowCurQuarter = false;
			this.selectperiod.ShowCurYear = false;
			this.selectperiod.WithTime = true;
			this.tblSettings.Add(this.selectperiod);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.tblSettings[this.selectperiod]));
			w17.BottomAttach = ((uint)(3));
			w17.LeftAttach = ((uint)(2));
			w17.RightAttach = ((uint)(4));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add(this.tblSettings);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.tblSettings]));
			w18.Position = 0;
			w18.Expand = false;
			w18.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.btnFilter = new global::Gtk.Button();
			this.btnFilter.CanFocus = true;
			this.btnFilter.Name = "btnFilter";
			this.btnFilter.UseUnderline = true;
			this.btnFilter.Label = global::Mono.Unix.Catalog.GetString("Фильтр");
			this.vbox2.Add(this.btnFilter);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.btnFilter]));
			w19.Position = 1;
			w19.Expand = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.vpaned1 = new global::Gtk.VPaned();
			this.vpaned1.CanFocus = true;
			this.vpaned1.Name = "vpaned1";
			this.vpaned1.Position = 200;
			// Container child vpaned1.Gtk.Paned+PanedChild
			this.GtkScrolledWindowChangesets = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindowChangesets.Name = "GtkScrolledWindowChangesets";
			this.GtkScrolledWindowChangesets.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindowChangesets.Gtk.Container+ContainerChild
			this.datatreeChangesets = new global::Gamma.GtkWidgets.yTreeView();
			this.datatreeChangesets.CanFocus = true;
			this.datatreeChangesets.Name = "datatreeChangesets";
			this.GtkScrolledWindowChangesets.Add(this.datatreeChangesets);
			this.vpaned1.Add(this.GtkScrolledWindowChangesets);
			global::Gtk.Paned.PanedChild w21 = ((global::Gtk.Paned.PanedChild)(this.vpaned1[this.GtkScrolledWindowChangesets]));
			w21.Resize = false;
			w21.Shrink = false;
			// Container child vpaned1.Gtk.Paned+PanedChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.label7 = new global::Gtk.Label();
			this.label7.Name = "label7";
			this.label7.Xalign = 0F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString("Детали изменений:");
			this.vbox3.Add(this.label7);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.label7]));
			w22.Position = 0;
			w22.Expand = false;
			w22.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.datatreeChanges = new global::Gamma.GtkWidgets.yTreeView();
			this.datatreeChanges.CanFocus = true;
			this.datatreeChanges.Name = "datatreeChanges";
			this.GtkScrolledWindow1.Add(this.datatreeChanges);
			this.vbox3.Add(this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.GtkScrolledWindow1]));
			w24.Position = 1;
			this.vpaned1.Add(this.vbox3);
			global::Gtk.Paned.PanedChild w25 = ((global::Gtk.Paned.PanedChild)(this.vpaned1[this.vbox3]));
			w25.Resize = false;
			w25.Shrink = false;
			this.vbox2.Add(this.vpaned1);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.vpaned1]));
			w26.Position = 2;
			this.Add(this.vbox2);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
			this.selectperiod.DatesChanged += new global::System.EventHandler(this.OnSelectperiodDatesChanged);
			this.entrySearchValue.Activated += new global::System.EventHandler(this.OnEntrySearchValueActivated);
			this.entrySearchEntity.Activated += new global::System.EventHandler(this.OnEntrySearchEntityActivated);
			this.comboAction.Changed += new global::System.EventHandler(this.OnComboActionChanged);
			this.buttonSearch.Clicked += new global::System.EventHandler(this.OnButtonSearchClicked);
			this.btnFilter.Clicked += new global::System.EventHandler(this.OnBtnFilterClicked);
		}
	}
}
