using Lookeach.Core.Common.Base;
using Lookeach.Core.Enums;
using Lookeach.Core.ViewModels.Carts;

namespace Lookeach.Core.Entities
{
    public class Order : Audit<DateTime, Guid>
    {
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Voucher>? Vouchers { get; set; }

        private Order()
        {

        }
        private Order(Guid id, Guid userId, decimal amount, Guid paymentMethodId, List<OrderItem> orderItems, string address, string phone, string status, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn, string? email = null!, List<Voucher>? vouchers = null!)
        {
            Id = id;
            decimal checkSum = 0;
            foreach (var item in orderItems)
            {
                checkSum += item.Quantity * item.Price;
            }
            if (checkSum != amount) throw new Exception("Check sum invalid (Amount)");
            Amount = amount;
            UserId = userId;
            PaymentMethodId = paymentMethodId;
            if (orderItems == null || orderItems.Count <= 0)
            {
                throw new ArgumentNullException("Order item invalid");
            }
            OrderItems = orderItems;
            if (vouchers != null && vouchers.Count > 0)
            {
                Vouchers = vouchers;
            }
            Address = address;
            Phone = phone;
            Status = status;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }

        public static Order Create(Guid userId, decimal amount, List<OrderItem> orderItems, Guid paymentMethodId, string address, string phone, Guid createdBy, string? email = null!, List<Voucher>? vouchers = null!)
        {
            return new Order(Guid.NewGuid(), userId, amount, paymentMethodId, orderItems, address, phone, OrderStatus.CREATED, createdBy, DateTime.Now, createdBy, DateTime.Now, email, vouchers);
        }

        public void Update(string status, Guid modified)
        {
            Status = status;
            ModifiedBy = modified;
            ModifiedOn = DateTime.Now;
        }

    }
}
