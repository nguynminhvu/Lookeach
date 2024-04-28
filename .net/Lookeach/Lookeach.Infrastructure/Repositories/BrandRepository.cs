using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly LookeachContext _context;
        public BrandRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Brand> GetBrandById(Guid id, string? includes = null)
        {
            IQueryable<Brand> query = _context.Brands;
            if (includes != null!)
            {
                foreach (var item in includes.Split(","))
                {
                    query = query.Include(item);
                }
            }
            return (await query.FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public async Task<List<Brand>> GetBrands(int? pageIndex = null, int? pageSize = null, string include = null!)
        {
            IQueryable<Brand> query = _context.Brands;
            if (include != null!)
            {
                foreach (var item in include.Split(","))
                {
                    query = query.Include(item);
                }
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                query = query.Skip(index * size).Take(size);
            }
            return await query.ToListAsync();
        }

        public async Task<List<Brand>> GetBrandsByBrandName(string name, int? pageIndex = null, int? pageSize = null, string? includes = null)
        {
            IQueryable<Brand> query = _context.Brands;
            if (includes != null!)
            {
                foreach (var item in includes.Split(","))
                {
                    query = query.Include(item);
                }
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                query = query.Skip(index * size).Take(size);
            }
            if (string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            return await query.ToListAsync();
        }
    }
}
