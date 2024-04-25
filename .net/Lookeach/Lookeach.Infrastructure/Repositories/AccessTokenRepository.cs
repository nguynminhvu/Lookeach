using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class AccessTokenRepository : Repository<AccessToken>, IAccessTokenRepository
    {
        private readonly LookeachContext _context;
        public AccessTokenRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<AccessToken> GetAccessTokenByToken(string token)
             => (await _context.AccessTokens.FirstOrDefaultAsync(x => x.Token == token))!;

        public async Task<AccessToken> GetAccessTokenByUserId(Guid userId)
            => (await _context.AccessTokens.FirstOrDefaultAsync(x => x.UserId == userId))!;

        public async Task RemoveTokenByUserId(Guid id)
        {
            var token = await GetAccessTokenByUserId(id);
            if (token != null)
            {
                Remove(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}
