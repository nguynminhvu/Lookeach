using Lookeach.Application.DTOs;

namespace Lookeach.Application.Services
{
    public interface IPermissionService
    {
        Task<List<PermissionDTO>> GetPermissionsCache(Guid roleId);
        Task<List<PermissionDTO>> GetPermissionsCache();
        Task SavePermissionCache();
        Task<bool> ValidPermission(PermissionDTO permissionDTO, string resourceAccess);
        Task InvalidPermissionCache();
    }
}
