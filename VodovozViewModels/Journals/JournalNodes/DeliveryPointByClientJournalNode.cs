﻿using QS.Project.Journal;
using Vodovoz.Domain.Client;

namespace Vodovoz.ViewModels.Journals.JournalNodes
{
	public class DeliveryPointByClientJournalNode : JournalEntityNodeBase<DeliveryPoint>
	{
		public string CompiledAddress { get; set; }
		public bool IsActive { get; set; }
		public string RowColor => IsActive ? "black" : "grey";
	}
}
