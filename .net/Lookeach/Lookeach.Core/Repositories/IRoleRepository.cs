using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IRoleRepository:IRepository<Role>
    {
        Task<Role> GetRoleById(Guid id );
        Task<List<Role>> GetRoles(string? includes=null!);
    }
}
