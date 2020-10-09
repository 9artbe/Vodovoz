using System.Linq;
using Vodovoz.Domain.Client;

namespace Vodovoz.Domain.Orders.Documents.OrderContract {
    public class OrderContractDocumentUpdater : OrderDocumentUpdaterBase {

        private readonly OrderContractDocumentFactory documentFactory;
        
        public override OrderDocumentType DocumentType => OrderDocumentType.Contract;

        public OrderContractDocumentUpdater(OrderContractDocumentFactory documentFactory) {
            this.documentFactory = documentFactory;
        }

        private OrderContract CreateNewDocument() {
            return documentFactory.Create();
        }

        private bool NeedCreateDocument(OrderBase order) {
            return order.PaymentType == PaymentType.cashless
                && !(order.ObservableOrderItems.Sum(i => i.Sum) <= 0m)
                && !order.ObservableOrderDepositItems.Any()
                && order.ObservableOrderItems.Any();
        }
        
        public override void UpdateDocument(OrderBase order) {
            if (NeedCreateDocument(order)) {
                if (order.ObservableOrderDocuments.All(x => x.Type != DocumentType)) {
                    AddExistingDocument(order, CreateNewDocument());
                }
            }
            else {
                var doc = order.ObservableOrderDocuments.SingleOrDefault(
                    x => x.Type == DocumentType);

                if (doc != null) {
                    RemoveExistingDocument(order, doc);
                }
            }
        }

        public override void AddExistingDocument(OrderBase order, OrderDocument existingDocument) {
            order.AddDocument(existingDocument);
        }

        public override void RemoveExistingDocument(OrderBase order, OrderDocument existingDocument) {
            order.RemoveDocument(existingDocument);
        }
    }
}