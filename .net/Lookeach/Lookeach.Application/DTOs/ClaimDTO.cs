using Lookeach.Core.Common.Base;

namespace Lookeach.Application.DTOs
{
    public class ClaimDTO : BaseEntity<Guid>
    {
        public string Email { get; set; }

    }
}
