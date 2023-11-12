using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductApi.DbContexts;
using ProductApi.Dto;
using ProductApi.Models;
using System;

namespace ProductApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            var product = new Product(productDto.Name, productDto.Price, productDto.Description, productDto.Image, productDto.Stock, productDto.CreatedBy);
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return ProductDto.FromEntity(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _db.Products.FirstOrDefaultAsync(e => e.Id == productId);
                if (product == null)
                {
                    return false;
                }
                _db.Products.Remove(product); //delete from Product where Id=productId
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        public async Task<ProductDto> UpdateProduct(ProductDto productDto)
        {
            var product = await _db.Products.FindAsync(productDto.Id);
            if (product is null)
                throw new Exception("Product Not Found");

            product.SetName(productDto.Name);
            product.SetPrice(product.Price);
            product.SetDescriptoin(productDto.Description);
            if (productDto.Image != null)
            {
                product.SetImage(productDto.Image);
            }
            product.AuditModify(productDto.ModifiedBy);

            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return ProductDto.FromEntity(product);
        }
    }
}
