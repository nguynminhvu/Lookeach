using Lookeach.Application.Commands.Products;
using Lookeach.Application.Queries.Products;
using Lookeach.Application.ViewModels.Products;
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
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id required");
            ResponseBase<ProductViewModel> responseViewModel = new ResponseBase<ProductViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetProductByIdQuery(id))
            };
            return Ok(responseViewModel);
        }

        [HttpGet("products/by-categories")]
        public async Task<IActionResult> GetProductByCategoryId([FromQuery] List<Guid> categoryIds, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            ResponseBase<List<ProductViewModel>> responseViewModel = new ResponseBase<List<ProductViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetProductByCategoryIdsQuery(categoryIds, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow))
            };
            return Ok(responseViewModel);
        }

        [HttpGet("products/by-colors")]
        public async Task<IActionResult> GetProductByColorId([FromQuery] List<Guid> colorIds, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            ResponseBase<List<ProductViewModel>> responseViewModel = new ResponseBase<List<ProductViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetProductByColorsIdsQuery(colorIds, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow))
            };
            return Ok(responseViewModel);
        }

        [HttpGet("products/by-sizes")]
        public async Task<IActionResult> GetProductBySizesId([FromQuery] List<Guid> sizeIds, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            ResponseBase<List<ProductViewModel>> responseViewModel = new ResponseBase<List<ProductViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetProductBySizeIdsQuery(sizeIds, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow))
            };
            return Ok(responseViewModel);
        }

        [HttpGet("products/{id}/by-brand")]
        public async Task<IActionResult> GetProductByBrandId([FromRoute] Guid id, [FromQuery] int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            ResponseBase<List<ProductViewModel>> responseViewModel = new ResponseBase<List<ProductViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetProductByBrandIdQuery(id, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow))
            };
            return Ok(responseViewModel);
        }
        [HttpGet("products/most-sale")]
        public async Task<IActionResult> GetProductMostSale(DateTime dateFrom, DateTime dateTo, int? pageIndex = null, int? pageSize = null)
        {
            ResponseBase<List<ProductViewModel>> responseViewModel = new ResponseBase<List<ProductViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetProductsMostSaleQuery(pageIndex, pageSize, dateFrom, dateTo))
            };
            return Ok(responseViewModel);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts(string? name = null, int? quantity = null, decimal? priceFrom = null, decimal? priceTo = null, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            ResponseBase<List<ProductViewModel>> responseViewModel = new ResponseBase<List<ProductViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = await _mediator.Send(new GetProductsQuery(name, quantity, priceFrom, priceTo, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow))
            };
            return Ok(responseViewModel);
        }


        [LookeachAuthorize(
          new string[] { ActionConstant.WRITE }
          )]
        [HttpPost("product/create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateModel productCreateModel)
        {
            var rs = await _mediator.Send(new CreateProductCommand(productCreateModel));
            ResponseBase<ProductViewModel> responseViewModel = new ResponseBase<ProductViewModel>();
            if (rs != null)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.Error = null!;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = rs;
                return StatusCode(StatusCodes.Status201Created, responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Something went wrong when create Product"
            };
            responseViewModel.IsSuccess = true;
            responseViewModel.Value = null!;
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }

        [LookeachAuthorize(
          new string[] { ActionConstant.OVERRIDE }
          )]
        [HttpPut("product/update")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateModel productUpdateModel)
        {
            var rs = await _mediator.Send(new UpdateProductCommand(productUpdateModel));
            ResponseBase<ProductViewModel> responseViewModel = new ResponseBase<ProductViewModel>();
            if (rs != null)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.Error = null!;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = rs;
                return Ok(responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Something went wrong when update Product"
            };
            responseViewModel.IsSuccess = true;
            responseViewModel.Value = null!;
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }

        [LookeachAuthorize(
          new string[] { ActionConstant.DELETE }
          )]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new DeleteProductCommand(id));
            ResponseBase<string> responseViewModel = new ResponseBase<string>();
            if (rs)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.Error = null!;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = rs + "";
                return Ok(responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Something went wrong when delete Product"
            };
            responseViewModel.IsSuccess = true;
            responseViewModel.Value = null!;
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }
    }
}
