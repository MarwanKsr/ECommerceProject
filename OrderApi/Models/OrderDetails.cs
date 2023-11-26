using SharedLibrary.Base.Entity;

namespace OrderApi.Models
{
    public class OrderDetails : BaseEntity
    {
        public OrderHeader OrderHeader { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
