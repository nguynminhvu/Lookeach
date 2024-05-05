using Lookeach.Application.Queries.Products;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductViewModel>
    {
        private readonly IProductService _productService;

        public GetProductByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ProductViewModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProduct(request.id);
        }
    }
}
