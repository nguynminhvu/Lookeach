using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.ViewModels.Colors;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        private readonly LookeachContext _context;
        public ColorRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Color> GetColorById(Guid id)
        => (await _context.Colors.FirstOrDefaultAsync(x => x.Id == id))!;

        public async Task<List<Color>> GetColorsByName(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Color> query = _context.Colors;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;

                query = query.Skip(index * size).Take(size);
            }
            return await query.ToListAsync();
        }

        public async Task<List<Color>> GetColorByIds(List<Guid> ids)
        {
            if (ids == null! || ids.Count == 0) return new List<Color>();
            return await _context.Colors.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
