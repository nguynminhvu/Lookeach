using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Order:Audit<DateTime,Guid>
    {
        public decimal Amount { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
