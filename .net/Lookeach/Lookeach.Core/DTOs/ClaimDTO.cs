using Lookeach.Core.Entities;

namespace Lookeach.Core.DTOs
{
    public class ClaimDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserRole> Roles { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}
