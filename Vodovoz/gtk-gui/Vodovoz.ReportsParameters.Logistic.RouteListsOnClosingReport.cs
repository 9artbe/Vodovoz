
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.ReportsParameters.Logistic
{
	public partial class RouteListsOnClosingReport
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.CheckButton buttonToDayRL;

		private global::Gtk.Button buttonCreateReport;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.ReportsParameters.Logistic.RouteListsOnClosingReport
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.ReportsParameters.Logistic.RouteListsOnClosingReport";
			// Container child Vodovoz.ReportsParameters.Logistic.RouteListsOnClosingReport.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.buttonToDayRL = new global::Gtk.CheckButton();
			this.buttonToDayRL.CanFocus = true;
			this.buttonToDayRL.Name = "buttonToDayRL";
			this.buttonToDayRL.Label = global::Mono.Unix.Catalog.GetString("Убрать сегодняшние МЛ");
			this.buttonToDayRL.DrawIndicator = true;
			this.buttonToDayRL.UseUnderline = true;
			this.vbox1.Add(this.buttonToDayRL);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.buttonToDayRL]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.buttonCreateReport = new global::Gtk.Button();
			this.buttonCreateReport.CanFocus = true;
			this.buttonCreateReport.Name = "buttonCreateReport";
			this.buttonCreateReport.UseUnderline = true;
			this.buttonCreateReport.Label = global::Mono.Unix.Catalog.GetString("Сформировать отчет");
			this.vbox1.Add(this.buttonCreateReport);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.buttonCreateReport]));
			w2.Position = 2;
			w2.Expand = false;
			w2.Fill = false;
			this.Add(this.vbox1);
			if((this.Child != null)) {
				this.Child.ShowAll();
			}
			this.Hide();
			this.buttonCreateReport.Clicked += new global::System.EventHandler(this.OnButtonCreateReportClicked);
		}
	}
}
