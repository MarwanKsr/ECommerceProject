using ShoppingCardApi.Models;
using ShoppingCardApi.Services.Base;

namespace ShoppingCardApi.Services.Products
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
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
