using Lookeach.Core.ViewModels.Products;

namespace Lookeach.Core.ViewModels.Orders
{
    public class OrderItemCreateModel
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ProductVersion { get; set; }
    }
}
