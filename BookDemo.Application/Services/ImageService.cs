using BookDemo.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BookDemo.Application.Services
{
    public class ImageService :IImageService
    {
        public async Task<string>UploadImageAsync(IFormFile image, string uploadsFolder)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentNullException("Upload image");

            if(!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            var fileName= $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }
    }
}
