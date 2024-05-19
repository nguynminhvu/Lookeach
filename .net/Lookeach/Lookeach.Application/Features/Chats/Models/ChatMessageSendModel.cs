using Microsoft.AspNetCore.Http;

namespace Lookeach.Application.Features.Chats.Models
{
    public class ChatMessageSendModel
    {
        public Guid ReceiverId { get; set; }
        public string Text { get; set; }
        public ICollection<IFormFile>? ChatImages { get; set; }
    }
}
