using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IBrandImageRepository : IRepository<BrandImage>
    {
        Task<List<BrandImage>> GetBrandImageByBrandId(Guid id);
    }
}
