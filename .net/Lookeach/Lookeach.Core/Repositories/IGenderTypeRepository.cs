using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IGenderTypeRepository : IRepository<GenderType>
    {
        Task<List<GenderType>> GetGenderTypes(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<GenderType> GetGenderTypeById(Guid id);
    }
}
