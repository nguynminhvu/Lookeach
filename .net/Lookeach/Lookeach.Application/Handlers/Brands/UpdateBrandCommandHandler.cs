using Lookeach.Application.Commands.Brands;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Handlers.Brands
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, BrandViewModel>
    {
        private readonly IBrandService _brandService;

        public UpdateBrandCommandHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public async Task<BrandViewModel> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var rs = await _brandService.UpdateBrand(request.BrandUpdateModel);
            if (rs == null) throw new Exception("Can't update Brand, try again");
            return rs;
        }
    }
}
