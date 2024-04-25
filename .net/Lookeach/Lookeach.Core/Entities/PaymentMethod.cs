using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class PaymentMethod:Audit<DateTime,Guid>
    {
        public string Name { get; set; }
    }
}
