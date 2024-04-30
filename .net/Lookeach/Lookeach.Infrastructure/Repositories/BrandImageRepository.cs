using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class BrandImageRepository : Repository<BrandImage>, IBrandImageRepository
    {
        private readonly LookeachContext _context;
        public BrandImageRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<BrandImage>> GetBrandImageByBrandId(Guid id)
             => await _context.BrandImages.Where(x => x.BrandId == id).ToListAsync();
    }
}
