using Lookeach.Core.Common.Base;

namespace Lookeach.Core.Entities
{
    public class ChatMessage:Audit<DateTime,Guid>
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public ICollection<Image>? Images { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
