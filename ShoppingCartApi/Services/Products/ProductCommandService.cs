using ShoppingCartApi.Models;
using ShoppingCartApi.Repository;

namespace ShoppingCardApi.Services.Products
{
    public class ProductCommandService : IProductCommandService
    {
        IRepository<Product> _productRepository;

        public ProductCommandService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> UpdateProductPrice(long Id, double newPrice)
        {
            var product = await _productRepository.FindAsync(Id);
            product.Price = newPrice;
            var affRows = await _productRepository.ModifyAndSaveAsync(product);
            return affRows > 0;
        }
    }
}
