using Lookeach.Core.Common.Base;
using Lookeach.Core.Entities;
using Lookeach.Core.ViewModels.PaymentMethods;
using Lookeach.Core.ViewModels.Vouchers;

namespace Lookeach.Core.ViewModels.Orders
{
    public class OrderViewModel : BaseEntity<Guid>
    {
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentMethodViewModel PaymentMethod { get; set; }
        public ICollection<OrderItemViewModel> OrderItems { get; set; }
        public ICollection<VoucherViewModel> Vouchers { get; set; }
    }
}
