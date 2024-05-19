namespace Lookeach.Application.Features.Chats.Models
{
    public class ChatMessageViewModel
    {
        public Guid ChatId { get; set; }
        public string Text { get; set; }
        public ICollection<ChatImageViewModel>? ChatImages { get; set; }
        public UserChatViewModel User { get; set; }
    }
}
