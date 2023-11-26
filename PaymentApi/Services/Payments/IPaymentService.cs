using Iyzipay.Model;
using PaymentApi.Models;

namespace PaymentApi.Services.Payments
{
    public interface IPaymentService
    {
        Payment Pay(PaymentModel payment);
    }
}
