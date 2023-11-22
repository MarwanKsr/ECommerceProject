using OrderApi.Models;

namespace OrderApi.Services
{
    public interface IOrderCommandService
    {
        Task<bool> AddOrder(OrderHeader orderHeader);
        Task<bool> UpdateOrderStatus(long orderHeaderId, bool status);
    }
}
