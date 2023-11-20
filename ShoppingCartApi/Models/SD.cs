namespace ShoppingCardApi.Models
{
    public static class SD
    {
        public static string GatewayAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
