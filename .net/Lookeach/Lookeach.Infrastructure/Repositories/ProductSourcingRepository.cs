using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class ProductSourcingRepository : Repository<ProductSourcing>, IProductSourcingRepository
    {
        public ProductSourcingRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
