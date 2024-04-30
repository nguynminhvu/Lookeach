using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class GenderTypeRepository : Repository<GenderType>, IGenderTypeRepository
    {
        private readonly LookeachContext _context;
        public GenderTypeRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<GenderType> GetGenderTypeById(Guid id)
        => (await _context.GenderTypes.FirstOrDefaultAsync(x => x.Id == id))!;

        public async Task<List<GenderType>> GetGenderTypes(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<GenderType> query = _context.GenderTypes;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int index = pageIndex.Value > 0 ? pageSize.Value - 1 : 0;
                int size = pageSize.Value > 0 ? pageSize.Value : 10;

                query = query.Skip(index * size).Take(size);
            }
            return await query.ToListAsync();
        }
    }
}
