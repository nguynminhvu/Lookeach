using Lookeach.Application.Commands.Sizes;
using Lookeach.Application.Queries.Sizes;
using Lookeach.Application.ViewModels.Sizes;
using Lookeach.AuthOptions;
using Lookeach.Constants;
using Lookeach.Core.Enums;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("size/{id}")]
        public async Task<IActionResult> GetSize([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new GetSizeByIdQuery(id));

            ResponseBase<SizeViewModel> responseViewModel = new ResponseBase<SizeViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };

            return Ok(responseViewModel);
        }

        [HttpGet("sizes")]
        public async Task<IActionResult> GetSizes(string? name, int? pageIndex, int? pageSize)
        {
            var rs = await _mediator.Send(new GetSizesQuery(name, pageIndex, pageSize));
            ResponseBase<List<SizeViewModel>> responseViewModel = new ResponseBase<List<SizeViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };

            return Ok(responseViewModel);
        }

        [LookeachAuthorize(
          new string[] { ActionConstant.WRITE }
          )]
        [HttpPost("size/create")]
        public async Task<IActionResult> CreateSize([FromBody] string name)
        {
            ResponseBase<SizeViewModel> responseViewModel = new ResponseBase<SizeViewModel>();

            var rs = await _mediator.Send(new CreateSizeCommand(name));

            if (rs != null!)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = rs;
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

        [LookeachAuthorize(
          new string[] { ActionConstant.OVERRIDE }
          )]
        [HttpPut("size/update")]
        public async Task<IActionResult> UpdateSize([FromBody] SizeUpdateModel sizeUpdateModel)
        {
            ResponseBase<SizeViewModel> responseViewModel = new ResponseBase<SizeViewModel>();

            var rs = await _mediator.Send(new UpdateSizeCommand(sizeUpdateModel));

            if (rs != null!)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = rs;
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
