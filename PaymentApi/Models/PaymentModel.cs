using PaymentApi.Models.Dto;
using SharedLibrary.Base.Message;

namespace PaymentApi.Models
{
    public class PaymentModel
    {
        public OrderHeaderDto OrderHeader { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
    }
}
