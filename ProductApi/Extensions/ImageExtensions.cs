using ProductApi.Configuration;
using ProductApi.Models;

namespace ProductApi.Extensions
{
    public static class ImageExtensions
    {
        public static string GetAbsoluteUrl(this Image image)
        {
            var hostAppSettings = HostAppSetting.Instance;

            var mediaUrl = hostAppSettings.MediaUrl;
            var baseUrl = hostAppSettings.SiteUrl;

            return image is null ? "" : $"{baseUrl}/{mediaUrl}/{image.ImageName}".ToUrl();
        }

        public static string ToUrl(this string url)
        {
            return url.Replace(@"\", "/");
        }

    }
}
