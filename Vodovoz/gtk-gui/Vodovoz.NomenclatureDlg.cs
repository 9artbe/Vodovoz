
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz
{
	public partial class NomenclatureDlg
	{
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.Button buttonSave;
		
		private global::Gtk.Button buttonCancel;
		
		private global::Gtk.Notebook notebook1;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::Gtk.DataBindings.DataTable datatable1;
		
		private global::Gtk.DataBindings.DataCheckButton checkNotReserve;
		
		private global::Gtk.DataBindings.DataCheckButton checkSerial;
		
		private global::Gtk.DataBindings.DataEntry entryModel;
		
		private global::Gtk.DataBindings.DataEntry entryName;
		
		private global::Gtk.DataBindings.DataEnumComboBox enumType;
		
		private global::Gtk.DataBindings.DataEnumComboBox enumVAT;
		
		private global::Gtk.Label labelClass;
		
		private global::Gtk.Label labelColor;
		
		private global::Gtk.Label labelDeposit;
		
		private global::Gtk.Label labelManufacturer;
		
		private global::Gtk.Label labelModel;
		
		private global::Gtk.Label labelName;
		
		private global::Gtk.Label labelReserve;
		
		private global::Gtk.Label labelSerial;
		
		private global::Gtk.Label labelType;
		
		private global::Gtk.Label labelUnit;
		
		private global::Gtk.Label labelVAT;
		
		private global::Gtk.Label labelWeight;
		
		private global::Gtk.DataBindings.DataEntryReference referenceColor;
		
		private global::Gtk.DataBindings.DataEntryReference referenceManufacturer;
		
		private global::Gtk.DataBindings.DataEntryReference referenceType;
		
		private global::Gtk.DataBindings.DataEntryReference referenceUnit;
		
		private global::Gtk.DataBindings.DataSpinButton spinDeposit;
		
		private global::Gtk.DataBindings.DataSpinButton spinWeight;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.DataBindings.DataTable datatable2;
		
		private global::Vodovoz.PricesView pricesView;
		
		private global::Gtk.Label label2;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Vodovoz.NomenclatureDlg
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Vodovoz.NomenclatureDlg";
			// Container child Vodovoz.NomenclatureDlg.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonSave = new global::Gtk.Button ();
			this.buttonSave.CanFocus = true;
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.UseUnderline = true;
			this.buttonSave.Label = global::Mono.Unix.Catalog.GetString ("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image ();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-floppy", global::Gtk.IconSize.Menu);
			this.buttonSave.Image = w1;
			this.hbox1.Add (this.buttonSave);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonSave]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString ("Отменить");
			global::Gtk.Image w3 = new global::Gtk.Image ();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w3;
			this.hbox1.Add (this.buttonCancel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonCancel]));
			w4.Position = 1;
			w4.Expand = false;
			this.vbox1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.notebook1 = new global::Gtk.Notebook ();
			this.notebook1.CanFocus = true;
			this.notebook1.Name = "notebook1";
			this.notebook1.CurrentPage = 0;
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			global::Gtk.Viewport w6 = new global::Gtk.Viewport ();
			w6.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.datatable1 = new global::Gtk.DataBindings.DataTable (((uint)(12)), ((uint)(2)), false);
			this.datatable1.Name = "datatable1";
			this.datatable1.RowSpacing = ((uint)(6));
			this.datatable1.ColumnSpacing = ((uint)(6));
			this.datatable1.BorderWidth = ((uint)(6));
			this.datatable1.InheritedDataSource = false;
			this.datatable1.InheritedBoundaryDataSource = false;
			this.datatable1.InheritedDataSource = false;
			this.datatable1.InheritedBoundaryDataSource = false;
			// Container child datatable1.Gtk.Table+TableChild
			this.checkNotReserve = new global::Gtk.DataBindings.DataCheckButton ();
			this.checkNotReserve.CanFocus = true;
			this.checkNotReserve.Name = "checkNotReserve";
			this.checkNotReserve.Label = "";
			this.checkNotReserve.DrawIndicator = true;
			this.checkNotReserve.UseUnderline = true;
			this.checkNotReserve.InheritedDataSource = true;
			this.checkNotReserve.Mappings = "DoNotReserve";
			this.checkNotReserve.InheritedBoundaryDataSource = false;
			this.checkNotReserve.Editable = true;
			this.checkNotReserve.AutomaticTitle = false;
			this.checkNotReserve.InheritedBoundaryDataSource = false;
			this.checkNotReserve.InheritedDataSource = true;
			this.checkNotReserve.Mappings = "DoNotReserve";
			this.datatable1.Add (this.checkNotReserve);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.checkNotReserve]));
			w7.TopAttach = ((uint)(10));
			w7.BottomAttach = ((uint)(11));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.checkSerial = new global::Gtk.DataBindings.DataCheckButton ();
			this.checkSerial.CanFocus = true;
			this.checkSerial.Name = "checkSerial";
			this.checkSerial.Label = "";
			this.checkSerial.DrawIndicator = true;
			this.checkSerial.UseUnderline = true;
			this.checkSerial.InheritedDataSource = true;
			this.checkSerial.Mappings = "Serial";
			this.checkSerial.InheritedBoundaryDataSource = false;
			this.checkSerial.Editable = true;
			this.checkSerial.AutomaticTitle = false;
			this.checkSerial.InheritedBoundaryDataSource = false;
			this.checkSerial.InheritedDataSource = true;
			this.checkSerial.Mappings = "Serial";
			this.datatable1.Add (this.checkSerial);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.checkSerial]));
			w8.TopAttach = ((uint)(11));
			w8.BottomAttach = ((uint)(12));
			w8.LeftAttach = ((uint)(1));
			w8.RightAttach = ((uint)(2));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.entryModel = new global::Gtk.DataBindings.DataEntry ();
			this.entryModel.CanFocus = true;
			this.entryModel.Name = "entryModel";
			this.entryModel.IsEditable = true;
			this.entryModel.InvisibleChar = '●';
			this.entryModel.InheritedDataSource = true;
			this.entryModel.Mappings = "Model";
			this.entryModel.InheritedBoundaryDataSource = false;
			this.entryModel.InheritedDataSource = true;
			this.entryModel.Mappings = "Model";
			this.entryModel.InheritedBoundaryDataSource = false;
			this.datatable1.Add (this.entryModel);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.entryModel]));
			w9.TopAttach = ((uint)(4));
			w9.BottomAttach = ((uint)(5));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.entryName = new global::Gtk.DataBindings.DataEntry ();
			this.entryName.CanFocus = true;
			this.entryName.Name = "entryName";
			this.entryName.IsEditable = true;
			this.entryName.InvisibleChar = '●';
			this.entryName.InheritedDataSource = true;
			this.entryName.Mappings = "Name";
			this.entryName.InheritedBoundaryDataSource = false;
			this.entryName.InheritedDataSource = true;
			this.entryName.Mappings = "Name";
			this.entryName.InheritedBoundaryDataSource = false;
			this.datatable1.Add (this.entryName);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.entryName]));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.enumType = new global::Gtk.DataBindings.DataEnumComboBox ();
			this.enumType.Name = "enumType";
			this.enumType.InheritedBoundaryDataSource = false;
			this.enumType.InheritedDataSource = true;
			this.enumType.Mappings = "Category";
			this.enumType.InheritedBoundaryDataSource = false;
			this.enumType.InheritedDataSource = true;
			this.enumType.Mappings = "Category";
			this.datatable1.Add (this.enumType);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.enumType]));
			w11.TopAttach = ((uint)(1));
			w11.BottomAttach = ((uint)(2));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.enumVAT = new global::Gtk.DataBindings.DataEnumComboBox ();
			this.enumVAT.Name = "enumVAT";
			this.enumVAT.InheritedBoundaryDataSource = false;
			this.enumVAT.InheritedDataSource = true;
			this.enumVAT.Mappings = "VAT";
			this.enumVAT.InheritedBoundaryDataSource = false;
			this.enumVAT.InheritedDataSource = true;
			this.enumVAT.Mappings = "VAT";
			this.datatable1.Add (this.enumVAT);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.enumVAT]));
			w12.TopAttach = ((uint)(9));
			w12.BottomAttach = ((uint)(10));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelClass = new global::Gtk.Label ();
			this.labelClass.Name = "labelClass";
			this.labelClass.Xalign = 1F;
			this.labelClass.LabelProp = global::Mono.Unix.Catalog.GetString ("Класс оборудования:");
			this.datatable1.Add (this.labelClass);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelClass]));
			w13.TopAttach = ((uint)(7));
			w13.BottomAttach = ((uint)(8));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelColor = new global::Gtk.Label ();
			this.labelColor.Name = "labelColor";
			this.labelColor.Xalign = 1F;
			this.labelColor.LabelProp = global::Mono.Unix.Catalog.GetString ("Цвет:");
			this.datatable1.Add (this.labelColor);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelColor]));
			w14.TopAttach = ((uint)(5));
			w14.BottomAttach = ((uint)(6));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelDeposit = new global::Gtk.Label ();
			this.labelDeposit.Name = "labelDeposit";
			this.labelDeposit.Xalign = 1F;
			this.labelDeposit.LabelProp = global::Mono.Unix.Catalog.GetString ("Сумма залога:");
			this.datatable1.Add (this.labelDeposit);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelDeposit]));
			w15.TopAttach = ((uint)(8));
			w15.BottomAttach = ((uint)(9));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelManufacturer = new global::Gtk.Label ();
			this.labelManufacturer.Name = "labelManufacturer";
			this.labelManufacturer.Xalign = 1F;
			this.labelManufacturer.LabelProp = global::Mono.Unix.Catalog.GetString ("Производитель:");
			this.datatable1.Add (this.labelManufacturer);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelManufacturer]));
			w16.TopAttach = ((uint)(6));
			w16.BottomAttach = ((uint)(7));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelModel = new global::Gtk.Label ();
			this.labelModel.Name = "labelModel";
			this.labelModel.Xalign = 1F;
			this.labelModel.LabelProp = global::Mono.Unix.Catalog.GetString ("Модель:");
			this.datatable1.Add (this.labelModel);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelModel]));
			w17.TopAttach = ((uint)(4));
			w17.BottomAttach = ((uint)(5));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelName = new global::Gtk.Label ();
			this.labelName.Name = "labelName";
			this.labelName.Xalign = 1F;
			this.labelName.LabelProp = global::Mono.Unix.Catalog.GetString ("Наименование:");
			this.datatable1.Add (this.labelName);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelName]));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelReserve = new global::Gtk.Label ();
			this.labelReserve.Name = "labelReserve";
			this.labelReserve.Xalign = 1F;
			this.labelReserve.LabelProp = global::Mono.Unix.Catalog.GetString ("Не резервировать:");
			this.datatable1.Add (this.labelReserve);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelReserve]));
			w19.TopAttach = ((uint)(10));
			w19.BottomAttach = ((uint)(11));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelSerial = new global::Gtk.Label ();
			this.labelSerial.Name = "labelSerial";
			this.labelSerial.Xalign = 1F;
			this.labelSerial.LabelProp = global::Mono.Unix.Catalog.GetString ("Посерийный учет:");
			this.datatable1.Add (this.labelSerial);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelSerial]));
			w20.TopAttach = ((uint)(11));
			w20.BottomAttach = ((uint)(12));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelType = new global::Gtk.Label ();
			this.labelType.Name = "labelType";
			this.labelType.Xalign = 1F;
			this.labelType.LabelProp = global::Mono.Unix.Catalog.GetString ("Тип:");
			this.datatable1.Add (this.labelType);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelType]));
			w21.TopAttach = ((uint)(1));
			w21.BottomAttach = ((uint)(2));
			w21.XOptions = ((global::Gtk.AttachOptions)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelUnit = new global::Gtk.Label ();
			this.labelUnit.Name = "labelUnit";
			this.labelUnit.Xalign = 1F;
			this.labelUnit.LabelProp = global::Mono.Unix.Catalog.GetString ("Единица измерения:");
			this.datatable1.Add (this.labelUnit);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelUnit]));
			w22.TopAttach = ((uint)(2));
			w22.BottomAttach = ((uint)(3));
			w22.XOptions = ((global::Gtk.AttachOptions)(4));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelVAT = new global::Gtk.Label ();
			this.labelVAT.Name = "labelVAT";
			this.labelVAT.Xalign = 1F;
			this.labelVAT.LabelProp = global::Mono.Unix.Catalog.GetString ("НДС:");
			this.datatable1.Add (this.labelVAT);
			global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelVAT]));
			w23.TopAttach = ((uint)(9));
			w23.BottomAttach = ((uint)(10));
			w23.XOptions = ((global::Gtk.AttachOptions)(4));
			w23.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.labelWeight = new global::Gtk.Label ();
			this.labelWeight.Name = "labelWeight";
			this.labelWeight.Xalign = 1F;
			this.labelWeight.LabelProp = global::Mono.Unix.Catalog.GetString ("Вес (кг):");
			this.datatable1.Add (this.labelWeight);
			global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.labelWeight]));
			w24.TopAttach = ((uint)(3));
			w24.BottomAttach = ((uint)(4));
			w24.XOptions = ((global::Gtk.AttachOptions)(4));
			w24.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.referenceColor = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceColor.Events = ((global::Gdk.EventMask)(256));
			this.referenceColor.Name = "referenceColor";
			this.referenceColor.DisplayFields = new string[] {
				"Name"
			};
			this.referenceColor.InheritedDataSource = true;
			this.referenceColor.Mappings = "Color";
			this.referenceColor.InheritedBoundaryDataSource = false;
			this.referenceColor.CursorPointsEveryType = false;
			this.datatable1.Add (this.referenceColor);
			global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.referenceColor]));
			w25.TopAttach = ((uint)(5));
			w25.BottomAttach = ((uint)(6));
			w25.LeftAttach = ((uint)(1));
			w25.RightAttach = ((uint)(2));
			w25.XOptions = ((global::Gtk.AttachOptions)(4));
			w25.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.referenceManufacturer = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceManufacturer.Events = ((global::Gdk.EventMask)(256));
			this.referenceManufacturer.Name = "referenceManufacturer";
			this.referenceManufacturer.DisplayFields = new string[] {
				"Name"
			};
			this.referenceManufacturer.InheritedDataSource = true;
			this.referenceManufacturer.Mappings = "Manufacturer";
			this.referenceManufacturer.InheritedBoundaryDataSource = false;
			this.referenceManufacturer.CursorPointsEveryType = false;
			this.datatable1.Add (this.referenceManufacturer);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.referenceManufacturer]));
			w26.TopAttach = ((uint)(6));
			w26.BottomAttach = ((uint)(7));
			w26.LeftAttach = ((uint)(1));
			w26.RightAttach = ((uint)(2));
			w26.XOptions = ((global::Gtk.AttachOptions)(4));
			w26.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.referenceType = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceType.Events = ((global::Gdk.EventMask)(256));
			this.referenceType.Name = "referenceType";
			this.referenceType.DisplayFields = new string[] {
				"Name"
			};
			this.referenceType.InheritedDataSource = true;
			this.referenceType.Mappings = "Type";
			this.referenceType.InheritedBoundaryDataSource = false;
			this.referenceType.CursorPointsEveryType = false;
			this.datatable1.Add (this.referenceType);
			global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.referenceType]));
			w27.TopAttach = ((uint)(7));
			w27.BottomAttach = ((uint)(8));
			w27.LeftAttach = ((uint)(1));
			w27.RightAttach = ((uint)(2));
			w27.XOptions = ((global::Gtk.AttachOptions)(4));
			w27.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.referenceUnit = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceUnit.Events = ((global::Gdk.EventMask)(256));
			this.referenceUnit.Name = "referenceUnit";
			this.referenceUnit.DisplayFields = new string[] {
				"Name"
			};
			this.referenceUnit.InheritedDataSource = true;
			this.referenceUnit.Mappings = "Unit";
			this.referenceUnit.InheritedBoundaryDataSource = false;
			this.referenceUnit.CursorPointsEveryType = false;
			this.datatable1.Add (this.referenceUnit);
			global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.referenceUnit]));
			w28.TopAttach = ((uint)(2));
			w28.BottomAttach = ((uint)(3));
			w28.LeftAttach = ((uint)(1));
			w28.RightAttach = ((uint)(2));
			w28.XOptions = ((global::Gtk.AttachOptions)(4));
			w28.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.spinDeposit = new global::Gtk.DataBindings.DataSpinButton (0, 999999, 1);
			this.spinDeposit.CanFocus = true;
			this.spinDeposit.Name = "spinDeposit";
			this.spinDeposit.Adjustment.PageIncrement = 10;
			this.spinDeposit.ClimbRate = 1;
			this.spinDeposit.Digits = ((uint)(2));
			this.spinDeposit.Numeric = true;
			this.spinDeposit.InheritedDataSource = true;
			this.spinDeposit.Mappings = "RentPrice";
			this.spinDeposit.InheritedBoundaryDataSource = false;
			this.spinDeposit.InheritedDataSource = true;
			this.spinDeposit.Mappings = "RentPrice";
			this.spinDeposit.InheritedBoundaryDataSource = false;
			this.datatable1.Add (this.spinDeposit);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.spinDeposit]));
			w29.TopAttach = ((uint)(8));
			w29.BottomAttach = ((uint)(9));
			w29.LeftAttach = ((uint)(1));
			w29.RightAttach = ((uint)(2));
			w29.XOptions = ((global::Gtk.AttachOptions)(4));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.spinWeight = new global::Gtk.DataBindings.DataSpinButton (0, 999, 1);
			this.spinWeight.CanFocus = true;
			this.spinWeight.Name = "spinWeight";
			this.spinWeight.Adjustment.PageIncrement = 10;
			this.spinWeight.ClimbRate = 1;
			this.spinWeight.Digits = ((uint)(3));
			this.spinWeight.Numeric = true;
			this.spinWeight.InheritedDataSource = true;
			this.spinWeight.Mappings = "Weight";
			this.spinWeight.InheritedBoundaryDataSource = false;
			this.spinWeight.InheritedDataSource = true;
			this.spinWeight.Mappings = "Weight";
			this.spinWeight.InheritedBoundaryDataSource = false;
			this.datatable1.Add (this.spinWeight);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.spinWeight]));
			w30.TopAttach = ((uint)(3));
			w30.BottomAttach = ((uint)(4));
			w30.LeftAttach = ((uint)(1));
			w30.RightAttach = ((uint)(2));
			w30.XOptions = ((global::Gtk.AttachOptions)(4));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			w6.Add (this.datatable1);
			this.GtkScrolledWindow.Add (w6);
			this.notebook1.Add (this.GtkScrolledWindow);
			// Notebook tab
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Информация");
			this.notebook1.SetTabLabel (this.GtkScrolledWindow, this.label1);
			this.label1.ShowAll ();
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.datatable2 = new global::Gtk.DataBindings.DataTable (((uint)(1)), ((uint)(2)), false);
			this.datatable2.Name = "datatable2";
			this.datatable2.RowSpacing = ((uint)(6));
			this.datatable2.ColumnSpacing = ((uint)(6));
			this.datatable2.InheritedDataSource = false;
			this.datatable2.InheritedBoundaryDataSource = false;
			this.datatable2.InheritedDataSource = false;
			this.datatable2.InheritedBoundaryDataSource = false;
			// Container child datatable2.Gtk.Table+TableChild
			this.pricesView = new global::Vodovoz.PricesView ();
			this.pricesView.Events = ((global::Gdk.EventMask)(256));
			this.pricesView.Name = "pricesView";
			this.datatable2.Add (this.pricesView);
			global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.datatable2 [this.pricesView]));
			w34.LeftAttach = ((uint)(1));
			w34.RightAttach = ((uint)(2));
			this.notebook1.Add (this.datatable2);
			global::Gtk.Notebook.NotebookChild w35 = ((global::Gtk.Notebook.NotebookChild)(this.notebook1 [this.datatable2]));
			w35.Position = 1;
			// Notebook tab
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Цены");
			this.notebook1.SetTabLabel (this.datatable2, this.label2);
			this.label2.ShowAll ();
			this.vbox1.Add (this.notebook1);
			global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.notebook1]));
			w36.Position = 1;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.buttonSave.Clicked += new global::System.EventHandler (this.OnButtonSaveClicked);
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.enumType.Changed += new global::System.EventHandler (this.OnEnumTypeChanged);
		}
	}
}
