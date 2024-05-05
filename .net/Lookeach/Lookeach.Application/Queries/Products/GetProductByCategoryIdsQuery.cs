using Lookeach.Core.ViewModels.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Queries.Products
{
    public record GetProductByCategoryIdsQuery(List<Guid> id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false):IRequest<List<ProductViewModel>>
    {
    }
}
