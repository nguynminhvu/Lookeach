using Lookeach.Core.Entities;

namespace Lookeach.Core.ViewModels.Carts
{
    public class CartItemUpdateModel
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
