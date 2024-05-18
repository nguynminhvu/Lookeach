using Lookeach.Application.Commands.Brands;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Handlers.Brands
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, BrandViewModel>
    {
        private readonly IBrandService _brandService;

        public CreateBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public async Task<BrandViewModel> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var rs = await _brandService.AddBrand(request.BrandCreateModel);
            if (rs == null!) throw new Exception("Can't create new brand, try again");

            return rs;
        }
    }
}
