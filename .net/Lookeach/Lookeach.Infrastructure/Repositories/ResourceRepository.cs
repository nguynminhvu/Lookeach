using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class ResourceRepository : Repository<Resource>, IResourceRepository
    {
        public ResourceRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
