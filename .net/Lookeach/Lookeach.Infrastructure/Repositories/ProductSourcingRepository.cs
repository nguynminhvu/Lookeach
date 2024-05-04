using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class ProductSourcingRepository : Repository<ProductSourcing>, IProductSourcingRepository
    {
        private readonly LookeachContext _context;
        public ProductSourcingRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<int> GetLastVersionByProductId(Guid id)
        {
            var product = await _context.ProductSourcings.Where(x => x.ProductId == id).OrderByDescending(x => x.ModifiedOn).FirstOrDefaultAsync();
            if (product == null) throw new Exception("Can't store product");
            return product.Version;
        }
    }
}
