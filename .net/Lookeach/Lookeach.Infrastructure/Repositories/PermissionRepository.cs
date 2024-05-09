using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly LookeachContext _context;
        public PermissionRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<Permission>> GetPermissions()
        {
           return await _context.Permissions.Include(x=>x.Role).Include(x=>x.Resource).AsNoTracking().ToListAsync();
        }
    }
}
