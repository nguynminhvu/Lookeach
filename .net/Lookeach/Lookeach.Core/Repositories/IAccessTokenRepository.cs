using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IAccessTokenRepository : IRepository<AccessToken>
    {
        Task<AccessToken> GetAccessTokenByUserId(Guid userId);
        Task<AccessToken> GetAccessTokenByToken(string token);
        Task RemoveTokenByUserId(Guid id);
    }
}
