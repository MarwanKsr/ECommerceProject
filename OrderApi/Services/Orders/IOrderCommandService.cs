using OrderApi.Models;

namespace OrderApi.Services.Orders
{
    public interface IOrderCommandService
    {
        Task<bool> AddOrder(OrderHeader orderHeader, IEnumerable<OrderDetails> orderDetails);
        Task<bool> UpdateOrderStatus(long orderHeaderId, bool status);
    }
}
