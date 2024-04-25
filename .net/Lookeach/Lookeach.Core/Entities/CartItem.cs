using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class CartItem : BaseEntity<Guid>
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ProductVersion { get; set; }
        public Cart Cart { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
