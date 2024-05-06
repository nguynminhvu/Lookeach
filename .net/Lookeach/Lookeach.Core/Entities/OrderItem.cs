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

        private OrderItem()
        {

        }

        protected OrderItem(Guid id, Guid orderId, Guid productId, decimal price, int quantity, int productVersion, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            if (price < 0)
            {
                throw new Exception("Price invalid");
            }
            Price = price;
            if (quantity <= 0)
            {
                throw new Exception("Quantity invalid");
            }
            Quantity = quantity;
            ProductVersion = productVersion;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
        }

        public static OrderItem Create(Guid orderId, Guid productId, decimal price, int quantity, int productVersion, Guid createdBy)
        {
            return new OrderItem(Guid.NewGuid(), orderId, productId, price, quantity, productVersion, createdBy, DateTime.Now, createdBy, DateTime.Now);
        }
    }
}
