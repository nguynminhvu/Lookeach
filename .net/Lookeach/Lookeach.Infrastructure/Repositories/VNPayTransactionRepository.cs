using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class VNPayTransactionRepository : Repository<VNPayTransaction>, IVNPayTransactionRepository
    {
        public VNPayTransactionRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
