using SharedLibrary.Base.Services;
using SharedLibrary.Models;

namespace PaymentApi.Services.Products
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> DecreaseProductStockById<T>(long productId, int wantedCount, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.GatewayAPIBase + $"/api/products/{productId}/DecreaseStock?wantedCount={wantedCount}",
                AccessToken = token
            });
        }

        public async Task<T> GetProductPriceById<T>(long productId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.GatewayAPIBase + $"/api/products/{productId}/GetPrice",
                AccessToken = token
            });
        }
    }
}
