﻿using System.Collections.Generic;
using QSBanks;
using QSContacts;
using QSOrmProject.Deletion;
using Vodovoz.Domain;
using Vodovoz.Domain.Documents;
using Vodovoz.Domain.Logistic;
using Vodovoz.Domain.Operations;
using Vodovoz.Domain.Orders;
using Vodovoz.Domain.Store;
using Vodovoz.Domain.Cash;
using Vodovoz.Domain.Accounting;
using Vodovoz.Domain.Orders.Documents;

namespace Vodovoz
{
	partial class MainClass
	{
		public static void ConfigureDeletion ()
		{
			logger.Info ("Настройка параметров удаления...");

			QSContactsMain.ConfigureDeletion ();
			QSBanksMain.ConfigureDeletion ();

			#region Goods

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Nomenclature),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.CreateFromBag<Nomenclature> (item => item.NomenclaturePrice),
					DeleteDependenceInfo.Create<Equipment> (item => item.Nomenclature),
					DeleteDependenceInfo.Create<WarehouseMovementOperation> (item => item.Nomenclature),
					DeleteDependenceInfo.Create<CounterpartyMovementOperation> (item => item.Nomenclature)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(EquipmentColors),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Nomenclature> (item => item.Color)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(EquipmentType),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<FreeRentPackage> (item => item.EquipmentType),
					DeleteDependenceInfo.Create<Nomenclature> (item => item.Type),
					DeleteDependenceInfo.Create<PaidRentPackage> (item => item.EquipmentType)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Equipment),
				SqlSelect = "SELECT @tablename.id, nomenclature.model, serial_number FROM @tablename " +
				"LEFT JOIN nomenclature ON nomenclature.id = nomenclature_id",
				DisplayString = "{1} ({0})",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<FreeRentEquipment> (item => item.Equipment),
					DeleteDependenceInfo.Create<IncomingInvoiceItem> (item => item.Equipment),
					DeleteDependenceInfo.Create<OrderEquipment> (item => item.Equipment),
					DeleteDependenceInfo.Create<OrderItem> (item => item.Equipment),
					DeleteDependenceInfo.Create<PaidRentEquipment> (item => item.Equipment),
					DeleteDependenceInfo.Create<WarehouseMovementOperation> (item => item.Equipment),
					DeleteDependenceInfo.Create<CounterpartyMovementOperation> (item => item.Equipment)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Manufacturer),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Nomenclature> (item => item.Manufacturer)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(MeasurementUnits),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Nomenclature> (item => item.Unit)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(NomenclaturePrice),
				SqlSelect = "SELECT id, price, min_count FROM @tablename ",
				DisplayString = "{1:C} (от {2})"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Warehouse),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<IncomingInvoice> (item => item.Warehouse)
					//FIXME добавить складские операции.
				}
			}.FillFromMetaInfo ()
			);
			#endregion

			//Наша организация
			#region Organization

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Organization),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.CreateFromBag<Organization> (item => item.Phones),
					DeleteDependenceInfo.CreateFromBag<Organization> (item => item.Accounts),
					DeleteDependenceInfo.Create<CounterpartyContract> (item => item.Organization),
					DeleteDependenceInfo.Create<AccountIncome> (item => item.Organization),
					DeleteDependenceInfo.Create<AccountExpense> (item => item.Organization)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddClearDependence<Account> (ClearDependenceInfo.Create<Organization> (item => item.DefaultAccount));

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Employee),
				SqlSelect = "SELECT id, last_name, name, patronymic FROM @tablename ",
				DisplayString = "{1} {2} {3}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.CreateFromBag<Employee> (item => item.Phones),
					DeleteDependenceInfo.Create<Income> (item => item.Casher),
					DeleteDependenceInfo.Create<Expense> (item => item.Casher),
					DeleteDependenceInfo.Create<AdvanceReport> (item => item.Casher),
					DeleteDependenceInfo.Create<AdvanceReport> (item => item.Accountable),
				},
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Car> (item => item.Driver),
					ClearDependenceInfo.Create<Counterparty> (item => item.Accountant),
					ClearDependenceInfo.Create<Counterparty> (item => item.SalesManager),
					ClearDependenceInfo.Create<Counterparty> (item => item.BottlesManager),
					ClearDependenceInfo.Create<MovementDocument> (item => item.ResponsiblePerson),
					ClearDependenceInfo.Create<WriteoffDocument> (item => item.ResponsibleEmployee),
					ClearDependenceInfo.Create<Organization> (item => item.Leader),
					ClearDependenceInfo.Create<Organization> (item => item.Buhgalter),
					ClearDependenceInfo.Create<Income> (item => item.Employee),
					ClearDependenceInfo.Create<Expense> (item => item.Employee),
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Nationality),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Employee> (item => item.Nationality)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(User),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Employee> (item => item.User)
				}
			}.FillFromMetaInfo ()
			);

			#endregion

			//Контрагент и все что сним связано
			#region NearCounterparty

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Counterparty),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.CreateFromBag<Counterparty> (item => item.Contacts),
					DeleteDependenceInfo.Create<BottlesMovementOperation> (item => item.Counterparty),
					DeleteDependenceInfo.CreateFromBag<Counterparty> (item => item.Phones),
					DeleteDependenceInfo.CreateFromBag<Counterparty> (item => item.Emails),
					DeleteDependenceInfo.CreateFromBag<Counterparty> (item => item.Accounts),
					DeleteDependenceInfo.CreateFromBag<Counterparty> (item => item.DeliveryPoints),
					DeleteDependenceInfo.CreateFromBag<Counterparty> (item => item.CounterpartyContracts)
                                .AddCheckProperty<CounterpartyContract> (item => item.Counterparty),
					DeleteDependenceInfo.CreateFromBag<Counterparty> (item => item.Proxies),
					DeleteDependenceInfo.Create<DepositOperation> (item => item.Counterparty),
					DeleteDependenceInfo.Create<CounterpartyMovementOperation> (item => item.WriteoffCounterparty),
					DeleteDependenceInfo.Create<CounterpartyMovementOperation> (item => item.IncomingCounterparty),
					DeleteDependenceInfo.Create<IncomingInvoice> (item => item.Contractor),
					DeleteDependenceInfo.Create<MoneyMovementOperation> (item => item.Counterparty),
					DeleteDependenceInfo.Create<MovementDocument> (item => item.FromClient),
					DeleteDependenceInfo.Create<MovementDocument> (item => item.ToClient),
					DeleteDependenceInfo.Create<Order> (item => item.Client),
					DeleteDependenceInfo.Create<WriteoffDocument> (item => item.Client),
					DeleteDependenceInfo.Create<AccountIncome> (item => item.Counterparty),
					DeleteDependenceInfo.Create<AccountExpense> (item => item.Counterparty)
				},
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Counterparty> (item => item.MainCounterparty)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddClearDependence<Account> (ClearDependenceInfo.Create<Counterparty> (item => item.DefaultAccount));

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Contact),
				SqlSelect = "SELECT id, lastname, name, surname FROM @tablename ",
				DisplayString = "{1} {2} {3}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.CreateFromBag<Contact> (item => item.Emails),
					DeleteDependenceInfo.CreateFromBag<Contact> (item => item.Phones),
				},
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Counterparty> (item => item.MainContact),
					ClearDependenceInfo.Create<Counterparty> (item => item.FinancialContact),
					ClearDependenceInfo.Create<DeliveryPoint> (item => item.Contact)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddClearDependence<Post> (ClearDependenceInfo.Create<Contact> (item => item.Post));

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Significance),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Counterparty> (item => item.Significance)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(CounterpartyStatus),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Counterparty> (item => item.Status)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Proxy),
				SqlSelect = "SELECT id, number, issue_date FROM @tablename ",
				DisplayString = "{1} от {2:d}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.CreateFromBag<Proxy> (item => item.Persons)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(CounterpartyContract),
				SqlSelect = "SELECT id, issue_date FROM @tablename ",
				DisplayString = "Договор №{0} от {1:d}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.CreateFromBag<CounterpartyContract> (item => item.AdditionalAgreements)
                                .AddCheckProperty<AdditionalAgreement> (item => item.Contract)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(AdditionalAgreement),
				SqlSelect = "SELECT id, number FROM @tablename ",
				DisplayString = "Доп. соглашение №{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<OrderItem> (item => item.AdditionalAgreement)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(FreeRentEquipment),
				SqlSelect = "SELECT @tablename.id, model, serial_number FROM @tablename " +
				"LEFT JOIN equipment ON equipment.id = @tablename.equipment_id " +
				"LEFT JOIN nomenclature ON equipment.nomenclature_id = nomenclature.id ",
				DisplayString = "Доп. соглашение №{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<OrderItem> (item => item.AdditionalAgreement)
				}
			}.FillFromMetaInfo ()
			);
				
			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(DeliveryPoint),
				SqlSelect = "SELECT id, compiled_address FROM @tablename ",
				DisplayString = "{1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<AdditionalAgreement> (item => item.DeliveryPoint),
					DeleteDependenceInfo.Create<BottlesMovementOperation> (item => item.DeliveryPoint),
					DeleteDependenceInfo.Create<DepositOperation> (item => item.DeliveryPoint),
					DeleteDependenceInfo.Create<CounterpartyMovementOperation> (item => item.WriteoffDeliveryPoint),
					DeleteDependenceInfo.Create<CounterpartyMovementOperation> (item => item.IncomingDeliveryPoint),
					DeleteDependenceInfo.Create<Proxy> (item => item.DeliveryPoint)
				}
			}.FillFromMetaInfo ()
			);
			#endregion
				
			#region Logistics
			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Car),
				SqlSelect = "SELECT id, model, reg_number FROM @tablename ",
				DisplayString = "Автомобиль {1} ({2})"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(DeliverySchedule),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<Order> (item => item.DeliverySchedule)	
				},
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<DeliveryPoint> (item => item.DeliverySchedule)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(LogisticsArea),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "{1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<DeliveryPoint> (item => item.LogisticsArea)
				}
			}.FillFromMetaInfo ()
			);


			#endregion

			//Вокруг заказа
			#region Order

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Order),
				SqlSelect = "SELECT id FROM @tablename ",
				DisplayString = "Заказ №{0}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<OrderItem> (item => item.Order),
					DeleteDependenceInfo.Create<OrderDocument> (item => item.Order),
					DeleteDependenceInfo.Create<OrderDepositItem> (item => item.Order)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(OrderItem),
				SqlSelect = "SELECT id, order_id FROM @tablename ",
				DisplayString = "Строка заказа №{1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<OrderEquipment> (item => item.OrderItem)
				}
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(OrderEquipment),
				SqlSelect = "SELECT id, order_id FROM @tablename ",
				DisplayString = "Оборудование заказа №{1}"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(OrderDocument),
				SqlSelect = "SELECT id, order_id FROM @tablename ",
				DisplayString = "Документ к заказу №{1}"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(OrderDepositItem),
				SqlSelect = "SELECT id, order_id FROM @tablename ",
				DisplayString = "Залог к заказу №{1}"
			}.FillFromMetaInfo ()
			);
				
			//FIXME Тут скорее всего еще не все зависимости.

			#endregion

			//Документы
			#region Documents

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(IncomingInvoice),
				SqlSelect = "SELECT id, time_stamp FROM @tablename ",
				DisplayString = "Входящая накладная №{0} от {1}"
				//TODO Строки 
				//TODO Зависимости
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(IncomingWater),
				SqlSelect = "SELECT id, time_stamp FROM @tablename ",
				DisplayString = "Документ производства №{0} от {1}"
				//TODO Строки 
				//TODO Зависимости
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(MovementDocument),
				SqlSelect = "SELECT id, time_stamp FROM @tablename ",
				DisplayString = "Документ перемещения №{0} от {1}"
				//TODO Строки
				//TODO Зависимости
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(WriteoffDocument),
				SqlSelect = "SELECT id, time_stamp FROM @tablename ",
				DisplayString = "Документ списания №{0} от {1}"
				//TODO Строки 
				//TODO Зависимости
			}.FillFromMetaInfo ()
			);
			#endregion

			//Операции в журналах
			#region Operations

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(BottlesMovementOperation),
				SqlSelect = "SELECT id, moved_to, moved_from FROM @tablename ",
				DisplayString = "Движения тары к контрагенту {1} от контрагента {2} бутылей"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(WarehouseMovementOperation),
				SqlSelect = "SELECT id, amount FROM @tablename ",
				//FIXME Указать название товара
				DisplayString = "Складское перемещение Х в количестве {1}"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(CounterpartyMovementOperation),
				SqlSelect = "SELECT id, amount FROM @tablename ",
				//FIXME Указать название товара
				DisplayString = "Перемещение у контрагента Х в количестве {1}"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(MoneyMovementOperation),
				SqlSelect = "SELECT id FROM @tablename ",
				//FIXME Создать грамотную строку отобржения.
				DisplayString = "Денежная операция {0}"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(DepositOperation),
				SqlSelect = "SELECT id, received_deposit, refund_deposit FROM @tablename ",
				DisplayString = "Залог: получено = {1:C}, возврат = {2:C}"
			}.FillFromMetaInfo ()
			);

			#endregion

			#region Cash

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Income),
				SqlSelect = "SELECT id, date FROM @tablename ",
				DisplayString = "Приходный ордер №{0} от {1:d}"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(Expense),
				SqlSelect = "SELECT id, date FROM @tablename ",
				DisplayString = "Расходный ордер №{0} от {1:d}"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(AdvanceReport),
				SqlSelect = "SELECT id, date FROM @tablename ",
				DisplayString = "Авансовый отчет №{0} от {1:d}"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(IncomeCategory),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "Статья дохода {1}",
				ClearItems = new List<ClearDependenceInfo> {
					ClearDependenceInfo.Create<Income> (item => item.IncomeCategory)
				}
			}.FillFromMetaInfo ()
			);
				
			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(ExpenseCategory),
				SqlSelect = "SELECT id, name FROM @tablename ",
				DisplayString = "Статья раcхода {1}",
				DeleteItems = new List<DeleteDependenceInfo> {
					DeleteDependenceInfo.Create<Expense> (item => item.ExpenseCategory),
					DeleteDependenceInfo.Create<AdvanceReport> (item => item.ExpenseCategory),
					DeleteDependenceInfo.Create<Income> (item => item.ExpenseCategory),
				}						
			}.FillFromMetaInfo ()
			);

			#endregion

			#region Операции по счету

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(AccountIncome),
				SqlSelect = "SELECT id, number, date, total FROM @tablename ",
				DisplayString = "Операция прихода №{1} от {2:d} на сумму {3}₽"
			}.FillFromMetaInfo ()
			);

			DeleteConfig.AddDeleteInfo (new DeleteInfo {
				ObjectClass = typeof(AccountExpense),
				SqlSelect = "SELECT id, number, date, total FROM @tablename ",
				DisplayString = "Операция расхода №{1} от {2:d} на сумму {3}₽"
			}.FillFromMetaInfo ()
			);
				
			DeleteConfig.ExistingConfig<Account> ().DeleteItems
				.AddRange (new List<DeleteDependenceInfo> {
				DeleteDependenceInfo.Create<AccountIncome> (item => item.CounterpartyAccount),
				DeleteDependenceInfo.Create<AccountIncome> (item => item.OrganizationAccount),
				DeleteDependenceInfo.Create<AccountExpense> (item => item.CounterpartyAccount),
				DeleteDependenceInfo.Create<AccountExpense> (item => item.OrganizationAccount)
			});

			#endregion

			//Для тетирования
			#if DEBUG
			DeleteConfig.IgnoreMissingClass.Add (typeof(NonfreeRentAgreement));
			DeleteConfig.IgnoreMissingClass.Add (typeof(DailyRentAgreement));
			DeleteConfig.IgnoreMissingClass.Add (typeof(FreeRentAgreement));
			DeleteConfig.IgnoreMissingClass.Add (typeof(WaterSalesAgreement));
			DeleteConfig.IgnoreMissingClass.Add (typeof(RepairAgreement));

			DeleteConfig.DeletionCheck ();
			#endif

			logger.Info ("Ок");
		}
	}
}
