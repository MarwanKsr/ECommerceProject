using ProductApi.Dto;

namespace ProductApi.Repository.Products
{
    public interface IProductCommandRepository
    {
        Task<ProductDto> CreateProduct(ProductDto productDto, IFormFile image);
        Task<ProductDto> UpdateProduct(ProductDto productDto, IFormFile image);
        Task<bool> DeleteProduct(int productId);
    }
}
