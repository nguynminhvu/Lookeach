namespace Lookeach.Application.Features.Auths.Models.Requests
{
    public class CreateRoleModel
    {
        public string Name { get; set; }
        public List<ResourceRecord> ResourceRecords { get; set; }
    }

    public record ResourceRecord(Guid ResourceId, List<Guid> ActionIds);
  
}
