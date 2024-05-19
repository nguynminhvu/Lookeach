namespace Lookeach.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailConfirm(string userEmail, string subject, Guid userId);
    }
}
