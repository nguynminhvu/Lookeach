using Lookeach.Core.Common.Base;
using Lookeach.Core.Enums;

namespace Lookeach.Core.Entities
{
    public class Order : Audit<DateTime, Guid>
    {
        public decimal Amount { get; private set; }
        public decimal DollarAmount { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public string? Email { get; private set; }
        public string Status { get; private set; }
        public ICollection<PaymentMethod> PaymentMethods { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; }
        public ICollection<Voucher>? Vouchers { get; private set; }

        private Order()
        {

        }
        private Order(Guid id, Guid userId, decimal amount, decimal dollarAmount, List<PaymentMethod> paymentMethods, List<OrderItem> orderItems, string address, string phone, string status, Guid createdBy, DateTime createdOn, Guid modifiedBy, DateTime modifiedOn, string? email = null!, List<Voucher>? vouchers = null!)
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
            PaymentMethods = paymentMethods;
            if (orderItems == null || orderItems.Count <= 0)
            {
                throw new ArgumentNullException("Order item invalid");
            }
            OrderItems = orderItems;
            if (vouchers != null && vouchers.Count > 0)
            {
                Vouchers = vouchers;
            }
            DollarAmount = dollarAmount;
            Address = address;
            Phone = phone;
            Status = status;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }

        public static Order Create(Guid userId, decimal amount, decimal dollarAmount, List<OrderItem> orderItems, List<PaymentMethod> paymentMethods, string address, string phone, Guid createdBy, string? email = null!, List<Voucher>? vouchers = null!)
        {
            return new Order(Guid.NewGuid(), userId, amount, dollarAmount, paymentMethods, orderItems, address, phone, OrderStatus.Created.ToString(), createdBy, DateTime.Now, createdBy, DateTime.Now, email, vouchers);
        }

        public void UpdateStatus(string status, Guid modified)
        {
          
            Status = status;
            ModifiedBy = modified;
            ModifiedOn = DateTime.Now;
        }
        public void UpdateAmount(decimal[] amount)
        {
            if (amount.Count() <= 1)
            {
                throw new Exception("Amount invalid");
            }
            Amount = amount[0];
            DollarAmount = amount[1];
        }
    }
}
