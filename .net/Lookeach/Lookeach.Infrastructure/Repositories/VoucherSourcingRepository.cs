using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class VoucherSourcingRepository : Repository<VoucherSourcing>, IVoucherSourcingRepository
    {
        public VoucherSourcingRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
