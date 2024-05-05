using Lookeach.Application.Commands.Colors;
using Lookeach.Application.Queries.Colors;
using Lookeach.Core.ViewModels.Colors;
using Lookeach.Core.ViewModels.GenderTypes;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("color/{id}")]
        public async Task<IActionResult> GetColor([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new GetColorByIdQuery(id));
            ResponseBase<ColorViewModel> responseViewModel = new ResponseBase<ColorViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };
            return Ok(responseViewModel);
        }

        [HttpGet("colors")]
        public async Task<IActionResult> GetColors(string? name, int? pageIndex, int? pageSize)
        {
            var rs = await _mediator.Send(new GetColorsQuery(name, pageIndex, pageSize));
            ResponseBase<List<ColorViewModel>> responseViewModel = new ResponseBase<List<ColorViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value =rs
            };
            return Ok(responseViewModel);
        }


        [HttpPost("color/create")]
        public async Task<IActionResult> CreateColor([FromBody] string name)
        {
            var rs = await _mediator.Send(new CreateColorCommand(name));
            ResponseBase<ColorViewModel> responseViewModel = new ResponseBase<ColorViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };
            return StatusCode(StatusCodes.Status201Created, responseViewModel);
        }


        [HttpPut("color/update")]
        public async Task<IActionResult> UpdateColor([FromBody] ColorUpdateModel ColorUpdateModel)
        {
            var rs = await _mediator.Send(new UpdateColorCommand(ColorUpdateModel));
            ResponseBase<ColorViewModel> responseViewModel = new ResponseBase<ColorViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };
            return StatusCode(StatusCodes.Status200OK,responseViewModel);
        }
    }
}
