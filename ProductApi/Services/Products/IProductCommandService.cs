using ProductApi.Dto;

namespace ProductApi.Services.Products
{
    public interface IProductCommandService
    {
        Task<ProductDto> CreateProduct(ProductDto productDto, IFormFile image);
        Task<ProductDto> UpdateProduct(ProductDto productDto, IFormFile image);
        Task<bool> DeleteProduct(long productId);
    }
}
