using Lookeach.Application.Commands.Auths;
using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                ErrorViewModel model = new ErrorViewModel
                {
                    Code = 400,
                    Message = "Username or password is empty"
                };
                return BadRequest(new ResponseViewModel
                {
                    Error = model,
                    IsFailure = true,
                    IsSuccess = false,
                    Value = null!
                });
            }
            var token = await _mediator.Send(new LoginCommand(loginDTO));
            ResponseViewModel responseViewModel = new();
            if (!string.IsNullOrEmpty(token + ""))
            {
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = token;
                responseViewModel.Error = null!;
                return Ok(responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 400,
                Message = "Can't create token"
            };
            return BadRequest(responseViewModel);
        }
    }
}
