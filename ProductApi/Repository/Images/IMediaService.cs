using ProductApi.Models;

namespace ProductApi.Repository.Images
{
    public interface IMediaService
    {
        Task<Image> CreateImageEntityInstanceByStream(Stream fileStream, string fileName, string contentType);

        Task<(Image, bool)> CreateAndSaveImageEntityInstanceByStream(Stream fileStream, string fileName, string contentType);

        Task<Image> CreateImageByFormFile(IFormFile file);

        Task<bool> RemoveByIdAsync(long imageId);
    }
}
