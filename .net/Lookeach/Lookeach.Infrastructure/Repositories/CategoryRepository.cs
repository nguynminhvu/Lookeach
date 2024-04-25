using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryReposiitory
    {
        public CategoryRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
