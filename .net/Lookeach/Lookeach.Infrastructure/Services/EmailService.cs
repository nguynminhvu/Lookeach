using Lookeach.Application.DTOs;
using Lookeach.Application.Services;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Lookeach.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IAesService _aesService;
        private readonly IConfiguration _configuration;
        private EmailDTO _emailDTO = new();
        private EmailSenderDTO _emailSenderDTO = new();

        public EmailService(IConfiguration configuration, IAesService aesService)
        {
            _aesService=aesService;
            _configuration = configuration;
            _configuration.GetSection(nameof(EmailDTO)).Bind(_emailDTO);
            _configuration.GetSection(nameof(EmailSenderDTO)).Bind(_emailSenderDTO);
        }

        public async Task SendEmailConfirm(string userEmail, string subject, Guid userId)
        {
            try
            {
                var tokenCode = _emailSenderDTO.Domain + _emailSenderDTO.Uri + "/" + Convert.ToBase64String(_aesService.Encrypt(userId + ""));
                var contentHtml = "<html><body><h1>Please click the link below to confirm your email:</h1>"
                                                              + $"<p><a href=\"{tokenCode}\">Confirm Password</a></p>" +
                                                              $"<br/>" +
                                                              $"<img src=\"https://media.cnn.com/api/v1/images/stellar/prod/220310092648-benzema-tease.jpg?q=h_2000,w_3000,x_0,y_0\" ></body></html>";
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
