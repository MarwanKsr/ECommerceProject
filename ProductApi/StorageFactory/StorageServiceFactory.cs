using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Local;

namespace ProductApi.StorageFactory
{
    public class StorageServiceFactory : IStorageServiceFactory
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StorageServiceFactory(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IStorageProvider GetProvider()
        {
            return new LocalStorageProvider(_webHostEnvironment.ContentRootPath);
        }
    }
}
