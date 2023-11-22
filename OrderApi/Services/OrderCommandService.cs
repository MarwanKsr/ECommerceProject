using OrderApi.Models;
using OrderApi.Repository;

namespace OrderApi.Services
{
    public class OrderCommandService : IOrderCommandService
    {
        private readonly IRepository<OrderHeader> _orderHeaderRepository;

        public OrderCommandService(IRepository<OrderHeader> orderHeaderRepository)
        {
            _orderHeaderRepository = orderHeaderRepository;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            if (orderHeader is null)
                return false;
            await _orderHeaderRepository.AddAndSaveAsync(orderHeader);
            return true;
        }

        public async Task<bool> UpdateOrderStatus(long orderHeaderId, bool status)
        {
            var orderHeader = await _orderHeaderRepository.FindAsync(orderHeaderId);
            if (orderHeader is null)
                throw new ArgumentException("Order header not found");
            orderHeader.IsSuccess = status;
            var affRows = await _orderHeaderRepository.ModifyAndSaveAsync(orderHeader);
            return affRows > 0;
        }
    }
}
