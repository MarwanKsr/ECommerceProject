using SharedLibrary.Base.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.RabbitMQSender
{
    public interface IRabbitMQSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
