using Lookeach.Core.Entities;
using Lookeach.Core.Repositories.Base;

namespace Lookeach.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductById(Guid id, string? include = null!);
        Task<List<Product>> GetProducts(string? name = null!, int? quantity = null!, decimal? priceFrom = null!, decimal? priceTo = null!, int? pageIndex = null!, int? pageSize = null!, string? includes = null!);
        Task<List<Product>> GetProductByCategoryId(List<Guid> categoryIds, int? pageIndex = null!, int? pageSize = null!, string? includes=null!);
        Task<List<Product>> GetProductBtSizeId(List<Guid> sizes, int? pageIndex = null!, int? pageSize = null!, string? includes = null!);
        Task<List<Product>> GetProductByColorIds(List<Guid> colors, int? pageIndex = null!, int? pageSize = null!, string? includes = null!);
        Task<List<Product>> GetProductByBrandIds(List<Guid> brands, int? pageIndex = null!, int? pageSize = null!, string? includes = null!);
        Task<List<Product>> GetProductByBrandId(Guid id, int? pageIndex = null!, int? pageSize = null!, string? includes = null!);
        Task<List<Product>> GetProducts(DateTime? dateFrom = null!, DateTime? dateTo = null!, int? pageIndex = null, int? pageSize = null, string? includes = null);
    }
}
