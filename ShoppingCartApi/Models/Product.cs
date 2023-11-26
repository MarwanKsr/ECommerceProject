using SharedLibrary.Base.Entity;

namespace ShoppingCartApi.Models
{
    public class Product : BaseEntity
    {
        protected Product() { }
        public Product(long productId, string name, double price, string description, Image? image) 
        { 
            ProductId = productId;
            Name = name;
            Price = price;
            Description = description;
            Image = image;
        }
        public long ProductId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public Image? Image { get;  set; }
    }
}
