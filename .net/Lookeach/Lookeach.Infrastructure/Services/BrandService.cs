using Lookeach.Application.AutoMappers;
using Lookeach.Application.DTOs;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Brands;
using Lookeach.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Services
{
    public class BrandService : TokenService, IBrandService
    {
        private readonly IUnitOfWork _uow;
        private readonly IBrandRepository _brandRepository;
        private readonly IBrandImageRepository _brandImageRepository;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly FirebaseDTO _fireBaseDTO;
        private readonly IImageRepository _imageRepository;
        private readonly IConfiguration _configuration;
        private const string BRAND = "Brand";
        public BrandService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache) : base(configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpAccessor = httpContextAccessor;
            _uow = unitOfWork;
            _brandRepository = _uow.BrandRepository;
            _brandImageRepository = _uow.BrandImageRepository;
            _configuration.GetSection(nameof(FirebaseDTO)).Bind(_fireBaseDTO);
            _imageRepository = _uow.ImageRepository;
        }

        public async Task<BrandViewModel> AddBrand(BrandCreateModel brandCreateModel)
        {
            if (brandCreateModel.Images != null)
            {
                string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };
                await brandCreateModel.Images.ValidImage(permittedExtensions);
            }
            var id = Guid.NewGuid();
            var brand = Brand.Create(id, brandCreateModel.Name);
            await _brandRepository.AddAsync(brand);

            var token = (_httpAccessor.HttpContext.Request.Headers["Authorization"].First() + "").Split(" ").Last();

            if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException("Your action not support, not found token");

            var claim = await GetClaimDTOByAccessToken(token);

            if (claim == null!) throw new UnauthorizedAccessException("Your action not support, not found token");

            List<string> subLink = new List<string>();
            try
            {
                if (brandCreateModel.Images != null && brandCreateModel.Images.Count > 0)
                {
                    foreach (var item in brandCreateModel.Images)
                    {
                        var url = await item.UploadToFirebase(BRAND, _fireBaseDTO);
                        if (url == null!) throw new Exception("Can't update Image");
                        subLink.Add(url);
                        var image = Image.Create(url!, claim.Id, claim.Id);
                        var brandImage = BrandImage.Create(brand.Id, image.Id);

                        await _imageRepository.AddAsync(image);
                        await _brandImageRepository.AddAsync(brandImage);
                    };
                }
                await _uow.SaveChangesAsync();
            }
            catch
            {
                foreach (var item in subLink)
                {
                    await item.DeleteImage(BRAND, _fireBaseDTO);
                }
            }

            return await GetBrand(brand.Id);
        }

        public async Task<BrandViewModel> GetBrand(Guid id)
          => AutoMapperConfig.Mapper.Map<BrandViewModel>(await _brandRepository.GetBrandByIdIncludeImage(id));

        public async Task<List<BrandViewModel>> GetBrands(int? pageIndex = null, int? pageSize = null, string? name = null)
          => AutoMapperConfig.Mapper.Map<List<BrandViewModel>>(await _brandRepository.GetBrandsIncludeImage(name, pageIndex, pageSize));

        public async Task<BrandViewModel> UpdateBrand(BrandUpdateModel brandUpdateModel)
        {
            List<string> subLink = new List<string>();
            try
            {
                var brand = await _brandRepository.GetBrandByIdIncludeImage(brandUpdateModel.Id);
                if (brand == null!) throw new ArgumentNullException(nameof(Brand));

                var tk = (_httpAccessor.HttpContext.Request.Headers["Authorization"].First() + "").Split(" ").Last();
                if (string.IsNullOrEmpty(tk)) throw new UnauthorizedAccessException("Your action not support, not found token");

                var claimDto = await GetClaimDTOByAccessToken(tk);

                if (claimDto == null!) throw new UnauthorizedAccessException("Your action not support, not found token");

                brand.Update(brandUpdateModel.Name, claimDto.Id);

                var brandImages = await _brandImageRepository.GetBrandImageByBrandId(brand.Id);

                var imageIds = brandImages.Select(x => x.ImageId).ToList();
                var images = await _imageRepository.GetImagesByListId(imageIds);
                _imageRepository.RemoveRange(images);
                _brandImageRepository.RemoveRange(brandImages);

                if (brand.BrandImages != null)
                {
                    brand.BrandImages.Clear();
                }
                _brandRepository.Update(brand);
                if (brandUpdateModel.Images != null! && brandUpdateModel.Images.Count >= 0)
                {
                    var token = (_httpAccessor.HttpContext.Request.Headers["Authorization"].First() + "").Split(" ").Last();

                    if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException("Your action not support, not found token");

                    var claim = await GetClaimDTOByAccessToken(token);

                    if (claim == null!) throw new UnauthorizedAccessException("Your action not support, not found token");

                    string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };

                    await brandUpdateModel.Images.ValidImage(permittedExtensions);

                    foreach (var item in brandUpdateModel.Images)
                    {
                        var url = await item.UploadToFirebase(BRAND, _fireBaseDTO);
                        if (url == null!) throw new Exception("Can't update Image");
                        var image = Image.Create(url!, claim.Id, claim.Id);
                        subLink.Add(url);
                        var brandImage = BrandImage.Create(brand.Id, image.Id);

                        await _imageRepository.AddAsync(image);
                        await _brandImageRepository.AddAsync(brandImage);
                    };
                }
                await _uow.SaveChangesAsync();
                return AutoMapperConfig.Mapper.Map<BrandViewModel>(brand);
            }
            catch (Exception ex)
            {
                foreach (var item in subLink)
                {
                    await item.DeleteImage(BRAND, _fireBaseDTO);
                }
                throw new Exception(ex.Message);
            }
        }
    }
}
