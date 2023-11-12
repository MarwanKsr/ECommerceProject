using ProductApi.Models;

namespace ProductApi.Dto
{
    public class ProductDto
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public double Price { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public int Stock { get; private set; }
        public Image Image { get; private set; }
        public string CreatedBy {get; set;}
        public DateTime CreatedAt { get; private set;}
        public string ModifiedBy { get; set;}
        public DateTime ModifiedAt { get; private set;}

        public static ProductDto FromEntity(Product product)
        {
            return new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                ImageUrl = ImageDto.FromEntity(product.Image)?.AbsoluteUrl,
                Price = product.Price,
                Stock = product.Stock,
                CreatedAt = product.CreatedAt,
                CreatedBy = product.CreatedBy,
                ModifiedBy = product.ModifiedBy,
                ModifiedAt = product.ModifiedAt.HasValue ? product.ModifiedAt.Value : default
            };
        }
    }
}
