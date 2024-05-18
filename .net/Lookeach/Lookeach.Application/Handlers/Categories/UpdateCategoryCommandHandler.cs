using Lookeach.Application.Commands.Categories;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Categories;
using MediatR;

namespace Lookeach.Application.Handlers.Categories
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryViewModel>
    {
        private readonly ICategoryService _categoryService;

        public UpdateCategoryCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<CategoryViewModel> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var rs = await _categoryService.UpdateCategory(request.CategoryUpdateModel);
            return rs != null ? rs : throw new Exception("Can't update category, try again");
        }
    }
}
