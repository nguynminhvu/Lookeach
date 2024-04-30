using Lookeach.Application.Queries.Categories;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Categories;
using MediatR;

namespace Lookeach.Application.Handlers.Categories
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryViewModel>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryByIdQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<CategoryViewModel> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategoryById(request.id);
        }
    }
}
