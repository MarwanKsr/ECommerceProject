using ProductApi.Configuration;
using ProductApi.DbContexts;
using ProductApi.Extensions;
using ProductApi.Models;
using ProductApi.StorageFactory;
using SharedLibrary.Repository;
using TwentyTwenty.Storage;

namespace ProductApi.Services.Images
{
    public class MediaService : IMediaService
    {
        private readonly string _galleryUrl;
        private readonly IStorageServiceFactory _storageServiceFactory;
        private readonly IStorageProvider _storageProvider;
        private readonly IRepository<Image, ApplicationDbContext> _imageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MediaService(
            IStorageServiceFactory storageServiceFactory,
            IRepository<Image, ApplicationDbContext> imageRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _storageServiceFactory = storageServiceFactory;
            _storageProvider = _storageServiceFactory.GetProvider();
            _imageRepository = imageRepository;
            _webHostEnvironment = webHostEnvironment;
            _galleryUrl = _webHostEnvironment.ContentRootPath + HostAppSetting.Instance.MediaUrl;
        }

        public async Task<(Image, bool)> CreateAndSaveImageEntityInstanceByStream(Stream fileStream, string fileName, string contentType, string modifiedBy)
        {
            var isFileSaved = false;

            var fileInfo = new FileInfo(fileName);

            var image = new Image()
            {
                ImageName = GenerateRandomFileName(fileInfo.Extension),
                DisplayName = fileInfo.Name,
                ContentType = contentType,
                FileSize = fileStream.Length,
                Path = _galleryUrl.ConvertToAppropriateDirectorySeperator(),
            };
            image.AuditCreate(modifiedBy);
            image.AuditModify(modifiedBy);
            try
            {
                var isSuccess = await image.SaveAttachedFile(_storageProvider, fileStream, _galleryUrl);

                if (!isSuccess)
                {
                    return (default, false);
                }
                isFileSaved = true;

                return new(image, true);
            }
            catch (Exception)
            {
                // check if file is saved to storage drive, and something occurred when saving to db, we will remove the file
                if (isFileSaved)
                    await image.DeleteAttachedFile(_storageProvider, _galleryUrl);

                throw;
            }
        }

        public async Task<Image> CreateImageByFormFile(IFormFile file, string modifiedBy)
        {
            if (file is null)
            {
                return default;
            }
            return await CreateImageEntityInstanceByStream(file.OpenReadStream(), file.FileName, file.ContentType, modifiedBy);
        }

        public async Task<Image> CreateImageEntityInstanceByStream(Stream fileStream, string fileName, string contentType, string modifiedBy)
        {
            var (image, isFileSaved) = await CreateAndSaveImageEntityInstanceByStream(fileStream, fileName, contentType, modifiedBy);

            if (!isFileSaved)
            {
                return default;
            }
            var isSuccess = await SaveMediaEntity(image);
            if (!isSuccess)
            {
                return default;
            }
            return image;
        }

        private async Task<bool> SaveMediaEntity(Image image)
        {
            try
            {
                await _imageRepository.AddAndSaveAsync(image);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GenerateRandomFileName(string fileExtension)
        {
            fileExtension = fileExtension.StartsWith('.') ? fileExtension : $".{fileExtension}";

            return Guid.NewGuid() + fileExtension;
        }

        public async Task<bool> RemoveByIdAsync(long imageId)
        {
            try
            {
                var image = await _imageRepository.FindAsync(imageId);
                if (image is null)
                    return true;

                File.Delete(image.GetAbsoluteUrl().ConvertToAppropriateDirectorySeperator());

                await _imageRepository.RemoveAndSaveAsync(image);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
