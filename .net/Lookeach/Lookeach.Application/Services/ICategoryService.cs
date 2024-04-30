using Lookeach.Core.ViewModels.Categories;

namespace Lookeach.Application.Services
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> CreateCategory(CategoryCreateModel categoryCreateModel);
        Task<CategoryViewModel> UpdateCategory(CategoryUpdateModel categoryUpdateModel);
        Task<List<CategoryViewModel>> GetCategories(string? name = null!, int? pageIndex = null!, int? pageSize = null!);
        Task<CategoryViewModel> GetCategoryById(Guid id);
    }
}
