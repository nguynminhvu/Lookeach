using AutoMapper;
using Lookeach.Application.AutoMappers;
using Lookeach.Application.Commands.Auths;
using Lookeach.Application.Services;
using Lookeach.Core.ViewModels.Users;
using MediatR;

namespace Lookeach.Application.Handlers.Auths
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserViewModel>
    {
        private readonly IAesService _aesService;
        private readonly IAuthService _authService;
        private readonly IRoleService _roleService;

        public CreateUserCommandHandler(IAuthService authService, IRoleService roleService, IAesService aesService)
        {
            _aesService = aesService;
            _authService = authService;
            _roleService = roleService;
        }
        public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetRoleById(request.id);
            if (role == null)
            {
                throw new ArgumentNullException("Not found this role");
            }

            if (role.Name == "Admin")
            {
                throw new UnauthorizedAccessException("Your action not support");
            }

            request.UserCreateModel.Password = Convert.ToBase64String(_aesService.Encrypt(request.UserCreateModel.Password));
            var user = await _authService.CreateUser(request.UserCreateModel, role.Id);
            return user != null ? AutoMapperConfig.Mapper.Map<UserViewModel>(user) : null!;
        }
    }
}
