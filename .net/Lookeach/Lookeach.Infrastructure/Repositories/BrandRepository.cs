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


        public async Task<Brand> GetBrandByIdIncludeImage(Guid id, string? includes = null)
        {
            IQueryable<Brand> query = _context.Brands;
            if (query.Select(x => x.BrandImages).Any(x => x != null))
            {
                query = query.Include(x => x.BrandImages).ThenInclude(x => x.Image);
            }
            return (await query.FirstOrDefaultAsync(x => x.Id == id))!;
        }



        public async Task<List<Brand>> GetBrandsIncludeImage(string? name = null!, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Brand> query = _context.Brands;
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;
                query = query.Skip(index * size).Take(size);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if (query.Select(x => x.BrandImages).Any(x => x != null!))
            {
                query = query.Include(x => x.BrandImages)!.ThenInclude(x => x.Image);
            }
            return await query.ToListAsync();
        }
    }
}
