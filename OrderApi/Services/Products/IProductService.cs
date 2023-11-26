using SharedLibrary.Base.Services;

namespace OrderApi.Services.Products
{
    public interface IProductService : IBaseService
    {
        Task<T> GetProductPriceById<T>(long productId, string token);
    }
}
