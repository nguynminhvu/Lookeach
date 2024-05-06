using Lookeach.Application.Commands.PaymentMethods;
using Lookeach.Application.Queries.PaymentMethods;
using Lookeach.Core.ViewModels.PaymentMethods;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentMethodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("paymentMethod/{id}")]
        public async Task<IActionResult> GetPaymentMethod([FromRoute] Guid id)
        {
            ResponseBase<PaymentMethodViewModel> responseBase = new ResponseBase<PaymentMethodViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetPaymentMethodByIdQuery(id))
            };
            return Ok(responseBase);
        }


        [HttpGet("paymentMethods")]
        public async Task<IActionResult> GetPaymentMethods(string? name = null!)
        {
            ResponseBase<List<PaymentMethodViewModel>> responseBase = new ResponseBase<List<PaymentMethodViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetPaymentMethodsQuery(name))
            };
            return Ok(responseBase);
        }


        [HttpPost("paymentMethod")]
        public async Task<IActionResult> CreatePaymentMethod(string name)
        {
            ResponseBase<PaymentMethodViewModel> responseBase = new ResponseBase<PaymentMethodViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new CreatePaymentMethodCommand(name))
            };
            return StatusCode(StatusCodes.Status201Created, responseBase);
        }

        [HttpPut("paymentMethod")]
        public async Task<IActionResult> UpdatePaymentMethod(PaymentMethodUpdateModel paymentMethodUpdateModel)
        {
            ResponseBase<PaymentMethodViewModel> responseBase = new ResponseBase<PaymentMethodViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new UpdatePaymentMethodCommand(paymentMethodUpdateModel))
            };
            return StatusCode(StatusCodes.Status200OK, responseBase);
        }
    }
}
