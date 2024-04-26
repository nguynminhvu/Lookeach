using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly LookeachContext _context;
        public RefreshTokenRepository(LookeachContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<RefreshToken> AddRefreshToken(string token, Guid userId)
        {
            var rt = RefreshToken.Create(userId, token);
            await _context.AddAsync(rt);
            await _context.SaveChangesAsync();
            return rt;
        }

        public async Task<bool> DeleteRefreshToken(string token)
        {
            var rt = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            if (rt != null)
            {
                Remove(rt);
                var rs = await _context.SaveChangesAsync();
                return rs > 0;
            }
            return false;
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
          => (await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token))!;
    }
}
