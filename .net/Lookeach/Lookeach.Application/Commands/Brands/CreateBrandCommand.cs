using Lookeach.Application.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Commands.Brands
{
    public record CreateBrandCommand(BrandCreateModel BrandCreateModel) : IRequest<BrandViewModel>
    {
    }
}
