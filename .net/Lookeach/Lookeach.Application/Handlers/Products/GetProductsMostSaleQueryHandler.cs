using Lookeach.Application.Queries.Products;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class GetProductsMostSaleQueryHandler : IRequestHandler<GetProductsMostSaleQuery, List<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public GetProductsMostSaleQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<List<ProductViewModel>> Handle(GetProductsMostSaleQuery request, CancellationToken cancellationToken)
        {
            if (request.dateFrom == null! || request.dateTo == null!) throw new ArgumentNullException("Date required");
            return await _productService.GetProductsMostSale(request.pageIndex, request.pageSize, request.dateFrom, request.dateTo);
        }
    }
}
