using ProductApi.Dto;

namespace ProductApi.Repository.Products
{
    public interface IProductQueryRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int productId);
    }
}
