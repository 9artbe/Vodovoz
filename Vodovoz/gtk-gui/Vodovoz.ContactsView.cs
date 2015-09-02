
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz
{
	public partial class ContactsView
	{
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.Label label1;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::QSOrmProject.OrmTableView datatreeviewContacts;
		
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.Button buttonAdd;
		
		private global::Gtk.Button buttonEdit;
		
		private global::Gtk.Button buttonDelete;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Vodovoz.ContactsView
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Vodovoz.ContactsView";
			// Container child Vodovoz.ContactsView.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			this.vbox1.BorderWidth = ((uint)(6));
			// Container child vbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 0F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Контактные лица контрагента");
			this.vbox1.Add (this.label1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.datatreeviewContacts = new global::QSOrmProject.OrmTableView ();
			this.datatreeviewContacts.CanFocus = true;
			this.datatreeviewContacts.Name = "datatreeviewContacts";
			this.datatreeviewContacts.CursorPointsEveryType = false;
			this.datatreeviewContacts.InheritedDataSource = false;
			this.datatreeviewContacts.InheritedBoundaryDataSource = false;
			this.datatreeviewContacts.InheritedDataSource = false;
			this.datatreeviewContacts.InheritedBoundaryDataSource = false;
			this.GtkScrolledWindow.Add (this.datatreeviewContacts);
			this.vbox1.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.GtkScrolledWindow]));
			w3.Position = 1;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonAdd = new global::Gtk.Button ();
			this.buttonAdd.CanFocus = true;
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.UseUnderline = true;
			this.buttonAdd.Label = global::Mono.Unix.Catalog.GetString ("Новый контакт");
			global::Gtk.Image w4 = new global::Gtk.Image ();
			w4.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			this.buttonAdd.Image = w4;
			this.hbox1.Add (this.buttonAdd);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonAdd]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonEdit = new global::Gtk.Button ();
			this.buttonEdit.CanFocus = true;
			this.buttonEdit.Name = "buttonEdit";
			this.buttonEdit.UseUnderline = true;
			this.buttonEdit.Label = global::Mono.Unix.Catalog.GetString ("Изменить");
			global::Gtk.Image w6 = new global::Gtk.Image ();
			w6.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-edit", global::Gtk.IconSize.Menu);
			this.buttonEdit.Image = w6;
			this.hbox1.Add (this.buttonEdit);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonEdit]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonDelete = new global::Gtk.Button ();
			this.buttonDelete.CanFocus = true;
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.UseUnderline = true;
			this.buttonDelete.Label = global::Mono.Unix.Catalog.GetString ("Удалить");
			global::Gtk.Image w8 = new global::Gtk.Image ();
			w8.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-remove", global::Gtk.IconSize.Menu);
			this.buttonDelete.Image = w8;
			this.hbox1.Add (this.buttonDelete);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonDelete]));
			w9.Position = 2;
			w9.Expand = false;
			w9.Fill = false;
			this.vbox1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
			w10.Position = 2;
			w10.Expand = false;
			w10.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.buttonAdd.Clicked += new global::System.EventHandler (this.OnButtonAddClicked);
			this.buttonEdit.Clicked += new global::System.EventHandler (this.OnButtonEditClicked);
			this.buttonDelete.Clicked += new global::System.EventHandler (this.OnButtonDeleteClicked);
		}
	}
}
