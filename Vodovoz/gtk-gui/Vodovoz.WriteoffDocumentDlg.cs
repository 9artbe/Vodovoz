
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz
{
	public partial class WriteoffDocumentDlg
	{
		private global::Gtk.VBox vbox4;
		
		private global::Gtk.HBox hbox5;
		
		private global::Gtk.Button buttonSave;
		
		private global::Gtk.Button buttonCancel;
		
		private global::Gtk.Button buttonPrint;
		
		private global::Gtk.DataBindings.DataTable tableWriteoff;
		
		private global::QSOrmProject.EnumComboBox comboType;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		
		private global::Gtk.DataBindings.DataTextView textComment;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.Label label12;
		
		private global::Gtk.Label label2;
		
		private global::Gtk.Label label4;
		
		private global::Gtk.Label label5;
		
		private global::Gtk.Label label6;
		
		private global::Gtk.Label label7;
		
		private global::Gtk.DataBindings.DataLabel labelTimeStamp;
		
		private global::Gamma.Widgets.yEntryReferenceVM referenceCounterparty;
		
		private global::Gtk.DataBindings.DataEntryReference referenceDeliveryPoint;
		
		private global::Gtk.DataBindings.DataEntryReference referenceEmployee;
		
		private global::Gtk.DataBindings.DataEntryReference referenceWarehouse;
		
		private global::Vodovoz.WriteoffDocumentItemsView writeoffdocumentitemsview1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Vodovoz.WriteoffDocumentDlg
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Vodovoz.WriteoffDocumentDlg";
			// Container child Vodovoz.WriteoffDocumentDlg.Gtk.Container+ContainerChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			this.vbox4.BorderWidth = ((uint)(6));
			// Container child vbox4.Gtk.Box+BoxChild
			this.hbox5 = new global::Gtk.HBox ();
			this.hbox5.Name = "hbox5";
			this.hbox5.Spacing = 6;
			// Container child hbox5.Gtk.Box+BoxChild
			this.buttonSave = new global::Gtk.Button ();
			this.buttonSave.CanFocus = true;
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.UseUnderline = true;
			this.buttonSave.Label = global::Mono.Unix.Catalog.GetString ("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image ();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-save", global::Gtk.IconSize.Menu);
			this.buttonSave.Image = w1;
			this.hbox5.Add (this.buttonSave);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.buttonSave]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox5.Gtk.Box+BoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString ("Отменить");
			global::Gtk.Image w3 = new global::Gtk.Image ();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w3;
			this.hbox5.Add (this.buttonCancel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.buttonCancel]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox5.Gtk.Box+BoxChild
			this.buttonPrint = new global::Gtk.Button ();
			this.buttonPrint.CanFocus = true;
			this.buttonPrint.Name = "buttonPrint";
			this.buttonPrint.UseUnderline = true;
			this.buttonPrint.Label = global::Mono.Unix.Catalog.GetString ("Печать");
			global::Gtk.Image w5 = new global::Gtk.Image ();
			w5.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-print", global::Gtk.IconSize.Menu);
			this.buttonPrint.Image = w5;
			this.hbox5.Add (this.buttonPrint);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.buttonPrint]));
			w6.PackType = ((global::Gtk.PackType)(1));
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			this.vbox4.Add (this.hbox5);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hbox5]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.tableWriteoff = new global::Gtk.DataBindings.DataTable (((uint)(4)), ((uint)(4)), false);
			this.tableWriteoff.Name = "tableWriteoff";
			this.tableWriteoff.RowSpacing = ((uint)(6));
			this.tableWriteoff.ColumnSpacing = ((uint)(6));
			this.tableWriteoff.InheritedDataSource = false;
			this.tableWriteoff.InheritedBoundaryDataSource = false;
			this.tableWriteoff.InheritedDataSource = false;
			this.tableWriteoff.InheritedBoundaryDataSource = false;
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.comboType = new global::QSOrmProject.EnumComboBox ();
			this.comboType.CanFocus = true;
			this.comboType.Name = "comboType";
			this.comboType.ItemsEnumName = "";
			this.comboType.ShowSpecialStateAll = false;
			this.comboType.ShowSpecialStateNot = false;
			this.tableWriteoff.Add (this.comboType);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.comboType]));
			w8.TopAttach = ((uint)(1));
			w8.BottomAttach = ((uint)(2));
			w8.LeftAttach = ((uint)(1));
			w8.RightAttach = ((uint)(2));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.textComment = new global::Gtk.DataBindings.DataTextView ();
			this.textComment.CanFocus = true;
			this.textComment.Name = "textComment";
			this.textComment.InheritedDataSource = true;
			this.textComment.Mappings = "Comment";
			this.textComment.InheritedBoundaryDataSource = false;
			this.textComment.InheritedDataSource = true;
			this.textComment.Mappings = "Comment";
			this.textComment.InheritedBoundaryDataSource = false;
			this.GtkScrolledWindow1.Add (this.textComment);
			this.tableWriteoff.Add (this.GtkScrolledWindow1);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.GtkScrolledWindow1]));
			w10.TopAttach = ((uint)(3));
			w10.BottomAttach = ((uint)(4));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(4));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Дата:");
			this.tableWriteoff.Add (this.label1);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.label1]));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.label12 = new global::Gtk.Label ();
			this.label12.Name = "label12";
			this.label12.LabelProp = global::Mono.Unix.Catalog.GetString ("Ответственное лицо:");
			this.tableWriteoff.Add (this.label12);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.label12]));
			w12.LeftAttach = ((uint)(2));
			w12.RightAttach = ((uint)(3));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Тип списания:");
			this.tableWriteoff.Add (this.label2);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.label2]));
			w13.TopAttach = ((uint)(1));
			w13.BottomAttach = ((uint)(2));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Склад:");
			this.tableWriteoff.Add (this.label4);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.label4]));
			w14.TopAttach = ((uint)(1));
			w14.BottomAttach = ((uint)(2));
			w14.LeftAttach = ((uint)(2));
			w14.RightAttach = ((uint)(3));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.Yalign = 0F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Комментарий:");
			this.tableWriteoff.Add (this.label5);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.label5]));
			w15.TopAttach = ((uint)(3));
			w15.BottomAttach = ((uint)(4));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Клиент:");
			this.tableWriteoff.Add (this.label6);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.label6]));
			w16.TopAttach = ((uint)(2));
			w16.BottomAttach = ((uint)(3));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("Точка доставки:");
			this.tableWriteoff.Add (this.label7);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.label7]));
			w17.TopAttach = ((uint)(2));
			w17.BottomAttach = ((uint)(3));
			w17.LeftAttach = ((uint)(2));
			w17.RightAttach = ((uint)(3));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.labelTimeStamp = new global::Gtk.DataBindings.DataLabel ();
			this.labelTimeStamp.Name = "labelTimeStamp";
			this.labelTimeStamp.Xalign = 0F;
			this.labelTimeStamp.InheritedDataSource = true;
			this.labelTimeStamp.Mappings = "DateString";
			this.labelTimeStamp.InheritedBoundaryDataSource = false;
			this.labelTimeStamp.Important = false;
			this.labelTimeStamp.InheritedDataSource = true;
			this.labelTimeStamp.Mappings = "DateString";
			this.labelTimeStamp.InheritedBoundaryDataSource = false;
			this.tableWriteoff.Add (this.labelTimeStamp);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.labelTimeStamp]));
			w18.LeftAttach = ((uint)(1));
			w18.RightAttach = ((uint)(2));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.referenceCounterparty = new global::Gamma.Widgets.yEntryReferenceVM ();
			this.referenceCounterparty.Events = ((global::Gdk.EventMask)(256));
			this.referenceCounterparty.Name = "referenceCounterparty";
			this.tableWriteoff.Add (this.referenceCounterparty);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.referenceCounterparty]));
			w19.TopAttach = ((uint)(2));
			w19.BottomAttach = ((uint)(3));
			w19.LeftAttach = ((uint)(1));
			w19.RightAttach = ((uint)(2));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.referenceDeliveryPoint = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceDeliveryPoint.Events = ((global::Gdk.EventMask)(256));
			this.referenceDeliveryPoint.Name = "referenceDeliveryPoint";
			this.referenceDeliveryPoint.DisplayFields = new string[] {
				"CompiledAddress"
			};
			this.referenceDeliveryPoint.DisplayFormatString = "{0}";
			this.referenceDeliveryPoint.InheritedDataSource = true;
			this.referenceDeliveryPoint.Mappings = "DeliveryPoint";
			this.referenceDeliveryPoint.InheritedBoundaryDataSource = false;
			this.referenceDeliveryPoint.CursorPointsEveryType = false;
			this.tableWriteoff.Add (this.referenceDeliveryPoint);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.referenceDeliveryPoint]));
			w20.TopAttach = ((uint)(2));
			w20.BottomAttach = ((uint)(3));
			w20.LeftAttach = ((uint)(3));
			w20.RightAttach = ((uint)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.referenceEmployee = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceEmployee.Events = ((global::Gdk.EventMask)(256));
			this.referenceEmployee.Name = "referenceEmployee";
			this.referenceEmployee.DisplayFields = new string[] {
				"FullName"
			};
			this.referenceEmployee.DisplayFormatString = "{0}";
			this.referenceEmployee.InheritedDataSource = true;
			this.referenceEmployee.Mappings = "ResponsibleEmployee";
			this.referenceEmployee.InheritedBoundaryDataSource = false;
			this.referenceEmployee.CursorPointsEveryType = false;
			this.tableWriteoff.Add (this.referenceEmployee);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.referenceEmployee]));
			w21.LeftAttach = ((uint)(3));
			w21.RightAttach = ((uint)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tableWriteoff.Gtk.Table+TableChild
			this.referenceWarehouse = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceWarehouse.Events = ((global::Gdk.EventMask)(256));
			this.referenceWarehouse.Name = "referenceWarehouse";
			this.referenceWarehouse.DisplayFields = new string[] {
				"Name"
			};
			this.referenceWarehouse.DisplayFormatString = "{0}";
			this.referenceWarehouse.InheritedDataSource = true;
			this.referenceWarehouse.Mappings = "WriteoffWarehouse";
			this.referenceWarehouse.InheritedBoundaryDataSource = false;
			this.referenceWarehouse.CursorPointsEveryType = false;
			this.tableWriteoff.Add (this.referenceWarehouse);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.tableWriteoff [this.referenceWarehouse]));
			w22.TopAttach = ((uint)(1));
			w22.BottomAttach = ((uint)(2));
			w22.LeftAttach = ((uint)(3));
			w22.RightAttach = ((uint)(4));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox4.Add (this.tableWriteoff);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.tableWriteoff]));
			w23.Position = 1;
			w23.Expand = false;
			w23.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.writeoffdocumentitemsview1 = new global::Vodovoz.WriteoffDocumentItemsView ();
			this.writeoffdocumentitemsview1.Events = ((global::Gdk.EventMask)(256));
			this.writeoffdocumentitemsview1.Name = "writeoffdocumentitemsview1";
			this.vbox4.Add (this.writeoffdocumentitemsview1);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.writeoffdocumentitemsview1]));
			w24.PackType = ((global::Gtk.PackType)(1));
			w24.Position = 2;
			this.Add (this.vbox4);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.buttonSave.Clicked += new global::System.EventHandler (this.OnButtonSaveClicked);
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.buttonPrint.Clicked += new global::System.EventHandler (this.OnButtonPrintClicked);
			this.referenceCounterparty.Changed += new global::System.EventHandler (this.OnReferenceCounterpartyChanged);
		}
	}
}
