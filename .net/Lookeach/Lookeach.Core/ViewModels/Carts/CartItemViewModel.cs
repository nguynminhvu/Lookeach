using Lookeach.Core.Common.Base;
using Lookeach.Core.Entities;
using Lookeach.Core.ViewModels.Products;

namespace Lookeach.Core.ViewModels.Carts
{
    public class CartItemViewModel : BaseEntity<Guid>
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
