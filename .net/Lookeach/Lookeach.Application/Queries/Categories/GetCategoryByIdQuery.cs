using Lookeach.Application.ViewModels.Categories;
using MediatR;

namespace Lookeach.Application.Queries.Categories
{
    public record GetCategoryByIdQuery(Guid id) : IRequest<CategoryViewModel>
    {
    }
}
