using Lookeach.Application.Commands.GenderTypes;
using Lookeach.Application.Queries.GenderTypes;
using Lookeach.Core.ViewModels.GenderTypes;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenderTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenderType([FromBody] string name)
        {
            ResponseBase<GenderTypeViewModel> responseViewModel = new ResponseBase<GenderTypeViewModel>();

            var rs = await _mediator.Send(new CreateGenderTypeCommand(name));

            if (rs != null)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = rs;
                return StatusCode(StatusCodes.Status201Created, responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't create gender type"
            };
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGenderType([FromBody] GenderTypeUpdateModel genderTypeUpdateModel)
        {
            ResponseBase<GenderTypeViewModel> responseViewModel = new ResponseBase<GenderTypeViewModel>();

            var rs = await _mediator.Send(new UpdateGenderTypeCommand(genderTypeUpdateModel));
            if (rs != null)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = rs;
                return StatusCode(StatusCodes.Status200OK, responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't create gender type"
            };
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }

        [HttpGet("{id}/genderType")]
        public async Task<IActionResult> GetGenderType([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new GetGenderTypeByIdQuery(id));
            ResponseBase<GenderTypeViewModel> responseViewModel = new ResponseBase<GenderTypeViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };
            return Ok(responseViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetGenderTypes(string? name, int? pageIndex, int? pageSize)
        {
            var rs = await _mediator.Send(new GetGenderTypesQuery(name, pageIndex, pageSize));
            ResponseBase<List<GenderTypeViewModel>> responseViewModel = new ResponseBase<List<GenderTypeViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };
            return Ok(responseViewModel);
        }
    }
}
