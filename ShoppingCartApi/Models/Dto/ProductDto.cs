namespace ShoppingCartApi.Models.Dto
{
    public class ProductDto
    {
        public long Id { get; private set; }
        public long ProductId { get; private set; }
        public string Name { get; private set; }
        public double Price { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public ImageDto Image { get; private set; }

        public static ProductDto FromEntity(Product product)
        {
            return new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = ImageDto.FromEntity(product.Image)?.AbsoluteUrl,
                Image = ImageDto.FromEntity(product.Image),
                Price = product.Price,
            };
        }
    }
}
