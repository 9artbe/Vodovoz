﻿using System;
using QSOrmProject;
using QSValidation;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Logistic;
using System.Linq;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using QSProjectsLib;
using Vodovoz.Repository;

namespace Vodovoz
{
	public partial class FuelDocumentDlg : OrmGtkDialogBase<FuelDocument>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();

		GenericObservableList<TicketsRow> rows;

		public RouteList RouteListClosing { get; set; }

		public decimal FuelBalance { get; set; }

		public FuelDocumentDlg ()
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<FuelDocument>();
			ConfigureDlg ();
		}

		public FuelDocumentDlg (RouteList routeListClosing, int id)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<FuelDocument> (id);
			RouteListClosing = routeListClosing;
			ConfigureDlg ();
		}

		//public FuelDocumentDlg (FuelDocument sub) : this (sub.Id) {}

		public FuelDocumentDlg(RouteList routeListClosing)
		{
			this.Build ();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<FuelDocument> ();

			RouteListClosing = routeListClosing;

			UoWGeneric.Root.Date 	  = RouteListClosing.Date;
			UoWGeneric.Root.Car 	  = RouteListClosing.Car;
			UoWGeneric.Root.Driver 	  = RouteListClosing.Driver;
			UoWGeneric.Root.Fuel 	  = RouteListClosing.Car.FuelType;
			UoWGeneric.Root.LiterCost = RouteListClosing.Car.FuelType.Cost;

			ConfigureDlg();
		}

		private void ConfigureDlg ()
		{
			ydatepicker.Binding.AddBinding(Entity, e => e.Date, w => w.Date).InitializeFromSource();

			yentrydriver.ItemsQuery = Repository.EmployeeRepository.DriversQuery ();
			yentrydriver.Binding.AddBinding(Entity, e => e.Driver, w => w.Subject).InitializeFromSource();

			yentryCar.SubjectType = typeof(Car);
			yentryCar.Binding.AddBinding(Entity, e => e.Car, w => w.Subject).InitializeFromSource();

			yentryfuel.SubjectType = typeof(FuelType);
			yentryfuel.Binding.AddBinding(Entity, e => e.Fuel, w => w.Subject).InitializeFromSource();

			ytreeTickets.ColumnsConfig = Gamma.GtkWidgets.ColumnsConfigFactory.Create<TicketsRow>()
				.AddColumn("Талон").AddTextRenderer(x => x.GasTicket.Name)
				.AddColumn("Количество").AddNumericRenderer(x => x.Count).Editing()
				.Adjustment(new Gtk.Adjustment(0, 0, 100, 1, 1, 1))
				.Finish();

			var tikets = Repository.Logistics.GasTicketRepository.GetGasTickets(UoW, Entity.Fuel);
			var list = tikets.Select(x => new TicketsRow{ GasTicket = x }).ToList();
			rows = new GenericObservableList<TicketsRow>(list);
			rows.ListContentChanged += Rows_ListContentChanged;

			disablespinMoney.Binding.AddBinding(Entity, e => e.PayedForFuel, w => w.ValueAsDecimal).InitializeFromSource();

			UpdateFuelInfo();
			UpdateResutlInfo();
			LoadTicketsFromEntity();

			ytreeTickets.ItemsDataSource = rows;
		}

		void Rows_ListContentChanged (object sender, EventArgs e)
		{
			Entity.UpdateOperation(rows.ToDictionary(k => k.GasTicket, v => v.Count));
			UpdateResutlInfo();
		}

		private void UpdateFuelInfo() {
			var text = new List<string>();
			decimal fc = (decimal)RouteListClosing.Car.FuelConsumption;

			var track = Repository.Logistics.TrackRepository.GetTrackForRouteList(UoW, RouteListClosing.Id);
			bool hasTrack = track != null && track.Distance.HasValue;

			if(hasTrack)
				text.Add(string.Format("Расстояние по треку: {0:f1} км.", track.Distance));
			
			if(RouteListClosing.ActualDistance > 0)
				text.Add(string.Format("Оплачиваемое расстояние {0}", RouteListClosing.ActualDistance));

			if (RouteListClosing.Car.FuelType != null)
			{
				var fuelOtlayedOp = RouteListClosing.FuelOutlayedOperation;
				var entityOp = Entity.Operation;

				text.Add(string.Format("Вид топлива: {0}", RouteListClosing.Car.FuelType.Name));

				var exclude = new List<int>();
				if (entityOp != null && entityOp.Id != 0)
				{
					exclude.Add(Entity.Operation.Id);
				}
				if (fuelOtlayedOp != null && fuelOtlayedOp.Id != 0)
				{
					exclude.Add(RouteListClosing.FuelOutlayedOperation.Id);
				}
				if (exclude.Count == 0)
					exclude = null;

				Car car = RouteListClosing.Car;
				Employee driver = RouteListClosing.Driver;
				if (car.IsCompanyHavings)
					driver = null;
				else
					car = null;
				
				FuelBalance = Repository.Operations.FuelRepository.GetFuelBalance(
					UoW, driver, car, RouteListClosing.Car.FuelType, null, exclude?.ToArray());

				text.Add(string.Format("Остаток без документа {0:F2} л.", FuelBalance));
			} else {
				text.Add("Не указан вид топлива");
			}

			text.Add(string.Format("Израсходовано топлива: {0:f2} л. ({1:f2} л/100км)",
				fc / 100 * RouteListClosing.ActualDistance, fc));

			ytextviewFuelInfo.Buffer.Text = String.Join("\n", text);
		}

		private void UpdateResutlInfo () 
		{
			decimal litersGived = Entity.Operation?.LitersGived ?? default(decimal);
			decimal spentFuel = (decimal)RouteListClosing.Car.FuelConsumption
								 / 100 * RouteListClosing.ActualDistance;
			
			var text = new List<string>();
			text.Add(string.Format("Итого выдано {0:N2} литров", litersGived));
			text.Add(string.Format("Баланс после выдачи {0:N2}", FuelBalance + litersGived - spentFuel));

			labelResultInfo.Text = string.Join("\n", text);
		}

		public override bool Save ()
		{
			var cashier = EmployeeRepository.GetEmployeeForCurrentUser(UoW);
			if (cashier == null)
			{
				MessageDialogWorks.RunErrorDialog("Ваш пользователь не привязан к действующему сотруднику, Вы не можете выдавать денежные средства, так как некого указывать в качестве кассира.");
				return false;
			}
			if (Entity.FuelCashExpense != null)
			{
				Entity.FuelCashExpense.Casher = cashier;
			}

			Entity.UpdateRowList(rows.ToDictionary(k => k.GasTicket, v => v.Count));
			var valid = new QSValidator<FuelDocument> (UoWGeneric.Root);
			if (valid.RunDlgIfNotValid ((Gtk.Window)this.Toplevel))
				return false;

			logger.Info ("Сохраняем топливный документ...");
			UoWGeneric.Save();
			return true;
		}

		protected void OnDisablespinMoneyValueChanged (object sender, EventArgs e)
		{
			Entity.UpdateOperation(rows.ToDictionary(k => k.GasTicket, v => v.Count));
			Entity.UpdateFuelCashExpense(UoW, RouteListClosing.Cashier, RouteListClosing.Id);
			UpdateResutlInfo();
			UpdateFuelCashExpenseInfo();
		}

		private void UpdateFuelCashExpenseInfo()
		{
			if (Entity.FuelCashExpense == null && !Entity.PayedForFuel.HasValue)
			{
				buttonOpenExpense.Sensitive = false;
				labelExpenseInfo.Text = "";
			}
			if (Entity.PayedForFuel.HasValue) {
				if (Entity.FuelCashExpense.Id <= 0) {
					buttonOpenExpense.Sensitive = false;
					labelExpenseInfo.Text = "Расходный ордер будет создан";
				}
				if (Entity.FuelCashExpense.Id > 0) {
					buttonOpenExpense.Sensitive = true;
					labelExpenseInfo.Text = "";
				}
			}
		}

		private void LoadTicketsFromEntity() 
		{
			foreach (var ticket in Entity.FuelTickets)
			{
				var item = rows.FirstOrDefault(x => x.GasTicket.Id == ticket.GasTicket.Id);
				if (item == null)
				{
					item = new TicketsRow();
					item.GasTicket = ticket.GasTicket;
					rows.Add(item);
				}
				item.Count = ticket.TicketsCount;
			}
		}

		protected void OnButtonSetRemainClicked (object sender, EventArgs e)
		{
			decimal litersBalance = 0;
			decimal litersGived = Entity.Operation?.LitersGived ?? default(decimal);
			decimal spentFuel = (decimal)RouteListClosing.Car.FuelConsumption
				/ 100 * RouteListClosing.ActualDistance;
			litersBalance = FuelBalance + litersGived - spentFuel;

			decimal moneyToPay = -litersBalance * Entity.Fuel.Cost;

			if (Entity.PayedForFuel == null && moneyToPay > 0)
				Entity.PayedForFuel = 0;
			
			Entity.PayedForFuel += moneyToPay;

			if (Entity.PayedForFuel <= 0)
				Entity.PayedForFuel = null;
		}

		protected void OnButtonOpenExpenseClicked (object sender, EventArgs e)
		{
			if (Entity.FuelCashExpense?.Id > 0)
				TabParent.AddSlaveTab(this, new CashExpenseDlg(Entity.FuelCashExpense.Id));
		}

		private class TicketsRow : PropertyChangedBase
		{
			public GazTicket GasTicket  { get; set; }
			int count;
			public int Count
			{
				get
				{
					return count;
				}
				set
				{
					SetField(ref count, value, () => Count);
				}
			}
		}
	}


}

