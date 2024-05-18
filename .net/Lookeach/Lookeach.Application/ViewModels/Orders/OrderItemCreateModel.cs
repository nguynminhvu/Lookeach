namespace Lookeach.Application.ViewModels.Orders
{
    public class OrderItemCreateModel
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal DollarPrice { get; set; }
        public int Quantity { get; set; }
        public int ProductVersion { get; set; }
    }
}
