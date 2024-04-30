using Lookeach.Core.ViewModels.Categories;
using MediatR;

namespace Lookeach.Application.Queries.Categories
{
    public record GetCategoriesQuery(string? name = null!, int? pageIndex = null!, int? pageSize = null!) : IRequest<List<CategoryViewModel>>
    {
    }
}
