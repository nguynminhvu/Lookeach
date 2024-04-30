using Lookeach.Application.Queries.Categories;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Categories;
using MediatR;

namespace Lookeach.Application.Handlers.Categories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryViewModel>>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoriesQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<List<CategoryViewModel>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategories(request.name, request.pageIndex, request.pageSize);
        }
    }
}
