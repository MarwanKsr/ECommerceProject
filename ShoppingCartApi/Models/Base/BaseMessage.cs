namespace ShoppingCardApi.Models.Base
{
    public abstract class BaseMessage
    {
        public long Id { get; set; }
        public DateTime MessageCreated { get; set; }
    }
}
