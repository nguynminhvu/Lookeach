using Lookeach.Application.Queries.Products;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class GetProductByBrandIdQueryHandler : IRequestHandler<GetProductByBrandIdQuery, List<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public GetProductByBrandIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<ProductViewModel>> Handle(GetProductByBrandIdQuery request, CancellationToken cancellationToken)
        {
            if (request.id == Guid.NewGuid()) throw new ArgumentNullException("BrandId required");
            return await _productService.GetProductByBrandId(request.id, request.pageIndex, request.pageSize, request.mostRecent, request.mostSale, request.lowToHigh, request.highToLow);
        }
    }
}
