using Lookeach.Core.ViewModels.Products;
using MediatR;

namespace Lookeach.Application.Queries.Products
{
    public record GetProductsMostSaleQuery(int? pageIndex = null, int? pageSize = null, DateTime? dateFrom = null, DateTime? dateTo = null):IRequest<List<ProductViewModel>>
    {
    }
}
