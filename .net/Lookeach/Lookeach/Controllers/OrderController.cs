using Lookeach.Application.Commands.Orders;
using Lookeach.Application.Queries.Orders;
using Lookeach.Application.ViewModels.Orders;
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
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [LookeachAuthorize(
          new RoleName[] { RoleName.Admin, RoleName.Merchant, RoleName.Customer },
          new string[] { ActionConstant.READ }
          )]
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

        [LookeachAuthorize(
          new RoleName[] { RoleName.Admin, RoleName.Merchant, RoleName.Customer },
          new string[] { ActionConstant.READ }
          )]
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

        [LookeachAuthorize(
          new RoleName[] { RoleName.Admin, RoleName.Merchant, RoleName.Customer },
          new string[] { ActionConstant.READ }
          )]
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

        [LookeachAuthorize(
          new RoleName[] { RoleName.Customer },
          new string[] { ActionConstant.WRITE }
          )]
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

        [LookeachAuthorize(
          new RoleName[] { RoleName.Admin, RoleName.Merchant },
          new string[] { ActionConstant.OVERRIDE }
          )]
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
