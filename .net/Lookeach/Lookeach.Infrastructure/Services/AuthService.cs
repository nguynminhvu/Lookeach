using Lookeach.Application.AutoMappers;
using Lookeach.Application.DTOs;
using Lookeach.Application.Features.Users.Models;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Users;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MimeKit;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Services
{
    public class AuthService : TokenService, IAuthService
    {
        private readonly IConnectionMultiplexer _connectionMutiple;
        private readonly IDistributedCache _distributedCache;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IAesService _aesService;
        private readonly IHttpContextAccessor _httpAccessor;
        private JwtDTO _jwtDTO = new();
        private RedisDTO _redisDTO = new();
        private EmailSenderDTO _emailSenderDTO = new();
        private EmailDTO _emailDTO = new();

        public AuthService(IUnitOfWork unitOfWork, IAesService aesService, IConfiguration configuration, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, configuration, httpContextAccessor, connectionMultiplexer, distributedCache)
        {
            _httpAccessor = httpContextAccessor;
            _connectionMutiple = connectionMultiplexer;
            _distributedCache = distributedCache;
            _uow = unitOfWork;
            _userRepository = _uow.UserRepository;
            _configuration = configuration;
            _aesService = aesService;
            _configuration.GetSection(nameof(JwtDTO)).Bind(_jwtDTO);
            _configuration.GetSection(nameof(RedisDTO)).Bind(_redisDTO);
            _configuration.GetSection(nameof(EmailSenderDTO)).Bind(_emailSenderDTO);
            _configuration.GetSection(nameof(EmailDTO)).Bind(_emailDTO);
        }

        public async Task<UserViewModel> ConfirmEmail(string tokenCode)
        {
            if (string.IsNullOrEmpty(tokenCode)) throw new ArgumentNullException("Token code invalid, something change");
            var userId = Guid.Parse(_aesService.Decrypt(Convert.FromBase64String(tokenCode)));
            var user = await _userRepository.GetUserById(userId);
            if (user == null!) throw new ArgumentNullException("Not found user account");
            if (user.EmailConfirm && user.IsActived) throw new Exception("Account already confirm email");
            user.IsActived = true;
            user.EmailConfirm = true;
            await _userRepository.UpdateUser(user);
            return AutoMapperConfig.Mapper.Map<UserViewModel>(user);
        }

        public async Task<User> CreateUser(UserCreateModel userCreateModel, Guid roleId)
        {
            var user = await _userRepository.GetUserByUsername(userCreateModel.Username);
            if (user != null) throw new Exception("Username already used");
            user = await _userRepository.GetUserByEmail(userCreateModel.Email);
            if (user != null) throw new Exception("Email already used");

            var id = Guid.NewGuid();
            var userCreate = User
                .CreateUser(id, userCreateModel.Username, userCreateModel.Name, userCreateModel.Dob, userCreateModel.Address,
                userCreateModel.Gender, userCreateModel.Email, userCreateModel.Phone, userCreateModel.Password, roleId, id, id);

            await _userRepository.AddAsync(userCreate);

            var tokenCode = _emailSenderDTO.Domain + _emailSenderDTO.Uri + "/" + Convert.ToBase64String(_aesService.Encrypt(userCreate.Id + ""));
            var contentHtml = "<html><body><h1>Please click the link below to confirm your email:</h1>"
                                                          + $"<p><a href=\"{tokenCode}\">Confirm Password</a></p>" +
                                                          $"<br/>" +
                                                          $"<img src=\"https://media.cnn.com/api/v1/images/stellar/prod/220310092648-benzema-tease.jpg?q=h_2000,w_3000,x_0,y_0\" ></body></html>";
          //  await SendEmail(userCreate.Email, "Lookeach confirm email", contentHtml);
            await _uow.SaveChangesAsync();
            return userCreate;
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByUsernameAndPassword(loginDTO.Username, Convert.ToBase64String(_aesService.Encrypt(loginDTO.Password)), "Role");
          
            if (user == null)
                throw new Exception("Wrong username or password");
            var tk = await GenerateToken(user);
            if (tk != null)
            {
                await SaveAccessToken(tk, user.Id);
                return tk;
            }
            throw new Exception("Can't create token");
        }

        public async Task<bool> Logout()
        {
            var tokenCt = _httpAccessor.HttpContext.Request.Headers["Authorization"].First()!.Split(" ").Last();
            return await DeleteAccessToken(tokenCt);
        }

        public async Task<string> RenewToken(string refreshToken)
        {
            var rs = await RenewAccessToken(refreshToken);
            if (rs == null) throw new Exception("Can't renew token");
            return rs;
        }

        protected async Task SendEmail(string userEmail, string subject, string contentHtml)
        {
            try
            {
                var builder = new BodyBuilder();

                builder.HtmlBody = contentHtml;

                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_emailDTO.NameApp, _emailDTO.EmailAddress));
                mail.Sender = new MailboxAddress(_emailDTO.NameApp, _emailDTO.EmailAddress);

                mail.To.Add(MailboxAddress.Parse(userEmail));

                mail.Body = builder.ToMessageBody();
                mail.Subject = subject;
                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                if (_emailDTO.UseSSL)
                {
                    await smtp.ConnectAsync(_emailDTO.Host, _emailDTO.Port, SecureSocketOptions.SslOnConnect);
                }
                else if (_emailDTO.UseStartTls)
                {
                    await smtp.ConnectAsync(_emailDTO.Host, _emailDTO.Port, SecureSocketOptions.StartTls);
                }
                await smtp.AuthenticateAsync(_emailDTO.UserName, _emailDTO.Password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
