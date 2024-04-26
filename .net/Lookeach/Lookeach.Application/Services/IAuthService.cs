using Lookeach.Core.DTOs;

namespace Lookeach.Application.Services
{
    public interface IAuthService
    {
        Task<string> Login(LoginDTO loginDTO);
        Task<bool> Logout();
        Task<ClaimDTO> GetClaimDTOByAccessToken(string token);
        Task<bool> GetTokenValid(string token);

    }
}
