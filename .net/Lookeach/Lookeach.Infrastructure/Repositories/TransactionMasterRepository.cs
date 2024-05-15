using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class TransactionMasterRepository : Repository<TransactionMaster>, ITransactionMasterRepository
    {
        public TransactionMasterRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
