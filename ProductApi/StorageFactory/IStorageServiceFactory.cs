using TwentyTwenty.Storage;

namespace ProductApi.StorageFactory
{
    public interface IStorageServiceFactory
    {
        IStorageProvider GetProvider();
    }
}
