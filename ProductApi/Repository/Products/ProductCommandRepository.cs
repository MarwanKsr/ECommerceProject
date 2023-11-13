using ProductApi.DbContexts;
using ProductApi.Dto;
using ProductApi.Models;
using ProductApi.Repository.Images;

namespace ProductApi.Repository.Products
{
    public class ProductCommandRepository : IProductCommandRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMediaService _imageService;

        public ProductCommandRepository(ApplicationDbContext db, IMediaService imageService)
        {
            _db = db;
            _imageService = imageService;
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto, IFormFile image)
        {
            var imageEntity = await _imageService.CreateImageByFormFile(image);
            var product = new Product(productDto.Name, productDto.Price, productDto.Description, imageEntity, productDto.Stock, productDto.CreatedBy);
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return ProductDto.FromEntity(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _db.Products.FindAsync(productId);
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

        public async Task<ProductDto> UpdateProduct(ProductDto productDto, IFormFile image)
        {
            var product = await _db.Products.FindAsync(productDto.Id);
            if (product is null)
                throw new Exception("Product Not Found");

            product.SetName(productDto.Name);
            product.SetPrice(product.Price);
            product.SetDescriptoin(productDto.Description);
            if (image != null)
            {
                var oldImageId = product.Image?.Id;
                var imageEntity = await _imageService.CreateImageByFormFile(image);
                if (oldImageId.HasValue)
                {
                    await _imageService.RemoveByIdAsync(oldImageId.Value);
                }
                product.SetImage(imageEntity);
            }
            product.AuditModify(productDto.ModifiedBy);

            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return ProductDto.FromEntity(product);
        }
    }
}
