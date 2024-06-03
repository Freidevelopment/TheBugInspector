using FreiBlogBuilder.Client.Helpers;
using System.Text.RegularExpressions;
using TheBugInspector.Models;

namespace TheBugInspector.Helpers
{
    public class FileHelper
    {
        public static readonly string DefaultProfilePicture = ImageHelper.DefaultProfilePicture;
        public static readonly string DefaultContactImage = ImageHelper.DefaultContactImage;
        public static readonly string DefaultCategoryImage = ImageHelper.DefaultCategoryImage;
        public static readonly string DefaultBlogImage = ImageHelper.DefaultBlogImage;
        public static readonly int MaxFileSize = ImageHelper.MaxFileSize;

        public static async Task<FileUpload> GetFileUploadAsync(IFormFile file)
        {
            using var ms = new MemoryStream();

            await file.CopyToAsync(ms);
            byte[] data = ms.ToArray();

            if (ms.Length > MaxFileSize)
            {
                throw new IOException("Images must be less than 5MB!");
            }

            FileUpload upload = new FileUpload()
            {
                Id = Guid.NewGuid(),
                Data = data,
                Type = file.ContentType
            };

            return upload;
        }

        public static FileUpload GetFileUpload(string dataUrl)
        {
            GroupCollection matchGroups = Regex.Match(dataUrl, @"data:(?<type>.+?);base64,(?<data>.+)").Groups;

            if (matchGroups.ContainsKey("type") && matchGroups.ContainsKey("data"))
            {
                string contentType = matchGroups["type"].Value;
                byte[] data = Convert.FromBase64String(matchGroups["data"].Value);

                if (data.Length <= MaxFileSize)
                {
                    FileUpload upload = new FileUpload()
                    {
                        Id = Guid.NewGuid(),
                        Data = data,
                        Type = contentType
                    };

                    return upload;
                }
            }

            throw new IOException("Data URL was either invalid or too large");
        }
    }
}
