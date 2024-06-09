using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class SuperAdminRepository : Repository<SuperAdmin>, ISuperAdminRepository
    {
        public SuperAdminRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
