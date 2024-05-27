using Lookeach.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace Lookeach.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatSignalRService _chatSignalRService;
      
        public ChatHub( IChatSignalRService chatSignalRService)
        {
            _chatSignalRService = chatSignalRService;
        }

        public override async Task OnConnectedAsync()
        {
            var chatVm = await _chatSignalRService.GetChatCache();
            foreach (var item in chatVm)
            {
                await this.Groups.AddToGroupAsync(Context.ConnectionId, item.Id + "");
            }
            await base.OnConnectedAsync();
        }

    }
}
