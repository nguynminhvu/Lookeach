using Lookeach.Application.Commands.Products;
using Lookeach.Application.Queries.Products;
using Lookeach.Core.Entities;
using Lookeach.Core.ViewModels.Products;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            return Ok(await _mediator.Send(new GetProductByIdQuery(id)));
        }

        [HttpGet("products/by-categories")]
        public async Task<IActionResult> GetProductByCategoryId([FromQuery] List<Guid> categoryIds, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            return Ok(await _mediator.Send(new GetProductByCategoryIdsQuery(categoryIds, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow)));
        }

        [HttpGet("products/by-colors")]
        public async Task<IActionResult> GetProductByColorId([FromQuery] List<Guid> colorIds, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            return Ok(await _mediator.Send(new GetProductByColorsIdsQuery(colorIds, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow)));
        }

        [HttpGet("products/by-sizes")]
        public async Task<IActionResult> GetProductBySizesId([FromQuery] List<Guid> sizeIds, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            return Ok(await _mediator.Send(new GetProductBySizeIdsQuery(sizeIds, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow)));
        }

        [HttpGet("products/{id}/by-brand")]
        public async Task<IActionResult> GetProductByBrandId([FromRoute] Guid brandId, [FromQuery] int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            return Ok(await _mediator.Send(new GetProductByBrandIdQuery(brandId, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow)));
        }
        [HttpGet("products/most-sale")]
        public async Task<IActionResult> GetProductMostSale(DateTime dateFrom, DateTime dateTo, int? pageIndex = null, int? pageSize = null)
        {
            return Ok(await _mediator.Send(new GetProductsMostSaleQuery(pageIndex, pageSize, dateFrom, dateTo)));
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts(string? name = null, int? quantity = null, decimal? priceFrom = null, decimal? priceTo = null, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            return Ok(await _mediator.Send(new GetProductsQuery(name, quantity, priceFrom, priceTo, pageIndex, pageSize, mostRecent, mostSale, lowToHigh, highToLow)));
        }

        [HttpPost("product/create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateModel productCreateModel)
        {
            var rs = await _mediator.Send(new CreateProductCommand(productCreateModel));
            ResponseViewModel responseViewModel = new ResponseViewModel();
            if (rs != null)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.Error = null!;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
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


        [HttpPut("product/update")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateModel productUpdateModel)
        {
            var rs = await _mediator.Send(new UpdateProductCommand(productUpdateModel));
            ResponseViewModel responseViewModel = new ResponseViewModel();
            if (rs != null)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.Error = null!;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new DeleteProductCommand(id));
            ResponseViewModel responseViewModel = new ResponseViewModel();
            if (rs)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.Error = null!;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
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
