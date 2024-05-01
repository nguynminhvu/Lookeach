using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Lookeach.Infrastructure.Repositories
{
    public class AccessTokenRepository : Repository<AccessToken>, IAccessTokenRepository
    {
        private readonly LookeachContext _context;
        public AccessTokenRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<AccessToken> AddAccessToken(string token, Guid userId, string refreshToken)
        {
            var at = AccessToken.Create(userId, token, refreshToken);
            await _context.AccessTokens.AddAsync(at);
            await _context.SaveChangesAsync();
            return at;
        }

        public async Task<AccessToken> GetAccessTokenByRefreshToken(string refresh)
         => (await _context.AccessTokens.FirstOrDefaultAsync(x => x.RefreshToken == refresh))!;

        public async Task<AccessToken> GetAccessTokenByToken(string token)
             => (await _context.AccessTokens.FirstOrDefaultAsync(x => x.Token == token))!;

        public async Task<AccessToken> GetAccessTokenByUserId(Guid userId)
            => (await _context.AccessTokens.FirstOrDefaultAsync(x => x.UserId == userId))!;

        public async Task<bool> RemoveTokenByToken(string tk)
        {
            var token = await GetAccessTokenByToken(tk);
            if (token != null)
            {
                Remove(token);
                var rs = await _context.SaveChangesAsync();
                return rs > 0;
            }
            return false;
        }
        public async Task<bool> RemoveTokenByUserId(Guid id)
        {
            var token = await GetAccessTokenByUserId(id);
            if (token != null)
            {
                Remove(token);
                var rs = await _context.SaveChangesAsync();
                return rs > 0;
            }
            return false;
        }

        public async Task<AccessToken> UpdateTokenByRefreshToken(string token, string refreshToken, Guid modifiedBy)
        {
            var tk = await _context.AccessTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (tk==null) throw new Exception("Can't refresh token, token invalid");
            tk.Update(token, modifiedBy);
            Update(tk);
            await _context.SaveChangesAsync();
            return tk;
        }
    }
}
