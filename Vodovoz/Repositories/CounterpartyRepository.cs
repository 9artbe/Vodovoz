﻿using QSOrmProject;
using Vodovoz.Domain;


namespace Vodovoz.Repository
{
	public class CounterpartyRepository
	{
		public static CounterpartyContract GetCounterpartyContract (IUnitOfWork uow)
		{
			Order order = uow.RootObject as Order;
			Organization organization = 
				(order.PaymentType == Payment.cash 
				? OrganizationRepository.GetCashOrganization (uow)
				: OrganizationRepository.GetCashlessOrganization (uow));

			return uow.Session.QueryOver<CounterpartyContract> ()
				.Where (co => (co.Counterparty.Id == order.Client.Id &&
			!co.IsArchive &&
			!co.OnCancellation &&
			co.Organization.Id == organization.Id))
				.SingleOrDefault ();
		}
	}
}

