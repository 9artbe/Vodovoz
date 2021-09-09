
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz
{
	public partial class IncomingWaterDlg
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.HBox hbox3;

		private global::Gtk.Button buttonSave;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonFill;

		private global::Gtk.Table tableWater;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry destinationWarehouseEntry;

		private global::Vodovoz.IncomingWaterMaterialView incomingwatermaterialview1;

		private global::Gtk.Label label1;

		private global::Gtk.Label label2;

		private global::Gtk.Label label3;

		private global::Gtk.Label label5;

		private global::Gtk.Label label6;

		private global::Gamma.GtkWidgets.yLabel labelTimeStamp;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry sourceWarehouseEntry;

		private global::Gamma.GtkWidgets.ySpinButton spinAmount;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry yentryProduct;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.IncomingWaterDlg
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.IncomingWaterDlg";
			// Container child Vodovoz.IncomingWaterDlg.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonSave = new global::Gtk.Button();
			this.buttonSave.CanFocus = true;
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.UseUnderline = true;
			this.buttonSave.Label = global::Mono.Unix.Catalog.GetString("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-save", global::Gtk.IconSize.Menu);
			this.buttonSave.Image = w1;
			this.hbox3.Add(this.buttonSave);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.buttonSave]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString("Отменить");
			global::Gtk.Image w3 = new global::Gtk.Image();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w3;
			this.hbox3.Add(this.buttonCancel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.buttonCancel]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonFill = new global::Gtk.Button();
			this.buttonFill.CanFocus = true;
			this.buttonFill.Name = "buttonFill";
			this.buttonFill.UseUnderline = true;
			this.buttonFill.Label = global::Mono.Unix.Catalog.GetString("Заполнить по спецификации");
			this.hbox3.Add(this.buttonFill);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.buttonFill]));
			w5.PackType = ((global::Gtk.PackType)(1));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			this.vbox2.Add(this.hbox3);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox3]));
			w6.Position = 0;
			w6.Expand = false;
			w6.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.tableWater = new global::Gtk.Table(((uint)(6)), ((uint)(2)), false);
			this.tableWater.Name = "tableWater";
			this.tableWater.RowSpacing = ((uint)(6));
			this.tableWater.ColumnSpacing = ((uint)(6));
			// Container child tableWater.Gtk.Table+TableChild
			this.destinationWarehouseEntry = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.destinationWarehouseEntry.Events = ((global::Gdk.EventMask)(256));
			this.destinationWarehouseEntry.Name = "destinationWarehouseEntry";
			this.destinationWarehouseEntry.CanEditReference = false;
			this.tableWater.Add(this.destinationWarehouseEntry);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.tableWater[this.destinationWarehouseEntry]));
			w7.TopAttach = ((uint)(3));
			w7.BottomAttach = ((uint)(4));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.incomingwatermaterialview1 = new global::Vodovoz.IncomingWaterMaterialView();
			this.incomingwatermaterialview1.Events = ((global::Gdk.EventMask)(256));
			this.incomingwatermaterialview1.Name = "incomingwatermaterialview1";
			this.tableWater.Add(this.incomingwatermaterialview1);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.tableWater[this.incomingwatermaterialview1]));
			w8.TopAttach = ((uint)(5));
			w8.BottomAttach = ((uint)(6));
			w8.RightAttach = ((uint)(2));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Дата документа:");
			this.tableWater.Add(this.label1);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.tableWater[this.label1]));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Количество единиц:");
			this.tableWater.Add(this.label2);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.tableWater[this.label2]));
			w10.TopAttach = ((uint)(2));
			w10.BottomAttach = ((uint)(3));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Склад поступления:");
			this.tableWater.Add(this.label3);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.tableWater[this.label3]));
			w11.TopAttach = ((uint)(3));
			w11.BottomAttach = ((uint)(4));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString("Склад списания:");
			this.tableWater.Add(this.label5);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.tableWater[this.label5]));
			w12.TopAttach = ((uint)(4));
			w12.BottomAttach = ((uint)(5));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString("Продукт производства:");
			this.tableWater.Add(this.label6);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.tableWater[this.label6]));
			w13.TopAttach = ((uint)(1));
			w13.BottomAttach = ((uint)(2));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.labelTimeStamp = new global::Gamma.GtkWidgets.yLabel();
			this.labelTimeStamp.Name = "labelTimeStamp";
			this.labelTimeStamp.Xalign = 0F;
			this.tableWater.Add(this.labelTimeStamp);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.tableWater[this.labelTimeStamp]));
			w14.LeftAttach = ((uint)(1));
			w14.RightAttach = ((uint)(2));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.sourceWarehouseEntry = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.sourceWarehouseEntry.Events = ((global::Gdk.EventMask)(256));
			this.sourceWarehouseEntry.Name = "sourceWarehouseEntry";
			this.sourceWarehouseEntry.CanEditReference = false;
			this.tableWater.Add(this.sourceWarehouseEntry);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.tableWater[this.sourceWarehouseEntry]));
			w15.TopAttach = ((uint)(4));
			w15.BottomAttach = ((uint)(5));
			w15.LeftAttach = ((uint)(1));
			w15.RightAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.spinAmount = new global::Gamma.GtkWidgets.ySpinButton(0D, 10000D, 1D);
			this.spinAmount.CanFocus = true;
			this.spinAmount.Name = "spinAmount";
			this.spinAmount.Adjustment.PageIncrement = 10D;
			this.spinAmount.ClimbRate = 1D;
			this.spinAmount.Numeric = true;
			this.spinAmount.Value = 1D;
			this.spinAmount.ValueAsDecimal = 0m;
			this.spinAmount.ValueAsInt = 0;
			this.tableWater.Add(this.spinAmount);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.tableWater[this.spinAmount]));
			w16.TopAttach = ((uint)(2));
			w16.BottomAttach = ((uint)(3));
			w16.LeftAttach = ((uint)(1));
			w16.RightAttach = ((uint)(2));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWater.Gtk.Table+TableChild
			this.yentryProduct = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.yentryProduct.Events = ((global::Gdk.EventMask)(256));
			this.yentryProduct.Name = "yentryProduct";
			this.yentryProduct.CanEditReference = false;
			this.tableWater.Add(this.yentryProduct);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.tableWater[this.yentryProduct]));
			w17.TopAttach = ((uint)(1));
			w17.BottomAttach = ((uint)(2));
			w17.LeftAttach = ((uint)(1));
			w17.RightAttach = ((uint)(2));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add(this.tableWater);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.tableWater]));
			w18.Position = 1;
			this.Add(this.vbox2);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
			this.buttonFill.Clicked += new global::System.EventHandler(this.OnButtonFillClicked);
		}
	}
}
