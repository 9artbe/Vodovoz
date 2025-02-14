﻿using NHibernate.Criterion;
using QS.DomainModel.UoW;
using Vodovoz.Domain.Operations;

namespace Vodovoz.EntityRepositories.Operations
{
	public class WagesMovementRepository : IWagesMovementRepository
	{
		public decimal GetCurrentEmployeeWageBalance(IUnitOfWork uow, int employeeId)
		{
			return uow.Session.QueryOver<WagesMovementOperations>()
				.Where(w => w.Employee.Id == employeeId)
				.Select(Projections.Sum<WagesMovementOperations>(w => w.Money))
				.SingleOrDefault<decimal>();
		}
	}
}

