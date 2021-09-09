
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Views.Employees
{
	public partial class PremiumRaskatGAZelleView
	{
		private global::Gtk.VBox vbox3;

		private global::Gtk.HBox hbox5;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Table tablePremium;

		private global::Gtk.Label label1;

		private global::Gtk.Label label2;

		private global::Gtk.Label label3;

		private global::Gtk.Label label4;

		private global::Gtk.Label label7;

		private global::Gamma.GtkWidgets.yLabel ylabelAuthor;

		private global::Gamma.GtkWidgets.yLabel ylabelDate;

		private global::Gamma.GtkWidgets.yLabel ylabelMoney;

		private global::Gamma.GtkWidgets.yLabel ylabelPremiumEmployee;

		private global::Gamma.GtkWidgets.yLabel ylabelReason;

		private global::Gtk.HBox hbox7;

		private global::Gamma.GtkWidgets.yLabel ylabelTotal;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.Views.Employees.PremiumRaskatGAZelleView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.Views.Employees.PremiumRaskatGAZelleView";
			// Container child Vodovoz.Views.Employees.PremiumRaskatGAZelleView.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox5 = new global::Gtk.HBox();
			this.hbox5.Name = "hbox5";
			this.hbox5.Spacing = 6;
			// Container child hbox5.Gtk.Box+BoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString("Отменить");
			global::Gtk.Image w1 = new global::Gtk.Image();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w1;
			this.hbox5.Add(this.buttonCancel);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox5[this.buttonCancel]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			this.vbox3.Add(this.hbox5);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hbox5]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.tablePremium = new global::Gtk.Table(((uint)(5)), ((uint)(2)), false);
			this.tablePremium.Name = "tablePremium";
			this.tablePremium.RowSpacing = ((uint)(6));
			this.tablePremium.ColumnSpacing = ((uint)(6));
			// Container child tablePremium.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Дата:");
			this.tablePremium.Add(this.label1);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.label1]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 0F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Премированный сотрудник:");
			this.tablePremium.Add(this.label2);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.label2]));
			w5.TopAttach = ((uint)(4));
			w5.BottomAttach = ((uint)(5));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Основание:");
			this.tablePremium.Add(this.label3);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.label3]));
			w6.TopAttach = ((uint)(3));
			w6.BottomAttach = ((uint)(4));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("Сумма премии:");
			this.tablePremium.Add(this.label4);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.label4]));
			w7.TopAttach = ((uint)(2));
			w7.BottomAttach = ((uint)(3));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString("Премию выдал:");
			this.tablePremium.Add(this.label7);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.label7]));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.ylabelAuthor = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelAuthor.Name = "ylabelAuthor";
			this.ylabelAuthor.Xalign = 0F;
			this.ylabelAuthor.LabelProp = global::Mono.Unix.Catalog.GetString("ylabelAuthor");
			this.tablePremium.Add(this.ylabelAuthor);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.ylabelAuthor]));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.ylabelDate = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelDate.Name = "ylabelDate";
			this.ylabelDate.Xalign = 0F;
			this.ylabelDate.LabelProp = global::Mono.Unix.Catalog.GetString("ylabelDate");
			this.tablePremium.Add(this.ylabelDate);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.ylabelDate]));
			w10.TopAttach = ((uint)(1));
			w10.BottomAttach = ((uint)(2));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.ylabelMoney = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelMoney.Name = "ylabelMoney";
			this.ylabelMoney.Xalign = 0F;
			this.ylabelMoney.LabelProp = global::Mono.Unix.Catalog.GetString("ylabelMoney");
			this.tablePremium.Add(this.ylabelMoney);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.ylabelMoney]));
			w11.TopAttach = ((uint)(2));
			w11.BottomAttach = ((uint)(3));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.ylabelPremiumEmployee = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelPremiumEmployee.Name = "ylabelPremiumEmployee";
			this.ylabelPremiumEmployee.Xalign = 0F;
			this.ylabelPremiumEmployee.LabelProp = global::Mono.Unix.Catalog.GetString("ylabelPremiumEmployee");
			this.tablePremium.Add(this.ylabelPremiumEmployee);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.ylabelPremiumEmployee]));
			w12.TopAttach = ((uint)(4));
			w12.BottomAttach = ((uint)(5));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child tablePremium.Gtk.Table+TableChild
			this.ylabelReason = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelReason.Name = "ylabelReason";
			this.ylabelReason.Xalign = 0F;
			this.ylabelReason.LabelProp = global::Mono.Unix.Catalog.GetString("ylabelReason");
			this.tablePremium.Add(this.ylabelReason);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.tablePremium[this.ylabelReason]));
			w13.TopAttach = ((uint)(3));
			w13.BottomAttach = ((uint)(4));
			w13.LeftAttach = ((uint)(1));
			w13.RightAttach = ((uint)(2));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox3.Add(this.tablePremium);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.tablePremium]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox7 = new global::Gtk.HBox();
			this.hbox7.Name = "hbox7";
			this.hbox7.Spacing = 6;
			// Container child hbox7.Gtk.Box+BoxChild
			this.ylabelTotal = new global::Gamma.GtkWidgets.yLabel();
			this.ylabelTotal.Name = "ylabelTotal";
			this.hbox7.Add(this.ylabelTotal);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.ylabelTotal]));
			w15.PackType = ((global::Gtk.PackType)(1));
			w15.Position = 0;
			w15.Expand = false;
			w15.Fill = false;
			this.vbox3.Add(this.hbox7);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hbox7]));
			w16.Position = 2;
			w16.Expand = false;
			w16.Fill = false;
			this.Add(this.vbox3);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
