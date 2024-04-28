using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly LookeachContext _context;
        public RoleRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Role> GetRoleById(Guid id)
            => (await _context.Roles.FirstOrDefaultAsync(x => x.Id == id))!;

        public async Task<List<Role>> GetRoles()
            => await GetMany().ToListAsync();
    }
}
