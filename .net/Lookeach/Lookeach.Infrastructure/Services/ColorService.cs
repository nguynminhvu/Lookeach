using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Colors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Services
{
    public class ColorService : TokenService, IColorService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConfiguration _configuration;
        private readonly IColorRepository _colorRepository;

        public ColorService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(configuration, httpContextAccessor)
        {
            _uow = unitOfWork;
            _httpAccessor = httpContextAccessor;
            _configuration = configuration;
            _colorRepository = _uow.ColorRepository;
        }

        public async Task<ColorViewModel> CreateColor(string name)
        {
            var claim = await GetClaimDTOByAccessToken();

            if (claim == null!) throw new UnauthorizedAccessException("Login session has expired");

            var color = Color.Create(name, claim.Id);

            await _colorRepository.AddAsync(color);
            await _uow.SaveChangesAsync();

            return await GetColorById(color.Id);
        }


        public async Task<ColorViewModel> GetColorById(Guid id)
        => AutoMapperConfig.Mapper.Map<ColorViewModel>(await _colorRepository.GetColorById(id));

        public async Task<List<ColorViewModel>> GetColors(string? name = null, int? pageIndex = null, int? pageSize = null)
        => AutoMapperConfig.Mapper.Map<List<ColorViewModel>>(await _colorRepository.GetColorsByName(name, pageIndex, pageSize));

        public async Task<ColorViewModel> UpdateColor(ColorUpdateModel colorUpdateModel)
        {
            var claim = await GetClaimDTOByAccessToken();

            if (claim == null!) throw new UnauthorizedAccessException("Login session has expired");

            var color = await _colorRepository.GetColorById(colorUpdateModel.Id);

            if (color == null!) throw new ArgumentNullException("Not found Color");

            color.Update(colorUpdateModel.Name, claim.Id);

             _colorRepository.Update(color);
            await _uow.SaveChangesAsync();

            return await GetColorById(color.Id);
        }
    }
}
