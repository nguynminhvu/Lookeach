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
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var rs = await _mediator.Send(new CreateGenderTypeCommand(name));

            if (rs != null)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
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
            ResponseViewModel responseViewModel = new ResponseViewModel();

            var rs = await _mediator.Send(new UpdateGenderTypeCommand(genderTypeUpdateModel));
            if (rs != null)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
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
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetGenderTypes(string? name, int? pageIndex, int? pageSize)
        {
            var rs = await _mediator.Send(new GetGenderTypesQuery(name, pageIndex, pageSize));
            return Ok(rs);
        }
    }
}
