using Lookeach.Core.DTOs;
using Lookeach.Core.Entities;
using Lookeach.Core.ViewModels.Users;

namespace Lookeach.Application.Services
{
    public interface IAuthService
    {
        Task<string> Login(LoginDTO loginDTO);
        Task<bool> Logout();
        Task<ClaimDTO> GetClaimDTOByAccessToken(string token);
        Task<bool> GetTokenValid(string token);
        Task<User> CreateUser(UserCreateModel userCreateModel, Guid roleId);
        Task<string> RenewToken(string refreshToken);
    }
}
