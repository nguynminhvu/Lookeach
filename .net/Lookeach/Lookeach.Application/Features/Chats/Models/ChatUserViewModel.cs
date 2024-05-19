using Lookeach.Application.ViewModels.Users;

namespace Lookeach.Application.Features.Chats.Models
{
    public class ChatUserViewModel
    {
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public UserViewModel User { get; set; }
    }
}
