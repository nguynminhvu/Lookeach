using Lookeach.Application.Features.Chats.Commands;
using Lookeach.Application.Features.Chats.Models;
using Lookeach.Application.Features.Chats.Queries;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
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
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageSendModel chatMessageSendModel)
        {
            return StatusCode(StatusCodes.Status201Created, new ResponseBase<ChatViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new SendMessageCommand(chatMessageSendModel))
            });
        }
    }
}
