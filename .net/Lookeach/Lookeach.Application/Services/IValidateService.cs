using Microsoft.AspNetCore.Http;

namespace Lookeach.Application.Services
{
    public interface IValidateService
    {
        Task ValidImage(List<IFormFile> formFiles, string[] format);
    }
}
