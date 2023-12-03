using SharedLibrary.Base.Message;

namespace OrderApi.Models.ViewModel
{
    public class RabbitMQCheckoutModel : BaseMessage
    {
        public CheckoutModel CheckoutModel { get; set; }
        public IEnumerable<CardDetailsDto> CardDetails { get; set; }
    }
}
