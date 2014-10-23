
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz
{
	public partial class CounterpartyDlg
	{
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.Button buttonSave;
		
		private global::Gtk.Button buttonCancel;
		
		private global::Gtk.VSeparator vseparator1;
		
		private global::Gtk.RadioButton radioInfo;
		
		private global::Gtk.RadioButton radioContacts;
		
		private global::Gtk.RadioButton radioDetails;
		
		private global::Gtk.RadioButton radioCurators;
		
		private global::Gtk.Notebook notebook1;
		
		private global::Gtk.DataBindings.DataTable datatable1;
		
		private global::Gtk.DataBindings.DataEntry entryFullName;
		
		private global::Gtk.DataBindings.DataEntry entryName;
		
		private global::Gtk.DataBindings.DataEnumComboBox enumCounterpartyType;
		
		private global::Gtk.DataBindings.DataEnumComboBox enumPayment;
		
		private global::Gtk.DataBindings.DataEnumComboBox enumPersonType;
		
		private global::Gtk.Label label15;
		
		private global::Gtk.Label label16;
		
		private global::Gtk.Label label17;
		
		private global::Gtk.Label label3;
		
		private global::Gtk.Label label4;
		
		private global::Gtk.Label label5;
		
		private global::Gtk.Label label6;
		
		private global::Gtk.Label label7;
		
		private global::Gtk.Label label8;
		
		private global::Gtk.Label label9;
		
		private global::Gtk.DataBindings.DataEntryReference referenceSignificance;
		
		private global::Gtk.DataBindings.DataEntryReference referenceStatus;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.DataBindings.DataTable datatable2;
		
		private global::Gtk.Label label11;
		
		private global::Gtk.Label label12;
		
		private global::Gtk.Label label13;
		
		private global::Gtk.Label label18;
		
		private global::Gtk.Label label19;
		
		private global::QSPhones.PhonesView phonesView;
		
		private global::Gtk.Label label20;
		
		private global::Gtk.Label label21;
		
		private global::Gtk.DataBindings.DataTable datatable3;
		
		private global::Gtk.Label label14;
		
		private global::Gtk.Label label22;
		
		private global::Gtk.Label label23;
		
		private global::Gtk.DataBindings.DataEntryReference referenceAccountant;
		
		private global::Gtk.DataBindings.DataEntryReference referenceBottleManager;
		
		private global::Gtk.DataBindings.DataEntryReference referenceSalesManager;
		
		private global::Gtk.Label label10;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Vodovoz.CounterpartyDlg
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Vodovoz.CounterpartyDlg";
			// Container child Vodovoz.CounterpartyDlg.Gtk.Container+ContainerChild
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
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString ("Отмена");
			global::Gtk.Image w3 = new global::Gtk.Image ();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w3;
			this.hbox1.Add (this.buttonCancel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonCancel]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vseparator1 = new global::Gtk.VSeparator ();
			this.vseparator1.Name = "vseparator1";
			this.hbox1.Add (this.vseparator1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vseparator1]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.radioInfo = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("Информация"));
			this.radioInfo.CanFocus = true;
			this.radioInfo.Name = "radioInfo";
			this.radioInfo.DrawIndicator = false;
			this.radioInfo.UseUnderline = true;
			this.radioInfo.Group = new global::GLib.SList (global::System.IntPtr.Zero);
			this.hbox1.Add (this.radioInfo);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.radioInfo]));
			w6.Position = 3;
			// Container child hbox1.Gtk.Box+BoxChild
			this.radioContacts = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("Контакты"));
			this.radioContacts.CanFocus = true;
			this.radioContacts.Name = "radioContacts";
			this.radioContacts.DrawIndicator = false;
			this.radioContacts.UseUnderline = true;
			this.radioContacts.Group = this.radioInfo.Group;
			this.hbox1.Add (this.radioContacts);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.radioContacts]));
			w7.Position = 4;
			// Container child hbox1.Gtk.Box+BoxChild
			this.radioDetails = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("Реквизиты"));
			this.radioDetails.CanFocus = true;
			this.radioDetails.Name = "radioDetails";
			this.radioDetails.DrawIndicator = false;
			this.radioDetails.UseUnderline = true;
			this.radioDetails.Group = this.radioInfo.Group;
			this.hbox1.Add (this.radioDetails);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.radioDetails]));
			w8.Position = 5;
			// Container child hbox1.Gtk.Box+BoxChild
			this.radioCurators = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("Кураторы"));
			this.radioCurators.CanFocus = true;
			this.radioCurators.Name = "radioCurators";
			this.radioCurators.DrawIndicator = false;
			this.radioCurators.UseUnderline = true;
			this.radioCurators.Group = this.radioInfo.Group;
			this.hbox1.Add (this.radioCurators);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.radioCurators]));
			w9.Position = 6;
			this.vbox1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.notebook1 = new global::Gtk.Notebook ();
			this.notebook1.CanFocus = true;
			this.notebook1.Name = "notebook1";
			this.notebook1.CurrentPage = 0;
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.datatable1 = new global::Gtk.DataBindings.DataTable (((uint)(10)), ((uint)(2)), false);
			this.datatable1.Name = "datatable1";
			this.datatable1.RowSpacing = ((uint)(6));
			this.datatable1.ColumnSpacing = ((uint)(6));
			this.datatable1.InheritedDataSource = false;
			this.datatable1.InheritedBoundaryDataSource = false;
			this.datatable1.InheritedDataSource = false;
			this.datatable1.InheritedBoundaryDataSource = false;
			// Container child datatable1.Gtk.Table+TableChild
			this.entryFullName = new global::Gtk.DataBindings.DataEntry ();
			this.entryFullName.CanFocus = true;
			this.entryFullName.Name = "entryFullName";
			this.entryFullName.IsEditable = true;
			this.entryFullName.InvisibleChar = '●';
			this.entryFullName.InheritedDataSource = true;
			this.entryFullName.Mappings = "FullName";
			this.entryFullName.InheritedBoundaryDataSource = false;
			this.entryFullName.InheritedDataSource = true;
			this.entryFullName.Mappings = "FullName";
			this.entryFullName.InheritedBoundaryDataSource = false;
			this.entryFullName.Editable = false;
			this.datatable1.Add (this.entryFullName);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.entryFullName]));
			w11.TopAttach = ((uint)(1));
			w11.BottomAttach = ((uint)(2));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
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
			this.entryName.Editable = false;
			this.datatable1.Add (this.entryName);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.entryName]));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.enumCounterpartyType = new global::Gtk.DataBindings.DataEnumComboBox ();
			this.enumCounterpartyType.Name = "enumCounterpartyType";
			this.enumCounterpartyType.InheritedBoundaryDataSource = false;
			this.enumCounterpartyType.InheritedDataSource = true;
			this.enumCounterpartyType.Mappings = "CounterpartyType";
			this.enumCounterpartyType.InheritedBoundaryDataSource = false;
			this.enumCounterpartyType.InheritedDataSource = true;
			this.enumCounterpartyType.Mappings = "CounterpartyType";
			this.datatable1.Add (this.enumCounterpartyType);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.enumCounterpartyType]));
			w13.TopAttach = ((uint)(6));
			w13.BottomAttach = ((uint)(7));
			w13.LeftAttach = ((uint)(1));
			w13.RightAttach = ((uint)(2));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.enumPayment = new global::Gtk.DataBindings.DataEnumComboBox ();
			this.enumPayment.Name = "enumPayment";
			this.enumPayment.InheritedBoundaryDataSource = false;
			this.enumPayment.InheritedDataSource = false;
			this.enumPayment.Mappings = "PaymentMethod";
			this.enumPayment.InheritedBoundaryDataSource = false;
			this.enumPayment.InheritedDataSource = false;
			this.enumPayment.Mappings = "PaymentMethod";
			this.datatable1.Add (this.enumPayment);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.enumPayment]));
			w14.TopAttach = ((uint)(8));
			w14.BottomAttach = ((uint)(9));
			w14.LeftAttach = ((uint)(1));
			w14.RightAttach = ((uint)(2));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.enumPersonType = new global::Gtk.DataBindings.DataEnumComboBox ();
			this.enumPersonType.Name = "enumPersonType";
			this.enumPersonType.InheritedBoundaryDataSource = false;
			this.enumPersonType.InheritedDataSource = true;
			this.enumPersonType.Mappings = "PersonType";
			this.enumPersonType.InheritedBoundaryDataSource = false;
			this.enumPersonType.InheritedDataSource = true;
			this.enumPersonType.Mappings = "PersonType";
			this.datatable1.Add (this.enumPersonType);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.enumPersonType]));
			w15.TopAttach = ((uint)(5));
			w15.BottomAttach = ((uint)(6));
			w15.LeftAttach = ((uint)(1));
			w15.RightAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label15 = new global::Gtk.Label ();
			this.label15.Name = "label15";
			this.label15.Xalign = 1F;
			this.label15.LabelProp = global::Mono.Unix.Catalog.GetString ("Комментарий:");
			this.datatable1.Add (this.label15);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label15]));
			w16.TopAttach = ((uint)(7));
			w16.BottomAttach = ((uint)(8));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label16 = new global::Gtk.Label ();
			this.label16.Name = "label16";
			this.label16.Xalign = 1F;
			this.label16.LabelProp = global::Mono.Unix.Catalog.GetString ("Форма оплаты \n(по умолчанию):");
			this.datatable1.Add (this.label16);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label16]));
			w17.TopAttach = ((uint)(8));
			w17.BottomAttach = ((uint)(9));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label17 = new global::Gtk.Label ();
			this.label17.Name = "label17";
			this.label17.Xalign = 1F;
			this.label17.LabelProp = global::Mono.Unix.Catalog.GetString ("Максимальная \nсумма кредита:");
			this.datatable1.Add (this.label17);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label17]));
			w18.TopAttach = ((uint)(9));
			w18.BottomAttach = ((uint)(10));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Наименование:");
			this.datatable1.Add (this.label3);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label3]));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Полное наименование:");
			this.datatable1.Add (this.label4);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label4]));
			w20.TopAttach = ((uint)(1));
			w20.BottomAttach = ((uint)(2));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Статус:");
			this.datatable1.Add (this.label5);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label5]));
			w21.TopAttach = ((uint)(2));
			w21.BottomAttach = ((uint)(3));
			w21.XOptions = ((global::Gtk.AttachOptions)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Значимость:");
			this.datatable1.Add (this.label6);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label6]));
			w22.TopAttach = ((uint)(3));
			w22.BottomAttach = ((uint)(4));
			w22.XOptions = ((global::Gtk.AttachOptions)(4));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("Основная организация:");
			this.datatable1.Add (this.label7);
			global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label7]));
			w23.TopAttach = ((uint)(4));
			w23.BottomAttach = ((uint)(5));
			w23.XOptions = ((global::Gtk.AttachOptions)(4));
			w23.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label ();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = global::Mono.Unix.Catalog.GetString ("Форма контрагента:");
			this.datatable1.Add (this.label8);
			global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label8]));
			w24.TopAttach = ((uint)(5));
			w24.BottomAttach = ((uint)(6));
			w24.XOptions = ((global::Gtk.AttachOptions)(4));
			w24.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.label9 = new global::Gtk.Label ();
			this.label9.Name = "label9";
			this.label9.Xalign = 1F;
			this.label9.LabelProp = global::Mono.Unix.Catalog.GetString ("Тип контрагента:");
			this.datatable1.Add (this.label9);
			global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.label9]));
			w25.TopAttach = ((uint)(6));
			w25.BottomAttach = ((uint)(7));
			w25.XOptions = ((global::Gtk.AttachOptions)(4));
			w25.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.referenceSignificance = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceSignificance.Events = ((global::Gdk.EventMask)(256));
			this.referenceSignificance.Name = "referenceSignificance";
			this.referenceSignificance.DisplayFields = new string[] {
				"Name"
			};
			this.referenceSignificance.InheritedDataSource = true;
			this.referenceSignificance.Mappings = "Significance";
			this.referenceSignificance.InheritedBoundaryDataSource = false;
			this.referenceSignificance.CursorPointsEveryType = false;
			this.datatable1.Add (this.referenceSignificance);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.referenceSignificance]));
			w26.TopAttach = ((uint)(3));
			w26.BottomAttach = ((uint)(4));
			w26.LeftAttach = ((uint)(1));
			w26.RightAttach = ((uint)(2));
			w26.XOptions = ((global::Gtk.AttachOptions)(4));
			w26.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable1.Gtk.Table+TableChild
			this.referenceStatus = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceStatus.Events = ((global::Gdk.EventMask)(256));
			this.referenceStatus.Name = "referenceStatus";
			this.referenceStatus.DisplayFields = new string[] {
				"Name"
			};
			this.referenceStatus.InheritedDataSource = true;
			this.referenceStatus.Mappings = "Status";
			this.referenceStatus.InheritedBoundaryDataSource = false;
			this.referenceStatus.CursorPointsEveryType = false;
			this.datatable1.Add (this.referenceStatus);
			global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.datatable1 [this.referenceStatus]));
			w27.TopAttach = ((uint)(2));
			w27.BottomAttach = ((uint)(3));
			w27.LeftAttach = ((uint)(1));
			w27.RightAttach = ((uint)(2));
			w27.XOptions = ((global::Gtk.AttachOptions)(4));
			w27.YOptions = ((global::Gtk.AttachOptions)(4));
			this.notebook1.Add (this.datatable1);
			// Notebook tab
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Информация");
			this.notebook1.SetTabLabel (this.datatable1, this.label1);
			this.label1.ShowAll ();
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.datatable2 = new global::Gtk.DataBindings.DataTable (((uint)(5)), ((uint)(2)), false);
			this.datatable2.Name = "datatable2";
			this.datatable2.RowSpacing = ((uint)(6));
			this.datatable2.ColumnSpacing = ((uint)(6));
			this.datatable2.InheritedDataSource = false;
			this.datatable2.InheritedBoundaryDataSource = false;
			this.datatable2.InheritedDataSource = false;
			this.datatable2.InheritedBoundaryDataSource = false;
			// Container child datatable2.Gtk.Table+TableChild
			this.label11 = new global::Gtk.Label ();
			this.label11.Name = "label11";
			this.label11.Xalign = 1F;
			this.label11.LabelProp = global::Mono.Unix.Catalog.GetString ("Общий e-mail:");
			this.datatable2.Add (this.label11);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.datatable2 [this.label11]));
			w29.TopAttach = ((uint)(1));
			w29.BottomAttach = ((uint)(2));
			w29.XOptions = ((global::Gtk.AttachOptions)(4));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable2.Gtk.Table+TableChild
			this.label12 = new global::Gtk.Label ();
			this.label12.Name = "label12";
			this.label12.Xalign = 1F;
			this.label12.LabelProp = global::Mono.Unix.Catalog.GetString ("Главное контактное лицо:");
			this.datatable2.Add (this.label12);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.datatable2 [this.label12]));
			w30.TopAttach = ((uint)(2));
			w30.BottomAttach = ((uint)(3));
			w30.XOptions = ((global::Gtk.AttachOptions)(4));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable2.Gtk.Table+TableChild
			this.label13 = new global::Gtk.Label ();
			this.label13.Name = "label13";
			this.label13.Xalign = 1F;
			this.label13.LabelProp = global::Mono.Unix.Catalog.GetString ("Контактное лицо по \nфинансовым вопросам:");
			this.label13.UseMarkup = true;
			this.datatable2.Add (this.label13);
			global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.datatable2 [this.label13]));
			w31.TopAttach = ((uint)(3));
			w31.BottomAttach = ((uint)(4));
			w31.XOptions = ((global::Gtk.AttachOptions)(4));
			w31.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable2.Gtk.Table+TableChild
			this.label18 = new global::Gtk.Label ();
			this.label18.Name = "label18";
			this.label18.Xalign = 1F;
			this.label18.LabelProp = global::Mono.Unix.Catalog.GetString ("Общие телефоны:");
			this.datatable2.Add (this.label18);
			global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.datatable2 [this.label18]));
			w32.XOptions = ((global::Gtk.AttachOptions)(4));
			w32.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable2.Gtk.Table+TableChild
			this.label19 = new global::Gtk.Label ();
			this.label19.Name = "label19";
			this.label19.Xalign = 1F;
			this.label19.LabelProp = global::Mono.Unix.Catalog.GetString ("Адрес отправки счета:");
			this.datatable2.Add (this.label19);
			global::Gtk.Table.TableChild w33 = ((global::Gtk.Table.TableChild)(this.datatable2 [this.label19]));
			w33.TopAttach = ((uint)(4));
			w33.BottomAttach = ((uint)(5));
			w33.XOptions = ((global::Gtk.AttachOptions)(4));
			w33.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable2.Gtk.Table+TableChild
			this.phonesView = new global::QSPhones.PhonesView ();
			this.phonesView.Events = ((global::Gdk.EventMask)(256));
			this.phonesView.Name = "phonesView";
			this.datatable2.Add (this.phonesView);
			global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.datatable2 [this.phonesView]));
			w34.LeftAttach = ((uint)(1));
			w34.RightAttach = ((uint)(2));
			w34.XOptions = ((global::Gtk.AttachOptions)(4));
			w34.YOptions = ((global::Gtk.AttachOptions)(4));
			this.notebook1.Add (this.datatable2);
			global::Gtk.Notebook.NotebookChild w35 = ((global::Gtk.Notebook.NotebookChild)(this.notebook1 [this.datatable2]));
			w35.Position = 1;
			// Notebook tab
			this.label20 = new global::Gtk.Label ();
			this.label20.Name = "label20";
			this.label20.LabelProp = global::Mono.Unix.Catalog.GetString ("Контакты");
			this.notebook1.SetTabLabel (this.datatable2, this.label20);
			this.label20.ShowAll ();
			// Notebook tab
			global::Gtk.Label w36 = new global::Gtk.Label ();
			w36.Visible = true;
			this.notebook1.Add (w36);
			this.label21 = new global::Gtk.Label ();
			this.label21.Name = "label21";
			this.label21.LabelProp = global::Mono.Unix.Catalog.GetString ("Реквизиты");
			this.notebook1.SetTabLabel (w36, this.label21);
			this.label21.ShowAll ();
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.datatable3 = new global::Gtk.DataBindings.DataTable (((uint)(3)), ((uint)(2)), false);
			this.datatable3.Name = "datatable3";
			this.datatable3.RowSpacing = ((uint)(6));
			this.datatable3.ColumnSpacing = ((uint)(6));
			this.datatable3.InheritedDataSource = false;
			this.datatable3.InheritedBoundaryDataSource = false;
			this.datatable3.InheritedDataSource = false;
			this.datatable3.InheritedBoundaryDataSource = false;
			// Container child datatable3.Gtk.Table+TableChild
			this.label14 = new global::Gtk.Label ();
			this.label14.Name = "label14";
			this.label14.Xalign = 1F;
			this.label14.LabelProp = global::Mono.Unix.Catalog.GetString ("Менеджер по продажам:");
			this.datatable3.Add (this.label14);
			global::Gtk.Table.TableChild w37 = ((global::Gtk.Table.TableChild)(this.datatable3 [this.label14]));
			w37.XOptions = ((global::Gtk.AttachOptions)(4));
			w37.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable3.Gtk.Table+TableChild
			this.label22 = new global::Gtk.Label ();
			this.label22.Name = "label22";
			this.label22.Xalign = 1F;
			this.label22.LabelProp = global::Mono.Unix.Catalog.GetString ("Бухгалтер:");
			this.datatable3.Add (this.label22);
			global::Gtk.Table.TableChild w38 = ((global::Gtk.Table.TableChild)(this.datatable3 [this.label22]));
			w38.TopAttach = ((uint)(1));
			w38.BottomAttach = ((uint)(2));
			w38.XOptions = ((global::Gtk.AttachOptions)(4));
			w38.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable3.Gtk.Table+TableChild
			this.label23 = new global::Gtk.Label ();
			this.label23.Name = "label23";
			this.label23.Xalign = 1F;
			this.label23.LabelProp = global::Mono.Unix.Catalog.GetString ("Менеджер по бутылям:");
			this.datatable3.Add (this.label23);
			global::Gtk.Table.TableChild w39 = ((global::Gtk.Table.TableChild)(this.datatable3 [this.label23]));
			w39.TopAttach = ((uint)(2));
			w39.BottomAttach = ((uint)(3));
			w39.XOptions = ((global::Gtk.AttachOptions)(4));
			w39.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable3.Gtk.Table+TableChild
			this.referenceAccountant = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceAccountant.Events = ((global::Gdk.EventMask)(256));
			this.referenceAccountant.Name = "referenceAccountant";
			this.referenceAccountant.DisplayFields = new string[] {
				"Name LastName Patronymic"
			};
			this.referenceAccountant.DisplayFormatString = "{0} {1} {2}";
			this.referenceAccountant.InheritedDataSource = true;
			this.referenceAccountant.Mappings = "Accountant";
			this.referenceAccountant.InheritedBoundaryDataSource = false;
			this.referenceAccountant.CursorPointsEveryType = false;
			this.datatable3.Add (this.referenceAccountant);
			global::Gtk.Table.TableChild w40 = ((global::Gtk.Table.TableChild)(this.datatable3 [this.referenceAccountant]));
			w40.TopAttach = ((uint)(1));
			w40.BottomAttach = ((uint)(2));
			w40.LeftAttach = ((uint)(1));
			w40.RightAttach = ((uint)(2));
			w40.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable3.Gtk.Table+TableChild
			this.referenceBottleManager = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceBottleManager.Events = ((global::Gdk.EventMask)(256));
			this.referenceBottleManager.Name = "referenceBottleManager";
			this.referenceBottleManager.DisplayFields = new string[] {
				"Name Lastname Patronymic"
			};
			this.referenceBottleManager.DisplayFormatString = "{0} {1} {2}";
			this.referenceBottleManager.InheritedDataSource = true;
			this.referenceBottleManager.Mappings = "BottlesManager";
			this.referenceBottleManager.InheritedBoundaryDataSource = false;
			this.referenceBottleManager.CursorPointsEveryType = false;
			this.datatable3.Add (this.referenceBottleManager);
			global::Gtk.Table.TableChild w41 = ((global::Gtk.Table.TableChild)(this.datatable3 [this.referenceBottleManager]));
			w41.TopAttach = ((uint)(2));
			w41.BottomAttach = ((uint)(3));
			w41.LeftAttach = ((uint)(1));
			w41.RightAttach = ((uint)(2));
			w41.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatable3.Gtk.Table+TableChild
			this.referenceSalesManager = new global::Gtk.DataBindings.DataEntryReference ();
			this.referenceSalesManager.Events = ((global::Gdk.EventMask)(256));
			this.referenceSalesManager.Name = "referenceSalesManager";
			this.referenceSalesManager.DisplayFields = new string[] {
				"Name LastName Patronymic"
			};
			this.referenceSalesManager.DisplayFormatString = "{0} {1} {2}";
			this.referenceSalesManager.InheritedDataSource = true;
			this.referenceSalesManager.Mappings = "SalesManager";
			this.referenceSalesManager.InheritedBoundaryDataSource = false;
			this.referenceSalesManager.CursorPointsEveryType = false;
			this.datatable3.Add (this.referenceSalesManager);
			global::Gtk.Table.TableChild w42 = ((global::Gtk.Table.TableChild)(this.datatable3 [this.referenceSalesManager]));
			w42.LeftAttach = ((uint)(1));
			w42.RightAttach = ((uint)(2));
			w42.YOptions = ((global::Gtk.AttachOptions)(4));
			this.notebook1.Add (this.datatable3);
			global::Gtk.Notebook.NotebookChild w43 = ((global::Gtk.Notebook.NotebookChild)(this.notebook1 [this.datatable3]));
			w43.Position = 3;
			// Notebook tab
			this.label10 = new global::Gtk.Label ();
			this.label10.Name = "label10";
			this.label10.LabelProp = global::Mono.Unix.Catalog.GetString ("Кураторы");
			this.notebook1.SetTabLabel (this.datatable3, this.label10);
			this.label10.ShowAll ();
			this.vbox1.Add (this.notebook1);
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.notebook1]));
			w44.Position = 1;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.buttonSave.Clicked += new global::System.EventHandler (this.OnButtonSaveClicked);
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.radioInfo.Toggled += new global::System.EventHandler (this.OnRadioInfoToggled);
			this.radioContacts.Toggled += new global::System.EventHandler (this.OnRadioContactsToggled);
			this.radioDetails.Toggled += new global::System.EventHandler (this.OnRadioDetailsToggled);
			this.radioCurators.Toggled += new global::System.EventHandler (this.OnRadioCuratorsToggled);
		}
	}
}
