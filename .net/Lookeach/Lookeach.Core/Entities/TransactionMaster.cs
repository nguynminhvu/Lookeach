using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class TransactionMaster : Audit<DateTime, Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool IsCompleted { get; set; }
        private TransactionMaster()
        {

        }
        protected TransactionMaster(Guid id, Guid orderId, Guid paymentMethodId, bool isComplted, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            OrderId = orderId;
            PaymentMethodId = paymentMethodId;
            IsCompleted = isComplted;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
        }

        public static TransactionMaster Create(Guid id, Guid orderId, Guid paymentMethodId, bool isCompleted, Guid createdBy)
        {
            return new TransactionMaster(id, orderId, paymentMethodId, isCompleted, createdBy, DateTime.Now, createdBy, DateTime.Now);
        }
    }
}
