﻿using QS.Project.Journal.EntitySelector;
using System.Collections.Generic;
using Vodovoz.Domain.Store;
using Vodovoz.FilterViewModels.Goods;

namespace Vodovoz.TempAdapters
{
	public interface INomenclatureSelectorFactory
	{
		IEntitySelector CreateNomenclatureSelectorForWarehouse(Warehouse warehouse, IEnumerable<int> excludedNomenclatures);
		IEntitySelector CreateNomenclatureSelector(IEnumerable<int> excludedNomenclatures);
		IEntitySelector CreateNomenclatureSelectorForFuelSelect();
		IEntityAutocompleteSelectorFactory GetWaterJournalFactory();
		IEntityAutocompleteSelectorFactory CreateNomenclatureAutocompleteSelectorFactory(NomenclatureFilterViewModel filterViewModel);
	}
}
