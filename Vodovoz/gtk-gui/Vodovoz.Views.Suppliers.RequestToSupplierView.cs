
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Views.Suppliers
{
	public partial class RequestToSupplierView
	{
		private global::Gtk.VBox vbxMain;

		private global::Gtk.HBox hboxDialogButtons;

		private global::Gamma.GtkWidgets.yButton btnSave;

		private global::Gamma.GtkWidgets.yButton btnCancel;

		private global::Gamma.GtkWidgets.yButton btnRefresh;

		private global::Gamma.GtkWidgets.yLabel lblMsg;

		private global::Gtk.Table table1;

		private global::Gamma.GtkWidgets.yEntry entName;

		private global::Gamma.Widgets.yEnumComboBox enumCmbSuppliersOrdering;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gamma.GtkWidgets.yTextView txtComment;

		private global::Gamma.GtkWidgets.yLabel lblComment;

		private global::Gamma.GtkWidgets.yLabel lblName;

		private global::Gamma.GtkWidgets.yLabel lblSuppliersOrdering;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gamma.GtkWidgets.yTreeView treeItems;

		private global::Gtk.HBox hbox1;

		private global::Gamma.GtkWidgets.yButton btnAdd;

		private global::Gamma.GtkWidgets.yButton btnRemove;

		private global::Gamma.GtkWidgets.yButton btnTransfer;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.Views.Suppliers.RequestToSupplierView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.Views.Suppliers.RequestToSupplierView";
			// Container child Vodovoz.Views.Suppliers.RequestToSupplierView.Gtk.Container+ContainerChild
			this.vbxMain = new global::Gtk.VBox();
			this.vbxMain.Name = "vbxMain";
			this.vbxMain.Spacing = 6;
			// Container child vbxMain.Gtk.Box+BoxChild
			this.hboxDialogButtons = new global::Gtk.HBox();
			this.hboxDialogButtons.Name = "hboxDialogButtons";
			this.hboxDialogButtons.Spacing = 6;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.btnSave = new global::Gamma.GtkWidgets.yButton();
			this.btnSave.CanFocus = true;
			this.btnSave.Name = "btnSave";
			this.btnSave.UseUnderline = true;
			this.btnSave.Label = global::Mono.Unix.Catalog.GetString("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-save", global::Gtk.IconSize.Menu);
			this.btnSave.Image = w1;
			this.hboxDialogButtons.Add(this.btnSave);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.btnSave]));
			w2.Position = 0;
			w2.Expand = false;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.btnCancel = new global::Gamma.GtkWidgets.yButton();
			this.btnCancel.CanFocus = true;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseUnderline = true;
			this.btnCancel.Label = global::Mono.Unix.Catalog.GetString("Отменить");
			global::Gtk.Image w3 = new global::Gtk.Image();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.btnCancel.Image = w3;
			this.hboxDialogButtons.Add(this.btnCancel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.btnCancel]));
			w4.Position = 1;
			w4.Expand = false;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.btnRefresh = new global::Gamma.GtkWidgets.yButton();
			this.btnRefresh.CanFocus = true;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.UseUnderline = true;
			global::Gtk.Image w5 = new global::Gtk.Image();
			w5.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-refresh", global::Gtk.IconSize.Menu);
			this.btnRefresh.Image = w5;
			this.hboxDialogButtons.Add(this.btnRefresh);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.btnRefresh]));
			w6.PackType = ((global::Gtk.PackType)(1));
			w6.Position = 2;
			w6.Expand = false;
			// Container child hboxDialogButtons.Gtk.Box+BoxChild
			this.lblMsg = new global::Gamma.GtkWidgets.yLabel();
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Xalign = 1F;
			this.lblMsg.LabelProp = global::Mono.Unix.Catalog.GetString("<span foreground=\'red\'>Список цен не актуален! Требуется обновить --></span>");
			this.lblMsg.UseMarkup = true;
			this.lblMsg.Selectable = true;
			this.hboxDialogButtons.Add(this.lblMsg);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hboxDialogButtons[this.lblMsg]));
			w7.PackType = ((global::Gtk.PackType)(1));
			w7.Position = 3;
			w7.Expand = false;
			this.vbxMain.Add(this.hboxDialogButtons);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbxMain[this.hboxDialogButtons]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbxMain.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(3)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.entName = new global::Gamma.GtkWidgets.yEntry();
			this.entName.CanFocus = true;
			this.entName.Name = "entName";
			this.entName.IsEditable = true;
			this.entName.InvisibleChar = '•';
			this.table1.Add(this.entName);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1[this.entName]));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.enumCmbSuppliersOrdering = new global::Gamma.Widgets.yEnumComboBox();
			this.enumCmbSuppliersOrdering.Name = "enumCmbSuppliersOrdering";
			this.enumCmbSuppliersOrdering.ShowSpecialStateAll = false;
			this.enumCmbSuppliersOrdering.ShowSpecialStateNot = false;
			this.enumCmbSuppliersOrdering.UseShortTitle = false;
			this.enumCmbSuppliersOrdering.DefaultFirst = false;
			this.table1.Add(this.enumCmbSuppliersOrdering);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1[this.enumCmbSuppliersOrdering]));
			w10.TopAttach = ((uint)(1));
			w10.BottomAttach = ((uint)(2));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.txtComment = new global::Gamma.GtkWidgets.yTextView();
			this.txtComment.CanFocus = true;
			this.txtComment.Name = "txtComment";
			this.txtComment.WrapMode = ((global::Gtk.WrapMode)(2));
			this.GtkScrolledWindow.Add(this.txtComment);
			this.table1.Add(this.GtkScrolledWindow);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1[this.GtkScrolledWindow]));
			w12.TopAttach = ((uint)(2));
			w12.BottomAttach = ((uint)(3));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.lblComment = new global::Gamma.GtkWidgets.yLabel();
			this.lblComment.Name = "lblComment";
			this.lblComment.Xalign = 1F;
			this.lblComment.LabelProp = global::Mono.Unix.Catalog.GetString("Комментарий:");
			this.table1.Add(this.lblComment);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table1[this.lblComment]));
			w13.TopAttach = ((uint)(2));
			w13.BottomAttach = ((uint)(3));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.lblName = new global::Gamma.GtkWidgets.yLabel();
			this.lblName.Name = "lblName";
			this.lblName.Xalign = 1F;
			this.lblName.LabelProp = global::Mono.Unix.Catalog.GetString("Название:");
			this.table1.Add(this.lblName);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table1[this.lblName]));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.lblSuppliersOrdering = new global::Gamma.GtkWidgets.yLabel();
			this.lblSuppliersOrdering.Name = "lblSuppliersOrdering";
			this.lblSuppliersOrdering.Xalign = 1F;
			this.lblSuppliersOrdering.LabelProp = global::Mono.Unix.Catalog.GetString("Кол-во поставщиков:");
			this.table1.Add(this.lblSuppliersOrdering);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table1[this.lblSuppliersOrdering]));
			w15.TopAttach = ((uint)(1));
			w15.BottomAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbxMain.Add(this.table1);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbxMain[this.table1]));
			w16.Position = 1;
			w16.Expand = false;
			// Container child vbxMain.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeItems = new global::Gamma.GtkWidgets.yTreeView();
			this.treeItems.CanFocus = true;
			this.treeItems.Name = "treeItems";
			this.GtkScrolledWindow1.Add(this.treeItems);
			this.vbxMain.Add(this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbxMain[this.GtkScrolledWindow1]));
			w18.Position = 2;
			// Container child vbxMain.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.btnAdd = new global::Gamma.GtkWidgets.yButton();
			this.btnAdd.CanFocus = true;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseUnderline = true;
			this.btnAdd.Label = global::Mono.Unix.Catalog.GetString("ТМЦ");
			global::Gtk.Image w19 = new global::Gtk.Image();
			w19.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "add", global::Gtk.IconSize.Menu);
			this.btnAdd.Image = w19;
			this.hbox1.Add(this.btnAdd);
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.btnAdd]));
			w20.Position = 0;
			w20.Expand = false;
			w20.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.btnRemove = new global::Gamma.GtkWidgets.yButton();
			this.btnRemove.CanFocus = true;
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.UseUnderline = true;
			this.btnRemove.Label = global::Mono.Unix.Catalog.GetString("ТМЦ");
			global::Gtk.Image w21 = new global::Gtk.Image();
			w21.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "remove", global::Gtk.IconSize.Menu);
			this.btnRemove.Image = w21;
			this.hbox1.Add(this.btnRemove);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.btnRemove]));
			w22.Position = 1;
			w22.Expand = false;
			w22.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.btnTransfer = new global::Gamma.GtkWidgets.yButton();
			this.btnTransfer.CanFocus = true;
			this.btnTransfer.Name = "btnTransfer";
			this.btnTransfer.UseUnderline = true;
			this.btnTransfer.Label = global::Mono.Unix.Catalog.GetString("Перенести в новую заявку");
			global::Gtk.Image w23 = new global::Gtk.Image();
			w23.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "redo", global::Gtk.IconSize.Menu);
			this.btnTransfer.Image = w23;
			this.hbox1.Add(this.btnTransfer);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.btnTransfer]));
			w24.PackType = ((global::Gtk.PackType)(1));
			w24.Position = 2;
			w24.Expand = false;
			w24.Fill = false;
			this.vbxMain.Add(this.hbox1);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.vbxMain[this.hbox1]));
			w25.Position = 3;
			w25.Expand = false;
			w25.Fill = false;
			this.Add(this.vbxMain);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
