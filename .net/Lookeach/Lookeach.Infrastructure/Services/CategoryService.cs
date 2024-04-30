using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Categories;
using Lookeach.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Services
{
    public class CategoryService : TokenService, ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICategoryReposiitory _categoryRepository;
        private readonly ICategoryImageRepository _categoryImageRepository;
        private string[] _permittedExtensions = { ".png", ".jpg", ".jpeg" };
        private const string CATEGORY = "Category";
        private FirebaseDTO _fireBaseDTO = new FirebaseDTO();
        private readonly IConfiguration _configuration;
        private readonly IImageRepository _imageRepository;

        public CategoryService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IConfiguration configuration) : base(unitOfWork, configuration, httpContextAccessor, connectionMultiplexer, distributedCache)
        {
            _uow = unitOfWork;
            _categoryRepository = _uow.CategoryRepository;
            _categoryImageRepository = _uow.CategoryImageRepository;
            _configuration = configuration;
            _configuration.GetSection(nameof(FirebaseDTO)).Bind(_fireBaseDTO);
            _imageRepository = _uow.ImageRepository;
        }

        public async Task<CategoryViewModel> CreateCategory(CategoryCreateModel categoryCreateModel)
        {
            if (string.IsNullOrEmpty(categoryCreateModel.Name)) throw new ArgumentNullException(nameof(categoryCreateModel.Name));

            var claimDto = await GetClaimDTOByAccessToken();
            var id = Guid.NewGuid();
            var category = Category.Create(id, categoryCreateModel.Name, DateTime.Now, claimDto.Id, DateTime.Now, claimDto.Id);
            await _categoryRepository.AddAsync(category);
            List<string> subUrl = new List<string>();
            try
            {
                if (categoryCreateModel.Images != null && categoryCreateModel.Images.Count > 0)
                {
                    await categoryCreateModel.Images.ValidImage(_permittedExtensions);
                    foreach (var item in categoryCreateModel.Images)
                    {
                        var url = await item.UploadToFirebase(CATEGORY, _fireBaseDTO);
                        if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("Can't upload image");
                        subUrl.Add(url);
                        var image = Image.Create(url, claimDto.Id, claimDto.Id);
                        await _imageRepository.AddAsync(image);
                        var categoryImage = CategoryImage.Create(category.Id, image.Id);
                        await _categoryImageRepository.AddAsync(categoryImage);
                    }
                }
                await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                foreach (var item in subUrl)
                {
                    await item.DeleteImage(CATEGORY, _fireBaseDTO);
                }
                throw new Exception(ex.Message);
            }
            return await GetCategoryById(category.Id);
        }

        public async Task<List<CategoryViewModel>> GetCategories(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            return AutoMapperConfig.Mapper.Map<List<CategoryViewModel>>(await _categoryRepository.GetCategoriesIncludeImage(name, pageIndex, pageSize));
        }

        public async Task<CategoryViewModel> UpdateCategory(CategoryUpdateModel categoryUpdateModel)
        {
            var category = await _categoryRepository.GetCategoryByIdIncludeImage(categoryUpdateModel.Id);
            if (category == null!) throw new ArgumentNullException(nameof(category));
            var claims = await GetClaimDTOByAccessToken();
            category.Update(categoryUpdateModel.Name, claims.Id);
            _categoryRepository.Update(category);
            var categoryImages = await _categoryImageRepository.GetCategoryImagesByCategoryId(category.Id);
            var imageId = categoryImages.Select(x => x.ImageId).ToList();
            var images = await _imageRepository.GetImagesByListId(imageId);

            _imageRepository.RemoveRange(images);
            _categoryImageRepository.RemoveRange(categoryImages);
            List<string> subUrl = new List<string>();
            try
            {
                if (categoryUpdateModel.Images != null && categoryUpdateModel.Images.Count > 0)
                {
                    await categoryUpdateModel.Images.ValidImage(_permittedExtensions);

                    foreach (var item in categoryUpdateModel.Images)
                    {
                        var url = await item.UploadToFirebase(CATEGORY, _fireBaseDTO);
                        if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("Can't upload image, try again");
                        subUrl.Add(url);

                        var image = Image.Create(url, claims.Id, claims.Id);
                        await _imageRepository.AddAsync(image);

                        var categoryImage = CategoryImage.Create(category.Id, image.Id);
                        await _categoryImageRepository.AddAsync(categoryImage);
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (var item in subUrl)
                {
                    await item.DeleteImage(CATEGORY, _fireBaseDTO);
                }
                throw new Exception(ex.Message);
            }
            await _uow.SaveChangesAsync();

            return await GetCategoryById(category.Id);
        }

        public async Task<CategoryViewModel> GetCategoryById(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<CategoryViewModel>(await _categoryRepository.GetCategoryByIdIncludeImage(id));
        }
    }
}
