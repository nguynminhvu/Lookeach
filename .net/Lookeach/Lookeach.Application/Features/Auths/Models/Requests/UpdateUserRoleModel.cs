namespace Lookeach.Application.Features.Auths.Models.Requests
{
    public class UpdateUserRoleModel
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
