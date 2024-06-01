using Lookeach.Application.AutoMappers;
using Lookeach.Application.Features.Users.Models;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Users;
using Lookeach.Core.Entities;
using Lookeach.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Merchants.Commands
{
    public record CreateMerchantCommand(UserCreateModel UserCreateModel) : IRequest<UserViewModel>
    {
    }
    public class CreateMerchantCommandHandler : IRequestHandler<CreateMerchantCommand, UserViewModel>
    {
        private readonly IAesService _aesService;
        private readonly IEmailService _emailService;
        private readonly ICleanContext _context;

        public CreateMerchantCommandHandler(ICleanContext cleanContext, IEmailService emailService, IAesService aesService)
        {
            _aesService = aesService;
            _emailService = emailService;
            _context = cleanContext;
        }

        public async Task<UserViewModel> Handle(CreateMerchantCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.UserCreateModel.Username);
            if (user != null) throw new Exception("Username already used");
            user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.UserCreateModel.Email);
            if (user != null) throw new Exception("Email already used");
            var id = Guid.NewGuid();
            var userCreate = User
                .CreateUser(id, request.UserCreateModel.Username, request.UserCreateModel.Name, request.UserCreateModel.Dob, request.UserCreateModel.Address,
                request.UserCreateModel.Gender, request.UserCreateModel.Email, request.UserCreateModel.Phone, Convert.ToBase64String(_aesService.Encrypt(request.UserCreateModel.Password)), id, id);
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == RoleName.Merchant.ToString());
            if (role == null) throw new Exception("No role in system");
            List<Role> roles = new List<Role>();
            roles.Add(role);
            userCreate.Roles = roles;
            await _context.Users.AddAsync(userCreate);
            // **Uncomment when run in production
            // await _emailService.SendEmailConfirm(request.UserCreateModel.Email, "Lookeach confirm email", userCreate.Id);
            await _context.SaveChangesAsync(new CancellationToken());
            return AutoMapperConfig.Mapper.Map<UserViewModel>(userCreate);
        }
    }
}
