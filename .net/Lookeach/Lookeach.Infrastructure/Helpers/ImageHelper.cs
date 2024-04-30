using Microsoft.AspNetCore.Http;

namespace Lookeach.Infrastructure.Helpers
{
    public static class ImageHelper
    {
        public static async Task ValidImage(this List<IFormFile> formFiles, string[] format)
        {
            foreach (var item in formFiles)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await item.CopyToAsync(memoryStream);

                    if (!FileHelper.IsValidFileExtensionAndSignature(item.FileName, memoryStream, format))
                    {
                        throw new Exception("Image required png, jpg, jpeg");
                    }
                }
            }
        }
    }
}
