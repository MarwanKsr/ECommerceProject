using SharedLibrary.Base.Entity;

namespace OrderApi.Models
{
    public class OrderHeader : BaseEntity
    {
        public string UserId { get; set; }
        public double OrderTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public int CardTotalItems { get; set; }
        public bool IsSuccess { get; set; }
    }
}
