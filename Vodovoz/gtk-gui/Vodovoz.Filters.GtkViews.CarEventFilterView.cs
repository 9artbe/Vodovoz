﻿
// This file has been generated by the GUI designer. Do not modify.
namespace Vodovoz.Filters.GtkViews
{
	public partial class CarEventFilterView
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.Table table1;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry entityviewmodelentryCar;

		private global::QS.Widgets.GtkUI.EntityViewModelEntry entityviewmodelentryCarEventType;

		private global::Gtk.Label labelAuthor;

		private global::Gtk.Label labelCar;

		private global::Gtk.Label labelCarEventType;

		private global::Gtk.Label labelDateCreate;

		private global::Gtk.Label labelDateEventEnd;

		private global::Gtk.Label labelDateEventStart;

		private global::Gtk.Label labelDriver;

		private global::QS.Widgets.GtkUI.RepresentationEntry referenceAuthor;

		private global::QS.Widgets.GtkUI.RepresentationEntry referenceDriver;

		private global::Gamma.Widgets.yDatePeriodPicker ydateperiodpickerCreateEventDate;

		private global::Gamma.Widgets.yDatePeriodPicker ydateperiodpickerEndEventDate;

		private global::Gamma.Widgets.yDatePeriodPicker ydateperiodpickerStartEventDate;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Vodovoz.Filters.GtkViews.CarEventFilterView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Vodovoz.Filters.GtkViews.CarEventFilterView";
			// Container child Vodovoz.Filters.GtkViews.CarEventFilterView.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(3)), ((uint)(6)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.entityviewmodelentryCar = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.entityviewmodelentryCar.Events = ((global::Gdk.EventMask)(256));
			this.entityviewmodelentryCar.Name = "entityviewmodelentryCar";
			this.entityviewmodelentryCar.CanEditReference = true;
			this.table1.Add(this.entityviewmodelentryCar);
			global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1[this.entityviewmodelentryCar]));
			w1.TopAttach = ((uint)(1));
			w1.BottomAttach = ((uint)(2));
			w1.LeftAttach = ((uint)(3));
			w1.RightAttach = ((uint)(4));
			w1.XOptions = ((global::Gtk.AttachOptions)(4));
			w1.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entityviewmodelentryCarEventType = new global::QS.Widgets.GtkUI.EntityViewModelEntry();
			this.entityviewmodelentryCarEventType.Events = ((global::Gdk.EventMask)(256));
			this.entityviewmodelentryCarEventType.Name = "entityviewmodelentryCarEventType";
			this.entityviewmodelentryCarEventType.CanEditReference = true;
			this.table1.Add(this.entityviewmodelentryCarEventType);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1[this.entityviewmodelentryCarEventType]));
			w2.TopAttach = ((uint)(1));
			w2.BottomAttach = ((uint)(2));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelAuthor = new global::Gtk.Label();
			this.labelAuthor.Name = "labelAuthor";
			this.labelAuthor.Xalign = 1F;
			this.labelAuthor.LabelProp = global::Mono.Unix.Catalog.GetString("Автор:");
			this.table1.Add(this.labelAuthor);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1[this.labelAuthor]));
			w3.TopAttach = ((uint)(2));
			w3.BottomAttach = ((uint)(3));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelCar = new global::Gtk.Label();
			this.labelCar.Name = "labelCar";
			this.labelCar.Xalign = 1F;
			this.labelCar.LabelProp = global::Mono.Unix.Catalog.GetString("Автомобиль:");
			this.table1.Add(this.labelCar);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1[this.labelCar]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.LeftAttach = ((uint)(2));
			w4.RightAttach = ((uint)(3));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelCarEventType = new global::Gtk.Label();
			this.labelCarEventType.Name = "labelCarEventType";
			this.labelCarEventType.Xalign = 1F;
			this.labelCarEventType.LabelProp = global::Mono.Unix.Catalog.GetString("Вид события ТС:");
			this.table1.Add(this.labelCarEventType);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1[this.labelCarEventType]));
			w5.TopAttach = ((uint)(1));
			w5.BottomAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelDateCreate = new global::Gtk.Label();
			this.labelDateCreate.Name = "labelDateCreate";
			this.labelDateCreate.Xalign = 1F;
			this.labelDateCreate.LabelProp = global::Mono.Unix.Catalog.GetString("Дата создания:");
			this.table1.Add(this.labelDateCreate);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1[this.labelDateCreate]));
			w6.LeftAttach = ((uint)(4));
			w6.RightAttach = ((uint)(5));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelDateEventEnd = new global::Gtk.Label();
			this.labelDateEventEnd.Name = "labelDateEventEnd";
			this.labelDateEventEnd.Xalign = 1F;
			this.labelDateEventEnd.LabelProp = global::Mono.Unix.Catalog.GetString("Дата окончания события:");
			this.table1.Add(this.labelDateEventEnd);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1[this.labelDateEventEnd]));
			w7.LeftAttach = ((uint)(2));
			w7.RightAttach = ((uint)(3));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelDateEventStart = new global::Gtk.Label();
			this.labelDateEventStart.Name = "labelDateEventStart";
			this.labelDateEventStart.Xalign = 1F;
			this.labelDateEventStart.LabelProp = global::Mono.Unix.Catalog.GetString("Дата начала события:");
			this.table1.Add(this.labelDateEventStart);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1[this.labelDateEventStart]));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelDriver = new global::Gtk.Label();
			this.labelDriver.Name = "labelDriver";
			this.labelDriver.Xalign = 1F;
			this.labelDriver.LabelProp = global::Mono.Unix.Catalog.GetString("Водитель:");
			this.table1.Add(this.labelDriver);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1[this.labelDriver]));
			w9.TopAttach = ((uint)(1));
			w9.BottomAttach = ((uint)(2));
			w9.LeftAttach = ((uint)(4));
			w9.RightAttach = ((uint)(5));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.referenceAuthor = new global::QS.Widgets.GtkUI.RepresentationEntry();
			this.referenceAuthor.Events = ((global::Gdk.EventMask)(256));
			this.referenceAuthor.Name = "referenceAuthor";
			this.table1.Add(this.referenceAuthor);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1[this.referenceAuthor]));
			w10.TopAttach = ((uint)(2));
			w10.BottomAttach = ((uint)(3));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.referenceDriver = new global::QS.Widgets.GtkUI.RepresentationEntry();
			this.referenceDriver.Events = ((global::Gdk.EventMask)(256));
			this.referenceDriver.Name = "referenceDriver";
			this.table1.Add(this.referenceDriver);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table1[this.referenceDriver]));
			w11.TopAttach = ((uint)(1));
			w11.BottomAttach = ((uint)(2));
			w11.LeftAttach = ((uint)(5));
			w11.RightAttach = ((uint)(6));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ydateperiodpickerCreateEventDate = new global::Gamma.Widgets.yDatePeriodPicker();
			this.ydateperiodpickerCreateEventDate.Events = ((global::Gdk.EventMask)(256));
			this.ydateperiodpickerCreateEventDate.Name = "ydateperiodpickerCreateEventDate";
			this.ydateperiodpickerCreateEventDate.StartDate = new global::System.DateTime(0);
			this.ydateperiodpickerCreateEventDate.EndDate = new global::System.DateTime(0);
			this.table1.Add(this.ydateperiodpickerCreateEventDate);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1[this.ydateperiodpickerCreateEventDate]));
			w12.LeftAttach = ((uint)(5));
			w12.RightAttach = ((uint)(6));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ydateperiodpickerEndEventDate = new global::Gamma.Widgets.yDatePeriodPicker();
			this.ydateperiodpickerEndEventDate.Events = ((global::Gdk.EventMask)(256));
			this.ydateperiodpickerEndEventDate.Name = "ydateperiodpickerEndEventDate";
			this.ydateperiodpickerEndEventDate.StartDate = new global::System.DateTime(0);
			this.ydateperiodpickerEndEventDate.EndDate = new global::System.DateTime(0);
			this.table1.Add(this.ydateperiodpickerEndEventDate);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table1[this.ydateperiodpickerEndEventDate]));
			w13.LeftAttach = ((uint)(3));
			w13.RightAttach = ((uint)(4));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ydateperiodpickerStartEventDate = new global::Gamma.Widgets.yDatePeriodPicker();
			this.ydateperiodpickerStartEventDate.Events = ((global::Gdk.EventMask)(256));
			this.ydateperiodpickerStartEventDate.Name = "ydateperiodpickerStartEventDate";
			this.ydateperiodpickerStartEventDate.StartDate = new global::System.DateTime(0);
			this.ydateperiodpickerStartEventDate.EndDate = new global::System.DateTime(0);
			this.table1.Add(this.ydateperiodpickerStartEventDate);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table1[this.ydateperiodpickerStartEventDate]));
			w14.LeftAttach = ((uint)(1));
			w14.RightAttach = ((uint)(2));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add(this.table1);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.table1]));
			w15.Position = 0;
			w15.Expand = false;
			w15.Fill = false;
			this.Add(this.vbox2);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
