﻿using System;
using System.Linq;
using NHibernate.Transform;
using QSOrmProject;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Operations;
using Order = Vodovoz.Domain.Orders.Order;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Logistic;

namespace Vodovoz.Repository.Operations
{
	public static class FuelRepository
	{
		public static int GetFuelBalance(IUnitOfWork UoW, Employee driver, FuelType fuel, DateTime? before = null)
		{
			FuelOperation operationAlias = null;
			FuelQueryResult result = null;
			var queryResult = UoW.Session.QueryOver<FuelOperation>(() => operationAlias)
				.Where(() => operationAlias.Driver.Id == driver.Id)
				.Where(() => operationAlias.Fuel.Id == fuel.Id);
			if (before.HasValue)
				queryResult.Where(() => operationAlias.OperationTime < before);			
			
			var bottles =  queryResult.SelectList(list => list
				.SelectSum(() => operationAlias.LitersGived).WithAlias(() => result.Gived)
				.SelectSum(() => operationAlias.LitersOutlayed).WithAlias(() => result.Outlayed)
				).TransformUsing(Transformers.AliasToBean<FuelQueryResult>()).List<FuelQueryResult>()
				.FirstOrDefault()?.FuelBalance ?? 0;
			return bottles;
		}

		class FuelQueryResult
		{
			public int Gived{get;set;}
			public int Outlayed{get;set;}
			public int FuelBalance{
				get{
					return Gived - Outlayed;
				}
			}
		}
	}


}