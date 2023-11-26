using SharedLibrary.Base.Message;

namespace PaymentApi.Models
{
    public class MailRequestModel : BaseMessage
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
