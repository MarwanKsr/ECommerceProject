namespace Identity.Configuration
{
    public class ApiConfig
    {
        public const string SECTION_NAME = "ApiConfig";
        public static ApiConfig Instance { get; private set; }
        public static void SetUpInstance(ApiConfig instance)
        {
            Instance = instance;
        }

        public string SecretKey { get; set; }

        public int KeyExpiration { get; set; }

        public string SecretRefreshKey { get; set; }

        public int RefreshKeyExpiration { get; set; }
    }
}
