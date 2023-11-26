using SharedLibrary.Repository;
using ShoppingCartApi.DbContexts;
using ShoppingCartApi.Models;

namespace ShoppingCardApi.Services.Products
{
    public class ProductCommandService : IProductCommandService
    {
        IRepository<Product, ApplicationDbContext> _productRepository;

        public ProductCommandService(IRepository<Product, ApplicationDbContext> productRepository)
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
