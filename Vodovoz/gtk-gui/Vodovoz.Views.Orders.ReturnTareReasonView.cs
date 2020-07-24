
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Views.Orders
{
	public partial class ReturnTareReasonView
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.HBox hbox1;

		private global::Gamma.GtkWidgets.yButton ybtnSave;

		private global::Gamma.GtkWidgets.yButton ybtnCancel;

		private global::Gtk.VSeparator vseparator1;

		private global::Gtk.HBox hbox16;

		private global::Gamma.GtkWidgets.yCheckButton yChkIsArchive;

		private global::Gtk.Table table2;

		private global::Gtk.Label labelReasonName;

		private global::Gamma.GtkWidgets.yEntry yentryReturnTareReasonName;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.Views.Orders.ReturnTareReasonView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.Views.Orders.ReturnTareReasonView";
			// Container child Vodovoz.Views.Orders.ReturnTareReasonView.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.ybtnSave = new global::Gamma.GtkWidgets.yButton();
			this.ybtnSave.CanFocus = true;
			this.ybtnSave.Name = "ybtnSave";
			this.ybtnSave.UseUnderline = true;
			this.ybtnSave.Label = global::Mono.Unix.Catalog.GetString("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-floppy", global::Gtk.IconSize.Menu);
			this.ybtnSave.Image = w1;
			this.hbox1.Add(this.ybtnSave);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.ybtnSave]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.ybtnCancel = new global::Gamma.GtkWidgets.yButton();
			this.ybtnCancel.CanFocus = true;
			this.ybtnCancel.Name = "ybtnCancel";
			this.ybtnCancel.UseUnderline = true;
			this.ybtnCancel.Label = global::Mono.Unix.Catalog.GetString("Отмена");
			this.hbox1.Add(this.ybtnCancel);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.ybtnCancel]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vseparator1 = new global::Gtk.VSeparator();
			this.vseparator1.Name = "vseparator1";
			this.hbox1.Add(this.vseparator1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vseparator1]));
			w4.Position = 2;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.hbox16 = new global::Gtk.HBox();
			this.hbox16.Name = "hbox16";
			this.hbox16.Spacing = 6;
			// Container child hbox16.Gtk.Box+BoxChild
			this.yChkIsArchive = new global::Gamma.GtkWidgets.yCheckButton();
			this.yChkIsArchive.CanFocus = true;
			this.yChkIsArchive.Name = "yChkIsArchive";
			this.yChkIsArchive.Label = global::Mono.Unix.Catalog.GetString("Архивный");
			this.yChkIsArchive.DrawIndicator = true;
			this.yChkIsArchive.UseUnderline = true;
			this.hbox16.Add(this.yChkIsArchive);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox16[this.yChkIsArchive]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			this.hbox1.Add(this.hbox16);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.hbox16]));
			w6.Position = 3;
			w6.Expand = false;
			this.vbox1.Add(this.hbox1);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.table2 = new global::Gtk.Table(((uint)(1)), ((uint)(2)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			// Container child table2.Gtk.Table+TableChild
			this.labelReasonName = new global::Gtk.Label();
			this.labelReasonName.Name = "labelReasonName";
			this.labelReasonName.Xalign = 1F;
			this.labelReasonName.LabelProp = global::Mono.Unix.Catalog.GetString("Название причины:");
			this.table2.Add(this.labelReasonName);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table2[this.labelReasonName]));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.yentryReturnTareReasonName = new global::Gamma.GtkWidgets.yEntry();
			this.yentryReturnTareReasonName.CanFocus = true;
			this.yentryReturnTareReasonName.Name = "yentryReturnTareReasonName";
			this.yentryReturnTareReasonName.IsEditable = true;
			this.yentryReturnTareReasonName.InvisibleChar = '•';
			this.table2.Add(this.yentryReturnTareReasonName);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table2[this.yentryReturnTareReasonName]));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox1.Add(this.table2);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.table2]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			this.Add(this.vbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}