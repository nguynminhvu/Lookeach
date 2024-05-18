using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.PaymentMethods
{
    public class PaymentMethodViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
