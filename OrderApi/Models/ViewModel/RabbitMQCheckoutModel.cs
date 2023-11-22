namespace OrderApi.Models.ViewModel
{
    public class RabbitMQCheckoutModel
    {
        public CheckoutModel CheckoutModel { get; set; }
        public IEnumerable<CardDetailsDto> CardDetails { get; set; }
    }
}
