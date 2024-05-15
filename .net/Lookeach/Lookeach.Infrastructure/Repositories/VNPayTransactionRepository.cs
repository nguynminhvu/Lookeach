using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class VNPayTransactionRepository : Repository<VNPayTransaction>, IVNPayTransactionRepository
    {
        private readonly LookeachContext _context;
        public VNPayTransactionRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<VNPayTransaction>> GetVNPayTransactions(Guid? id = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<VNPayTransaction> query = _context.VNPayTransactions;

            if (id.HasValue)
            {
                query = query.Where(x => x.Id == id);
            }
            else
            {
                if (dateFrom.HasValue)
                {
                    query = query.Where(x => x.CreatedOn >= dateFrom);
                }
                if (dateTo.HasValue)
                {
                    query = query.Where(x => x.CreatedOn <= dateTo);
                }
                if (pageIndex.HasValue && pageSize.HasValue)
                {
                    int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                    int size = pageSize.Value > 0 ? pageSize.Value : 10;
                    query = query.Skip(index * size).Take(size);
                }
            }
            return await query.ToListAsync();
        }
    }
}
