using Lookeach.Core.Common.Base;

namespace Lookeach.Application.DTOs
{
    public class ActionDTO:BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }
    }
}
