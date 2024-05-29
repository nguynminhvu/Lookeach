namespace Lookeach.Application.Features.Auths.Models.Requests
{
    public class AddPermissionModel
    {
        public Guid RoleId { get; set; }
        public List<ResourceRecord> ResourceRecords { get; set; }
    }
}
