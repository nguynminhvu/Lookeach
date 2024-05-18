using Lookeach.Application.ViewModels.Categories;
using MediatR;

namespace Lookeach.Application.Commands.Categories
{
    public record UpdateCategoryCommand(CategoryUpdateModel CategoryUpdateModel):IRequest<CategoryViewModel>
    {
    }
}
