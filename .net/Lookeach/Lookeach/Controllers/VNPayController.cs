using Lookeach.Application.Commands.VNPays;
using Lookeach.Application.Queries.VNPays;
using Lookeach.Core.Entities;
using Lookeach.Core.ViewModels.Orders;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VNPayController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("url/vnpay")]
        public async Task<IActionResult> GetUrlVNPay(Guid orderId)
        {
            return StatusCode(StatusCodes.Status201Created, new ResponseBase<string>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetUrlVNPayCommand(orderId))
            });
        }

        [HttpPost("order/handleQuery")]
        public async Task<IActionResult> HandlerQuery()
        {
            var query = HttpContext.Request.QueryString + "";
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Fuck");
            }
            return StatusCode(StatusCodes.Status201Created, new ResponseBase<OrderViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new HandleQueryVNPayCommand(query))
            });
        }


        [HttpPost("order/refund")]
        public async Task<IActionResult> RefundOrder([FromBody] Guid orderId)
        {
            return StatusCode(StatusCodes.Status201Created, new ResponseBase<OrderViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new RefundVNPayCommand(orderId))
            });
        }

        [HttpGet("vnpay/transaction/{orderId}")]
        public async Task<IActionResult> GetVNPayTransaction([FromRoute] Guid? orderId, DateTime? dateFrom = null!, DateTime? dateTo = null!, int? pageIndex = null!, int? pageSize = null!)
        {
            return StatusCode(StatusCodes.Status201Created, new ResponseBase<List<VNPayTransaction>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetVNPayTransactionQuery(orderId, dateFrom, dateTo, pageIndex, pageSize))
            });
        }
    }
}
