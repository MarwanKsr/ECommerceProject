using ShoppingCartApi.Models;

namespace ShoppingCartApi.Extensions
{
    public static class ImageExtensions
    {
        public static string GetAbsoluteUrl(this Image image)
        {
            return image is null ? "" : $"{image.Path}/{image.ImageName}".ToUrl();
        }

        public static string ToUrl(this string url)
        {
            return url.Replace(@"\", "/");
        }
    }
}
