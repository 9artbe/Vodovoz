﻿using System;
using System.Collections.Generic;
using QSOrmProject;
using Vodovoz.Domain;
using Vodovoz.Domain.Client;

namespace Vodovoz.Repository
{
	public class CounterpartyContractRepository
	{
		public static CounterpartyContract GetCounterpartyContractByPaymentType (IUnitOfWork uow, Counterparty counterparty, PaymentType paymentType)
		{
			Organization organization = OrganizationRepository.GetOrganizationByPaymentType (uow,paymentType);
			if (organization == null)
				throw new InvalidProgramException(String.Format("В параметрах базы не указана организация для типа оплаты {0}",
					paymentType));

			Counterparty counterpartyAlias = null;
			Organization organizationAlias = null;

			return uow.Session.QueryOver<CounterpartyContract> ()
				.JoinAlias (co => co.Counterparty, () => counterpartyAlias)
				.JoinAlias (co => co.Organization, () => organizationAlias)
				.Where (co => (counterpartyAlias.Id == counterparty.Id &&
			!co.IsArchive &&
			!co.OnCancellation &&
			organizationAlias.Id == organization.Id))
				.SingleOrDefault ();
		}

		public static IList<CounterpartyContract> GetActiveContractsWithOrganization (IUnitOfWork uow, Counterparty counterparty, Organization org)
		{
			Counterparty counterpartyAlias = null;
			Organization organizationAlias = null;

			return uow.Session.QueryOver<CounterpartyContract> ()
				.Where (co => (co.Counterparty.Id == counterparty.Id &&
					!co.IsArchive &&
					!co.OnCancellation &&
					co.Organization.Id == org.Id))
				.List();
		}

	}
}

