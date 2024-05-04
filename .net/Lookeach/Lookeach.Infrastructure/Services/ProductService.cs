using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Products;
using Lookeach.Infrastructure.Helpers;
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
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICategoryReposiitory _categoryRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductSourcingRepository _productSourcingRepository;
        private readonly IProductSourcingImageRepository _productSourcingImageRepository;
        private string[] _permittedExtensions = { ".png", ".jpg", ".jpeg" };
        private const string PRODUCT = "Product";
        private FirebaseDTO _fireBaseDTO = new FirebaseDTO();

        public ProductService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(configuration, httpContextAccessor)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            _productRepository = _uow.ProductRepository;
            _sizeRepository = _uow.SizeRepository;
            _colorRepository = _uow.ColorRepository;
            _categoryRepository = _uow.CategoryRepository;
            _configuration.GetSection(nameof(FirebaseDTO)).Bind(_fireBaseDTO);
            _imageRepository = _uow.ImageRepository;
            _productImageRepository = _uow.ProductImageRepository;
            _productSourcingRepository = _uow.ProductSourcingRepository;
            _productSourcingImageRepository = _uow.ProductSourcingImageRepository;
        }

        public async Task<ProductViewModel> CreateProduct(ProductCreateModel productCreateModel)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Login session expired");

            if (productCreateModel.Images == null! || productCreateModel.Images.Count == 0)
                throw new ArgumentNullException("Product image required at least 1");

            if (productCreateModel.Colors == null! || productCreateModel.Colors.Count == 0)
                throw new ArgumentNullException("Product color required at least 1");

            if (productCreateModel.Categories == null! || productCreateModel.Categories.Count == 0)
                throw new ArgumentNullException("Product category required at least 1");

            if (productCreateModel.Sizes == null! || productCreateModel.Sizes.Count == 0)
                throw new ArgumentNullException("Product size required at least 1");

            var sizes = await _sizeRepository.GetSizeByIds(productCreateModel.Sizes.ToList());
            if (sizes.Count == 0) throw new Exception("Size invalid, try again");

            var colors = await _colorRepository.GetColorByIds(productCreateModel.Colors.ToList());
            if (colors.Count == 0) throw new Exception("Color invalid, try again");

            var categories = await _categoryRepository.GetCategoriesByIds(productCreateModel.Categories.ToList());
            if (categories.Count == 0) throw new Exception("Category invalid, try again");

            var product = Product.CreateProduct(productCreateModel.Name, productCreateModel.Quantity, productCreateModel.Price, productCreateModel.Description, sizes, colors, categories, productCreateModel.BrandId, claim.Id);

            await _productRepository.AddAsync(product);

            var productSourcing = ProductSourcing.CreateProductSourcing(productCreateModel.Name, productCreateModel.Quantity, productCreateModel.Price, productCreateModel.Description, sizes, colors, categories, productCreateModel.BrandId, claim.Id, 0, product.Id);

            await _productSourcingRepository.AddAsync(productSourcing);

            await productCreateModel.Images.ToList().ValidImage(_permittedExtensions);
            List<string> subUrl = new List<string>();
            try
            {
                foreach (var item in productCreateModel.Images)
                {
                    var url = await item.UploadToFirebase(PRODUCT, _fireBaseDTO);
                    if (string.IsNullOrEmpty(url)) throw new ArgumentNullException();
                    subUrl.Add(url);
                    var image = Image.Create(url, claim.Id, claim.Id);
                    await _imageRepository.AddAsync(image);
                    var productImage = ProductImage.Create(product.Id, image.Id);
                    await _productImageRepository.AddAsync(productImage);
                    var productSourcingImage = ProductSourcingImage.Create(product.Id, image.Id);
                    await _productSourcingImageRepository.AddAsync(productSourcingImage);
                }
                await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                foreach (var item in subUrl)
                {
                    await item.DeleteImage(PRODUCT, _fireBaseDTO);
                }
                throw new Exception(ex.Message);
            }
            return await GetProduct(product.Id);
        }

        public async Task<ProductViewModel> GetProduct(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<ProductViewModel>(await _productRepository.GetProductById(id, null!));
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
            var products = await _productRepository.GetProducts(name, quantity, priceFrom, priceTo, pageIndex, pageSize, null!);
            SortOption(ref products, mostRecent, mostSale, lowToHigh, highToLow);
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(products);
        }

        public async Task<List<ProductViewModel>> GetProductsMostSale(int? pageIndex = null, int? pageSize = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var products = await _productRepository.GetProducts(dateFrom, dateTo, pageIndex, pageSize, null!);
            SortOption(ref products, false, true, false, false);
            return AutoMapperConfig.Mapper.Map<List<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> UpdateProduct(ProductUpdateModel productUpdateModel)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Login session expired");

            var product = await _productRepository.GetProductById(productUpdateModel.Id);
            if (product == null) throw new ArgumentNullException("Not found product");

            if (productUpdateModel.Images == null! || productUpdateModel.Images.Count == 0) throw new Exception("Product image required at least 1");

            productUpdateModel.Sizes ??= new List<Guid>();
            productUpdateModel.Categories ??= new List<Guid>();
            productUpdateModel.Colors ??= new List<Guid>();

            var sizes = await _sizeRepository.GetSizeByIds(productUpdateModel.Sizes!.ToList());

            var colors = await _colorRepository.GetColorByIds(productUpdateModel.Colors.ToList());

            var categories = await _categoryRepository.GetCategoriesByIds(productUpdateModel.Categories.ToList());
            if (categories.Count == 0) throw new Exception("Category invalid, try again");

            product.UpdateProduct(productUpdateModel.Name, productUpdateModel.Quantity, productUpdateModel.Price, productUpdateModel.Description, sizes, colors, categories, productUpdateModel.BrandId, claim.Id);
            int version = await _productSourcingRepository.GetLastVersionByProductId(productUpdateModel.Id);

            var productSourcing = ProductSourcing.CreateProductSourcing(productUpdateModel.Name, productUpdateModel.Quantity, productUpdateModel.Price, productUpdateModel.Description, sizes, colors, categories, productUpdateModel.BrandId, claim.Id, version++, product.Id);
           
            _productRepository.Update(product);
            await _productSourcingRepository.AddAsync(productSourcing);
            if (productUpdateModel.Images.Count > 0)
            {
                await productUpdateModel.Images.ToList().ValidImage(_permittedExtensions);
            }
            List<string> subUrl = new List<string>();
            try
            {
                foreach (var item in productUpdateModel.Images)
                {
                    var url = await item.UploadToFirebase(PRODUCT, _fireBaseDTO);
                    if (string.IsNullOrEmpty(url)) throw new ArgumentNullException();
                    subUrl.Add(url);
                    var image = Image.Create(url, claim.Id, claim.Id);
                    await _imageRepository.AddAsync(image);
                    var productImage = ProductImage.Create(product.Id, image.Id);
                    await _productImageRepository.AddAsync(productImage);
                    var productSourcingImage = ProductSourcingImage.Create(product.Id, image.Id);
                    await _productSourcingImageRepository.AddAsync(productSourcingImage);
                }
                await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                foreach (var item in subUrl)
                {
                    await item.DeleteImage(PRODUCT, _fireBaseDTO);
                }
                throw new Exception(ex.Message);
            }
            return await GetProduct(product.Id);
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

        public async Task<bool> DeteleProduct(Guid id)
        {
            var claim = await GetClaimDTOByAccessToken();
            var product = await _productRepository.GetProductById(id);
            if (product == null!) throw new ArgumentNullException("Not found product");
            product.DeleteProduct(false, claim.Id);
            _productRepository.Update(product);
            var rs = await _uow.SaveChangesAsync();
            return rs > 0;
        }
    }
}
