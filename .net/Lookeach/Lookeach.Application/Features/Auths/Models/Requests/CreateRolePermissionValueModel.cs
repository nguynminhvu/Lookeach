namespace Lookeach.Application.Features.Auths.Models.Requests
{
    public class CreateRolePermissionValueModel
    {
        public string Name { get; set; }
        public List<ResourceRecordPermissionValueModel> ResourceRecords { get; set; }
        public List<Guid>? UserIds { get; set; }
    }

    public record ResourceRecordPermissionValueModel(Guid ResourceId, int PermissionValue);

}
