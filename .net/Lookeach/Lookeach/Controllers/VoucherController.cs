using Lookeach.Application.Commands.Vouchers;
using Lookeach.Application.Queries.Vouchers;
using Lookeach.Application.ViewModels.Vouchers;
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
        /// <summary>
        /// Vouchers of merchant
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("vouchers/{userid}/by-userid")]
        public async Task<IActionResult> GetVoucherByUserId([FromRoute] Guid userid)
        {
            ResponseBase<List<VoucherViewModel>> responseBase = new ResponseBase<List<VoucherViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetVoucherByUserIdQuery(userid))
            };
            return Ok(responseBase);
        }
        /// <summary>
        /// Vouchers of admin
        /// </summary>
        /// <returns></returns>
        [HttpGet("vouchers/voucherSystem")]
        public async Task<IActionResult> GetVoucherSystem()
        {
            ResponseBase<List<VoucherViewModel>> responseBase = new ResponseBase<List<VoucherViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetVoucherSystemQuery())
            };
            return Ok(responseBase);
        }
        /// <summary>
        /// Date format yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="voucherCreateModel"></param>
        /// <returns></returns>
        [LookeachAuthorize(
        new string[] { ActionConstant.WRITE }
        )]
        [HttpPost("voucher/create")]
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

        /// <summary>
        /// Date format yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="voucherUpdateModel"></param>
        /// <returns></returns>
        
        [LookeachAuthorize(
          new string[] { ActionConstant.OVERRIDE }
          )]
        [HttpPut("voucher/update")]
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

        [LookeachAuthorize(
            new string[] { ActionConstant.READ }
            )]
        [HttpGet("test-admin")]
        public IActionResult GetRscrAdmin()
        {
            return Ok("wow you good");
        }

        [LookeachAuthorize(
            new string[] { ActionConstant.WRITE }
            )]
        [HttpGet("test-customer")]
        public IActionResult GetRscrCustomer()
        {
            return Ok("wow you good");
        }


        [LookeachAuthorize(
            new string[] { ActionConstant.READ }
            )]
        [HttpGet("test-admin-customer")]
        public IActionResult GetRscrBoth()
        {
            return Ok("wow you good");
        }

    }
}
