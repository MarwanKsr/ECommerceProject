using SharedLibrary.Base.Entity;

namespace OrderApi.Models
{
    public class OrderHeaderOrderDetails : BaseEntity
    {
        public long OrderHeaderId { get; set; }
        public long OrderDetailsId { get; set; }
    }
}
