using Microsoft.EntityFrameworkCore;
using ProductApi.DbContexts;
using ProductApi.Dto;
using ProductApi.Models;
using SharedLibrary.Repository;

namespace ProductApi.Services.Products
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IRepository<Product, ApplicationDbContext> _productRepository;

        public ProductQueryService(IRepository<Product, ApplicationDbContext> productRepository)
        {
            _productRepository = productRepository;
        }

        private async Task<Product> GetById(long productId)
        {
            return await _productRepository.Query(e => e.Id == productId).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<ProductDto> GetProductById(long productId)
        {
            var product = await GetById(productId);
            if (product is null)
                return default;
            return ProductDto.FromEntity(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _productRepository.QueryAll().AsNoTracking().ToListAsync();
            if (!products.Any())
            {
                return Enumerable.Empty<ProductDto>();
            }

            var ProductDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                ProductDtos.Add(ProductDto.FromEntity(product));
            }
            return ProductDtos;
        }

        public async Task<double> GetProductPriceById(long productId)
        {
            var product = await GetById(productId);
            if (product is null)
                throw new ArgumentNullException("Product not found");
            return product.Price;

        }
    }
}
