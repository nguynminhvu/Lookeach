using Lookeach.Application.Queries.Products;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class GetProductByColorsIdsQueryHandler : IRequestHandler<GetProductByColorsIdsQuery, List<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public GetProductByColorsIdsQueryHandler(IProductService productService)
        {
            _productService= productService;
        }
        public async Task<List<ProductViewModel>> Handle(GetProductByColorsIdsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductByColorsIds(request.id, request.pageIndex, request.pageSize, request.mostRecent, request.mostSale, request.lowToHigh, request.highToLow);
        }
    }
}
