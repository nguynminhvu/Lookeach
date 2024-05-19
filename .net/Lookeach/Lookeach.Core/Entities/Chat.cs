using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class Chat : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<ChatMessage> Messages { get; set; }
        public ICollection<ChatUser> ChatUsers { get; set; }
    }
}
