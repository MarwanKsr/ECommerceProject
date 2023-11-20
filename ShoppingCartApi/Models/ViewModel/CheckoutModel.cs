using ShoppingCartApi.Models.Dto;

namespace ShoppingCardApi.Models.ViewModel
{
    public class CheckoutModel
    {
        public long CardHeaderId { get; set; }
        public string UserId { get; set; }
        public double OrderTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        //public IEnumerable<CardDetailsDto> CardDetails { get; set; }
    }
}
