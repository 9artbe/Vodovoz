﻿using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using QSOrmProject;
using Vodovoz.Domain.Operations;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;

namespace Vodovoz.Domain.Documents
{
	[OrmSubject (JournalName = "Производство воды", ObjectName = "Документ производства")]
	public class IncomingWater: Document
	{
		int amount;

		[Min (1)]
		[Display (Name = "Количество")]
		public virtual int Amount {
			get { return amount; }
			set { SetField (ref amount, value, () => Amount); }
		}

		Warehouse incomingWarehouse;

		[Required (ErrorMessage = "Склад поступления должен быть указан.")]
		[Display (Name = "Склад поступления")]
		public virtual Warehouse IncomingWarehouse {
			get { return incomingWarehouse; }
			set { SetField (ref incomingWarehouse, value, () => IncomingWarehouse); }
		}

		Warehouse writeOffWarehouse;

		[Required (ErrorMessage = "Склад списания должен быть указан.")]
		[Display (Name = "Склад списания")]
		public virtual Warehouse WriteOffWarehouse {
			get { return writeOffWarehouse; }
			set { SetField (ref writeOffWarehouse, value, () => WriteOffWarehouse); }
		}

		#region IDocument implementation

		new public virtual string DocType {
			get { return "Документ производства"; }
		}

		new public virtual string Description {
			get { return String.Format ("Количество: {0}; Склад поступления: {1};", 
				Amount,
				WriteOffWarehouse == null ? "не указан" : WriteOffWarehouse.Name); 
			}
		}

		#endregion

		GoodsMovementOperation produceOperation = new GoodsMovementOperation ();

		public GoodsMovementOperation ProduceOperation {
			get { return produceOperation; }
			set { SetField (ref produceOperation, value, () => ProduceOperation); }
		}

		IList<IncomingWaterMaterial> materials = new List<IncomingWaterMaterial> ();

		[Display (Name = "Строки")]
		public virtual IList<IncomingWaterMaterial> Materials {
			get { return materials; }
			set { SetField (ref materials, value, () => Materials);
				observableMaterials = null;
			}
		}

		GenericObservableList<IncomingWaterMaterial> observableMaterials;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public GenericObservableList<IncomingWaterMaterial> ObservableMaterials {
			get {if (observableMaterials == null)
				observableMaterials = new GenericObservableList<IncomingWaterMaterial> (Materials);
				return observableMaterials;
			}
		}

	}
}

