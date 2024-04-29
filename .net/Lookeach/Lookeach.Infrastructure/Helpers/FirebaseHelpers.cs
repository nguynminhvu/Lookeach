using Firebase.Storage;
using Lookeach.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Helpers
{
    public class FirebaseHelpers
    {
        private readonly FirebaseDTO _firebaseDTO = new();
        private readonly IConfiguration _configuration;

        public FirebaseHelpers(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.GetSection(nameof(FirebaseDTO)).Bind(_firebaseDTO);
        }

        public async Task<string?> UploadToFirebase(IFormFile file, string label)
        {
            var storage = new FirebaseStorage(_firebaseDTO.BaseAddress);
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imageUrl = await storage.Child(label)
                                        .Child(imageName)
                                        .PutAsync(file.OpenReadStream());
            return imageUrl;
        }

        public async Task DeleteImage(string name, string label)
        {
            var storage = new FirebaseStorage(_firebaseDTO.BaseAddress);
            await storage.Child(label).Child(name)
                                          .DeleteAsync();
        }
    }
}
