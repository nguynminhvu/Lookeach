using Lookeach.Application.Services;
using Lookeach.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;

namespace Lookeach.Infrastructure.Services
{
    public class ValidateService : IValidateService
    {
        public async Task ValidImage(List<IFormFile> formFiles, string[] format)
        {
            await formFiles.ValidImage(format);
        }
    }
}

