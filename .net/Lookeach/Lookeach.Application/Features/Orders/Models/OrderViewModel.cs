using Lookeach.Application.ViewModels.PaymentMethods;
using Lookeach.Application.ViewModels.Vouchers;
using Lookeach.Core.Common.Base;
using Lookeach.Core.Entities;

namespace Lookeach.Application.ViewModels.Orders
{
    public class OrderViewModel : BaseEntity<Guid>
    {
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentMethodViewModel PaymentMethod { get; set; }
        public ICollection<OrderItemViewModel> OrderItems { get; set; }
        public ICollection<VoucherViewModel>? Vouchers { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
