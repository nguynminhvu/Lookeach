using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Services
{
    public class ProductService : TokenService, IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IProductRepository _productRepository;


        public ProductService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(configuration, httpContextAccessor)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            _productRepository = _uow.ProductRepository;
        }
        public async Task<ProductViewModel> GetProduct(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<ProductViewModel>(await _productRepository.GetProductById(id));
        }

        public async Task<List<ProductViewModel>> GetProductByBrandId(Guid id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(await _productRepository.GetProductByBrandId(id));
        }

        public async Task<List<ProductViewModel>> GetProductByCategoryIds(List<Guid> id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            var products = await _productRepository.GetProductByCategoryId(id, pageIndex, pageSize, null!);
            SortOption(ref products, mostRecent, mostSale, lowToHigh, highToLow);
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(products);
        }

        public async Task<List<ProductViewModel>> GetProductByColorsIds(List<Guid> id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            var products = await _productRepository.GetProductByColorIds(id, pageIndex, pageSize, null!);
            SortOption(ref products, mostRecent, mostSale, lowToHigh, highToLow);
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(products);
        }

        public async Task<List<ProductViewModel>> GetProductBySizeIds(List<Guid> id, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            var products = await _productRepository.GetProductBtSizeId(id, pageIndex, pageSize, null!);
            SortOption(ref products, mostRecent, mostSale, lowToHigh, highToLow);
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(products);
        }

        public async Task<List<ProductViewModel>> GetProducts(string? name = null, int? quantity = null, decimal? priceFrom = null, decimal? priceTo = null, int? pageIndex = null, int? pageSize = null, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            var products = await _productRepository.GetProducts(name, quantity, priceFrom, priceTo,pageIndex,pageSize,null!);
            SortOption(ref products, mostRecent, mostSale, lowToHigh, highToLow);
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(products);
        }

        public async Task<List<ProductViewModel>> GetProductsMostSale(int? pageIndex = null, int? pageSize = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var products = await _productRepository.GetProducts(dateFrom, dateTo, pageIndex, pageSize, null!);
            SortOption(ref products, false, true, false, false);
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(products);
        }

        private void SortOption(ref List<Core.Entities.Product> products, bool? mostRecent = false, bool? mostSale = false, bool? lowToHigh = false, bool? highToLow = false)
        {
            if (mostRecent != null && mostRecent is true)
            {
                products = products.OrderByDescending(x => x.CreatedOn).ToList();
            }
            else if (highToLow != null && highToLow is true)
            {
                products = products.OrderByDescending(x => x.Price).ToList();
            }
            else if (lowToHigh != null && lowToHigh is true)
            {
                products = products.OrderBy(x => x.Price).ToList();
            }
            else
            {
                products = products.OrderByDescending(x => x.Sold).ToList();
            }
        }
    }
}
