using Lookeach.Application.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Commands.Brands
{
    public record UpdateBrandCommand(BrandUpdateModel BrandUpdateModel):IRequest<BrandViewModel>
    {
    }
}
