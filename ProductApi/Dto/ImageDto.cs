using Microsoft.AspNetCore.Mvc.Formatters;
using ProductApi.Extensions;
using ProductApi.Models;

namespace ProductApi.Dto
{
    public class ImageDto
    {
        public long Id { get; private set; }
        public string DisplayName { get; private set; }
        public string ImageName { get; private set; }
        public string Path { get; private set; }
        public string AbsoluteUrl { get; private set; }

        public static ImageDto FromEntity(Image image)
        {
            if (image is null)
                return default;

            return new()
            {
                Id = image.Id,
                DisplayName = image.DisplayName,
                Path = image.Path,
                ImageName = image.ImageName,
                AbsoluteUrl = image.GetAbsoluteUrl()
            };
        }
    }
}
