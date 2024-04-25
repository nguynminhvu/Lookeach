using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;

namespace Lookeach.Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>,IRefreshTokenRepository
    {
        public RefreshTokenRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
