using Lookeach.Application.Commands.Products;
using Lookeach.Application.Services;
using MediatR;

namespace Lookeach.Application.Handlers.Products
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductService _productService;

        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService=productService;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var rs = await _productService.DeteleProduct(request.id);
            return rs ? true : throw new Exception("Can't delete product");
        }
    }
}
