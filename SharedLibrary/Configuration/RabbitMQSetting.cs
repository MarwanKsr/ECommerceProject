namespace SharedLibrary.Configuration
{
    public class RabbitMQSetting
    {
        public const string SECTION_NAME = "RabbitMQSetting";
        public static RabbitMQSetting Instance { get; private set; }
        public static void SetUpInstance(RabbitMQSetting instance)
        {
            Instance = instance;
        }

        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
