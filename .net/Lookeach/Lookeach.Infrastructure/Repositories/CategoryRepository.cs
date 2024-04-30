using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryReposiitory
    {
        private readonly LookeachContext _context;
        public CategoryRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<Category>> GetCategoriesIncludeImage(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Category> query = _context.Categories;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int sz = pageSize.Value > 0 ? pageSize.Value : 10;
                query = query.Skip(index * sz).Take(sz);
            }
            if (query.Select(x => x.CategoryImages).Any(x => x != null!))
            {
                query = query.Include(x => x.CategoryImages)!.ThenInclude(x => x.Image).AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid id)
            => (await _context.Categories.FirstOrDefaultAsync(x => x.Id == id))!;

        public async Task<Category> GetCategoryByIdIncludeImage(Guid id)
        {
            if (_context.Categories.Select(x => x.CategoryImages).Any(x => x != null!))
            {
                return (await _context.Categories.Include(x => x.CategoryImages)!.ThenInclude(x => x.Image).FirstOrDefaultAsync(x => x.Id == id))!;
            }
            return await GetCategoryById(id);
        }
    }
}
