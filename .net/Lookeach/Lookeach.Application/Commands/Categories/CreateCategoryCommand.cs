using Lookeach.Core.ViewModels.Categories;
using MediatR;

namespace Lookeach.Application.Commands.Categories
{
    public record CreateCategoryCommand(CategoryCreateModel CategoryCreateModel) : IRequest<CategoryViewModel>
    {
    }
}
