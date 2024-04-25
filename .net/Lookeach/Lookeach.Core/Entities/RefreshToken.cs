using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class RefreshToken:Audit<DateTime,Guid>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
    }
}
