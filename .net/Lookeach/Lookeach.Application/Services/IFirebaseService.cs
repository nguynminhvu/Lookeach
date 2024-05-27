using Microsoft.AspNetCore.Http;

namespace Lookeach.Application.Services
{
    public interface IFirebaseService
    {
        Task<string?> UploadToFirebase(IFormFile file, string label);
        Task DeleteImage(string name, string label);


    }
}
