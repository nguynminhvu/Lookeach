using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetRefreshToken(string token);
        Task<bool> DeleteRefreshToken(string token);
        Task<RefreshToken> AddRefreshToken(string token, Guid userId);
    }
}
