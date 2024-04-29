using Lookeach.Application.Queries.Brands;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Brands;
using MediatR;

namespace Lookeach.Application.Handlers.Brands
{
    public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandQuery, List<BrandViewModel>>
    {
        private readonly IBrandService _brandService;

        public GetAllBrandQueryHandler(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<List<BrandViewModel>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
        {
            return await _brandService.GetBrands(pageIndex: request.pageIndex, pageSize: request.pageSize);
        }
    }
}
