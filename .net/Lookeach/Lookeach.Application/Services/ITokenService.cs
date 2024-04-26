using Lookeach.Core.Entities;

namespace Lookeach.Application.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
        string GenerateRefreshToken();
        Task<bool> CheckRefreshTokenExpire(string refreshToken);
        Task SaveAccessToken(string token, Guid userId);
        Task<string> GetAccessTokenFromRequest();
        Task<bool> DeleteAccessToken(string token);

    }
}
