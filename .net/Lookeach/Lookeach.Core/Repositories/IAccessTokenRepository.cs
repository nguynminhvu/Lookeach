using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IAccessTokenRepository : IRepository<AccessToken>
    {
        Task<AccessToken> GetAccessTokenByUserId(Guid userId);
        Task<AccessToken> GetAccessTokenByToken(string token);
        Task<AccessToken> GetAccessTokenByRefreshToken(string refresh);
        Task<bool> RemoveTokenByUserId(Guid id);
        Task<bool> RemoveTokenByToken(string token);
        Task<AccessToken> AddAccessToken(string token, Guid userId, string refreshToken);
        Task<AccessToken> UpdateTokenByRefreshToken(string token, string refreshToken);
    }
}
