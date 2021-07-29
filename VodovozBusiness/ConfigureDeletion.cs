﻿using System.Collections.Generic;
using QS.Banks;
using QS.Banks.Domain;
using QS.BusinessCommon.Domain;
using QS.Deletion;
using QS.Deletion.Configuration;
using QS.HistoryLog.Domain;
using QS.Project.Domain;
using Vodovoz.Domain;
using Vodovoz.Domain.Accounting;
using Vodovoz.Domain.Cash;
using Vodovoz.Domain.Cash.CashTransfer;
using Vodovoz.Domain.Chats;
using Vodovoz.Domain.Client;
using Vodovoz.Domain.Complaints;
using Vodovoz.Domain.Contacts;
using Vodovoz.Domain.Documents;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Fuel;
using Vodovoz.Domain.Goods;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Operations;
using Vodovoz.Domain.Orders;
using Vodovoz.Domain.Orders.Documents;
using Vodovoz.Domain.Organizations;
using Vodovoz.Domain.Payments;
using Vodovoz.Domain.Permissions;
using Vodovoz.Domain.Proposal;
using Vodovoz.Domain.Sale;
using Vodovoz.Domain.Service;
using Vodovoz.Domain.Store;
using Vodovoz.Domain.StoredEmails;
using Vodovoz.Domain.StoredResources;
using Vodovoz.Domain.Suppliers;
using Vodovoz.Domain.WageCalculation;

