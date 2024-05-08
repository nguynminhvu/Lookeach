using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Core.DTOs;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Roles;

namespace Lookeach.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleRepository _roleRepository;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _roleRepository = _uow.RoleRepository;
        }
        public async Task<RoleViewModel> GetRoleById(Guid id)
         => AutoMapperConfig.Mapper.Map<RoleViewModel>(await _roleRepository.GetRoleById(id));

        public async Task<List<RoleDTO>> GetRoleResource(Guid id)
          => AutoMapperConfig.Mapper.Map<List<RoleDTO>>(await _roleRepository.GetRoles("Resources"));


        public async Task<List<RoleViewModel>> GetRoles()
        => AutoMapperConfig.Mapper.Map<List<RoleViewModel>>(await _roleRepository.GetRoles());

    }
}
