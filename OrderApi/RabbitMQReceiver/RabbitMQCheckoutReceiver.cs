using Newtonsoft.Json;
using OrderApi.Configuration;
using OrderApi.Models.ViewModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OrderApi.RabbitMQReceiver
{
    public class RabbitMQCheckoutReceiver : BackgroundService
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQCheckoutReceiver()
        {
            var rabbitMQSetting = RabbitMQSetting.Instance;
            _hostname = rabbitMQSetting.HostName;
            _password = rabbitMQSetting.Password;
            _username = rabbitMQSetting.UserName;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            
            CreateModel();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (obj, deliverEvent) =>
            {
                var content = Encoding.UTF8.GetString(deliverEvent.Body.ToArray());
                RabbitMQCheckoutModel rabbitMQCheckout = JsonConvert.DeserializeObject<RabbitMQCheckoutModel>(content);
                HandleMessage(rabbitMQCheckout).GetAwaiter().GetResult();

                _channel.BasicAck(deliverEvent.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(RabbitMQCheckoutModel rabbitMQCheckout)
        {

        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                //log exception
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConnection();
            return _connection != null;
        }

        private void CreateModel()
        {
            if (ConnectionExists())
            {
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
            }
        }
    }
}
