using Lookeach.Application.Queries.VNPays;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using MediatR;

namespace Lookeach.Application.Handlers.VNPays
{
    public class GetVNPayTransactionQueryHandler : IRequestHandler<GetVNPayTransactionQuery, List<VNPayTransaction>>
    {
        private readonly IVNPayService _vnPayService;

        public GetVNPayTransactionQueryHandler(IVNPayService vNPayService)
        {
            _vnPayService = vNPayService;
        }
        public async Task<List<VNPayTransaction>> Handle(GetVNPayTransactionQuery request, CancellationToken cancellationToken)
        {
            return await _vnPayService.GetVNPayTransactions(request.id, request.dateFrom, request.dateTo, request.pageIndex, request.pageSize);
        }
    }
}
