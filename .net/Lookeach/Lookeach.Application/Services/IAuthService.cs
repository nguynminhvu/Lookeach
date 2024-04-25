using Lookeach.Core.DTOs;

namespace Lookeach.Application.Services
{
    public interface IAuthService
    {
        Task<string> Login(LoginDTO loginDTO);
        Task<ClaimDTO> GetClaimDTOByAccessToken(string token);
        Task<bool> GetTokenValid(string token);

    }
}
