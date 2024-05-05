using Lookeach.Application.Commands.Carts;
using Lookeach.Application.Queries.Carts;
using Lookeach.Core.ViewModels.Carts;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCart([FromBody] CartUpdateModel cartUpdateModel)
        {
            var rs = await _mediator.Send(new UpdateCartCommand(cartUpdateModel));
            ResponseBase<CartViewModel> responseBase = new ResponseBase<CartViewModel>();
            if (rs != null)
            {
                responseBase.IsSuccess = true;
                responseBase.Error = null!;
                responseBase.IsFailure = false;
                responseBase.Value = rs;
                return Ok(responseBase);
            }
            responseBase.IsSuccess = false;
            responseBase.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't update cart, try again"
            };
            responseBase.IsFailure = true;
            responseBase.Value = null!;
            return StatusCode(StatusCodes.Status500InternalServerError, responseBase);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCart()
        {
            ResponseBase<CartViewModel> responseBase = new ResponseBase<CartViewModel>();
            responseBase.IsSuccess = true;
            responseBase.Error = null!;
            responseBase.IsFailure = false;
            responseBase.Value = await _mediator.Send(new GetCartQuery());
            return Ok(responseBase);
        }
    }
}
