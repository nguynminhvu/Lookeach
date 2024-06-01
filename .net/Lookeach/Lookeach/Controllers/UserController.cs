using Lookeach.Application.Features.Users.Queries;
using Lookeach.Application.ViewModels.Users;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(string? name = null!, string? username = null!, string? email = null!, int? pageIndex = null!, int? pageSize = null!)
        {
            return Ok(new ResponseBase<List<UserViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetUsersQuery(name, username, email, pageIndex, pageSize))
            });
        }
    }
}
