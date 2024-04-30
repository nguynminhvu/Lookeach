using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface ICategoryReposiitory : IRepository<Category>
    {
        Task<Category> GetCategoryByIdIncludeImage(Guid id);
        Task<Category> GetCategoryById(Guid id);
        Task<List<Category>> GetCategoriesIncludeImage(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
    }
}
