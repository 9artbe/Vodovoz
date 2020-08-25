
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Views.Mango
{
	public partial class FullIncomingCallView
	{
		private global::Gtk.VBox MainVbox;

		private global::Gtk.HBox hbox2;

		private global::Gamma.GtkWidgets.yButton RollUpButton;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Label Label1_Conversation;

		private global::Gamma.GtkWidgets.yLabel CallNumberYLabel;

		private global::Gamma.GtkWidgets.yLabel TimerYLabel;

		private global::Gtk.HBox hbox3;

		private global::Gamma.GtkWidgets.yButton NewClientButton;

		private global::Gamma.GtkWidgets.yButton ExistingClientButton;

		private global::Gtk.HBox WidgetPlaceBox;

		private global::Gtk.Table table1;

		private global::Gtk.Notebook WidgetPlace;

		private global::Gtk.VBox vbox6;

		private global::Gamma.GtkWidgets.yButton NewOrderButton;

		private global::Gtk.Table table5;

		private global::Gtk.Button BottleButton;

		private global::Gtk.Button ComplaintButton;

		private global::Gtk.Button CostAndDeliveryIntervalButton;

		private global::Gtk.Button StockBalanceButton;

		private global::Gtk.Table table4;

		private global::Gamma.GtkWidgets.yButton FinishButton;

		private global::Gamma.GtkWidgets.yButton ForwardingToConsultationButton;

		private global::Gamma.GtkWidgets.yButton ForwaringButton;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.Views.Mango.FullIncomingCallView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.Views.Mango.FullIncomingCallView";
			// Container child Vodovoz.Views.Mango.FullIncomingCallView.Gtk.Container+ContainerChild
			this.MainVbox = new global::Gtk.VBox();
			this.MainVbox.Name = "MainVbox";
			this.MainVbox.Spacing = 6;
			this.MainVbox.BorderWidth = ((uint)(9));
			// Container child MainVbox.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.RollUpButton = new global::Gamma.GtkWidgets.yButton();
			this.RollUpButton.CanFocus = true;
			this.RollUpButton.Name = "RollUpButton";
			this.RollUpButton.UseUnderline = true;
			this.RollUpButton.Label = global::Mono.Unix.Catalog.GetString("-");
			this.hbox2.Add(this.RollUpButton);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.RollUpButton]));
			w1.Position = 2;
			w1.Expand = false;
			this.MainVbox.Add(this.hbox2);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.MainVbox[this.hbox2]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child MainVbox.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.Label1_Conversation = new global::Gtk.Label();
			this.Label1_Conversation.Name = "Label1_Conversation";
			this.Label1_Conversation.Xalign = 1F;
			this.Label1_Conversation.LabelProp = global::Mono.Unix.Catalog.GetString("Разговор:");
			this.hbox1.Add(this.Label1_Conversation);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.Label1_Conversation]));
			w3.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.CallNumberYLabel = new global::Gamma.GtkWidgets.yLabel();
			this.CallNumberYLabel.Name = "CallNumberYLabel";
			this.CallNumberYLabel.Yalign = 0F;
			this.CallNumberYLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Телефон");
			this.CallNumberYLabel.Justify = ((global::Gtk.Justification)(2));
			this.CallNumberYLabel.WidthChars = 0;
			this.CallNumberYLabel.MaxWidthChars = 0;
			this.hbox1.Add(this.CallNumberYLabel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.CallNumberYLabel]));
			w4.Position = 1;
			w4.Padding = ((uint)(72));
			// Container child hbox1.Gtk.Box+BoxChild
			this.TimerYLabel = new global::Gamma.GtkWidgets.yLabel();
			this.TimerYLabel.Name = "TimerYLabel";
			this.TimerYLabel.Xalign = 1F;
			this.TimerYLabel.LabelProp = global::Mono.Unix.Catalog.GetString("Timer");
			this.TimerYLabel.Justify = ((global::Gtk.Justification)(1));
			this.hbox1.Add(this.TimerYLabel);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.TimerYLabel]));
			w5.Position = 2;
			this.MainVbox.Add(this.hbox1);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.MainVbox[this.hbox1]));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			// Container child MainVbox.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.NewClientButton = new global::Gamma.GtkWidgets.yButton();
			this.NewClientButton.CanFocus = true;
			this.NewClientButton.Name = "NewClientButton";
			this.NewClientButton.UseUnderline = true;
			this.NewClientButton.Label = global::Mono.Unix.Catalog.GetString("Новый контрагент");
			this.hbox3.Add(this.NewClientButton);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.NewClientButton]));
			w7.Position = 0;
			// Container child hbox3.Gtk.Box+BoxChild
			this.ExistingClientButton = new global::Gamma.GtkWidgets.yButton();
			this.ExistingClientButton.CanFocus = true;
			this.ExistingClientButton.Name = "ExistingClientButton";
			this.ExistingClientButton.UseUnderline = true;
			this.ExistingClientButton.Label = "Существующий контрагент";
			this.hbox3.Add(this.ExistingClientButton);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.ExistingClientButton]));
			w8.Position = 1;
			this.MainVbox.Add(this.hbox3);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.MainVbox[this.hbox3]));
			w9.Position = 2;
			w9.Expand = false;
			// Container child MainVbox.Gtk.Box+BoxChild
			this.WidgetPlaceBox = new global::Gtk.HBox();
			this.WidgetPlaceBox.Name = "WidgetPlaceBox";
			this.WidgetPlaceBox.Spacing = 6;
			// Container child WidgetPlaceBox.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(3)), ((uint)(3)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.WidgetPlace = new global::Gtk.Notebook();
			this.WidgetPlace.CanFocus = true;
			this.WidgetPlace.Name = "WidgetPlace";
			this.WidgetPlace.CurrentPage = 0;
			this.table1.Add(this.WidgetPlace);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1[this.WidgetPlace]));
			w10.BottomAttach = ((uint)(3));
			w10.RightAttach = ((uint)(3));
			this.WidgetPlaceBox.Add(this.table1);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.WidgetPlaceBox[this.table1]));
			w11.Position = 0;
			this.MainVbox.Add(this.WidgetPlaceBox);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.MainVbox[this.WidgetPlaceBox]));
			w12.Position = 3;
			// Container child MainVbox.Gtk.Box+BoxChild
			this.vbox6 = new global::Gtk.VBox();
			this.vbox6.Name = "vbox6";
			this.vbox6.Spacing = 6;
			// Container child vbox6.Gtk.Box+BoxChild
			this.NewOrderButton = new global::Gamma.GtkWidgets.yButton();
			this.NewOrderButton.CanFocus = true;
			this.NewOrderButton.Name = "NewOrderButton";
			this.NewOrderButton.UseUnderline = true;
			this.NewOrderButton.Label = global::Mono.Unix.Catalog.GetString("+Новый заказ");
			this.vbox6.Add(this.NewOrderButton);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox6[this.NewOrderButton]));
			w13.Position = 0;
			w13.Expand = false;
			w13.Fill = false;
			// Container child vbox6.Gtk.Box+BoxChild
			this.table5 = new global::Gtk.Table(((uint)(1)), ((uint)(4)), false);
			this.table5.Name = "table5";
			this.table5.RowSpacing = ((uint)(6));
			this.table5.ColumnSpacing = ((uint)(6));
			// Container child table5.Gtk.Table+TableChild
			this.BottleButton = new global::Gtk.Button();
			this.BottleButton.CanFocus = true;
			this.BottleButton.Name = "BottleButton";
			this.BottleButton.UseUnderline = true;
			this.BottleButton.Label = global::Mono.Unix.Catalog.GetString("Акт по бутылям и залогам");
			this.table5.Add(this.BottleButton);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table5[this.BottleButton]));
			w14.LeftAttach = ((uint)(1));
			w14.RightAttach = ((uint)(2));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table5.Gtk.Table+TableChild
			this.ComplaintButton = new global::Gtk.Button();
			this.ComplaintButton.CanFocus = true;
			this.ComplaintButton.Name = "ComplaintButton";
			this.ComplaintButton.UseUnderline = true;
			this.ComplaintButton.Label = global::Mono.Unix.Catalog.GetString("+Жалоба");
			this.table5.Add(this.ComplaintButton);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table5[this.ComplaintButton]));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table5.Gtk.Table+TableChild
			this.CostAndDeliveryIntervalButton = new global::Gtk.Button();
			this.CostAndDeliveryIntervalButton.CanFocus = true;
			this.CostAndDeliveryIntervalButton.Name = "CostAndDeliveryIntervalButton";
			this.CostAndDeliveryIntervalButton.UseUnderline = true;
			this.CostAndDeliveryIntervalButton.Label = global::Mono.Unix.Catalog.GetString("\tСтоимость и \n интервалы доставки");
			this.table5.Add(this.CostAndDeliveryIntervalButton);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table5[this.CostAndDeliveryIntervalButton]));
			w16.LeftAttach = ((uint)(3));
			w16.RightAttach = ((uint)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table5.Gtk.Table+TableChild
			this.StockBalanceButton = new global::Gtk.Button();
			this.StockBalanceButton.CanFocus = true;
			this.StockBalanceButton.Name = "StockBalanceButton";
			this.StockBalanceButton.UseUnderline = true;
			this.StockBalanceButton.Label = global::Mono.Unix.Catalog.GetString("Складские остатки");
			this.table5.Add(this.StockBalanceButton);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table5[this.StockBalanceButton]));
			w17.LeftAttach = ((uint)(2));
			w17.RightAttach = ((uint)(3));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox6.Add(this.table5);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox6[this.table5]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			// Container child vbox6.Gtk.Box+BoxChild
			this.table4 = new global::Gtk.Table(((uint)(1)), ((uint)(6)), false);
			this.table4.Name = "table4";
			this.table4.RowSpacing = ((uint)(6));
			this.table4.ColumnSpacing = ((uint)(6));
			// Container child table4.Gtk.Table+TableChild
			this.FinishButton = new global::Gamma.GtkWidgets.yButton();
			this.FinishButton.CanFocus = true;
			this.FinishButton.Name = "FinishButton";
			this.FinishButton.UseUnderline = true;
			this.FinishButton.Label = global::Mono.Unix.Catalog.GetString("Завершить");
			this.table4.Add(this.FinishButton);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table4[this.FinishButton]));
			w19.LeftAttach = ((uint)(5));
			w19.RightAttach = ((uint)(6));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table4.Gtk.Table+TableChild
			this.ForwardingToConsultationButton = new global::Gamma.GtkWidgets.yButton();
			this.ForwardingToConsultationButton.CanFocus = true;
			this.ForwardingToConsultationButton.Name = "ForwardingToConsultationButton";
			this.ForwardingToConsultationButton.UseUnderline = true;
			this.ForwardingToConsultationButton.Label = global::Mono.Unix.Catalog.GetString("Переадресация с консультацией");
			this.table4.Add(this.ForwardingToConsultationButton);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table4[this.ForwardingToConsultationButton]));
			w20.LeftAttach = ((uint)(1));
			w20.RightAttach = ((uint)(2));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table4.Gtk.Table+TableChild
			this.ForwaringButton = new global::Gamma.GtkWidgets.yButton();
			this.ForwaringButton.CanFocus = true;
			this.ForwaringButton.Name = "ForwaringButton";
			this.ForwaringButton.UseUnderline = true;
			this.ForwaringButton.Label = global::Mono.Unix.Catalog.GetString("|| Переадресация");
			this.table4.Add(this.ForwaringButton);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.table4[this.ForwaringButton]));
			w21.XOptions = ((global::Gtk.AttachOptions)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox6.Add(this.table4);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.vbox6[this.table4]));
			w22.Position = 2;
			w22.Expand = false;
			w22.Fill = false;
			this.MainVbox.Add(this.vbox6);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.MainVbox[this.vbox6]));
			w23.Position = 4;
			w23.Expand = false;
			w23.Fill = false;
			this.Add(this.MainVbox);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
