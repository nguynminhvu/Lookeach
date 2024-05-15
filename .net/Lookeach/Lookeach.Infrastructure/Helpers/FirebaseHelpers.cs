using Firebase.Storage;
using Lookeach.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Lookeach.Infrastructure.Helpers
{
    public static class FirebaseHelpers
    {
        public static async Task<string?> UploadToFirebase(this IFormFile file, string label, FirebaseDTO _firebaseDTO)
        {
            var storage = new FirebaseStorage(_firebaseDTO.BaseAddress);
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imageUrl = await storage.Child(label)
                                        .Child(imageName)
                                        .PutAsync(file.OpenReadStream());
            return imageUrl;
        }

        public static async Task DeleteImage(this string name, string label, FirebaseDTO _firebaseDTO)
        {
            var storage = new FirebaseStorage(_firebaseDTO.BaseAddress);
            await storage.Child(label).Child(name)
                                          .DeleteAsync();
        }
    }
}
