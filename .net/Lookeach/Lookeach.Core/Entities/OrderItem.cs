using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class OrderItem : Audit<DateTime, Guid>
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ProductVersion { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
