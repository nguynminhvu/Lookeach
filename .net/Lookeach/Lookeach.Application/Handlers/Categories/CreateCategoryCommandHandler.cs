using Lookeach.Application.Commands.Categories;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Categories;
using MediatR;

namespace Lookeach.Application.Handlers.Categories
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryViewModel>
    {
        private readonly ICategoryService _categoryService;

        public CreateCategoryCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<CategoryViewModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var rs = await _categoryService.CreateCategory(request.CategoryCreateModel);
            if (rs != null)
            {
                return rs;
            }
            throw new Exception("Can't create category, try again");
        }
    }
}
