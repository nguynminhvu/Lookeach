using Lookeach.Application.ViewModels.Images;

namespace Lookeach.Application.Features.Chats.Models
{
    public class ChatImageViewModel
    {
        public Guid ChatId { get; set; }
        public Guid ImageId { get; set; }
        public ImageViewModel Image { get; set; }
    }
}
