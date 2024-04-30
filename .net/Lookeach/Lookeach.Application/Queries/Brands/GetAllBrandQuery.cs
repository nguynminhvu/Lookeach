using Lookeach.Core.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Queries.Brands
{
    public record GetAllBrandQuery(int? pageIndex = null!, int? pageSize = null!) : IRequest<List<BrandViewModel>>
    {
    }
}
