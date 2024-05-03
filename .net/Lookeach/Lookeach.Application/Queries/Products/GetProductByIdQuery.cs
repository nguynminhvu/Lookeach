using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Queries.Products
{
    public record GetProductByIdQuery(Guid id) : IRequest<ProductViewModel>
    {
    }
}
