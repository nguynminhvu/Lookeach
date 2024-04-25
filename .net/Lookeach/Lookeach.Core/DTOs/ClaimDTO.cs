using Lookeach.Core.Entities;

namespace Lookeach.Core.DTOs
{
    public class ClaimDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
