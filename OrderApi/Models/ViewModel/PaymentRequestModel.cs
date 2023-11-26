using SharedLibrary.Base.Message;

namespace OrderApi.Models.ViewModel
{
    public class PaymentRequestModel : BaseMessage
    {
        public OrderHeaderDto OrderHeader { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
    }
}
