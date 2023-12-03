using ProductApi.Models;

namespace ProductApi.Services.Images
{
    public interface IMediaService
    {
        Task<Image> CreateImageEntityInstanceByStream(Stream fileStream, string fileName, string contentType, string modifiedBy);

        Task<(Image, bool)> CreateAndSaveImageEntityInstanceByStream(Stream fileStream, string fileName, string contentType, string modifiedBy);

        Task<Image> CreateImageByFormFile(IFormFile file, string modifiedBy);

        Task<bool> RemoveByIdAsync(long imageId);
    }
}
