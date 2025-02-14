﻿using QS.DomainModel.Entity;
using QS.DomainModel.Entity.EntityPermissions;
using QS.HistoryLog;
using QS.Utilities.Text;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Operations;
using Vodovoz.Domain.Store;

namespace Vodovoz.Domain.Documents.DriverTerminal
{
	[Appellative(Gender = GrammaticalGender.Masculine,
		Nominative = "документ выдачи терминала водителя",
		NominativePlural = "документы выдачи терминалов водителей")]
	[EntityPermission]
	[HistoryTrace]
	public class DriverAttachedTerminalGiveoutDocument : DriverAttachedTerminalDocumentBase
	{
		public virtual string Title =>
			$"Выдача терминала водителю {PersonHelper.PersonNameWithInitials(Driver.LastName, Driver.Name, Driver.Patronymic)}";

		public override string ToString() =>
			$"Выдача терминала {CreationDate.ToShortDateString()} в {CreationDate.ToShortTimeString()}\r\n" +
			$"со склада {WarehouseMovementOperation.WriteoffWarehouse.Name}";

		public override void CreateMovementOperations(Warehouse writeoffWarehouse, Nomenclature terminal)
		{
			WarehouseMovementOperation = new WarehouseMovementOperation
			{
				WriteoffWarehouse = writeoffWarehouse,
				IncomingWarehouse = null,
				Amount = 1,
				Equipment = null,
				Nomenclature = terminal,
				OperationTime = CreationDate
			};

			EmployeeNomenclatureMovementOperation = new EmployeeNomenclatureMovementOperation
			{
				Amount = 1,
				Employee = Driver,
				Nomenclature = terminal,
				OperationTime = CreationDate
			};
		}
	}
}
