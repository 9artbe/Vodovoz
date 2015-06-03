﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings;
using QSContacts;
using QSOrmProject;

namespace Vodovoz.Domain
{
	[OrmSubject( Gender = QSProjectsLib.GrammaticalGender.Feminine,
		NominativePlural = "доверенности",
		Nominative = "доверенность",
		Genitive = "доверенности",
		Accusative = "доверенность"
	)]
	public class Proxy : BaseNotifyPropertyChanged, IDomainObject
	{
		#region Свойства
		public virtual int Id { get; set; }

		[Display(Name = "Контрагент")]
		public virtual Counterparty Counterparty { get; set; }

		[Display(Name = "Номер")]
		public virtual string Number { get; set; }

		[Display(Name = "Дата подписания")]
		public virtual DateTime IssueDate { get; set; }

		[Display(Name = "Начало действия")]
		public virtual DateTime StartDate { get; set; }

		[Display(Name = "Окончание действия")]
		public virtual DateTime ExpirationDate { get; set; }

		[Display(Name = "Список лиц")]
		public virtual IList<Person> Persons { get; set; }

		[Display(Name = "Точка доставки")]
		public virtual DeliveryPoint DeliveryPoint { get; set; }
		#endregion

		public Proxy ()
		{
			Number = String.Empty;
		}

		public virtual string Title { 
			get { return String.Format ("Доверенность №{0} от {1:d}", Number, IssueDate); }
		}

		public string Issue { get { return IssueDate.ToShortDateString(); } } 
		public string Start { get { return StartDate.ToShortDateString(); } } 
		public string Expiration { get { return ExpirationDate.ToShortDateString(); } }

		//Конструкторы
		public static IUnitOfWorkGeneric<Proxy> Create(Counterparty counterparty)
		{
			var uow = UnitOfWorkFactory.CreateWithNewRoot<Proxy> ();
			uow.Root.Counterparty = counterparty;
			return uow;
		}
	}

	public interface IProxyOwner
	{
		IList<Proxy> Proxies { get; set;}
	}
}

