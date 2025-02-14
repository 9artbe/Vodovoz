﻿using QS.Project.Journal;
using Vodovoz.Domain.Store;

namespace Vodovoz.ViewModels.Journals.JournalNodes
{
	public class NomenclatureBalanceByStockJournalNode : JournalEntityNodeBase<Warehouse>
	{
		public string WarehouseName { get; set; }
		public decimal NomenclatureAmount => Added - Removed;
		public decimal Added { get; set; }
		public decimal Removed { get; set; }
	}
}
