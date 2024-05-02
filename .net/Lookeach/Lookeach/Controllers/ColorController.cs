using Lookeach.Application.Commands.Colors;
using Lookeach.Application.Queries.Colors;
using Lookeach.Core.ViewModels.Colors;
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

        [HttpGet("{id}/size")]
        public async Task<IActionResult> GetColor([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new GetColorByIdQuery(id));

            ResponseViewModel responseViewModel = new ResponseViewModel
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = JsonConvert.SerializeObject(rs)
            };
            return Ok(responseViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetColors(string? name, int? pageIndex, int? pageSize)
        {
            var rs = await _mediator.Send(new GetColorsQuery(name, pageIndex, pageSize));
            ResponseViewModel responseViewModel = new ResponseViewModel
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = JsonConvert.SerializeObject(rs)
            };
            return Ok(responseViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateColor([FromBody] string name)
        {
            var rs = await _mediator.Send(new CreateColorCommand(name));
            ResponseViewModel responseViewModel = new ResponseViewModel
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = JsonConvert.SerializeObject(rs)
            };
            return StatusCode(StatusCodes.Status201Created, responseViewModel);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateColor([FromBody] ColorUpdateModel ColorUpdateModel)
        {
            var rs = await _mediator.Send(new UpdateColorCommand(ColorUpdateModel));
            ResponseViewModel responseViewModel = new ResponseViewModel
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = JsonConvert.SerializeObject(rs)
            };
            return StatusCode(StatusCodes.Status200OK,responseViewModel);
        }
    }
}
