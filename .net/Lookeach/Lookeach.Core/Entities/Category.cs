using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Category : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<GenderType>? GenderTypes { get; set; }
    }
}
