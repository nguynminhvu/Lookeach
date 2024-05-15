namespace Lookeach.Core.ViewModels.Orders
{
    public class OrderCreateModel
    {
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public List<Guid> PaymentMethodIds { get; set; }
        public List<OrderItemCreateModel> OrderItems { get; set; }
        public List<Guid>? VoucherIds { get; set; }
    }
}
