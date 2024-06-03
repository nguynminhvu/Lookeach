namespace Lookeach.Application.Features.Auths.Models.Requests
{
    public class UpdatePermissionActionValue
    {
        public Guid RoleId { get; set; }
        public List<ResourceValueRecord> ResourceValueRecords { get; set; }
    }

    public sealed record ResourceValueRecord(Guid ResourceId, int PermissionValue);
}
