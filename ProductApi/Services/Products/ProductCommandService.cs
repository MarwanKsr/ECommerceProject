using ProductApi.DbContexts;
using ProductApi.Dto;
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

        public async Task<ProductDto> CreateProduct(ProductDto productDto, IFormFile image)
        {
            var imageEntity = await _imageService.CreateImageByFormFile(image);
            var product = new Product(productDto.Name, productDto.Price, productDto.Description, imageEntity, productDto.Stock, productDto.CreatedBy);
            await _productRepository.AddAndSaveAsync(product);
            return ProductDto.FromEntity(product);
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

        public async Task<ProductDto> UpdateProduct(ProductDto productDto, IFormFile image)
        {
            var product = await _productRepository.FindAsync(productDto.Id);
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

            await _productRepository.ModifyAndSaveAsync(product);
            return ProductDto.FromEntity(product);
        }
    }
}
