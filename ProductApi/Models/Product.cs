using ProductApi.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Product : AuditableBaseEntity
    {
        protected Product() { }

        public Product(string name, double price, string description, Image image, int stock, string createdBy)
        {
            SetName(name);
            SetPrice(price);
            SetDescriptoin(description);
            //SetImage(image);
            SetStock(stock);
            AuditCreate(createdBy);
            AuditModify(createdBy);
        }

        public string Name { get; private set; }
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name cann't be empty");
            Name = name;
        }

        public double Price { get; private set; }
        public void SetPrice(double price)
        {
            if (price <= 0) 
                throw new ArgumentOutOfRangeException("Price cann't be less or equal to zero");
            Price = price;
        }

        public string Description { get; private set; }
        public void SetDescriptoin(string descriptoin)
        {
            if (string.IsNullOrEmpty(descriptoin))
                throw new ArgumentNullException("Description cann't be empty");
            Description = descriptoin;
        }

        public Image? Image { get; private set; }
        public void SetImage(Image? image)
        {
            if (image == null) 
                throw new ArgumentNullException("image cann't be empty");
            Image = image;
        }

        public int Stock { get; private set; }
        public void SetStock(int stock)
        {
            if (stock <= 0) 
                throw new ArgumentOutOfRangeException("Stock cann't be less or equal to zero");
            Stock = stock;
        }
    }
}
