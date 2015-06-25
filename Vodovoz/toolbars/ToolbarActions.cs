using Gtk;
using NHibernate;
using QSOrmProject;
using Vodovoz;
using System.Collections.Generic;
using Vodovoz.Domain;

public partial class MainWindow : Gtk.Window
{
	Action ActionNewOrder;
	Action ActionOrdersTable;
	Action ActionServiceTickets;
	Action ActionWarehouseDocuments;
	Action ActionWarehouseStock;

	public void BuildToolbarActions ()
	{
		#region Creating actions
		//Заказы
		ActionNewOrder = new Action ("ActionNewOrder", "Новый заказ", null, "gtk-new");
		ActionOrdersTable = new Action ("ActionOrdersTable", "Журнал заказов", null, "table");
		//Сервис
		ActionServiceTickets = new Action ("ActionServiceTickets", "Журнал заявок", null, "table");
		//Склад
		ActionWarehouseDocuments = new Action ("ActionWarehouseDocuments", "Журнал документов", null, "table");
		ActionWarehouseStock = new Action ("ActionWarehouseStock", "Складские остатки", null, "table");
		#endregion
		#region Inserting actions to the toolbar
		ActionGroup w1 = new ActionGroup ("ToolbarActions");
		w1.Add (ActionNewOrder, null);
		w1.Add (ActionOrdersTable, null);
		w1.Add (ActionServiceTickets, null);
		w1.Add (ActionWarehouseDocuments, null);
		w1.Add (ActionWarehouseStock, null);
		UIManager.InsertActionGroup (w1, 0);
		#endregion
		#region Creating events
		ActionNewOrder.Activated += ActionNewOrderActivated;
		ActionOrdersTable.Activated += ActionOrdersTableActivated;
		ActionServiceTickets.Activated += ActionServiceTicketsActivated;
		ActionWarehouseDocuments.Activated += ActionWarehouseDocumentsActivated;
		ActionWarehouseStock.Activated += ActionWarehouseStock_Activated;
		#endregion
	}

	void ActionWarehouseStock_Activated (object sender, System.EventArgs e)
	{
		var tab = new StockBalanceView ();
		tdiMain.AddTab (tab);
	}

	void ActionWarehouseDocumentsActivated (object sender, System.EventArgs e)
	{
		var tab = new WarehouseDocumentsView ();
		tdiMain.AddTab (tab);
	}

	void ActionServiceTicketsActivated (object sender, System.EventArgs e)
	{
		//ISession session = OrmMain.OpenSession ();
	}

	void ActionOrdersTableActivated (object sender, System.EventArgs e)
	{
		ISession session = OrmMain.OpenSession ();
		var criteria = session.CreateCriteria<Order> ();

		OrmReference refWin = new OrmReference (typeof(Order), session, criteria);
		tdiMain.AddTab (refWin);
	}

	void ActionNewOrderActivated (object sender, System.EventArgs e)
	{
		//ISession session = OrmMain.OpenSession ();
	}


}