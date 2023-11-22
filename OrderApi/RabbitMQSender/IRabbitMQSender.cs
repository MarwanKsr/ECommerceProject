using OrderApi.Models.Base;

namespace OrderApi.RabbitMQSender
{
    public interface IRabbitMQSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
