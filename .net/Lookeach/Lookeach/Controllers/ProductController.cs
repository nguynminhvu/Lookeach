using Lookeach.Application.Queries.Products;
using Lookeach.Core.Entities;
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
    }
}
