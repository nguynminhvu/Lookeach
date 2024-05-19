using Lookeach.Application.Commands.Auths;
using Lookeach.Application.DTOs;
using Lookeach.Application.Features.Users.Commands;
using Lookeach.Application.Features.Users.Models;
using Lookeach.Application.ViewModels.Users;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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


        [HttpPost("login")]
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

        /// <summary>
        /// Create user (user role and merchant role) 
        /// </summary>
        /// <param name="userCreateModel"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateModel userCreateModel)
        {
            ResponseBase<UserViewModel> responseViewModel = new ResponseBase<UserViewModel>();
            if (ModelState.IsValid)
            {
                var rs = await _mediator.Send(new CreateUserCommand(userCreateModel));
                if (rs != null)
                {
                    responseViewModel.Error = null!;
                    responseViewModel.Value = rs;
                    return StatusCode(StatusCodes.Status201Created, responseViewModel);
                }
                responseViewModel.Error = new ErrorViewModel
                {
                    Code = 400,
                    Message = "Can't create user, try again"
                };
                responseViewModel.Value = null!;
                return BadRequest(responseViewModel);
            }
            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 400,
                Message = string.Join("; ", errors)
            };
            responseViewModel.Value = null!;
            return BadRequest(responseViewModel);
        }


        /// <summary>
        /// Destroy session login
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = (HttpContext.Request.Headers.Authorization.FirstOrDefault() + "").Split(" ").Last();
            if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException();
            await _mediator.Send(new LogoutCommand());
            return NoContent();
        }


        /// <summary>
        /// Renew access token via refresh token, param header require access token expired and pass more "RefreshToken"
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpPost("token/renew")]
        public async Task<IActionResult> ReNew()
        {
            ResponseBase<string> responseViewModel = new ResponseBase<string>();

            var token = (HttpContext.Request.Headers["RefreshToken"].FirstOrDefault() + "").Split(" ").Last();

            if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException("Token invalid, login please");

            var rs = await _mediator.Send(new RenewTokenCommand(token));
            if (!string.IsNullOrEmpty(rs))
            {
                responseViewModel.Value = rs;
                responseViewModel.Error = null!;
                return StatusCode(StatusCodes.Status201Created, responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't renew token, try login"
            };
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }


        /// <summary>
        /// Endpoint confirm email, user will call in their email
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("confirm-email/{code}")]
        public async Task<IActionResult> ConfirmEmail([FromRoute] string code)
        {
            ResponseBase<string> responseViewModel = new ResponseBase<string>();

            var rs = await _mediator.Send(new ConfirmEmailCommand(code));
            if (rs != null)
            {
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
                responseViewModel.Error = null!;
                return StatusCode(StatusCodes.Status202Accepted, responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't confirm email, try again"
            };
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }
    }
}
