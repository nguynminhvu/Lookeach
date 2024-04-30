using Lookeach.Core.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Queries.Brands
{
    public record GetBrandByIdQuery(Guid id) : IRequest<BrandViewModel>
    {
    }
}
