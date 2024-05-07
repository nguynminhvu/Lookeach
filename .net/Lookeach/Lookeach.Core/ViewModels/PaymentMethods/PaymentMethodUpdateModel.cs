using Lookeach.Core.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Lookeach.Core.ViewModels.PaymentMethods
{
    public class PaymentMethodUpdateModel : BaseEntity<Guid>
    {
        [Required]
        public override Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
