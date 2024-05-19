using Lookeach.Core.Common.Base;

namespace Lookeach.Application.Features.Chats.Models
{
    public class UserChatViewModel:BaseEntity<Guid>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Thumbnail { get; set; }
    }
}
