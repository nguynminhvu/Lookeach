using Lookeach.Core.ViewModels.Products;

namespace Lookeach.Application.Services
{
    public interface IProductService
    {
        Task<ProductViewModel> GetProduct(Guid id);
        Task<List<ProductViewModel>> GetProductBySizeIds(List<Guid> id, int? pageIndex = null, int? pageSize = null, bool? mostRecent=false, bool? mostSale=false, bool? lowToHigh=false, bool? highToLow = false);
        Task<List<ProductViewModel>> GetProductByCategoryIds(List<Guid> id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false);
        Task<List<ProductViewModel>> GetProductByColorsIds(List<Guid> id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false);
        Task<List<ProductViewModel>> GetProducts(string? name = null, int? quantity = null, decimal? priceFrom = null, decimal? priceTo = null, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false);
        Task<List<ProductViewModel>> GetProductByBrandId(Guid id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false);
        Task<List<ProductViewModel>> GetProductsMostSale(int? pageIndex = null, int? pageSize = null, DateTime? dateFrom = null!, DateTime? dateTo = null!);
    }
}
