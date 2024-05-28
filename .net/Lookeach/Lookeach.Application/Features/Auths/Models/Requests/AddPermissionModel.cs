namespace Lookeach.Application.Features.Auths.Models.Requests
{
    public class AddPermissionModel
    {
        public Guid RoleId { get; set; }
        public Guid ResourceId { get; set; }
        public List<Guid> ActionIds { get; set; } = new List<Guid>();
    }
}
