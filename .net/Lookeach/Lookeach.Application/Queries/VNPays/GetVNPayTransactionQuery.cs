using Lookeach.Core.Entities;
using MediatR;

namespace Lookeach.Application.Queries.VNPays
{
    public record GetVNPayTransactionQuery(Guid? id = null!, DateTime? dateFrom = null!, DateTime? dateTo = null!, int? pageIndex = null!, int? pageSize = null!):IRequest<List<VNPayTransaction>>
    {
    }
}
