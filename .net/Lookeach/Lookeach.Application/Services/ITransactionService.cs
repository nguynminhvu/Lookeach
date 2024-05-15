using Lookeach.Core.Entities;

namespace Lookeach.Application.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionMaster>> GetTransactions(Guid? orderId = null!, DateTime? dateFrom = null!, DateTime? dateTo = null!, int? pageIndex = null!, int? pageSize = null!);
    }
}
