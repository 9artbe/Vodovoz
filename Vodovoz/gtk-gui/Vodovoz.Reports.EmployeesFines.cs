
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Reports
{
	public partial class EmployeesFines
	{
		private global::Gtk.Table table1;
		
		private global::Gtk.Button buttonRun;
		
		private global::QSWidgetLib.DatePeriodPicker dateperiodpicker1;
		
		private global::Gtk.Label label1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Vodovoz.Reports.EmployeesFines
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Vodovoz.Reports.EmployeesFines";
			// Container child Vodovoz.Reports.EmployeesFines.Gtk.Container+ContainerChild
			this.table1 = new global::Gtk.Table (((uint)(6)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.table1.BorderWidth = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.buttonRun = new global::Gtk.Button ();
			this.buttonRun.Sensitive = false;
			this.buttonRun.CanFocus = true;
			this.buttonRun.Name = "buttonRun";
			this.buttonRun.UseUnderline = true;
			this.buttonRun.Label = global::Mono.Unix.Catalog.GetString ("Сформировать отчет");
			this.table1.Add (this.buttonRun);
			global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1 [this.buttonRun]));
			w1.TopAttach = ((uint)(5));
			w1.BottomAttach = ((uint)(6));
			w1.RightAttach = ((uint)(2));
			w1.XOptions = ((global::Gtk.AttachOptions)(0));
			w1.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.dateperiodpicker1 = new global::QSWidgetLib.DatePeriodPicker ();
			this.dateperiodpicker1.Events = ((global::Gdk.EventMask)(256));
			this.dateperiodpicker1.Name = "dateperiodpicker1";
			this.dateperiodpicker1.StartDate = new global::System.DateTime (0);
			this.dateperiodpicker1.EndDate = new global::System.DateTime (0);
			this.table1.Add (this.dateperiodpicker1);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this.dateperiodpicker1]));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Период:");
			this.table1.Add (this.label1);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			this.Add (this.table1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.dateperiodpicker1.PeriodChanged += new global::System.EventHandler (this.OnDateperiodpicker1PeriodChanged);
			this.buttonRun.Clicked += new global::System.EventHandler (this.OnButtonRunClicked);
		}
	}
}
