using Lookeach.Application.Queries.Products;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public record GetProductBySizeIdsQueryHandler : IRequestHandler<GetProductBySizeIdsQuery, List<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public GetProductBySizeIdsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<ProductViewModel>> Handle(GetProductBySizeIdsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductBySizeIds(request.id, request.pageIndex, request.pageSize, request.mostRecent, request.mostSale, request.lowToHigh, request.highToLow);
        }
    }
}
