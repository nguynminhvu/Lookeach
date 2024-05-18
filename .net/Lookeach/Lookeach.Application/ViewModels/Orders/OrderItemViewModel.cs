using Lookeach.Application.ViewModels.Products;

namespace Lookeach.Application.ViewModels.Orders
{
    public class OrderItemViewModel
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ProductVersion { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
