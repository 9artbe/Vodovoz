
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Views.Complaints
{
	public partial class GuiltyItemView
	{
		private global::Gtk.VBox vbxCreateGuilty;

		private global::Gamma.Widgets.yEnumComboBox yEnumGuiltyType;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry entVmEmployee;

		private global::Gamma.Widgets.ySpecComboBox yCmbSubdivision;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.Views.Complaints.GuiltyItemView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.Views.Complaints.GuiltyItemView";
			// Container child Vodovoz.Views.Complaints.GuiltyItemView.Gtk.Container+ContainerChild
			this.vbxCreateGuilty = new global::Gtk.VBox();
			this.vbxCreateGuilty.Name = "vbxCreateGuilty";
			this.vbxCreateGuilty.Spacing = 6;
			// Container child vbxCreateGuilty.Gtk.Box+BoxChild
			this.yEnumGuiltyType = new global::Gamma.Widgets.yEnumComboBox();
			this.yEnumGuiltyType.Name = "yEnumGuiltyType";
			this.yEnumGuiltyType.ShowSpecialStateAll = true;
			this.yEnumGuiltyType.ShowSpecialStateNot = false;
			this.yEnumGuiltyType.UseShortTitle = false;
			this.yEnumGuiltyType.DefaultFirst = false;
			this.vbxCreateGuilty.Add(this.yEnumGuiltyType);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbxCreateGuilty[this.yEnumGuiltyType]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbxCreateGuilty.Gtk.Box+BoxChild
			this.entVmEmployee = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.entVmEmployee.Events = ((global::Gdk.EventMask)(256));
			this.entVmEmployee.Name = "entVmEmployee";
			this.entVmEmployee.CanEditReference = false;
			this.vbxCreateGuilty.Add(this.entVmEmployee);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbxCreateGuilty[this.entVmEmployee]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbxCreateGuilty.Gtk.Box+BoxChild
			this.yCmbSubdivision = new global::Gamma.Widgets.ySpecComboBox();
			this.yCmbSubdivision.Name = "yCmbSubdivision";
			this.yCmbSubdivision.AddIfNotExist = false;
			this.yCmbSubdivision.DefaultFirst = false;
			this.yCmbSubdivision.ShowSpecialStateAll = false;
			this.yCmbSubdivision.ShowSpecialStateNot = false;
			this.yCmbSubdivision.NameForSpecialStateNot = "Выберите отдел";
			this.vbxCreateGuilty.Add(this.yCmbSubdivision);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbxCreateGuilty[this.yCmbSubdivision]));
			w3.Position = 2;
			w3.Expand = false;
			w3.Fill = false;
			this.Add(this.vbxCreateGuilty);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
