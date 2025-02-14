using System;
using System.Collections.Generic;
using System.Linq;
using QS.DomainModel.UoW;
using QS.Project.Domain;
using QS.Services;
using QS.ViewModels;
using Vodovoz.Domain.Employees;
using Vodovoz.Domain.Orders;
using Vodovoz.Services;
using Vodovoz.Tools.CallTasks;
using Order = Vodovoz.Domain.Orders.Order;

namespace Vodovoz.ViewModels.Cash
{
    public class PaymentByCardViewModel: EntityTabViewModelBase<Order> 
    {
        private readonly Employee _currentEmployee;
        private readonly CallTaskWorker _callTaskWorker;

        public PaymentByCardViewModel(
            IEntityUoWBuilder uowBuilder,
            IUnitOfWorkFactory unitOfWorkFactory,
            ICommonServices commonServices,
            CallTaskWorker callTaskWorker,
            IOrderPaymentSettings orderPaymentSettings,
            IOrderParametersProvider orderParametersProvider,
            Employee currentEmployee) : base(uowBuilder, unitOfWorkFactory, commonServices)
        {
	        if(orderPaymentSettings == null)
	        {
		        throw new ArgumentNullException(nameof(orderPaymentSettings));
	        }
	        
	        if(orderParametersProvider == null)
	        {
		        throw new ArgumentNullException(nameof(orderParametersProvider));
	        }
	        
            _callTaskWorker = callTaskWorker ?? throw new ArgumentNullException(nameof(callTaskWorker));
            _currentEmployee = currentEmployee;

            TabName = "Оплата по карте";

            ItemsList = UoW.GetAll<PaymentFrom>().ToList();

            if (PaymentByCardFrom == null)
            {
                PaymentByCardFrom = ItemsList.FirstOrDefault(p => p.Id == orderPaymentSettings.DefaultSelfDeliveryPaymentFromId);
            }

            Entity.PropertyChanged += Entity_PropertyChanged;
            
            ValidationContext.ServiceContainer.AddService(typeof(IOrderParametersProvider), orderParametersProvider);
        }

        void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.PaymentByCardFrom)){
                OnPropertyChanged(nameof(PaymentByCardFrom));
            }
        }

        public PaymentFrom PaymentByCardFrom
        {
            get => Entity.PaymentByCardFrom;
            set => Entity.PaymentByCardFrom = value;
        }

        public List<PaymentFrom> ItemsList { get; private set; }

        protected override void BeforeValidation()
        {
	        Entity.ChangePaymentTypeToByCard(_callTaskWorker);

	        if(!Entity.PayAfterShipment)
	        {
		        Entity.SelfDeliveryToLoading(_currentEmployee, CommonServices.CurrentPermissionService, _callTaskWorker);
	        }

	        if(Entity.SelfDelivery)
	        {
		        Entity.IsSelfDeliveryPaid = true;
	        }
        }
    }
}