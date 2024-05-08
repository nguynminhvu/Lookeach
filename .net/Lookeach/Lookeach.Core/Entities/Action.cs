using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Action : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }
    }
}
