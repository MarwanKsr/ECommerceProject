using Microsoft.AspNetCore.Mvc.Formatters;
using ProductApi.Extensions;
using ProductApi.Models;

namespace ProductApi.Dto
{
    public class ImageDto
    {
        public long Id { get; private set; }
        public string DisplayName { get; private set; }
        public string AbsoluteUrl { get; private set; }

        public static ImageDto FromEntity(Image image)
        {
            return new()
            {
                Id = image.Id,
                DisplayName = image.DisplayName,
                AbsoluteUrl = image.GetAbsoluteUrl()
            };
        }
    }
}
