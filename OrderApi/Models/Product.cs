using OrderApi.Models.Base;

namespace OrderApi.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
