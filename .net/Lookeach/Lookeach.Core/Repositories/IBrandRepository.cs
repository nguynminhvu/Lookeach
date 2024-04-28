using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<Brand> GetBrandById(Guid id, string? includes = null!);
        Task<List<Brand>> GetBrandsByBrandName(string name, int? pageIndex = null, int? pageSize = null, string? includes = null);
        Task<List<Brand>> GetBrands(int? pageIndex = null, int? pageSize = null, string include = null!);
    }
}
