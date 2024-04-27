using Lookeach.Core.ViewModels.Roles;

namespace Lookeach.Application.Services
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetRoles();
        Task<RoleViewModel> GetRoleById(Guid id);

    }
}
