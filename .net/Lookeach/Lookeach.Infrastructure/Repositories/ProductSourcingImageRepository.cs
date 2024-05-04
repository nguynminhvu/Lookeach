using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class ProductSourcingImageRepository : Repository<ProductSourcingImage>, IProductSourcingImageRepository
    {
        public ProductSourcingImageRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
