using SharedLibrary.Base.Services;
using SharedLibrary.Models;

namespace PaymentApi.Services.Orders
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IHttpClientFactory _httpClient;

        public OrderService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> MakeOrderSuccess<T>(long orderId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.GatewayAPIBase + $"/api/Orders/{orderId}/MakeOrderSuccess",
                AccessToken = token
            });
        }
    }
}
