using Lookeach.Application.Commands.Orders;
using Lookeach.Application.Queries.Orders;
using Lookeach.Core.ViewModels.Orders;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id)
        {
            ResponseBase<OrderViewModel> responseBase = new ResponseBase<OrderViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetOrderByIdQuery(id))
            };
            return Ok(responseBase);
        }

        [HttpGet("orders/{userId}/userId")]
        public async Task<IActionResult> GetOrdersByUserId([FromRoute] Guid userId, DateTime? dateFrom, DateTime? dateTo, int? pageIndex, int? pageSize)
        {
            ResponseBase<List<OrderViewModel>> responseBase = new ResponseBase<List<OrderViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetOrdersByUserIdQuery(userId, dateFrom, dateTo, pageIndex, pageSize))
            };
            return Ok(responseBase);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders(DateTime? dateFrom, DateTime? dateTo, int? pageIndex, int? pageSize)
        {
            ResponseBase<List<OrderViewModel>> responseBase = new ResponseBase<List<OrderViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetOrdersQuery(dateFrom, dateTo, pageIndex, pageSize))
            };
            return Ok(responseBase);
        }

        [HttpPost("order/create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateModel orderCreateModel)
        {
            ResponseBase<OrderViewModel> responseBase = new ResponseBase<OrderViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new CreateOrderCommand(orderCreateModel))
            };
            return StatusCode(StatusCodes.Status201Created, responseBase);
        }

        [HttpPut("order/update")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateModel orderUpdateModel)
        {
            ResponseBase<OrderViewModel> responseBase = new ResponseBase<OrderViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new UpdateOrderCommand(orderUpdateModel))
            };
            return StatusCode(StatusCodes.Status200OK, responseBase);
        }
    }
}
