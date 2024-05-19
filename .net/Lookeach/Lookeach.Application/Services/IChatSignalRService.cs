using Lookeach.Application.Features.Chats.Models;

namespace Lookeach.Application.Services
{
    public interface IChatSignalRService
    {
        Task<List<ChatViewModel>> GetChatCache();
    }
}
