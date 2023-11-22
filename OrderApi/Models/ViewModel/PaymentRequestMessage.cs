using OrderApi.Models.Base;

namespace OrderApi.Models.ViewModel
{
    public class PaymentRequestMessage : BaseMessage
    {
        public long OrderId { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public double OrderTotal { get; set; }
        public string Email { get; set; }
    }
}
