using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class GenderTypeRepository : Repository<GenderType>, IGenderTypeRepository
    {
        public GenderTypeRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
