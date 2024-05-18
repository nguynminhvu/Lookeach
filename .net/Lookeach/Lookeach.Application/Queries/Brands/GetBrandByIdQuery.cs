using Lookeach.Application.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Queries.Brands
{
    public record GetBrandByIdQuery(Guid id) : IRequest<BrandViewModel>
    {
    }
}
