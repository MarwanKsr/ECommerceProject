using ProductApi.Configuration;
using ProductApi.Models;
using System.Runtime.InteropServices;
using TwentyTwenty.Storage;

namespace ProductApi.Extensions
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

        public static string ConvertToAppropriateDirectorySeperator(this string path)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                path = path.Replace(@"\", "/");
            else
                path = path.Replace("/", @"\");

            return path.Replace(@"\\", @"\").Replace(@"//", @"/");
        }

        public static async Task<bool> SaveAttachedFile(this Image image, IStorageProvider storageProvider, Stream stream, string imageUrl)
        {
            try
            {
                await storageProvider.SaveBlobStreamAsync(imageUrl.ConvertToAppropriateDirectorySeperator(), $"{image.ImageName}", stream);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> DeleteAttachedFile(this Image image, IStorageProvider storageProvider, string imageUrl)
        {
            try
            {
                await storageProvider.DeleteBlobAsync(imageUrl.ConvertToAppropriateDirectorySeperator(), $"{image.ImageName}");
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
