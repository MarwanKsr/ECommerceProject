using ProductApi.Dto;

namespace ProductApi.Services.Products
{
    public interface IProductQueryService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int productId);
    }
}
