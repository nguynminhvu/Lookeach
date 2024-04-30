using Lookeach.Application.Queries.Brands;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Handlers.Brands
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandViewModel>
    {
        private readonly IBrandService _brandService;

        public GetBrandByIdQueryHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public async Task<BrandViewModel> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            return await _brandService.GetBrand(request.id);
        }
    }
}
