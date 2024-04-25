using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Size : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
    }
}
