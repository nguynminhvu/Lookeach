using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class UserClaim : Audit<DateTime, Guid>
    {
        public Guid UserId { get; set; }
        public string ClaimName { get; set; }
        public string ClaimValue { get; set; }
        public User User { get; set; }
    }
}
