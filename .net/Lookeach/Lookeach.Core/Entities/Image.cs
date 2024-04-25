using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Image : Audit<DateTime, Guid>
    {
        public string Url { get; set; }
    }
}
