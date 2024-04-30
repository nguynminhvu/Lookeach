using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<Brand> GetBrandByIdIncludeImage(Guid id, string? includes = null!);
        Task<List<Brand>> GetBrandsIncludeImage(string? name = null!, int? pageIndex = null, int? pageSize = null);
    }
}
