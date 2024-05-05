using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        private readonly LookeachContext _context;
        public SizeRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Size> GetSizeById(Guid id)
         => (await _context.Sizes.FirstOrDefaultAsync(x => x.Id == id))!;

        public async Task<List<Size>> GetSizes(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Size> query = _context.Sizes;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageIndex.Value : 10;
                query = query.Skip(index * size).Take(size);
            }
            return await query.ToListAsync();
        }

        public async Task<List<Size>> GetSizeByIds(List<Guid> sizedIds)
        {
            if (sizedIds == null! || sizedIds.Count == 0) return new List<Size>();
            return await _context.Sizes.Where(x => sizedIds.Contains(x.Id)).ToListAsync();
        }
    }
}
