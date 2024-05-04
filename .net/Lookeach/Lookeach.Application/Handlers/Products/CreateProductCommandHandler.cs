using Lookeach.Application.Commands.Products;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
    {
        private readonly IProductService _productService;

        public CreateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ProductViewModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var rs = await _productService.CreateProduct(request.ProductCreateModel);
            return rs != null ? rs : throw new Exception("Can't create Product");
        }
    }
}
