using Lookeach.Core.Common.Base;

namespace Lookeach.Core.ViewModels.PaymentMethods
{
    public class PaymentMethodViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
