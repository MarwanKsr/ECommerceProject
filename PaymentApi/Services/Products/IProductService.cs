using SharedLibrary.Base.Services;

namespace PaymentApi.Services.Products
{
    public interface IProductService : IBaseService
    {
        Task<T> GetProductPriceById<T>(long productId, string token);
        Task<T> DecreaseProductStockById<T>(long productId, int wantedCount, string token);
    }
}
