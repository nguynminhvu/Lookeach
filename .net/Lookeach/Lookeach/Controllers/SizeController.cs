using Lookeach.Application.Commands.Sizes;
using Lookeach.Application.Queries.Sizes;
using Lookeach.Core.ViewModels.Sizes;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SizeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/size")]
        public async Task<IActionResult> GetSize([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new GetSizeByIdQuery(id));

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
        public async Task<IActionResult> GetSizes(string? name, int? pageIndex, int? pageSize)
        {
            var rs = await _mediator.Send(new GetSizesQuery(name, pageIndex, pageSize));
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
        public async Task<IActionResult> CreateSize([FromBody] string name)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var rs = await _mediator.Send(new CreateSizeCommand(name));

            if (rs != null!)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
                return StatusCode(StatusCodes.Status201Created, responseViewModel);
            }
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't create Size"
            };
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;

            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateSize([FromBody] SizeUpdateModel sizeUpdateModel)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var rs = await _mediator.Send(new UpdateSizeCommand(sizeUpdateModel));

            if (rs != null!)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
                return StatusCode(StatusCodes.Status200OK, responseViewModel);
            }
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't update Size"
            };
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;

            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }
    }
}
