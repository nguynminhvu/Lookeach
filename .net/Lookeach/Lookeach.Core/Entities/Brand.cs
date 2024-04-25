using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Brand : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<Image>? Images { get; set; }
    }
}
