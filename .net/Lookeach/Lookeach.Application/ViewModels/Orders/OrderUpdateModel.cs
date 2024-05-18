using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.Orders
{
    public class OrderUpdateModel : BaseEntity<Guid>
    {
        public string Status { get; set; }
    }
}
