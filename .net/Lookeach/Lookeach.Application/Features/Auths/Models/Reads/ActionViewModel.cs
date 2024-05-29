using Lookeach.Core.Common.Base;

namespace Lookeach.Application.Features.Auths.Models.Reads
{
    public class ActionViewModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }
    }
}
