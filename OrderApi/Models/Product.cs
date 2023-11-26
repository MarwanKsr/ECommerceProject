using SharedLibrary.Base.Entity;

namespace OrderApi.Models
{
    public class Product : BaseEntity
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
