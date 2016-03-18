﻿using System;
using System.Collections.Generic;
using Gtk;

namespace Vodovoz.Panel
{
	public static class PanelViewFactory
	{
		public static Widget Create(PanelViewType type)
		{
			switch (type)
			{
				case PanelViewType.CounterpartyView:
					return new CounterpartyPanelView();
				case PanelViewType.DeliveryPointView:
					return new DeliveryPointPanelView();
				case PanelViewType.AdditionalAgreementPanelView:
					return new AdditionalAgreementPanelView();
				default:
					throw new NotSupportedException();
			}
		}

		public static IEnumerable<Widget> CreateAll(IEnumerable<PanelViewType> types)
		{
			var iterator = types.GetEnumerator();
			while (iterator.MoveNext())
				yield return Create(iterator.Current);
		}
	}

	public enum PanelViewType{
		CounterpartyView,
		DeliveryPointView,
		AdditionalAgreementPanelView,
	}
}

