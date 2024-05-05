using Lookeach.Application.Commands.Vouchers;
using Lookeach.Application.Queries.Vouchers;
using Lookeach.Core.ViewModels.Vouchers;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VoucherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("voucher/{id}")]
        public async Task<IActionResult> GetVoucher([FromRoute] Guid id)
        {
            ResponseBase<VoucherViewModel> responseBase = new ResponseBase<VoucherViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetVoucherByIdQuery(id))
            };
            return Ok(responseBase);
        }

        [HttpGet("vouchers/{id}/by-userid")]
        public async Task<IActionResult> GetVoucherByUserId([FromRoute] Guid id)
        {
            ResponseBase<List<VoucherViewModel>> responseBase = new ResponseBase<List<VoucherViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetVoucherByUserIdQuery(id))
            };
            return Ok(responseBase);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoucher([FromBody] VoucherCreateModel voucherCreateModel)
        {
            ResponseBase<VoucherViewModel> responseBase = new ResponseBase<VoucherViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new CreateVoucherCommand(voucherCreateModel))
            };
            return StatusCode(StatusCodes.Status201Created, responseBase);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateVoucher([FromBody] VoucherUpdateModel voucherUpdateModel)
        {
            ResponseBase<VoucherViewModel> responseBase = new ResponseBase<VoucherViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new UpdateVoucherCommand(voucherUpdateModel))
            };
            return StatusCode(StatusCodes.Status200OK, responseBase);
        }
    }
}
