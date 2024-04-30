using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface ICategoryImageRepository : IRepository<CategoryImage>
    {
        Task<List<CategoryImage>> GetCategoryImagesByCategoryId(Guid categoryId);
    }
}
