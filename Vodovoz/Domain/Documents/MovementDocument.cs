﻿using System;
using QSOrmProject;

namespace Vodovoz
{
	public class MovementDocument: Document
	{
		//TODO Movement type (CP - CP or WH - WH)
		//TODO List of elements
		Employee responsiblePerson;

		public virtual Employee ResponsiblePerson {
			get { return responsiblePerson; }
			set { responsiblePerson = value; }
		}

		Counterparty fromClient;

		public virtual Counterparty FromClient {
			get { return fromClient; }
			set { fromClient = value; }
		}

		Counterparty toClient;

		public virtual Counterparty ToClient {
			get { return toClient; }
			set { toClient = value; }
		}

		DeliveryPoint fromDeliveryPoint;

		public virtual DeliveryPoint FromDeliveryPoint {
			get { return fromDeliveryPoint; }
			set { fromDeliveryPoint = value; }
		}

		DeliveryPoint toDeliveryPoint;

		public virtual DeliveryPoint ToDeliveryPoint {
			get { return toDeliveryPoint; }
			set { toDeliveryPoint = value; }
		}

		Warehouse fromWarehouse;

		public virtual Warehouse FromWarehouse {
			get { return fromWarehouse; }
			set { fromWarehouse = value; }
		}

		Warehouse toWarehouse;

		public virtual Warehouse ToWarehouse {
			get { return toWarehouse; }
			set { toWarehouse = value; }
		}
	}
}

