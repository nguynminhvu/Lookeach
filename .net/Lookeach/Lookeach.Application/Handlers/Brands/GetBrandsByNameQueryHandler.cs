using Lookeach.Application.Queries.Brands;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Handlers.Brands
{
    public class GetBrandsByNameQueryHandler : IRequestHandler<GetBrandsByNameQuery, List<BrandViewModel>>
    {
        private readonly IBrandService _brandService;

        public GetBrandsByNameQueryHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public async Task<List<BrandViewModel>> Handle(GetBrandsByNameQuery request, CancellationToken cancellationToken)
        {
            return await _brandService.GetBrands(request.pageIndex, request.pageSize, request.name);
        }
    }
}
