using ShoppingCardApi.Models.Base;

namespace ShoppingCardApi.RabbitMQSender
{
    public interface IRabbitMQSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
