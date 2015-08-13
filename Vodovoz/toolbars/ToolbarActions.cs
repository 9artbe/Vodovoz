using Gtk;
using QSOrmProject;
using Vodovoz;
using Vodovoz.ViewModel;

public partial class MainWindow : Window
{
	Action ActionOrdersTable;
	Action ActionServiceClaims;
	Action ActionWarehouseDocuments;
	Action ActionWarehouseStock;
	Action ActionClientBalance;
	Action ActionRouteListTable;
	Action ActionAddOrder;

	public void BuildToolbarActions ()
	{
		#region Creating actions
		//Заказы
		ActionOrdersTable = new Action ("ActionOrdersTable", "Журнал заказов", null, "table");
		ActionAddOrder = new Action ("ActionAddOrder", "Новый заказ", null, "table");
		//Сервис
		ActionServiceClaims = new Action ("ActionServiceTickets", "Журнал заявок", null, "table");
		//Склад
		ActionWarehouseDocuments = new Action ("ActionWarehouseDocuments", "Журнал документов", null, "table");
		ActionWarehouseStock = new Action ("ActionWarehouseStock", "Складские остатки", null, "table");
		ActionClientBalance = new Action ("ActionClientBalance", "Оборудование у клиентов", null, "table");
		//Логистика
		ActionRouteListTable = new Action ("ActionRouteListTable", "Маршрутные листы", null, "table");
		#endregion
		#region Inserting actions to the toolbar
		ActionGroup w1 = new ActionGroup ("ToolbarActions");
		w1.Add (ActionOrdersTable, null);
		w1.Add (ActionAddOrder, null);
		w1.Add (ActionServiceClaims, null);
		w1.Add (ActionWarehouseDocuments, null);
		w1.Add (ActionWarehouseStock, null);
		w1.Add (ActionClientBalance, null);
		w1.Add (ActionRouteListTable, null);
		UIManager.InsertActionGroup (w1, 0);
		#endregion
		#region Creating events
		ActionOrdersTable.Activated += ActionOrdersTableActivated;
		ActionAddOrder.Activated += ActionAddOrder_Activated;
		ActionServiceClaims.Activated += ActionServiceClaimsActivated;
		ActionWarehouseDocuments.Activated += ActionWarehouseDocumentsActivated;
		ActionWarehouseStock.Activated += ActionWarehouseStock_Activated;
		ActionClientBalance.Activated += ActionClientBalance_Activated;
		ActionRouteListTable.Activated += ActionRouteListTable_Activated;
		#endregion
	}

	void ActionRouteListTable_Activated (object sender, System.EventArgs e)
	{
		//TODO FIXME Сделать нормальный вид.
		OrmReference refWin = new OrmReference (typeof(Vodovoz.Domain.Logistic.RouteList));
		tdiMain.AddTab (refWin);
	}

	void ActionClientBalance_Activated (object sender, System.EventArgs e)
	{
		var refWin = new ReferenceRepresentation (new ClientBalanceVM ());
		tdiMain.AddTab (refWin);
	}

	void ActionAddOrder_Activated (object sender, System.EventArgs e)
	{
		var tab = new OrderDlg ();
		tdiMain.AddTab (tab);
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

	void ActionServiceClaimsActivated (object sender, System.EventArgs e)
	{
		ReferenceRepresentation refWin = new ReferenceRepresentation (new ServiceClaimVM ());
		tdiMain.AddTab (refWin);
	}

	void ActionOrdersTableActivated (object sender, System.EventArgs e)
	{
		ReferenceRepresentation refWin = new ReferenceRepresentation (new OrdersVM ());
		tdiMain.AddTab (refWin);
	}
}