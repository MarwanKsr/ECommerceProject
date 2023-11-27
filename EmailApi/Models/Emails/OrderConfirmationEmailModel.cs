using EmailApi.Models.Emails.Base;
using System.Text;

namespace EmailApi.Models.Emails
{
    public class OrderConfirmationEmailModel : BaseEmailSenderModel
    {
        public string OrderHeaderId { get; set; }
        public string FullName { get; set; }
        public string Subject => "Order Confirmation";
        public string Body { get => GetBody(); }

        private string GetBody()
        {
            var messages = new StringBuilder();
            messages.AppendLine($"Hi {FullName},");
            messages.AppendLine($"Your order with number {OrderHeaderId} has confirmed successfully.");
            messages.AppendLine("");
            messages.AppendLine("We will email you as soon as it ships");

            return messages.ToString();
        }
    }
}
