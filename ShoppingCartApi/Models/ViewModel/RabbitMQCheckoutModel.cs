using SharedLibrary.Base.Message;
using ShoppingCartApi.Models.Dto;

namespace ShoppingCardApi.Models.ViewModel
{
    public class RabbitMQCheckoutModel : BaseMessage
    {
        public CheckoutModel CheckoutModel { get; set; }
        public IEnumerable<CardDetailsDto> CardDetails { get; set; }
    }
}
