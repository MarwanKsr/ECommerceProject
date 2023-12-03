namespace ProductApi.Configuration
{
    public class HostAppSetting
    {
        public const string SECTION_NAME = "HostAppSetting";

        public static HostAppSetting Instance { get; private set; }
        public static void SetUpInstance(HostAppSetting instance)
        {
            Instance = instance;
        }
        public string SecretToken { get; set; }

        public double SessionTimeout { get; set; }

        public string SiteUrl { get; set; }

        public string MediaUrl { get; set; }
    }
}
