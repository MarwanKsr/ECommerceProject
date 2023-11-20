using ShoppingCartApi.Models.Base;

namespace ShoppingCartApi.Models
{
    public class Product : BaseEntity
    {
        protected Product() { }
        public Product(string name, double price, string description, Image? image) 
        { 
            Name = name;
            Price = price;
            Description = description;
            Image = image;
        }
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public Image? Image { get;  set; }
    }
}
