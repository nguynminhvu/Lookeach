using Lookeach.Application.Commands.Products;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductViewModel>
    {
        private readonly IProductService _productService;

        public UpdateProductCommandHandler(IProductService productService)
        {
            _productService= productService;
        }
        public async Task<ProductViewModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var rs = await _productService.UpdateProduct(request.ProductUpdateModel);
            return rs != null ? rs : throw new Exception("Can't update this product");
        }
    }
}
