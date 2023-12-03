using SharedLibrary.Base.Services;
using SharedLibrary.Models;

namespace PaymentApi.Services.ShoppingCard
{
    public class ShoppingCardService : BaseService, IShoppingCardService
    {
        private readonly IHttpClientFactory _httpClient;

        public ShoppingCardService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> ClearCard<T>(string userId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.GatewayAPIBase + $"/api/Cards/ClearCard",
                AccessToken = token
            });
        }
    }
}
