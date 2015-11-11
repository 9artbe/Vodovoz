﻿using System;
using QSOrmProject;
using QSTDI;
using Vodovoz.Domain;
using Vodovoz.Domain.Cash;

namespace Vodovoz
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class UnclosedAdvancesView : TdiTabBase
	{
		private IUnitOfWork uow;

		public IUnitOfWork UoW {
			get {
				return uow;
			}
			set {
				if (uow == value)
					return;
				uow = value;
				unclosedadvancesfilter1.UoW = value;
				var vm = new ViewModel.UnclosedAdvancesVM (unclosedadvancesfilter1);

				representationUnclosed.RepresentationModel = vm;
				representationUnclosed.RepresentationModel.UpdateNodes ();
			}
		}

		public UnclosedAdvancesView(Employee accountable, ExpenseCategory expense): this()
		{
			if (accountable != null)
				unclosedadvancesfilter1.RestrictAccountable = accountable;
			if (expense != null)
				unclosedadvancesfilter1.RestrictExpenseCategory = expense;
		}

		public UnclosedAdvancesView ()
		{
			this.Build ();
			this.TabName = "Незакрытые авансы";
			unclosedadvancesfilter1.Refiltered += Accountableslipfilter1_Refiltered;
			UoW = UnitOfWorkFactory.CreateWithoutRoot ();
		}

		void Accountableslipfilter1_Refiltered (object sender, EventArgs e)
		{
			if(unclosedadvancesfilter1.RestrictAccountable == null)
				TabName = "Незакрытые авансы";
			else
				TabName = String.Format ("Незакрытые авансы по {0}", unclosedadvancesfilter1.RestrictAccountable.ShortName);
		}
	}
}

