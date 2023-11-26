using Microsoft.IdentityModel.Tokens;
using OrderApi.DbContexts;
using OrderApi.Models;
using SharedLibrary.Repository;

namespace OrderApi.Services.Orders
{
    public class OrderCommandService : IOrderCommandService
    {
        private readonly IRepository<OrderHeader, ApplicationDbContext> _orderHeaderRepository;
        private readonly IRepository<OrderHeaderOrderDetails, ApplicationDbContext> _orderHeaderOrderDetailsRepository;

        public OrderCommandService(
            IRepository<OrderHeader, ApplicationDbContext> orderHeaderRepository,
            IRepository<OrderHeaderOrderDetails, ApplicationDbContext> orderHeaderOrderDetailsRepository)
        {
            _orderHeaderRepository = orderHeaderRepository;
            _orderHeaderOrderDetailsRepository = orderHeaderOrderDetailsRepository;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader, IEnumerable<OrderDetails> orderDetails)
        {
            if (orderHeader is null)
                return false;
            await _orderHeaderRepository.AddAndSaveAsync(orderHeader);
            if (orderHeader.Id <= 0)
            {
                throw new ArgumentException("An error occurs while add a new order");
            }
            if (orderDetails is not null)
            {
                var orderHeaderOrderDetailsList = GetOrderHeaderOrderDetailsList(orderHeader.Id, orderDetails);
                await _orderHeaderOrderDetailsRepository.AddRangeAndSaveAsync(orderHeaderOrderDetailsList.ToList());
            };
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

        private IEnumerable<OrderHeaderOrderDetails> GetOrderHeaderOrderDetailsList(long orderHeaderId, IEnumerable<OrderDetails> orderDetails)
        {
            if (orderHeaderId <= 0 || orderDetails.IsNullOrEmpty())
            {
                Enumerable.Empty<OrderHeaderOrderDetails>();
            }

            foreach (var item in orderDetails)
            {
                yield return new OrderHeaderOrderDetails()
                {
                    OrderHeaderId = orderHeaderId,
                    OrderDetailsId = item.Id
                };
            }
        }
    }
}
