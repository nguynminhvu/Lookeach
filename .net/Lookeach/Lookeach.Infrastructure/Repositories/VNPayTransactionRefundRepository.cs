using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class VNPayTransactionRefundRepository : Repository<VNPayTransactionRefund>, IVNPayTransactionRefundRepository
    {
        public VNPayTransactionRefundRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
