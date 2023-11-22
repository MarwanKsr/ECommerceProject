namespace OrderApi.Models.ViewModel
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public Product ToEntity()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                Price = Price
            };
        }
    }
}
