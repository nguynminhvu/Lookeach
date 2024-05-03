using Lookeach.Application.Queries.Products;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public GetProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<ProductViewModel>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProducts(request.name, request.quantity, request.priceFrom, request.priceTo, request.pageIndex, request.pageSize, request.mostRecent, request.mostSale, request.lowToHigh, request.highToLow);
        }
    }
}
