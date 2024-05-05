using Lookeach.Application.Queries.Products;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class GetProductByCategoryIdsQueryHandler : IRequestHandler<GetProductByCategoryIdsQuery, List<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public GetProductByCategoryIdsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<ProductViewModel>> Handle(GetProductByCategoryIdsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductByCategoryIds(request.id, request.pageIndex, request.pageSize, request.mostRecent, request.mostSale, request.lowToHigh, request.highToLow);
        }
    }
}
