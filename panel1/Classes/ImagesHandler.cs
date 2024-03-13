
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace panel1.Classes
{
    public static class ImagesHandler
    {
        public static string SaveImage(IFormFile image)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            string imagePath = Path.Combine("Public/images", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return $"/images/{fileName}";
        }
    }
}

