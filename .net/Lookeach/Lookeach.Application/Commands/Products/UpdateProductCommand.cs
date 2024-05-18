using Lookeach.Application.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Commands.Products
{
    public record UpdateProductCommand(ProductUpdateModel ProductUpdateModel):IRequest<ProductViewModel>
    {
    }
}
