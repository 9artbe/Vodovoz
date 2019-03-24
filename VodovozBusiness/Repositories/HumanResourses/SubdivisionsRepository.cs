﻿using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using QS.DomainModel.UoW;
using QSSupportLib;
using System.Linq;
using Vodovoz.Domain.Store;
using QS.Project.Domain;
using Vodovoz.Domain.Permissions;

namespace Vodovoz.Repositories.HumanResources
{
	public static class SubdivisionsRepository
	{
		public static Subdivision GetQCDepartment(IUnitOfWork uow)
		{
			var qcDep = "номер_отдела_ОКК";
			if(!MainSupport.BaseParameters.All.ContainsKey(qcDep))
				throw new InvalidProgramException("В параметрах базы не указан номер отдела контроля качества [номер_отдела_ОКК]");
			return uow.GetById<Subdivision>(int.Parse(MainSupport.BaseParameters.All[qcDep]));
		}

		/// <summary>
		/// Возврат отсортированного списка подразделений компании
		/// </summary>
		/// <returns>Список подразделений</returns>
		/// <param name="uow">UoW</param>
		/// <param name="orderByDescending">Если <c>true</c>, то сортируется список по убыванию.</param>
		public static IList<Subdivision> GetAllDepartments(IUnitOfWork uow, bool orderByDescending = false)
		{
			var query = uow.Session.QueryOver<Subdivision>()
			   .OrderBy(i => i.Name);
			return orderByDescending ? query.Desc().List() : query.Asc().List();
		}

		/// <summary>
		/// Список дочерних подразделений
		/// </summary>
		/// <returns>Список дочерних подразделений</returns>
		/// <param name="uow">Unit of Work</param>
		/// <param name="parentSubdivision">Подразделение, список дочерних подразделений которого требуется вернуть</param>
		/// <param name="orderByDescending">Если <c>true</c>, то сортируется список по убыванию.</param>
		public static IList<Subdivision> GetChildDepartments(IUnitOfWork uow, Subdivision parentSubdivision, bool orderByDescending = false)
		{
			return GetAllDepartments(uow, orderByDescending).Where(s => s.ParentSubdivision == parentSubdivision).ToList();
		}

		/// <summary>
		/// Список подразделений в которых произодится работа с указанными документами
		/// </summary>
		public static IList<Subdivision> GetSubdivisionsForDocumentTypes(IUnitOfWork uow, Type[] documentTypes)
		{
			Subdivision subdivisionAlias = null;
			TypeOfEntity typeOfEntityAlias = null;
			return uow.Session.QueryOver<Subdivision>(() => subdivisionAlias)
				.Left.JoinAlias(() => subdivisionAlias.DocumentTypes, () => typeOfEntityAlias)
				.WhereRestrictionOn(() => typeOfEntityAlias.Type).IsIn(documentTypes.Select(x => x.Name).ToArray())
				.List();
		}

		/// <summary>
		/// Список складов, которые привязаны к подразделению
		/// </summary>
		/// <returns>Склады подразделения</returns>
		/// <param name="uow">Uow.</param>
		/// <param name="subdivision">подразделение</param>
		/// <param name="orderByDescending">Если <c>true</c>, то сортируется список по убыванию.</param>
		public static IList<Warehouse> GetWarehouses(IUnitOfWork uow, Subdivision subdivision, bool orderByDescending = false)
		{
			var query = uow.Session.QueryOver<Warehouse>()
							.Where(w => w.OwningSubdivision == subdivision)
							.OrderBy(w => w.Name);
			return orderByDescending ? query.Desc().List() : query.Asc().List();
		}

		public static IEnumerable<Subdivision> GetAvailableSubdivionsForUser(IUnitOfWork uow, Type[] documentsTypes)
		{
			var validationResult = EntitySubdivisionForUserPermissionValidator.Validate(uow, documentsTypes);

			var subdivisionsList = new List<Subdivision>();
			foreach(var item in documentsTypes) {
				subdivisionsList.AddRange(validationResult
					.Where(x => x.GetPermission(item).Read)
					.Select(x => x.Subdivision)
				);
			}

			return subdivisionsList.Distinct();
		}
	}
}
