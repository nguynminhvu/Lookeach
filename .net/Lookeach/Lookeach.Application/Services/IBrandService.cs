using Lookeach.Application.Features.Brands.Models;
using Lookeach.Application.ViewModels.Brands;

namespace Lookeach.Application.Services
{
    public interface IBrandService
    {
        Task<BrandViewModel> GetBrand(Guid id);
        Task<List<BrandViewModel>> GetBrands(int? pageIndex = null!, int? pageSize = null!, string? name = null!);
        Task<BrandViewModel> UpdateBrand(BrandUpdateModel brandUpdateModel);
    }
}