namespace Vodovoz
{
	public static class Configure
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public static void ConfigureDeletion()
		{
			logger.Info("Настройка параметров удаления...");

			ConfigureDeletionBanks.ConfigureDeletion();

			#region Goods

			DeleteConfig.AddHibernateDeleteInfo<Nomenclature>()
						.AddDeleteDependenceFromCollection(item => item.NomenclaturePrice)
						.AddDeleteDependence<Equipment>(item => item.Nomenclature)
						.AddDeleteDependence<OrderItem>(x => x.Nomenclature)
						.AddDeleteDependence<OrderEquipment>(x => x.Nomenclature)
						.AddDeleteDependence<ServiceClaim>(x => x.Nomenclature)
						.AddDeleteDependence<ServiceClaimItem>(x => x.Nomenclature)
						.AddDeleteDependence<IncomingInvoiceItem>(item => item.Nomenclature)
						.AddDeleteDependence<IncomingWater>(x => x.Product)
						.AddDeleteDependence<IncomingWaterMaterial>(x => x.Nomenclature)
						.AddDeleteDependence<MovementDocumentItem>(x => x.Nomenclature)
						.AddDeleteDependence<WriteoffDocumentItem>(x => x.Nomenclature)
						.AddDeleteDependence<InventoryDocumentItem>(x => x.Nomenclature)
						.AddDeleteDependence<SelfDeliveryDocumentItem>(x => x.Nomenclature)
						.AddDeleteDependence<SelfDeliveryDocumentReturned>(x => x.Nomenclature)
						.AddDeleteDependence<RegradingOfGoodsDocumentItem>(x => x.NomenclatureOld)
						.AddDeleteDependence<RegradingOfGoodsDocumentItem>(x => x.NomenclatureNew)
						.AddDeleteDependence<RegradingOfGoodsTemplateItem>(x => x.NomenclatureOld)
						.AddDeleteDependence<RegradingOfGoodsTemplateItem>(x => x.NomenclatureNew)
						.AddDeleteDependence<ProductSpecificationMaterial>(x => x.Material)
						.AddDeleteDependence<ProductSpecification>(x => x.Product)
						.AddDeleteDependence<FineNomenclature>(x => x.Nomenclature)
						.AddDeleteDependence<CarLoadDocumentItem>(x => x.Nomenclature)
						.AddDeleteDependence<WarehouseMovementOperation>(item => item.Nomenclature)
						.AddDeleteDependence<CounterpartyMovementOperation>(item => item.Nomenclature)
						.AddDeleteDependence<NomenclatureImage>(x => x.Nomenclature)
						.AddDeleteDependence<PromotionalSetItem>(x => x.Nomenclature)
						.AddClearDependence<PaidRentPackage>(x => x.RentServiceDaily)
						.AddClearDependence<PaidRentPackage>(x => x.RentServiceMonthly)
						.AddClearDependence<PaidRentPackage>(x => x.DepositService)
						.AddClearDependence<DeliveryPoint>(x => x.DefaultWaterNomenclature)
						.AddClearDependence<Nomenclature>(x => x.DependsOnNomenclature)
						.AddDeleteDependence<ShiftChangeWarehouseDocumentItem>(x => x.Nomenclature)
						.AddDeleteDependence<NomenclaturePrice>(x => x.Nomenclature)
						.AddDeleteDependence<OrderDepositItem>(x => x.EquipmentNomenclature)
						.AddRemoveFromDependence<Certificate>(x => x.Nomenclatures)
						.AddDeleteDependence<SupplierPriceItem>(x => x.NomenclatureToBuy)
						.AddDeleteDependence<RequestToSupplierItem>(x => x.Nomenclature)
						;

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(EquipmentColors),
					SqlSelect = "SELECT id, name FROM @tablename ",
					DisplayString = "{1}",
					ClearItems = new List<ClearDependenceInfo> {
						ClearDependenceInfo.Create<Nomenclature> (item => item.EquipmentColor)
					}
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(EquipmentKind),
					SqlSelect = "SELECT id, name FROM @tablename ",
					DisplayString = "{1}",
					DeleteItems = new List<DeleteDependenceInfo> {
						DeleteDependenceInfo.Create<Nomenclature> (item => item.Kind),
						DeleteDependenceInfo.Create<PaidRentPackage> (item => item.EquipmentKind)
					}
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddHibernateDeleteInfo<Equipment>()
				.AddDeleteDependence<IncomingInvoiceItem>(item => item.Equipment)
				.AddDeleteDependence<OrderEquipment>(item => item.Equipment)
				.AddDeleteDependence<OrderItem>(item => item.Equipment)
				.AddDeleteDependence<ServiceClaim>(x => x.Equipment)
				.AddDeleteDependence<WarehouseMovementOperation>(item => item.Equipment)
				.AddDeleteDependence<CounterpartyMovementOperation>(item => item.Equipment)
				.AddDeleteDependence<WriteoffDocumentItem>(x => x.Equipment)
				.AddDeleteDependence<SelfDeliveryDocumentItem>(x => x.Equipment)
				.AddDeleteDependence<SelfDeliveryDocumentReturned>(x => x.Equipment)
				.AddDeleteDependence<CarLoadDocumentItem>(x => x.Equipment)
				.AddClearDependence<ServiceClaim>(x => x.ReplacementEquipment);

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(Manufacturer),
					SqlSelect = "SELECT id, name FROM @tablename ",
					DisplayString = "{1}",
					ClearItems = new List<ClearDependenceInfo> {
						ClearDependenceInfo.Create<Nomenclature> (item => item.Manufacturer)
					}
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(MeasurementUnits),
					SqlSelect = "SELECT id, name FROM @tablename ",
					DisplayString = "{1}",
					ClearItems = new List<ClearDependenceInfo> {
						ClearDependenceInfo.Create<Nomenclature> (item => item.Unit)
					}
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(NomenclaturePrice),
					SqlSelect = "SELECT id, price, min_count FROM @tablename ",
					DisplayString = "{1:C} (от {2})"
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddHibernateDeleteInfo<CullingCategory>()
						.AddClearDependence<WriteoffDocumentItem>(x => x.CullingCategory)
						.AddClearDependence<CarUnloadDocumentItem>(x => x.TypeOfDefect)
						.AddClearDependence<RegradingOfGoodsDocumentItem>(x => x.TypeOfDefect)
						;

			DeleteConfig.AddHibernateDeleteInfo<Folder1c>()
						.AddDeleteDependence<Folder1c>(x => x.Parent)
						.AddClearDependence<Nomenclature>(x => x.Folder1C);

			DeleteConfig.AddHibernateDeleteInfo<NomenclatureImage>();

			DeleteConfig.AddHibernateDeleteInfo<ProductGroup>()
				.AddDeleteDependence<ProductGroup>(x => x.Parent)
				.AddClearDependence<Nomenclature>(x => x.ProductGroup);

			DeleteConfig.AddHibernateDeleteInfo<Certificate>()
				.AddDeleteDependence<NomenclatureCertificateDocument>(x => x.Certificate)
				;
			#endregion

			#region Organization

			DeleteConfig.AddHibernateDeleteInfo<Organization>()
				.AddDeleteDependenceFromCollection(item => item.Phones)
				.AddDeleteDependenceFromCollection(item => item.Accounts)
				.AddDeleteDependence<CounterpartyContract>(item => item.Organization)
				.AddDeleteDependence<AccountIncome>(item => item.Organization)
				.AddDeleteDependence<AccountExpense>(item => item.Organization)
				.AddDeleteDependence<DocTemplate>(x => x.Organization)
				.AddDeleteDependence<EmployeeContract>(x => x.Organization)
				.AddClearDependence<Employee>(x => x.OrganisationForSalary)
				.AddClearDependence<Expense>(x => x.Organisation)
				.AddClearDependence<AdvanceReport>(x => x.Organisation);

			DeleteConfig.AddHibernateDeleteInfo<PaidRentPackage>();

			#endregion

			#region Contacts

			DeleteConfig.AddDeleteInfo(new DeleteInfo {
				ObjectClass = typeof(PhoneType),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "Тип телефона {1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<Phone> (item => item.PhoneType)
				}
			}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(new DeleteInfo {
				ObjectClass = typeof(Phone),
				SqlSelect = "SELECT id, number, additional FROM @tablename ",
				DisplayString = "Телефон {1} доб. {2}"
			}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(new DeleteInfo {
				ObjectClass = typeof(EmailType),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "Тип E-mail {1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<Email> (item => item.EmailType)
				}
			}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(new DeleteInfo {
				ObjectClass = typeof(Email),
				SqlSelect = "SELECT id, address FROM @tablename ",
				DisplayString = "{1}"
			}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(new DeleteInfo {
				ObjectClass = typeof(Person),
				SqlSelect = "SELECT id, surname, name FROM @tablename ",
				DisplayString = "{1} {2}"
			}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(new DeleteInfo {
				ObjectClass = typeof(Post),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}"
			}.FillFromMetaInfo()
			);

			#endregion

			#region Сотрудники

			DeleteConfig.AddHibernateDeleteInfo<Trainee>();

			//основной класс. не удаляем. в тестах настроен игнор.
			DeleteConfig.AddHibernateDeleteInfo<Employee>()
				.AddDeleteDependenceFromCollection(item => item.Phones)
				.AddDeleteDependenceFromCollection(item => item.Accounts)
				.AddDeleteDependence<Income>(item => item.Casher)
				.AddDeleteDependence<Expense>(item => item.Casher)
				.AddDeleteDependence<AdvanceReport>(item => item.Casher)
				.AddDeleteDependence<AdvanceReport>(item => item.Accountable)
				.AddDeleteDependence<RouteList>(x => x.Driver)
				.AddDeleteDependence<RouteList>(x => x.Forwarder)
				.AddDeleteDependence<RouteList>(x => x.Logistician)
				.AddDeleteDependence<PremiumItem>(x => x.Employee)
				.AddDeleteDependence<FineItem>(x => x.Employee)
				.AddDeleteDependence<EmployeeWorkChart>(item => item.Employee)
				.AddDeleteDependence<FuelDocument>(x => x.Driver)
				.AddDeleteDependence<FuelOperation>(x => x.Driver)
				.AddDeleteDependence<WagesMovementOperations>(x => x.Employee)
				.AddDeleteDependence<Track>(x => x.Driver)
				.AddDeleteDependence<Chat>(x => x.Driver)
				.AddDeleteDependence<AtWorkDriver>(x => x.Employee)
				.AddDeleteDependence<AtWorkForwarder>(x => x.Employee)
				.AddDeleteDependence<DriverDistrictPrioritySet>(x => x.Driver)
				.AddDeleteDependence<DriverWorkScheduleSet>(x => x.Driver)
				.AddDeleteDependence<EmployeeContract>(x => x.Employee)
				.AddClearDependence<DriverDistrictPrioritySet>(x => x.Author)
				.AddClearDependence<DriverDistrictPrioritySet>(x => x.LastEditor)
				.AddClearDependence<DriverWorkScheduleSet>(x => x.Author)
				.AddClearDependence<DriverWorkScheduleSet>(x => x.LastEditor)
				.AddClearDependence<Car>(item => item.Driver)
				.AddClearDependence<Counterparty>(item => item.Accountant)
				.AddClearDependence<Counterparty>(item => item.SalesManager)
				.AddClearDependence<Counterparty>(item => item.BottlesManager)
				.AddClearDependence<Order>(x => x.Author)
				.AddClearDependence<ServiceClaim>(x => x.Engineer)
				.AddClearDependence<ServiceClaimHistory>(x => x.Employee)
				.AddClearDependence<WriteoffDocument>(item => item.ResponsibleEmployee)
				.AddClearDependence<Organization>(item => item.Leader)
				.AddClearDependence<Organization>(item => item.Buhgalter)
				.AddClearDependence<Income>(item => item.Employee)
				.AddClearDependence<Expense>(item => item.Employee)
				.AddClearDependence<AccountExpense>(item => item.Employee)
				.AddClearDependence<CarLoadDocument>(item => item.Author)
				.AddClearDependence<CarLoadDocument>(item => item.LastEditor)
				.AddClearDependence<CarUnloadDocument>(x => x.Author)
				.AddClearDependence<CarUnloadDocument>(x => x.LastEditor)
				.AddClearDependence<IncomingInvoice>(x => x.Author)
				.AddClearDependence<IncomingInvoice>(x => x.LastEditor)
				.AddClearDependence<IncomingWater>(x => x.Author)
				.AddClearDependence<IncomingWater>(x => x.LastEditor)
				.AddClearDependence<MovementDocument>(x => x.Author)
				.AddClearDependence<MovementDocument>(x => x.LastEditor)
				.AddClearDependence<WriteoffDocument>(x => x.Author)
				.AddClearDependence<WriteoffDocument>(x => x.LastEditor)
				.AddClearDependence<InventoryDocument>(x => x.Author)
				.AddClearDependence<InventoryDocument>(x => x.LastEditor)
				.AddClearDependence<RegradingOfGoodsDocument>(x => x.Author)
				.AddClearDependence<RegradingOfGoodsDocument>(x => x.LastEditor)
				.AddClearDependence<SelfDeliveryDocument>(x => x.Author)
				.AddClearDependence<SelfDeliveryDocument>(x => x.LastEditor)
				.AddClearDependence<RouteList>(x => x.Cashier)
				.AddClearDependence<RouteList>(x => x.ClosedBy)
				.AddClearDependence<Residue>(x => x.Author)
				.AddClearDependence<Residue>(x => x.LastEditAuthor)
				.AddClearDependence<Subdivision>(x => x.Chief)
				.AddClearDependence<ChatMessage>(x => x.Sender)
				.AddClearDependence<Employee>(x => x.DefaultForwarder)
				.AddClearDependence<RequestToSupplier>(x => x.Creator)
				;

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(Citizenship),
					SqlSelect = "SELECT id, name FROM @tablename ",
					DisplayString = "{1}",
					ClearItems = new List<ClearDependenceInfo> {
						ClearDependenceInfo.Create<Employee> (item => item.Citizenship)
					}
				}.FillFromMetaInfo()
			);
			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(Nationality),
					SqlSelect = "SELECT id, name FROM @tablename ",
					DisplayString = "{1}",
					ClearItems = new List<ClearDependenceInfo> {
						ClearDependenceInfo.Create<Employee> (item => item.Nationality)
					}
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddHibernateDeleteInfo<User>()
				.AddDeleteDependence<UserSettings>(x => x.User)
				.AddClearDependence<Employee>(item => item.User)
				.AddClearDependence<QS.HistoryLog.Domain.ChangeSet>(x => x.User)
				.AddDeleteDependence<EntityUserPermission>(x => x.User)
				.AddDeleteDependence<PresetUserPermission>(x => x.User)
				;

			DeleteConfig.AddHibernateDeleteInfo<UserBase>();

			DeleteConfig.AddHibernateDeleteInfo<UserSettings>();

			DeleteConfig.AddHibernateDeleteInfo<Premium>()
						.AddDeleteDependence<PremiumItem>(x => x.Premium);

			DeleteConfig.AddHibernateDeleteInfo<PremiumItem>()
						.AddDeleteCascadeDependence(item => item.WageOperation);

			DeleteConfig.AddHibernateDeleteInfo<Fine>()
				.AddDeleteDependence<FineItem>(x => x.Fine)
				.AddDeleteDependence<FineNomenclature>(x => x.Fine)
				.AddClearDependence<InventoryDocumentItem>(x => x.Fine)
				.AddClearDependence<WriteoffDocumentItem>(x => x.Fine)
				.AddClearDependence<RegradingOfGoodsDocumentItem>(x => x.Fine)
				.AddClearDependence<RouteList>(x => x.BottleFine)
				.AddRemoveFromDependence<Complaint>(x => x.Fines)
				;

			DeleteConfig.AddHibernateDeleteInfo<FineItem>()
				.AddDeleteCascadeDependence(item => item.WageOperation)
				.AddDeleteCascadeDependence(item => item.FuelOutlayedOperation);

			DeleteConfig.AddHibernateDeleteInfo<FineTemplate>();

			//основной класс. не удаляем. в тестах настроен игнор.
			DeleteConfig.AddHibernateDeleteInfo<Subdivision>()
						.AddClearDependence<Subdivision>(item => item.ParentSubdivision)
						.AddClearDependence<Employee>(item => item.Subdivision)
						.AddClearDependence<Warehouse>(item => item.OwningSubdivision)
						.AddClearDependence<RouteList>(item => item.ClosingSubdivision)
						.AddDeleteDependence<EntitySubdivisionPermission>(item => item.Subdivision)
						.AddDeleteDependence<EntitySubdivisionPermissionExtended>(item => item.Subdivision)
						.AddClearDependence<UndeliveredOrder>(item => item.InProcessAtDepartment)
						.AddClearDependence<GuiltyInUndelivery>(item => item.GuiltyDepartment)
						.AddDeleteDependence<Income>(item => item.RelatedToSubdivision)
						.AddDeleteDependence<Expense>(item => item.RelatedToSubdivision)
						.AddDeleteDependence<CashTransferOperation>(item => item.SubdivisionTo)
						.AddDeleteDependence<CashTransferOperation>(item => item.SubdivisionFrom)
						.AddDeleteDependence<CashTransferDocumentBase>(item => item.CashSubdivisionTo)
						.AddDeleteDependence<CashTransferDocumentBase>(item => item.CashSubdivisionFrom)
						.AddDeleteDependence<AdvanceReport>(item => item.RelatedToSubdivision)
						;

			DeleteConfig.AddHibernateDeleteInfo<EmployeeContract>();
			DeleteConfig.AddHibernateDeleteInfo<EmployeeDocument>()
						.AddClearDependence<EmployeeContract>(x => x.Document)
						.AddRemoveFromDependence<Personnel>(x => x.Documents)
						;
			DeleteConfig.AddHibernateDeleteInfo<Personnel>();
			DeleteConfig.AddHibernateDeleteInfo<EmployeeWorkChart>();
			DeleteConfig.AddHibernateDeleteInfo<CarProxyDocument>();
			DeleteConfig.AddHibernateDeleteInfo<M2ProxyDocument>();
			DeleteConfig.AddHibernateDeleteInfo<ProxyDocument>();

			DeleteConfig.AddHibernateDeleteInfo<Chat>();

			//Не добавляем сообщения чата чтобы не заполонять вывод удаления. все сообщения удалятся вместе с чатом.

			DeleteConfig.AddHibernateDeleteInfo<ChatMessage>();

			DeleteConfig.AddHibernateDeleteInfo<EmployeeWageParameter>();

			DeleteConfig.AddHibernateDeleteInfo<SalesPlanWageParameterItem>()
				.AddDeleteDependence<EmployeeWageParameter>(x => x.WageParameterItem);

			DeleteConfig.AddHibernateDeleteInfo<SalesPlanItem>().HasSubclasses();

			DeleteConfig.AddHibernateDeleteInfo<SalesPlan>()
				.AddDeleteDependence<SalesPlanItem>(x => x.SalesPlan)
				.AddDeleteDependence<SalesPlanWageParameterItem>(x => x.SalesPlan)
				.AddClearDependence<Subdivision>(x => x.DefaultSalesPlan);

			#endregion

			//Контрагент и все что сним связано
			#region NearCounterparty

			//основной класс. не удаляем. в тестах настроен игнор.
			DeleteConfig.AddHibernateDeleteInfo<Counterparty>()
				.AddDeleteDependenceFromCollection(item => item.Phones)
				.AddDeleteDependenceFromCollection(item => item.Emails)
				.AddDeleteDependenceFromCollection(item => item.Accounts)
				.AddDeleteDependence<DeliveryPoint>(item => item.Counterparty)
				.AddDeleteDependence<Proxy>(item => item.Counterparty)
				.AddDeleteDependence<Contact>(item => item.Counterparty)
				.AddDeleteDependence<CounterpartyContract>(item => item.Counterparty)
				.AddDeleteDependence<BottlesMovementOperation>(item => item.Counterparty)
				.AddDeleteDependence<DepositOperation>(item => item.Counterparty)
				.AddDeleteDependence<CounterpartyMovementOperation>(item => item.WriteoffCounterparty)
				.AddDeleteDependence<CounterpartyMovementOperation>(item => item.IncomingCounterparty)
				.AddDeleteDependence<IncomingInvoice>(item => item.Contractor)
				.AddDeleteDependence<MoneyMovementOperation>(item => item.Counterparty)
				.AddDeleteDependence<Order>(item => item.Client)
				.AddDeleteDependence<ServiceClaim>(x => x.Counterparty)
				.AddDeleteDependence<WriteoffDocument>(item => item.Client)
				.AddDeleteDependence<AccountIncome>(item => item.Counterparty)
				.AddDeleteDependence<AccountExpense>(item => item.Counterparty)
				.AddDeleteDependence<Income>(x => x.Customer)
				.AddDeleteDependence<Residue>(x => x.Customer)
				.AddClearDependence<Counterparty>(item => item.MainCounterparty)
				.AddClearDependence<Counterparty>(x => x.PreviousCounterparty)
				.AddClearDependence<Equipment>(x => x.AssignedToClient)
				.AddDeleteDependence<SupplierPriceItem>(x => x.Supplier)
				.AddDeleteDependence<CallTask>(x => x.Counterparty)
				;

			DeleteConfig.AddHibernateDeleteInfo<Contact>()
				.AddDeleteDependenceFromCollection(item => item.Emails)
				.AddDeleteDependenceFromCollection(item => item.Phones)
				.AddClearDependence<Counterparty>(item => item.MainContact)
				.AddClearDependence<Counterparty>(item => item.FinancialContact)
				.AddRemoveFromDependence<DeliveryPoint>(x => x.ResponsiblePersons);

			DeleteConfig.AddClearDependence<Post>(ClearDependenceInfo.Create<Contact>(item => item.Post));

			DeleteConfig.AddHibernateDeleteInfo<Proxy>()
				.AddDeleteDependenceFromCollection(item => item.Persons);

			DeleteConfig.AddHibernateDeleteInfo<CounterpartyContract>()
				.AddDeleteDependence<OrderContract>(x => x.Contract)
				.AddClearDependence<Order>(x => x.Contract);

			DeleteConfig.AddHibernateDeleteInfo<SupplierPriceItem>()
						;

			DeleteConfig.AddHibernateDeleteInfo<RequestToSupplier>()
						.AddDeleteDependence<RequestToSupplierItem>(x => x.RequestToSupplier)
						;

			DeleteConfig.AddHibernateDeleteInfo<RequestToSupplierItem>()
						.AddDeleteDependence<RequestToSupplierItem>(x => x.TransferedFromItem)
						;

			//основной класс. не удаляем. в тестах настроен игнор.
			DeleteConfig.AddHibernateDeleteInfo<DeliveryPoint>()
				.AddClearDependence<BottlesMovementOperation>(item => item.DeliveryPoint)
				.AddDeleteDependence<TransferOperationDocument>(item => item.FromDeliveryPoint)
				.AddDeleteDependence<TransferOperationDocument>(item => item.ToDeliveryPoint)
				.AddClearDependence<DepositOperation>(item => item.DeliveryPoint)
				.AddDeleteDependence<CounterpartyMovementOperation>(item => item.WriteoffDeliveryPoint)
				.AddDeleteDependence<CounterpartyMovementOperation>(item => item.IncomingDeliveryPoint)
				.AddClearDependence<Order>(x => x.DeliveryPoint)
				.AddDeleteDependence<WriteoffDocument>(x => x.DeliveryPoint)
				.AddDeleteDependence<Residue>(x => x.DeliveryPoint)
				.AddRemoveFromDependence<Proxy>(item => item.DeliveryPoints)
				.AddRemoveFromDependence<Contact>(x => x.DeliveryPoints)
				.AddClearDependence<CallTask>(x => x.DeliveryPoint)
				.AddClearDependence<ServiceClaim>(x => x.DeliveryPoint);

			DeleteConfig.AddHibernateDeleteInfo<TransferOperationDocument>()
				.AddDeleteCascadeDependence(item => item.IncBottlesOperation)
				.AddDeleteCascadeDependence(item => item.OutBottlesOperation)
				.AddDeleteCascadeDependence(item => item.IncBottlesDepositOperation)
				.AddDeleteCascadeDependence(item => item.OutBottlesDepositOperation)
				.AddDeleteCascadeDependence(item => item.IncEquipmentDepositOperation)
				.AddDeleteCascadeDependence(item => item.OutEquipmentDepositOperation);

			DeleteConfig.AddHibernateDeleteInfo<CallTask>();

			DeleteConfig.AddHibernateDeleteInfo<DocTemplate>()
				.AddClearDependence<CounterpartyContract>(x => x.DocumentTemplate)
				.AddClearDependence<EmployeeContract>(x => x.EmployeeContractTemplate)
				;

			DeleteConfig.AddHibernateDeleteInfo<ClientCameFrom>()
						.AddClearDependence<Counterparty>(x => x.CameFrom);

			DeleteConfig.AddHibernateDeleteInfo<Tag>();

			DeleteConfig.AddHibernateDeleteInfo<DeliveryPointCategory>()
				.AddClearDependence<DeliveryPoint>(x => x.Category)
				;

			DeleteConfig.AddHibernateDeleteInfo<CounterpartyActivityKind>();

			#endregion

			#region Logistics

			DeleteConfig.AddHibernateDeleteInfo<Car>()
				.AddDeleteDependence<RouteList>(x => x.Car)
				.AddDeleteDependence<FuelDocument>(x => x.Car)
				.AddDeleteDependence<FuelOperation>(x => x.Car)
				.AddDeleteDependence<AtWorkDriver>(x => x.Car)
				.AddDeleteDependence<CashTransferDocumentBase>(x => x.Car)
				.AddDeleteDependence<CarProxyDocument>(x => x.Car)
				;

			DeleteConfig.AddHibernateDeleteInfo<FuelType>()
				.AddDeleteDependence<FuelDocument>(x => x.Fuel)
				.AddDeleteDependence<FuelOperation>(x => x.Fuel)
				.AddClearDependence<Car>(x => x.FuelType);

			DeleteConfig.AddHibernateDeleteInfo<DeliveryShift>()
				.AddClearDependence<RouteList>(x => x.Shift);

			DeleteConfig.AddHibernateDeleteInfo<RouteList>()
				.AddDeleteDependence<RouteListItem>(x => x.RouteList)
				.AddDeleteDependence<CarLoadDocument>(x => x.RouteList)
				.AddDeleteDependence<CarUnloadDocument>(x => x.RouteList)
				.AddDeleteDependence<AddressTransferDocument>(x => x.RouteListFrom)
				.AddDeleteDependence<AddressTransferDocument>(x => x.RouteListTo)
				.AddDeleteDependence<Track>(x => x.RouteList)
				.AddDeleteDependence<FuelDocument>(x => x.RouteList)
				.AddClearDependence<Fine>(x => x.RouteList)
				.AddDeleteCascadeDependence(x => x.FuelOutlayedOperation)
				.AddDeleteCascadeDependence(x => x.DriverWageOperation)
				.AddDeleteCascadeDependence(x => x.ForwarderWageOperation);

			DeleteConfig.AddHibernateDeleteInfo<RouteList>()
				.AddClearDependence<Expense>(x => x.RouteListClosing)
				.AddClearDependence<Income>(x => x.RouteListClosing);

			DeleteConfig.AddHibernateDeleteInfo<RouteColumn>()
				.AddClearDependence<Nomenclature>(x => x.RouteListColumn);

			DeleteConfig.AddHibernateDeleteInfo<RouteListItem>()
						.AddDeleteDependence<DeliveryDocument>(x => x.RouteListItem)
						.AddDeleteDependence<RouteListItemCashDistributionDocument>(x => x.RouteListItem)
						.AddDeleteDependence<AddressTransferDocumentItem>(x => x.OldAddress)
						.AddDeleteDependence<AddressTransferDocumentItem>(x => x.NewAddress)
						.AddRemoveFromDependence<RouteList>(x => x.Addresses);

			DeleteConfig.AddHibernateDeleteInfo<Track>();

			DeleteConfig.AddHibernateDeleteInfo<GeographicGroup>()
						.AddDeleteDependence<AtWorkDriver>(x => x.GeographicGroup)
						.AddDeleteDependence<Subdivision>(x => x.GeographicGroup)
						.AddRemoveFromDependence<Car>(x => x.GeographicGroups)
						.AddRemoveFromDependence<RouteList>(x => x.GeographicGroups)
						.AddRemoveFromDependence<District>(x => x.GeographicGroup)
						;

			DeleteConfig.AddHibernateDeleteInfo<CarEventType>();

			DeleteConfig.AddHibernateDeleteInfo<CarEvent>();

			#region Формирование МЛ

			DeleteConfig.AddHibernateDeleteInfo<AtWorkDriverDistrictPriority>();

			DeleteConfig.AddHibernateDeleteInfo<AtWorkDriver>()
				.AddDeleteDependence<AtWorkDriverDistrictPriority>(x => x.Driver);

			DeleteConfig.AddHibernateDeleteInfo<AtWorkForwarder>()
				.AddClearDependence<AtWorkDriver>(x => x.WithForwarder);

			DeleteConfig.AddHibernateDeleteInfo<DriverDistrictPriority>();
			
			DeleteConfig.AddHibernateDeleteInfo<DriverDistrictPrioritySet>()
				.AddDeleteDependence<DriverDistrictPriority>(x => x.DriverDistrictPrioritySet);

			DeleteConfig.AddHibernateDeleteInfo<DeliveryDaySchedule>()
				.AddDeleteDependence<AtWorkDriver>(x => x.DaySchedule);
				//.AddClearDependence<Employee>(x => x.DefaultDaySheldule);
				
			DeleteConfig.AddHibernateDeleteInfo<DriverWorkSchedule>();
			
			DeleteConfig.AddHibernateDeleteInfo<DriverWorkScheduleSet>()
				.AddDeleteDependence<DriverWorkSchedule>(x => x.DriverWorkScheduleSet);

			#endregion

			#endregion

			#region Financial District

			DeleteConfig.AddHibernateDeleteInfo<FinancialDistrictsSet>()
				.AddDeleteDependence<FinancialDistrict>(x => x.FinancialDistrictsSet);
			
			DeleteConfig.AddHibernateDeleteInfo<FinancialDistrict>()
				.AddClearDependence<FinancialDistrict>(i => i.CopyOf)
				.AddRemoveFromDependence<FinancialDistrictsSet>(x => x.FinancialDistricts);

			#endregion

			#region District

			DeleteConfig.AddHibernateDeleteInfo<DistrictsSet>()
				.AddDeleteDependence<District>(x => x.DistrictsSet);

			DeleteConfig.AddHibernateDeleteInfo<District>()
				.AddDeleteDependence<DriverDistrictPriority>(i => i.District)
				.AddClearDependence<AtWorkDriverDistrictPriority>(i => i.District)
				.AddClearDependence<DeliveryPoint>(i => i.District)
				.AddClearDependence<District>(i => i.CopyOf)
				.AddDeleteDependence<CommonDistrictRuleItem>(item => item.District)
				.AddDeleteDependence<DeliveryScheduleRestriction>(item => item.District)
				.AddDeleteDependence<WeekDayDistrictRuleItem>(item => item.District)
				.AddRemoveFromDependence<DistrictsSet>(x => x.Districts);

			DeleteConfig.AddHibernateDeleteInfo<CommonDistrictRuleItem>();
			DeleteConfig.AddHibernateDeleteInfo<WeekDayDistrictRuleItem>();
			DeleteConfig.AddHibernateDeleteInfo<DeliveryScheduleRestriction>();
			
			DeleteConfig.AddHibernateDeleteInfo<TariffZone>()
				.AddClearDependence<District>(i => i.TariffZone);
			
			DeleteConfig.AddHibernateDeleteInfo<DeliveryPriceRule>()
				.AddDeleteDependence<CommonDistrictRuleItem>(item => item.DeliveryPriceRule)
				.AddDeleteDependence<WeekDayDistrictRuleItem>(item => item.DeliveryPriceRule);

			DeleteConfig.AddHibernateDeleteInfo<DeliverySchedule>()
				.AddClearDependence<Order>(x => x.DeliverySchedule)
				.AddClearDependence<DeliveryPoint>(x => x.DeliverySchedule)
				.AddDeleteDependence<DeliveryScheduleRestriction>(x => x.DeliverySchedule);

			DeleteConfig.AddHibernateDeleteInfo<AcceptBefore>()
				.AddClearDependence<DeliveryScheduleRestriction>(i => i.AcceptBefore);

			#endregion

			//Вокруг заказа
			#region Order

			//основной класс. не удаляем. в тестах настроен игнор.
			DeleteConfig.AddHibernateDeleteInfo<Order>() //FIXME : Костыль пока не будет нормального механизма блокировки 
						.AddDeleteDependence<OrderItem>(item => item.Order)
						.AddDeleteDependence<OrderEquipment>(x => x.Order)
						.AddDeleteDependence<OrderDocument>(item => item.Order)
						.AddDeleteDependence<OrderDepositItem>(item => item.Order)
						.AddDeleteDependence<RouteListItem>(x => x.Order)
						.AddDeleteDependence<BottlesMovementOperation>(item => item.Order)
						.AddDeleteDependence<DepositOperation>(x => x.Order)
						.AddDeleteDependence<MoneyMovementOperation>(x => x.Order)
						.AddDeleteDependence<SelfDeliveryDocument>(x => x.Order)
						.AddDeleteDependence<OrderDocument>(x => x.AttachedToOrder)
						.AddDeleteCascadeDependence(x => x.BottlesMovementOperation)
						.AddDeleteCascadeDependence(x => x.MoneyMovementOperation)
						.AddClearDependence<Order>(x => x.PreviousOrder)
						.AddClearDependence<ServiceClaim>(x => x.InitialOrder)
						.AddClearDependence<ServiceClaim>(x => x.FinalOrder)
						.AddDeleteDependence<UndeliveredOrder>(x => x.OldOrder)
						.AddClearDependence<UndeliveredOrder>(x => x.NewOrder)
						.AddDeleteDependence<StoredEmail>(x => x.Order)
						.AddClearDependence<Income>(x => x.Order)
						.AddClearDependence<Expense>(x => x.Order)
						.AddClearDependence<M2ProxyDocument>(x => x.Order)
						.AddClearDependence<Counterparty>(x => x.FirstOrder)
						;

			DeleteConfig.AddHibernateDeleteInfo<OrderItem>()
				.AddDeleteDependence<OrderEquipment>(item => item.OrderItem)
				.AddDeleteDependence<SelfDeliveryDocumentItem>(x => x.OrderItem)
				.AddDeleteCascadeDependence(x => x.CounterpartyMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<OrderEquipment>()
						.AddDeleteDependence<SelfDeliveryDocumentItem>(x => x.OrderEquipment)
						;

			DeleteConfig.AddHibernateDeleteInfo<OrderDocument>().HasSubclasses();

			DeleteConfig.AddHibernateDeleteInfo<OrderDepositItem>()
				.AddDeleteCascadeDependence(x => x.DepositOperation);

			DeleteConfig.AddHibernateDeleteInfo<CommentTemplate>();

			DeleteConfig.AddHibernateDeleteInfo<ServiceClaim>()
				.AddDeleteDependence<ServiceClaimItem>(x => x.ServiceClaim)
				.AddDeleteDependence<ServiceClaimHistory>(x => x.ServiceClaim);

			DeleteConfig.AddHibernateDeleteInfo<ServiceClaimItem>();

			DeleteConfig.AddHibernateDeleteInfo<ServiceClaimHistory>();

			DeleteConfig.AddHibernateDeleteInfo<DiscountReason>()
				.AddDeleteDependence<OrderItem>(x => x.DiscountReason)
				.AddDeleteDependence<PromotionalSet>(x => x.PromoSetDiscountReason)
				;

			DeleteConfig.AddHibernateDeleteInfo<NonReturnReason>();

			DeleteConfig.AddHibernateDeleteInfo<PaymentFrom>()
						.AddClearDependence<Order>(x => x.PaymentByCardFrom)
						;

			#endregion

			#region Недовозы

			DeleteConfig.AddHibernateDeleteInfo<UndeliveredOrder>()
						.AddDeleteDependence<GuiltyInUndelivery>(x => x.UndeliveredOrder)
						.AddDeleteDependence<UndeliveredOrderComment>(x => x.UndeliveredOrder)
						.AddDeleteDependence<Fine>(x => x.UndeliveredOrder)
						;
			DeleteConfig.AddHibernateDeleteInfo<UndeliveredOrderComment>();
			DeleteConfig.AddHibernateDeleteInfo<GuiltyInUndelivery>();

			#endregion

			#region Рекламные наборы

			DeleteConfig.AddHibernateDeleteInfo<PromotionalSet>()
						.AddDeleteDependence<PromotionalSetItem>(x => x.PromoSet)
						.AddClearDependence<OrderItem>(x => x.PromoSet)
						;
			DeleteConfig.AddHibernateDeleteInfo<PromotionalSetItem>();

			#endregion

			#region Документы заказа

			DeleteConfig.AddHibernateDeleteInfo<BillDocument>();

			DeleteConfig.AddHibernateDeleteInfo<DoneWorkDocument>();

			DeleteConfig.AddHibernateDeleteInfo<EquipmentTransferDocument>();

			DeleteConfig.AddHibernateDeleteInfo<InvoiceBarterDocument>();

			DeleteConfig.AddHibernateDeleteInfo<InvoiceDocument>();

			DeleteConfig.AddHibernateDeleteInfo<OrderContract>();

			DeleteConfig.AddHibernateDeleteInfo<UPDDocument>();

			DeleteConfig.AddHibernateDeleteInfo<DriverTicketDocument>();

			DeleteConfig.AddHibernateDeleteInfo<ShetFacturaDocument>();

			DeleteConfig.AddHibernateDeleteInfo<Torg12Document>();

			DeleteConfig.AddHibernateDeleteInfo<SpecialBillDocument>();
			DeleteConfig.AddHibernateDeleteInfo<SpecialUPDDocument>();
			DeleteConfig.AddHibernateDeleteInfo<OrderM2Proxy>();
			DeleteConfig.AddHibernateDeleteInfo<EquipmentReturnDocument>();
			DeleteConfig.AddHibernateDeleteInfo<InvoiceContractDoc>();
			DeleteConfig.AddHibernateDeleteInfo<NomenclatureCertificateDocument>();
			#endregion

			//Документы
			#region Склад

			//основной класс. не удаляем. в тестах настроен игнор.
			DeleteConfig.AddHibernateDeleteInfo<Warehouse>()
				.AddDeleteDependence<IncomingInvoice>(item => item.Warehouse)
				.AddDeleteDependence<CarLoadDocument>(x => x.Warehouse)
				.AddDeleteDependence<CarUnloadDocument>(x => x.Warehouse)
				.AddDeleteDependence<IncomingWater>(x => x.IncomingWarehouse)
				.AddDeleteDependence<IncomingWater>(x => x.WriteOffWarehouse)
				.AddDeleteDependence<MovementDocument>(x => x.FromWarehouse)
				.AddDeleteDependence<MovementDocument>(x => x.ToWarehouse)
				.AddDeleteDependence<WarehouseMovementOperation>(x => x.IncomingWarehouse)
				.AddDeleteDependence<WarehouseMovementOperation>(x => x.WriteoffWarehouse)
				.AddDeleteDependence<WriteoffDocument>(x => x.WriteoffWarehouse)
				.AddDeleteDependence<InventoryDocument>(x => x.Warehouse)
				.AddDeleteDependence<ShiftChangeWarehouseDocument>(x => x.Warehouse)
				.AddDeleteDependence<RegradingOfGoodsDocument>(x => x.Warehouse)
				.AddDeleteDependence<SelfDeliveryDocument>(x => x.Warehouse)
				.AddClearDependence<UserSettings>(x => x.DefaultWarehouse);

			DeleteConfig.AddHibernateDeleteInfo<IncomingInvoice>()
				.AddDeleteDependence<IncomingInvoiceItem>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<IncomingInvoiceItem>()
				.AddDeleteCascadeDependence(x => x.IncomeGoodsOperation);

			DeleteConfig.AddHibernateDeleteInfo<IncomingWater>()
				.AddDeleteDependence<IncomingWaterMaterial>(x => x.Document)
				.AddDeleteCascadeDependence(x => x.ProduceOperation);

			DeleteConfig.AddHibernateDeleteInfo<IncomingWaterMaterial>()
				.AddDeleteCascadeDependence(x => x.ConsumptionMaterialOperation);

			DeleteConfig.AddHibernateDeleteInfo<MovementDocument>()
				.AddDeleteDependence<MovementDocumentItem>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<MovementDocumentItem>()
				.AddDeleteCascadeDependence(x => x.WarehouseWriteoffOperation)
				.AddDeleteCascadeDependence(x => x.WarehouseIncomeOperation);

			DeleteConfig.AddHibernateDeleteInfo<WriteoffDocument>()
				.AddDeleteDependence<WriteoffDocumentItem>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<WriteoffDocumentItem>()
				.AddDeleteCascadeDependence(x => x.CounterpartyWriteoffOperation)
				.AddDeleteCascadeDependence(x => x.WarehouseWriteoffOperation);

			DeleteConfig.AddHibernateDeleteInfo<ProductSpecification>()
				.AddDeleteDependenceFromCollection(x => x.Materials);

			DeleteConfig.AddHibernateDeleteInfo<ProductSpecificationMaterial>();

			DeleteConfig.AddHibernateDeleteInfo<CarLoadDocument>()
				.AddDeleteDependence<CarLoadDocumentItem>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<CarLoadDocumentItem>()
				.AddDeleteCascadeDependence(x => x.WarehouseMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<CarUnloadDocument>()
				.AddDeleteDependence<CarUnloadDocumentItem>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<CarUnloadDocumentItem>()
				.AddDeleteCascadeDependence(x => x.WarehouseMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<InventoryDocument>()
				.AddDeleteDependence<InventoryDocumentItem>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<InventoryDocumentItem>()
				.AddDeleteCascadeDependence(x => x.WarehouseChangeOperation);

			DeleteConfig.AddHibernateDeleteInfo<SelfDeliveryDocument>()
				.AddDeleteDependence<SelfDeliveryDocumentItem>(x => x.Document)
				.AddDeleteDependence<SelfDeliveryDocumentReturned>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<SelfDeliveryDocumentItem>()
				.AddDeleteCascadeDependence(x => x.WarehouseMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<SelfDeliveryDocumentReturned>()
				.AddDeleteCascadeDependence(x => x.WarehouseMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<RegradingOfGoodsDocument>()
				.AddDeleteDependence<RegradingOfGoodsDocumentItem>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<ShiftChangeWarehouseDocument>()
				.AddDeleteDependence<ShiftChangeWarehouseDocumentItem>(x => x.Document);

			DeleteConfig.AddHibernateDeleteInfo<ShiftChangeWarehouseDocumentItem>();

			DeleteConfig.AddHibernateDeleteInfo<RegradingOfGoodsDocumentItem>()
				.AddDeleteCascadeDependence(x => x.WarehouseIncomeOperation)
				.AddDeleteCascadeDependence(x => x.WarehouseWriteOffOperation);

			DeleteConfig.AddHibernateDeleteInfo<RegradingOfGoodsTemplate>()
				.AddDeleteDependence<RegradingOfGoodsTemplateItem>(x => x.Template);

			DeleteConfig.AddHibernateDeleteInfo<RegradingOfGoodsTemplateItem>();

			DeleteConfig.AddHibernateDeleteInfo<MovementWagon>()
				.AddClearDependence<MovementDocument>(x => x.MovementWagon);

			DeleteConfig.AddHibernateDeleteInfo<DriverComplaintReason>();

			DeleteConfig.AddHibernateDeleteInfo<Residue>()
				.AddDeleteDependence<ResidueEquipmentDepositItem>(item => item.Residue)
				.AddDeleteCascadeDependence(x => x.BottlesDepositOperation)
				.AddDeleteCascadeDependence(x => x.EquipmentDepositOperation)
				.AddDeleteCascadeDependence(x => x.BottlesMovementOperation)
				.AddDeleteCascadeDependence(x => x.MoneyMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<ResidueEquipmentDepositItem>();

			#endregion

			//Операции в журналах
			#region Operations

			DeleteConfig.AddHibernateDeleteInfo<BottlesMovementOperation>()
				.RequiredCascadeDeletion()
				.AddClearDependence<Order>(x => x.BottlesMovementOperation)
				.AddDeleteDependence<Residue>(x => x.BottlesMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<WarehouseMovementOperation>()
				.RequiredCascadeDeletion()
				.AddDeleteDependence<CarLoadDocumentItem>(x => x.WarehouseMovementOperation)
				.AddDeleteDependence<CarUnloadDocumentItem>(x => x.WarehouseMovementOperation)
				.AddDeleteDependence<IncomingInvoiceItem>(x => x.IncomeGoodsOperation)
				.AddDeleteDependence<IncomingWater>(x => x.ProduceOperation)
				.AddDeleteDependence<IncomingWaterMaterial>(x => x.ConsumptionMaterialOperation)
				.AddDeleteDependence<MovementDocumentItem>(x => x.WarehouseWriteoffOperation)
				.AddDeleteDependence<MovementDocumentItem>(x => x.WarehouseIncomeOperation)
				.AddDeleteDependence<WriteoffDocumentItem>(x => x.WarehouseWriteoffOperation)
				.AddDeleteDependence<InventoryDocumentItem>(x => x.WarehouseChangeOperation)
				.AddDeleteDependence<RegradingOfGoodsDocumentItem>(x => x.WarehouseIncomeOperation)
				.AddDeleteDependence<RegradingOfGoodsDocumentItem>(x => x.WarehouseWriteOffOperation)
				.AddDeleteDependence<SelfDeliveryDocumentItem>(x => x.WarehouseMovementOperation)
				.AddDeleteDependence<SelfDeliveryDocumentReturned>(x => x.WarehouseMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<CounterpartyMovementOperation>()
				.RequiredCascadeDeletion()
				.AddDeleteDependence<WriteoffDocumentItem>(x => x.CounterpartyWriteoffOperation)
				.AddDeleteDependence<OrderItem>(x => x.CounterpartyMovementOperation)
				.AddDeleteDependence<OrderEquipment>(x => x.CounterpartyMovementOperation)
				;

			DeleteConfig.AddHibernateDeleteInfo<MoneyMovementOperation>()
				.RequiredCascadeDeletion()
				.AddClearDependence<Order>(x => x.MoneyMovementOperation)
				.AddDeleteDependence<AccountExpense>(x => x.MoneyOperation)
				.AddDeleteDependence<AccountIncome>(x => x.MoneyOperation)
				.AddDeleteDependence<Residue>(x => x.MoneyMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<DepositOperation>()
				.RequiredCascadeDeletion()
				.AddDeleteDependence<OrderDepositItem>(x => x.DepositOperation)
				.AddDeleteDependence<Residue>(x => x.BottlesDepositOperation)
				.AddDeleteDependence<Residue>(x => x.EquipmentDepositOperation);

			DeleteConfig.AddHibernateDeleteInfo<WagesMovementOperations>()
				.RequiredCascadeDeletion()
				.AddDeleteDependence<FineItem>(item => item.WageOperation)
				.AddClearDependence<RouteList>(item => item.DriverWageOperation)
				.AddClearDependence<RouteList>(item => item.ForwarderWageOperation)
				.AddDeleteDependence<Expense>(item => item.WagesOperation);

			#endregion

			#region Права

			DeleteConfig.AddHibernateDeleteInfo<TypeOfEntity>()
				.AddDeleteDependence<EntityUserPermission>(x => x.TypeOfEntity)
				;
			DeleteConfig.AddHibernateDeleteInfo<EntityUserPermission>();
			DeleteConfig.AddHibernateDeleteInfo<PresetUserPermission>();

			#endregion

			#region Cash

			DeleteConfig.AddHibernateDeleteInfo<Income>()
				.AddDeleteDependence<AdvanceClosing>(x => x.Income)
				.AddDeleteDependence<AdvanceReport>(x => x.ChangeReturn)
				.AddDeleteDependence<CashOrganisationDistributionDocument>(x => x.Income);

			DeleteConfig.AddHibernateDeleteInfo<Expense>()
				.AddDeleteDependence<AdvanceClosing>(x => x.AdvanceExpense)
				.AddDeleteDependence<FuelDocument>(x => x.FuelCashExpense)
				.AddDeleteDependence<CashOrganisationDistributionDocument>(x => x.Expense)
				.AddDeleteCascadeDependence(x => x.WagesOperation);

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(AdvanceReport),
					SqlSelect = "SELECT id, date FROM @tablename ",
					DisplayString = "Авансовый отчет №{0} от {1:d}",
					DeleteItems = new List<DeleteDependenceInfo> {
						DeleteDependenceInfo.Create<AdvanceClosing> (item => item.AdvanceReport), //FIXME Запустить перерасчет калки закрытия. 
						DeleteDependenceInfo.Create<AdvanceIncomeCashDistributionDocument> (item => item.AdvanceReport),
						DeleteDependenceInfo.Create<AdvanceExpenseCashDistributionDocument> (item => item.AdvanceReport)
					}
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(AdvanceClosing),
					SqlSelect = "SELECT id FROM @tablename ",
					DisplayString = "Строка закрытия аванса №{0} на сумму #FIXME",
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddDeleteInfo(
				new DeleteInfo {
					ObjectClass = typeof(IncomeCategory),
					SqlSelect = "SELECT id, name FROM @tablename ",
					DisplayString = "Статья дохода {1}",
					DeleteItems = new List<DeleteDependenceInfo> {
						DeleteDependenceInfo.Create<AccountIncome> (item => item.Category),
						DeleteDependenceInfo.Create<Income> (item => item.IncomeCategory)
					}
				}.FillFromMetaInfo()
			);

			DeleteConfig.AddHibernateDeleteInfo<ExpenseCategory>()
				.AddDeleteDependence<Expense>(item => item.ExpenseCategory)
				.AddDeleteDependence<AdvanceReport>(item => item.ExpenseCategory)
				.AddDeleteDependence<Income>(item => item.ExpenseCategory)
				.AddDeleteDependence<AccountExpense>(item => item.Category)
				.AddDeleteDependence<ExpenseCategory>(x => x.Parent)
				.AddClearDependence<Counterparty>(item => item.DefaultExpenseCategory);

			DeleteConfig.AddHibernateDeleteInfo<FineNomenclature>();

			DeleteConfig.AddHibernateDeleteInfo<CashTransferDocumentBase>()
				.AddDeleteCascadeDependence(item => item.CashTransferOperation)
				.AddDeleteCascadeDependence(item => item.IncomeOperation)
				.AddDeleteCascadeDependence(item => item.ExpenseOperation);

			DeleteConfig.AddHibernateDeleteInfo<CommonCashTransferDocument>()
				.AddDeleteCascadeDependence(item => item.IncomeOperation)
				.AddDeleteCascadeDependence(item => item.ExpenseOperation);

			DeleteConfig.AddHibernateDeleteInfo<IncomeCashTransferDocument>()
				.AddDeleteCascadeDependence(item => item.CashTransferOperation)
				.AddDeleteCascadeDependence(item => item.IncomeOperation)
				.AddDeleteCascadeDependence(item => item.ExpenseOperation)
				.AddDeleteDependenceFromCollection(item => item.CashTransferDocumentIncomeItems)
				.AddDeleteDependenceFromCollection(item => item.CashTransferDocumentExpenseItems);

			DeleteConfig.AddHibernateDeleteInfo<IncomeCashTransferedItem>()
				.AddClearDependence<Income>(x => x.TransferedBy);
			DeleteConfig.AddHibernateDeleteInfo<ExpenseCashTransferedItem>()
				.AddClearDependence<Expense>(x => x.TransferedBy);

			DeleteConfig.AddHibernateDeleteInfo<CashTransferOperation>()
				.RequiredCascadeDeletion()
				.AddDeleteDependence<CashTransferDocumentBase>(item => item.CashTransferOperation);

			DeleteConfig.AddHibernateDeleteInfo<CashOrganisationDistributionDocument>()
				.AddDeleteCascadeDependence(x => x.OrganisationCashMovementOperation);

			DeleteConfig.AddHibernateDeleteInfo<IncomeCashDistributionDocument>();

			DeleteConfig.AddHibernateDeleteInfo<ExpenseCashDistributionDocument>();
			
			DeleteConfig.AddHibernateDeleteInfo<RouteListItemCashDistributionDocument>();
			
			DeleteConfig.AddHibernateDeleteInfo<SelfDeliveryCashDistributionDocument>();
			
			DeleteConfig.AddHibernateDeleteInfo<AdvanceIncomeCashDistributionDocument>();
			
			DeleteConfig.AddHibernateDeleteInfo<AdvanceExpenseCashDistributionDocument>();
			
			DeleteConfig.AddHibernateDeleteInfo<FuelExpenseCashDistributionDocument>();
			
			DeleteConfig.AddHibernateDeleteInfo<OrganisationCashMovementOperation>();

			DeleteConfig.AddHibernateDeleteInfo<CashRequest>()
				.AddDeleteDependence<CashRequestSumItem>(x => x.CashRequest);
			
			DeleteConfig.AddHibernateDeleteInfo<CashRequestSumItem>()
				.AddDeleteDependence<Expense>(x => x.CashRequestSumItem);

			DeleteConfig.AddHibernateDeleteInfo<OrganizationCashTransferDocument>()
				.AddDeleteCascadeDependence(x => x.OrganisationCashMovementOperationFrom)
				.AddDeleteCascadeDependence(x => x.OrganisationCashMovementOperationTo);

			DeleteConfig.AddHibernateDeleteInfo<PremiumTemplate>();

			#endregion

			#region Топливо

			DeleteConfig.AddHibernateDeleteInfo<FuelDocument>()
						.AddDeleteCascadeDependence(x => x.FuelOperation)
						.AddDeleteCascadeDependence(x => x.FuelExpenseOperation)
						.AddDeleteDependence<FuelExpenseCashDistributionDocument>(x => x.FuelDocument);

			DeleteConfig.AddHibernateDeleteInfo<FuelOperation>()
				.RequiredCascadeDeletion()
				.AddDeleteDependence<RouteList>(x => x.FuelOutlayedOperation)
				.AddDeleteDependence<FuelDocument>(x => x.FuelOperation);

			DeleteConfig.AddHibernateDeleteInfo<FuelTransferDocument>()
				.AddDeleteCascadeDependence(item => item.FuelExpenseOperation)
				.AddDeleteCascadeDependence(item => item.FuelIncomeOperation)
				.AddDeleteCascadeDependence(item => item.FuelTransferOperation);

			DeleteConfig.AddHibernateDeleteInfo<FuelIncomeInvoice>()
				.AddDeleteDependenceFromCollection(item => item.FuelIncomeInvoiceItems);

			DeleteConfig.AddHibernateDeleteInfo<FuelIncomeInvoiceItem>()
				.AddDeleteCascadeDependence(item => item.FuelIncomeOperation);

			DeleteConfig.AddHibernateDeleteInfo<FuelExpenseOperation>()
				.RequiredCascadeDeletion()
				.AddDeleteDependence<FuelTransferDocument>(item => item.FuelExpenseOperation)
				.AddDeleteDependence<FuelDocument>(item => item.FuelExpenseOperation)
				.AddDeleteDependence<FuelWriteoffDocumentItem>(item => item.FuelExpenseOperation);

			DeleteConfig.AddHibernateDeleteInfo<FuelIncomeOperation>()
				.RequiredCascadeDeletion()
				.AddDeleteDependence<FuelTransferDocument>(item => item.FuelIncomeOperation)
				.AddDeleteDependence<FuelIncomeInvoiceItem>(item => item.FuelIncomeOperation);

			DeleteConfig.AddHibernateDeleteInfo<FuelTransferOperation>();

			DeleteConfig.AddHibernateDeleteInfo<FuelWriteoffDocument>()
				.AddDeleteDependenceFromCollection(item => item.FuelWriteoffDocumentItems);

			DeleteConfig.AddHibernateDeleteInfo<FuelWriteoffDocumentItem>()
				.AddDeleteCascadeDependence(item => item.FuelExpenseOperation);

			#endregion Топливо

			#region DeliveryDocument

			DeleteConfig.AddHibernateDeleteInfo<DeliveryDocument>()
				.AddDeleteDependenceFromCollection(x => x.Items);
			
			DeleteConfig.AddHibernateDeleteInfo<DeliveryDocumentItem>()
				.AddDeleteCascadeDependence(x => x.EmployeeNomenclatureMovementOperation);

			#endregion

			#region AddressTransferDocument

			DeleteConfig.AddHibernateDeleteInfo<AddressTransferDocument>()
				.AddDeleteDependenceFromCollection(x => x.AddressTransferDocumentItems);
			
			DeleteConfig.AddHibernateDeleteInfo<AddressTransferDocumentItem>()
				.AddDeleteDependenceFromCollection(x => x.DriverNomenclatureTransferDocumentItems);

			DeleteConfig.AddHibernateDeleteInfo<DriverNomenclatureTransferItem>()
				.AddDeleteCascadeDependence(x => x.EmployeeNomenclatureMovementOperationFrom)
				.AddDeleteCascadeDependence(x => x.EmployeeNomenclatureMovementOperationTo);

			#endregion

			#region Операции по счету

			DeleteConfig.AddHibernateDeleteInfo<AccountIncome>()
				.AddDeleteCascadeDependence(x => x.MoneyOperation);

			DeleteConfig.AddHibernateDeleteInfo<AccountExpense>()
				.AddDeleteCascadeDependence(x => x.MoneyOperation);

			DeleteConfig.ExistingDeleteRule<Account>()
						.AddDeleteDependence<AccountIncome>(item => item.CounterpartyAccount)
						.AddDeleteDependence<AccountIncome>(item => item.OrganizationAccount)
						.AddDeleteDependence<AccountExpense>(item => item.CounterpartyAccount)
						.AddDeleteDependence<AccountExpense>(item => item.OrganizationAccount)
						.AddDeleteDependence<AccountExpense>(item => item.EmployeeAccount)
						.AddRemoveFromDependence<Personnel>(x => x.Accounts)
						;

			#endregion

			DeleteConfig.AddHibernateDeleteInfo<PaymentByCardOnline>();
			DeleteConfig.AddHibernateDeleteInfo<OrderIdProviderForMobileApp>();

			#region Журнал изменений
			//Добавлено чтобы было, вдруг понадобится ослеживать зависимости. Сейчас это не надо для реального удаления.

			DeleteConfig.AddHibernateDeleteInfo<ChangeSet>()
				.AddDeleteDependence<ChangedEntity>(x => x.ChangeSet);
			DeleteConfig.AddHibernateDeleteInfo<ChangedEntity>()
				.AddDeleteDependence<FieldChange>(x => x.Entity);
			DeleteConfig.AddHibernateDeleteInfo<FieldChange>();

			#endregion

			#region Рекламации

			DeleteConfig.AddHibernateDeleteInfo<Complaint>()
						.AddDeleteDependence<ComplaintDiscussion>(item => item.Complaint)
						.AddDeleteDependence<ComplaintFile>(item => item.Complaint)
						.AddDeleteDependence<ComplaintGuiltyItem>(item => item.Complaint)
						;

			DeleteConfig.AddHibernateDeleteInfo<ComplaintDiscussion>()
						.AddDeleteDependence<ComplaintDiscussionComment>(item => item.ComplaintDiscussion)
						;

			DeleteConfig.AddHibernateDeleteInfo<ComplaintFile>()
						;

			DeleteConfig.AddHibernateDeleteInfo<ComplaintGuiltyItem>()
						;

			DeleteConfig.AddHibernateDeleteInfo<ComplaintDiscussionComment>()
						.AddDeleteDependence<ComplaintFile>(item => item.ComplaintDiscussionComment)
						;

			DeleteConfig.AddHibernateDeleteInfo<ComplaintObject>();

			#endregion Рекламации

			#region stuff

			DeleteConfig.AddHibernateDeleteInfo<StoredImageResource>();
			DeleteConfig.AddHibernateDeleteInfo<StoredResource>();

			#endregion

			#region Предложения (ApplicationDevelopmentProposal)

			DeleteConfig.AddHibernateDeleteInfo<ApplicationDevelopmentProposal>();

			#endregion

			#region Листовки (Flyer)

			DeleteConfig.AddHibernateDeleteInfo<Flyer>()
				.AddDeleteDependence<FlyerActionTime>(item => item.Flyer);

			DeleteConfig.AddHibernateDeleteInfo<FlyerActionTime>();

			#endregion

			logger.Info("Ок");
		}
	}
}
