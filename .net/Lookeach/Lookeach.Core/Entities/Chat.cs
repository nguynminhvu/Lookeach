using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Chat : Audit<DateTime, Guid>
    {
        public ICollection<Message> Messages { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
