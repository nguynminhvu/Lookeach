using Lookeach.Application.AutoMappers;
using Lookeach.Application.Features.Brands.Models;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Interfaces.Services;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Brands;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Brands.Commands
{
    public record CreateBrandCommand(BrandCreateModel BrandCreateModel) : IRequest<BrandViewModel>
    {
    }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, BrandViewModel>
    {
        private readonly IValidateService _validateService;
        private readonly ICleanContext _context;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IFirebaseService _firebaseService;
        private const string BRAND = "Brand";

        public CreateBrandCommandHandler(ICleanContext cleanContext, IIdentityUser<Guid> identityUser, IFirebaseService firebaseService, IValidateService validateService)
        {
            _validateService = validateService;
            _context = cleanContext;
            _identityUser = identityUser;
            _firebaseService = firebaseService;
        }

        public async Task<BrandViewModel> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var checkBrand = await _context.Brands.Where(x => x.Name.ToLower() == request.BrandCreateModel.Name.ToLower()).ToListAsync();
            if (checkBrand.Count > 0) throw new Exception("Brand name already exist");
            if (request.BrandCreateModel.Images != null)
            {
                string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };
                await _validateService.ValidImage(request.BrandCreateModel.Images, permittedExtensions);
            }
            var id = Guid.NewGuid();
            var brand = Brand.Create(id, request.BrandCreateModel.Name);
            await _context.Brands.AddAsync(brand);

            var userId = _identityUser.UserId;

            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("Login session has expired");
            }

            List<string> subLink = new List<string>();
            try
            {
                if (request.BrandCreateModel.Images != null && request.BrandCreateModel.Images.Count > 0)
                {
                    foreach (var item in request.BrandCreateModel.Images)
                    {
                        var url = await _firebaseService.UploadToFirebase(item, BRAND);
                        if (url == null!) throw new Exception("Can't update Image");
                        subLink.Add(url);
                        var image = Image.Create(url!, userId, userId);
                        var brandImage = BrandImage.Create(brand.Id, image.Id);

                        await _context.Images.AddAsync(image);
                        await _context.BrandImages.AddAsync(brandImage);
                    };
                }
                await _context.SaveChangesAsync(new CancellationToken());
            }
            catch
            {
                foreach (var item in subLink)
                {
                    await _firebaseService.DeleteImage(item, BRAND);
                }
            }
            var newBrand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == brand.Id);

            if (newBrand != null)
            {
                if (newBrand.BrandImages != null)
                {
                    newBrand = await _context.Brands
                        .Include(x => x.BrandImages!)
                        .ThenInclude(x => x.Image)
                        .FirstOrDefaultAsync(x => x.Id == brand.Id);
                }
            }
            if (newBrand == null) throw new Exception("Can't create new brand");
            return AutoMapperConfig.Mapper.Map<BrandViewModel>(newBrand);
        }
    }
}
