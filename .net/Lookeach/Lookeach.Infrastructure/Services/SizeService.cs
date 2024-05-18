using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Sizes;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Services
{
    public class SizeService : TokenService, ISizeService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly ISizeRepository _sizeRepository;

        public SizeService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _uow = unitOfWork;
            _httpAccessor = httpContextAccessor;
            _sizeRepository = _uow.SizeRepository;
        }
        public async Task<SizeViewModel> CreateSize(string name)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null) throw new UnauthorizedAccessException("Login session has expired");

            var size = Size.Create(name, claim.Id);

            await _sizeRepository.AddAsync(size);
            await _uow.SaveChangesAsync();
            return await GetSize(size.Id);
        }

        public async Task<SizeViewModel> GetSize(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<SizeViewModel>(await _sizeRepository.GetSizeById(id));
        }

        public async Task<List<SizeViewModel>> GetSizes(string? name = null, int? pageIndex = null, int? pageSize = null)
        {
            return AutoMapperConfig.Mapper.Map<List<SizeViewModel>>(await _sizeRepository.GetSizes(name, pageIndex, pageSize));
        }

        public async Task<SizeViewModel> UpdateSize(SizeUpdateModel sizeUpdateModel)
        {
            var claim = await GetClaimDTOByAccessToken();

            if (claim == null!) throw new UnauthorizedAccessException("Login session has expired");

            var size = await _sizeRepository.GetSizeById(sizeUpdateModel.Id);

            if (size == null!) throw new ArgumentNullException(nameof(size));

            size.UpdateSize(sizeUpdateModel.Name, claim.Id);

            _sizeRepository.Update(size);
            await _uow.SaveChangesAsync();

            return await GetSize(size.Id);
        }
    }
}
