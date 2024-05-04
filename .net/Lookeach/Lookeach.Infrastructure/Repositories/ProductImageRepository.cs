using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
