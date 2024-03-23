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

            return $"{fileName}";
        }

        public static void DeleteImage(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    Console.WriteLine("Image path is empty or null.");
                    return;
                }

                // Construct the full path by combining the base directory and the image path
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "public", "images", imagePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    Console.WriteLine($"Image deleted: {fullPath}");
                }
                else
                {
                    Console.WriteLine($"File does not exist: {fullPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }
    }
    //public static void DeleteImage(string imagePath)
    //{
    //    if (!string.IsNullOrEmpty(imagePath))
    //    {

    //        return;
    //    }
    //    if (File.Exists(imagePath))
    //    {
    //        try
    //        {
    //            File.Delete(imagePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error deleting image: {ex.Message}");
    //        }
    //    }
    //}



}


