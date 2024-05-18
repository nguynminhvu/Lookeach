using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.GenderTypes;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Services
{
    public class GenderTypeService : TokenService, IGenderTypeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGenderTypeRepository _genderTypeRepository;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConfiguration _configuration;
        public GenderTypeService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer) : base( configuration, httpContextAccessor)
        {
            _uow = unitOfWork;
            _httpAccessor = httpContextAccessor;
            _configuration = configuration;
            _genderTypeRepository = _uow.GenderTypeRepository;
        }
        public async Task<GenderTypeViewModel> CreateGenderType(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            var claim = await GetClaimDTOByAccessToken();

            var genderType = GenderType.Create(name, claim.Id, claim.Id);

            await _genderTypeRepository.AddAsync(genderType);
            await _uow.SaveChangesAsync();
            return await GetGenderType(genderType.Id);
        }

        public async Task<GenderTypeViewModel> GetGenderType(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<GenderTypeViewModel>(await _genderTypeRepository.GetGenderTypeById(id));
        }

        public async Task<List<GenderTypeViewModel>> GetGenderTypes(string? name = null, int? pageIndex = null!, int? pageSize = null!)
        {
            return AutoMapperConfig.Mapper.Map<List<GenderTypeViewModel>>(await _genderTypeRepository.GetGenderTypes(name,pageIndex,pageSize));
        }


        public async Task<GenderTypeViewModel> UpdateGenderType(GenderTypeUpdateModel genderTypeUpdateModel)
        {
            var genderType = await _genderTypeRepository.GetGenderTypeById(genderTypeUpdateModel.Id);
            if (genderType == null!) throw new ArgumentNullException(nameof(genderType));

            var claim = await GetClaimDTOByAccessToken();

            genderType.Update(genderTypeUpdateModel.Name, claim.Id);

            _genderTypeRepository.Update(genderType);
            await _uow.SaveChangesAsync();

            return await GetGenderType(genderType.Id);

        }
    }
}
