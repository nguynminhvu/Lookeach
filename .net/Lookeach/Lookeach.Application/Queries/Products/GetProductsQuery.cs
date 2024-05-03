using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Queries.Products
{
    public record GetProductsQuery(string? name = null, int? quantity = null, decimal? priceFrom = null, decimal? priceTo = null, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false) :IRequest<List<ProductViewModel>>
    {
    }
}
