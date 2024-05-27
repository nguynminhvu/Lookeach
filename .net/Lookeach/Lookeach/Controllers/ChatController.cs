using Lookeach.Application.Features.Chats.Commands;
using Lookeach.Application.Features.Chats.Models;
using Lookeach.Application.Features.Chats.Queries;
using Lookeach.Hubs;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubcontext;
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator, IHubContext<ChatHub> hubcontext)
        {
            _hubcontext = hubcontext;
            _mediator = mediator;
        }


        /// <summary>
        /// Get chat by receiver name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("chats")]
        public async Task<IActionResult> GetChats(string? name = null!, int? pageIndex = null!, int? pageSize = null!)
        {
            return Ok(new ResponseBase<List<ChatViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetChatsQuery(name, pageIndex, pageSize))
            }); ;
        }


        [HttpPost("chat")]
        public async Task<IActionResult> SendMessage([FromForm] ChatMessageSendModel chatMessageSendModel)
        {
            var rs = await _mediator.Send(new SendMessageCommand(chatMessageSendModel));
            await _hubcontext.Clients.Group(rs.Id + "").SendAsync("NewMessage", rs);
            return StatusCode(StatusCodes.Status201Created, new ResponseBase<ChatViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            });
        }
    }
}
