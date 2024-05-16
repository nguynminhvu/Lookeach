using Lookeach.Application.Commands.Orders;
using Lookeach.Application.Commands.VNPays;
using Lookeach.Application.Paypals.Commands;
using Lookeach.Core.ViewModels.Orders;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayPalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("url")]
        public async Task<IActionResult> GetUrlPayPal([FromBody] Guid orderId)
        {
            if (Guid.Empty == orderId)
            {
                throw new ArgumentNullException("Order id invalid");
            }
            return Ok(new ResponseBase<string>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new CreateOrderPayPalCommand(orderId))
            });
        }

        [HttpGet("callback/paypal")]
        public async Task<IActionResult> HandleQueryPaypal()
        {
            string token = "";
            try
            {
                var query = Request.QueryString + "";
                token = (query.Split("&")[0]).Split("=")[1];
            }
            catch
            {
                throw new Exception("Parameter");
            }

            if (string.IsNullOrEmpty(token)) throw new Exception("Invalid transaction");

            return Ok(new ResponseBase<OrderViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new CompleteOrderPayPalCommand(token))
            });
        }
    }
}
