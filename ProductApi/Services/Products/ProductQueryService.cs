using Microsoft.EntityFrameworkCore;
using ProductApi.Dto;
using ProductApi.Models;
using ProductApi.Repository;

namespace ProductApi.Services.Products
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductQueryService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _productRepository.Query(x => x.Id == productId).AsNoTracking().FirstOrDefaultAsync();
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
    }
}
