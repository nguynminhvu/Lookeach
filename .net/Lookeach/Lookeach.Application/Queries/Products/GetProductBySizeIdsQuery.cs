using Lookeach.Application.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Queries.Products
{
    public record GetProductBySizeIdsQuery(List<Guid> id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false) : IRequest<List<ProductViewModel>>
    {
    }
}
