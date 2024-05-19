using Lookeach.Core.Common.Base;

namespace Lookeach.Application.Features.Chats.Models
{
    public class ChatViewModel : Audit<DateTime, Guid>
    {
        public string Name { get; set; }
        public ICollection<ChatMessageViewModel> Messages { get; set; }
        public ICollection<ChatUserViewModel> ChatUsers { get; set; }
    }
}
