using SharedLibrary.Base.Services;

namespace PaymentApi.Services.ShoppingCard
{
    public interface IShoppingCardService : IBaseService
    {
        Task<T> ClearCard<T>(string userId, string token);
    }
}
