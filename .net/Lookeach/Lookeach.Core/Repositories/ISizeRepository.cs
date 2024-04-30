using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface ISizeRepository : IRepository<Size>
    {
        Task<List<Size>> GetSizes(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<Size> GetSizeById(Guid id);
    }
}
