using System;
using Autofac;
using Autofac.Core;
using QS.Dialog;
using QS.Navigation;
using QS.Services;
using QS.ViewModels.Dialog;
using Vodovoz.Dialogs.Email;
using Vodovoz.Domain.Orders;
using Vodovoz.Infrastructure.Print;
using Vodovoz.ViewModels.Dialogs.Orders;

namespace Vodovoz.ViewModels.ViewModels.Orders
{
    public abstract class OrderMainViewModelBase : DialogViewModelBase, IAutofacScopeHolder
    {
        private OrderBase order;
        protected OrderBase Order
        {
            get => order;
            set => SetField(ref order, value);
        }
        
        public ILifetimeScope AutofacScope { get; set; }
        
        protected readonly ITdiCompatibilityNavigation tdiCompatibilityNavigation;
        public OrderInfoViewModelBase OrderInfoViewModelBase { get; }

        private OrderDocumentsViewModel orderDocumentsViewModel;
        public OrderDocumentsViewModel OrderDocumentsViewModel
        {
            get
            {
                if (orderDocumentsViewModel == null)
                {
                    Parameter[] parameters = {
                        new TypedParameter(typeof(OrderBase), Order),
                        new TypedParameter(typeof(ITdiCompatibilityNavigation), tdiCompatibilityNavigation),
                        new TypedParameter(typeof(ICommonServices), AutofacScope.Resolve<ICommonServices>()),
                        new TypedParameter(typeof(IRDLPreviewOpener), AutofacScope.Resolve<IRDLPreviewOpener>()),
                        new TypedParameter(typeof(CommonMessages), AutofacScope.Resolve<CommonMessages>()),
                        new TypedParameter(typeof(SendDocumentByEmailViewModel),
                            AutofacScope.Resolve<SendDocumentByEmailViewModel>()),
                    };
                    orderDocumentsViewModel = AutofacScope.Resolve<OrderDocumentsViewModel>(parameters);
                }

                return orderDocumentsViewModel;
            }
        }

        private WorkingOnOrderViewModel workingOnOrderViewModel;
        public WorkingOnOrderViewModel WorkingOnOrderViewModel
        {
            get
            {
                if (workingOnOrderViewModel == null)
                {
                    Parameter[] parameters = {
                        new TypedParameter(typeof(OrderBase), Order)
                    };
                    workingOnOrderViewModel = AutofacScope.Resolve<WorkingOnOrderViewModel>(parameters);
                }

                return workingOnOrderViewModel;
            }
        }
        
        protected OrderMainViewModelBase(
            OrderInfoViewModelBase orderInfoViewModelBase,
            ITdiCompatibilityNavigation tdiCompatibilityNavigation) : base (tdiCompatibilityNavigation)
        {
            this.tdiCompatibilityNavigation = 
                tdiCompatibilityNavigation ?? throw new ArgumentNullException(nameof(tdiCompatibilityNavigation));
            OrderInfoViewModelBase = orderInfoViewModelBase;
        }
    }
}