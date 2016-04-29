﻿using System;
using System.Collections.Generic;
using Gamma.ColumnConfig;
using Gtk;
using NHibernate.Transform;
using QSOrmProject;
using QSOrmProject.RepresentationModel;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Logistic;

namespace Vodovoz.ViewModel
{
	public class DeliveryPointsVM : RepresentationModelEntityBase<DeliveryPoint, DeliveryPointVMNode>
	{
		#region IRepresentationModel implementation

		public override void UpdateNodes ()
		{
			DeliveryPoint deliveryPointAlias = null;
			Counterparty counterpartyAlias = null;
			LogisticsArea logisticsAreaAlias = null;
			DeliveryPointVMNode resultAlias = null;

			var deliveryPointslist = UoW.Session.QueryOver<DeliveryPoint> (() => deliveryPointAlias)
				.JoinAlias (c => c.Counterparty, () => counterpartyAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
				.JoinAlias (c => c.LogisticsArea, () => logisticsAreaAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
				.SelectList (list => list
					.Select (() => deliveryPointAlias.Id).WithAlias (() => resultAlias.Id)
					.Select (() => deliveryPointAlias.CompiledAddress).WithAlias (() => resultAlias.CompiledAddress)
					.Select (() => deliveryPointAlias.FoundOnOsm).WithAlias (() => resultAlias.FoundOnOsm)
					.Select (() => deliveryPointAlias.IsActive).WithAlias (() => resultAlias.IsActive)
					.Select (() => counterpartyAlias.FullName).WithAlias (() => resultAlias.Client)
					.Select (() => logisticsAreaAlias.Name).WithAlias (() => resultAlias.LogisticsArea)
			                         )
				.TransformUsing (Transformers.AliasToBean<DeliveryPointVMNode> ())
				.List<DeliveryPointVMNode> ();

			SetItemsSource (deliveryPointslist);
		}

		IColumnsConfig columnsConfig = FluentColumnsConfig<DeliveryPointVMNode>.Create ()
			.AddColumn("OSM").AddTextRenderer(x => x.FoundOnOsm ? "Да": "")
			.AddColumn("Логистический район").AddTextRenderer(x => x.LogisticsArea)
			.AddColumn ("Адрес").SetDataProperty (node => node.CompiledAddress)
			.AddColumn("Клиент").AddTextRenderer(x => x.Client)
			.RowCells ().AddSetter<CellRendererText> ((c, n) => c.Foreground = n.RowColor)
			.Finish ();

		public override IColumnsConfig ColumnsConfig {
			get { return columnsConfig; }
		}

		#endregion

		#region implemented abstract members of RepresentationModelBase

		protected override bool NeedUpdateFunc (DeliveryPoint updatedSubject)
		{
			return true;
		}

		#endregion

		public DeliveryPointsVM () : this(UnitOfWorkFactory.CreateWithoutRoot ()){}

		public DeliveryPointsVM (IUnitOfWork uow)
		{
			this.UoW = uow;
		}
	}

	public class DeliveryPointVMNode
	{

		public int Id { get; set; }

		[UseForSearch]
		public string CompiledAddress { get; set; }

		public string LogisticsArea { get; set; }

		[UseForSearch]
		public string Client { get; set; }

		public bool IsActive { get; set; }

		public bool FoundOnOsm { get; set; }

		public string RowColor { get { return IsActive ? "black" : "grey"; } }
	}
}

