using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IUserRoleRepository:IRepository<UserRole>
    {
        Task<List<UserRole>> GetRolesByUserId(Guid userId);
    }
}
