﻿using System;
using QSOrmProject;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Logistic;

namespace Vodovoz.Repository.Logistics
{
	public static class CarRepository
	{
		public static Car GetCarByDriver(IUnitOfWork uow, Employee driver)
		{
			return uow.Session.QueryOver<Car>()
					  .Where(x => x.Driver == driver)
					  .Take(1)
					  .SingleOrDefault();
		}
	}
}
