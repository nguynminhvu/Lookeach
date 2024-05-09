using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class ActionRepository : Repository<Lookeach.Core.Entities.Action>, IActionRepository
    {
        public ActionRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
