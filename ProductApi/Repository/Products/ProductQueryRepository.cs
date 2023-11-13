using Microsoft.EntityFrameworkCore;
using ProductApi.DbContexts;
using ProductApi.Dto;

namespace ProductApi.Repository.Products
{
    public class ProductQueryRepository : IProductQueryRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductQueryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _db.Products.Where(x => x.Id == productId).AsNoTracking().FirstOrDefaultAsync();
            if (product is null)
                return default;
            return ProductDto.FromEntity(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _db.Products.AsNoTracking().ToListAsync();
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
