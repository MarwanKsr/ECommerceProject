using SharedLibrary.Base.Services;

namespace ShoppingCardApi.Services.Products
{
    public interface IProductService : IBaseService
    {
        Task<T> GetProductPriceById<T>(long productId, string token);
    }
}
