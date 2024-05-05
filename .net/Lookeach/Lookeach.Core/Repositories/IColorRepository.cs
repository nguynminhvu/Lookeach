using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;
using Lookeach.Core.ViewModels.Colors;

namespace Lookeach.Core.Repositories
{
    public interface IColorRepository:IRepository<Color>
    {
        Task<Color> GetColorById(Guid id);
        Task<List<Color>> GetColorsByName(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<List<Color>> GetColorByIds(List<Guid> ids);
    }
}
