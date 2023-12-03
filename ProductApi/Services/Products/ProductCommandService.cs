using ProductApi.DbContexts;
using ProductApi.Dto;
using ProductApi.Extensions;
using ProductApi.Models;
using ProductApi.Services.Images;
using SharedLibrary.Repository;

namespace ProductApi.Services.Products
{
    public class ProductCommandService : IProductCommandService
    {
        private readonly IRepository<Product, ApplicationDbContext> _productRepository;
        private readonly IMediaService _imageService;

        public ProductCommandService(IRepository<Product, ApplicationDbContext> productRepository, IMediaService imageService)
        {
            _productRepository = productRepository;
            _imageService = imageService;
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            var product = new Product(productDto.Name, productDto.Price, productDto.Description, productDto.Stock, productDto.CreatedBy);
            await _productRepository.AddAndSaveAsync(product);
            return ProductDto.FromEntity(product);
        }

        public async Task<bool> DecreaseStock(long productId, int wantedCount)
        {
            var product = await _productRepository.FindAsync(productId) ?? throw new ArgumentException("Product not found");
            product.DecreaseStock(wantedCount);

            var affRows = await _productRepository.ModifyAndSaveAsync(product);

            return affRows >= 1;
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            try
            {
                var product = await _productRepository.FindAsync(productId);
                if (product == null)
                {
                    return false;
                }
                await _productRepository.RemoveAndSaveAsync(product);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> UpdateProduct(ProductDto productDto)
        {
            var product = await _productRepository.FindAsync(productDto.Id);
            if (product is null)
                throw new Exception("Product Not Found");

            product.SetName(productDto.Name);
            product.SetPrice(product.Price);
            product.SetDescriptoin(productDto.Description);
            product.AuditModify(productDto.ModifiedBy);

            await _productRepository.ModifyAndSaveAsync(product);
            return ProductDto.FromEntity(product);
        }

        public async Task<string> UpdateProductImage(long productId, IFormFile image, string modifiedBy)
        {
            if (image is null)
                throw new Exception("Image Not Found");

            var product = await _productRepository.FindAsync(productId);
            if (product is null)
                throw new Exception("Product Not Found");

            var oldImageId = product.Image?.Id;
            var imageEntity = await _imageService.CreateImageByFormFile(image, modifiedBy);
            if (oldImageId != null)
            {
                await _imageService.RemoveByIdAsync(oldImageId.Value);
            }
            product.SetImage(imageEntity);
            product.AuditModify(modifiedBy);

            await _productRepository.ModifyAndSaveAsync(product);
            return product.Image.GetAbsoluteUrl();
        }
    }
}
