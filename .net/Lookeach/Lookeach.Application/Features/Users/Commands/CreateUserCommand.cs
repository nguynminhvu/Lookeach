using Lookeach.Application.AutoMappers;
using Lookeach.Application.DTOs;
using Lookeach.Application.Features.Users.Models;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Users;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Users.Commands
{
    public record CreateUserCommand(UserCreateModel UserCreateModel) : IRequest<UserViewModel>
    {
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserViewModel>
    {
        private readonly IAesService _aesService;
        private readonly IEmailService _emailService;
        private readonly ICleanContext _context;

        public CreateUserCommandHandler(ICleanContext cleanContext, IEmailService emailService, IAesService aesService)
        {
            _aesService= aesService;
            _emailService = emailService;
            _context = cleanContext;
        }

        public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.UserCreateModel.Username);
            if (user != null) throw new Exception("Username already used");
            user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.UserCreateModel.Email);
            if (user != null) throw new Exception("Email already used");
            var id = Guid.NewGuid();
            var userCreate = User
                .CreateUser(id, request.UserCreateModel.Username, request.UserCreateModel.Name, request.UserCreateModel.Dob, request.UserCreateModel.Address,
                request.UserCreateModel.Gender, request.UserCreateModel.Email, request.UserCreateModel.Phone, Convert.ToBase64String(_aesService.Encrypt(request.UserCreateModel.Password)) , request.UserCreateModel.RoleId, id, id);
            await _context.Users.AddAsync(userCreate);

          //  await _emailService.SendEmailConfirm(request.UserCreateModel.Email, "Lookeach confirm email", userCreate.Id);
            await _context.SaveChangesAsync(new CancellationToken());
            return AutoMapperConfig.Mapper.Map<UserViewModel>(userCreate);
        }
    }
}
