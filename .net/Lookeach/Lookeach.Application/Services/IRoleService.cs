using Lookeach.Application.DTOs;
using Lookeach.Application.ViewModels.Roles;

namespace Lookeach.Application.Services
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetRoles();
        Task<RoleViewModel> GetRoleById(Guid id);
        Task<List<RoleDTO>> GetRoleResource(Guid id);

    }
}
