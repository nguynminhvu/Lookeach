using Lookeach.Core.Common.Base;

namespace Lookeach.Core.ViewModels.Orders
{
    public class OrderUpdateModel : BaseEntity<Guid>
    {
        public string Status { get; set; }
    }
}
