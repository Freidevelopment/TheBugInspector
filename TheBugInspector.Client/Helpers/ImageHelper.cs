using Microsoft.AspNetCore.Components.Forms;

namespace FreiBlogBuilder.Client.Helpers
{
    public static class ImageHelper
    {

        public static readonly string DefaultProfilePicture = "/Images/DefaultProfilePicture.svg";
        public static readonly string DefaultContactImage = "/Images/DefaultContactImage.png";
        public static readonly string DefaultCategoryImage = "/Images/DefaultCategoryImage.svg";
        public static readonly string DefaultBlogImage = "/Images/DefaultBlogImage.jpg";
        public static readonly string DefaultCompanyImage = "/Images/DefaultCompanyImage.jpg";
        public static readonly int MaxFileSize = 5 * 1024 * 1024;

        public static async Task<string> GetDataUrl(IBrowserFile file)
        {
            using Stream fileStream = file.OpenReadStream(MaxFileSize);
            using MemoryStream ms = new MemoryStream();
            await fileStream.CopyToAsync(ms);

            byte[] imageBytes = ms.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);
            string dataUrl = $"data:{file.ContentType};base64,{base64String}";

            return dataUrl;
        }
    }
}
