using Lookeach.Application.DTOs;

namespace Lookeach.Application.Services
{
    public interface IPermissionService
    {
        Task<PermissionDTO> GetPermission(Guid roleId);
        Task<bool> ValidPermission(PermissionDTO permissionDTO, string resourceAccess);
    }
}
