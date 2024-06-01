using Lookeach.Application.DTOs;
using Lookeach.Application.Features.Users.Models;
using Lookeach.Application.ViewModels.Users;
using Lookeach.Core.Entities;

namespace Lookeach.Application.Services
{
    public interface IAuthService
    {
        Task<string> Login(LoginDTO loginDTO);
        Task<bool> Logout();
        Task<ClaimDTO> GetClaimDTOByAccessToken(string token);
        Task<bool> GetTokenValid(string token);
        Task<string> RenewToken(string refreshToken);
        Task<UserViewModel> ConfirmEmail(string tokenCode);
    }
}
