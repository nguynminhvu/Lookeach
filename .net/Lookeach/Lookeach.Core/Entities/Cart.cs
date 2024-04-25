using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Cart:Audit<DateTime,Guid>
    {
        public decimal Amount { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
