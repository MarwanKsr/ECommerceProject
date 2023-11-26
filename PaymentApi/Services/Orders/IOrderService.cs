using SharedLibrary.Base.Services;

namespace PaymentApi.Services.Orders
{
    public interface IOrderService : IBaseService
    {
        Task<T> MakeOrderSuccess<T>(long orderId, string token);
    }
}
