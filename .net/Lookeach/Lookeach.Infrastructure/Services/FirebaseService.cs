using Lookeach.Application.DTOs;
using Lookeach.Application.Services;
using Lookeach.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IConfiguration _configuration;
        private readonly FirebaseDTO _firebaseDTO;
        public FirebaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.GetSection(nameof(FirebaseDTO)).Bind(_firebaseDTO);
        }
        public async Task DeleteImage(string name, string label)
        {
            await name.DeleteImage(label, _firebaseDTO);
        }

        public async Task<string?> UploadToFirebase(IFormFile file, string label)
        {
            return await file.UploadToFirebase(label, _firebaseDTO);
        }
    }
}
