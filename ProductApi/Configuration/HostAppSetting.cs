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
        /// <summary>
        /// random string to generate JWT tokens
        /// </summary>
        public string SecretToken { get; set; }

        /// <summary>
        /// Represent the timeout period for the session
        /// </summary>
        public double SessionTimeout { get; set; }

        /// <summary>
        /// Represent the base url of the site
        /// </summary>
        public string SiteUrl { get; set; }

        public string MediaUrl { get; set; }
    }
}
