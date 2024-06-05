using Lookeach.Application.DTOs;
using Lookeach.Application.Features.Auths.Commands;
using Lookeach.Application.Features.Auths.Models.Reads;
using Lookeach.Application.Features.Auths.Models.Requests;
using Lookeach.Application.Features.Auths.Queries;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuperAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> SAdminLogin([FromBody] LoginDTO loginDto)
        {
            return Ok(new ResponseBase<string>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = await _mediator.Send(new SuperAdminLoginCommand(loginDto))
            });
        }
        [HttpGet("permissions")]
        public async Task<IActionResult> GetAllPermission()
        {
            return Ok(new ResponseBase<List<PermissionViewModel>>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = await _mediator.Send(new GetAllPermissionQuery())
            });
        }

        [HttpGet("actions")]
        public async Task<IActionResult> GetAllAction()
        {
            return Ok(new ResponseBase<List<ActionViewModel>>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = await _mediator.Send(new GetAllActionQuery())
            });
        }

        [HttpGet("resources")]
        public async Task<IActionResult> GetAllResource()
        {
            return Ok(new ResponseBase<List<ResourceViewModel>>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = await _mediator.Send(new GetAllResourceQuery())
            });
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRole(string? name = null!)
        {
            return Ok(new ResponseBase<List<RoleViewModel>>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = await _mediator.Send(new GetAllRoleQuery(name))
            });
        }

        [HttpPut("role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleModel UpdateUserRoleModel)
        {
            return Ok(new ResponseBase<string>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = (await _mediator.Send(new UpdateRoleOfUserCommand(UpdateUserRoleModel))) ? "true" : "false"
            });
        }

        [HttpPut("permission")]
        public async Task<IActionResult> UpdatePermissionRole([FromBody] AddPermissionModel AddPermissionModel)
        {
            return Ok(new ResponseBase<string>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = (await _mediator.Send(new UpdatePermissionIntoRoleCommand(AddPermissionModel))) ? "true" : "false"
            });
        }

        [HttpPut("permissions")]
        public async Task<IActionResult> UpdatePermissionRoleActionValue([FromBody] UpdatePermissionActionValue updatePermissionActionValue)
        {
            return Ok(new ResponseBase<string>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = (await _mediator.Send(new UpdatePermissionIntoRoleActionValueCommand(updatePermissionActionValue))) ? "true" : "false"
            });
        }

        [HttpPost("create/role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleModel CreateRoleModel)
        {
            return Ok(new ResponseBase<string>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = (await _mediator.Send(new CreateRoleCommand(CreateRoleModel))) ? "true" : "false"
            });
        }
        [HttpPost("create/role/value")]
        public async Task<IActionResult> CreateRolePermissionValue([FromBody] CreateRolePermissionValueModel createRolePermissionValueModel)
        {
            return Ok(new ResponseBase<string>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = (await _mediator.Send(new CreateRoleWithPermissionValueCommand(createRolePermissionValueModel))) ? "true" : "false"
            });
        }

        [HttpPost("create/permission")]
        public async Task<IActionResult> CreatePermission([FromBody] UpdatePermissionActionValue updatePermissionActionValue)
        {
            return Ok(new ResponseBase<string>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = (await _mediator.Send(new AddPermissionIntoRoleCommand(updatePermissionActionValue))) ? "true" : "false"
            });
        }

        [HttpPost("create/action")]
        public async Task<IActionResult> CreateAction([FromBody] string name)
        {
            return Ok(new ResponseBase<string>
            {
                IsFailure = false,
                Error = null!,
                IsSuccess = true,
                Value = (await _mediator.Send(new CreateActionCommand(name))) ? "true" : "false"
            });
        }
    }
}
