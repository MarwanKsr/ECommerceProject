using ProductApi.Dto;

namespace ProductApi.Services.Products
{
    public interface IProductCommandService
    {
        Task<ProductDto> CreateProduct(ProductDto productDto);
        Task<ProductDto> UpdateProduct(ProductDto productDto);
        Task<string> UpdateProductImage(long productId, IFormFile image, string modifiedBy);
        Task<bool> DeleteProduct(long productId);
        Task<bool> DecreaseStock(long productId, int wantedCount);
    }
}
