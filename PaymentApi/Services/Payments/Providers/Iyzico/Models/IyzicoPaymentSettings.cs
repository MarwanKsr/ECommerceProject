using SharedLibrary.Configuration;

namespace PaymentApi.Services.Payments.Providers.Iyzico.Models
{
    public class IyzicoPaymentSettings
    {
        public const string SECTION_NAME = "PaymentProviders:IyzicoSettings";
        public static IyzicoPaymentSettings Instance { get; private set; }
        public static void SetUpInstance(IyzicoPaymentSettings instance)
        {
            Instance = instance;
        }

        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string BaseUrl { get; set; }
    }
}
