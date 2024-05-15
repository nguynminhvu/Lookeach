using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IVNPayTransactionRepository : IRepository<VNPayTransaction>
    {
        Task<List<VNPayTransaction>> GetVNPayTransactions(Guid? id = null!, DateTime? dateFrom = null!, DateTime? dateTo = null!, int? pageIndex = null!, int? pageSize = null!);
    }
}
