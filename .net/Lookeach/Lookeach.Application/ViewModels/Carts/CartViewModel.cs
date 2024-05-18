using Lookeach.Application.ViewModels.Users;
using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Carts
{
    public class CartViewModel : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public UserViewModel User { get; set; }
        public decimal Amount { get; set; }
        public ICollection<CartItemViewModel> CartItems { get; set; }
    }
}
