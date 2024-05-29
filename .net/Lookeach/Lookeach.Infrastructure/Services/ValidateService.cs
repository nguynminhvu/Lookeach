using Lookeach.Application.Services;
using Lookeach.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace Lookeach.Infrastructure.Services
{
    public class ValidateService : IValidateService
    {
        public async Task ValidImage(List<IFormFile> formFiles, string[] format)
        {
            await formFiles.ValidImage(format);
        }

        public string HashPassword(string pwd)
        {
            using (SHA256 hasher = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(pwd);
                byte[] hashedBytes = hasher.ComputeHash(inputBytes);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
    }
}

