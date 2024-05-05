using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Commands.Products
{
    public record CreateProductCommand(ProductCreateModel ProductCreateModel):IRequest<ProductViewModel>
    {
    }
}
