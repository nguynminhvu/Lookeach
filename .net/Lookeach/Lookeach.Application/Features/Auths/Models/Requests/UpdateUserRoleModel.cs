namespace Lookeach.Application.Features.Auths.Models.Requests
{
    public class UpdateUserRoleModel
    {
        public Guid UserId { get; set; }
        public List<Guid>? RoleIds { get; set; } 
    }
}
