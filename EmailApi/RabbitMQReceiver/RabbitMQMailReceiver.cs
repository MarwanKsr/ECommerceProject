using EmailApi.Models;
using EmailApi.Models.Emails;
using EmailApi.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedLibrary.Configuration;
using System.Text;

namespace EmailApi.RabbitMQReceiver
{
    public class RabbitMQMailReceiver : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQMailReceiver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                EmailModel mailModel = JsonConvert.DeserializeObject<EmailModel>(content);
                HandleMessage(mailModel).GetAwaiter().GetResult();

                _channel.BasicAck(deliverEvent.DeliveryTag, false);
            };
            _channel.BasicConsume("mailQueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task HandleMessage(EmailModel mailModel)
        {
            if (mailModel == null)
            {
                throw new ArgumentException("Mail model is not found");
            }

            using IServiceScope scope = _serviceProvider.CreateScope();
            IEmailMessageSender emailMessageSender = scope.ServiceProvider.GetService<IEmailMessageSender>();
            var orderConfirmation = new OrderConfirmationEmailModel()
            {
                FullName = mailModel.FullName,
                To = mailModel.Email,
                OrderHeaderId = mailModel.OrderHeaderId,
            };

            await emailMessageSender.SendMessageAsync(orderConfirmation.To, orderConfirmation.Subject, orderConfirmation.Body);
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
                _channel.QueueDeclare(queue: "mailQueue", false, false, false, arguments: null);
            }
        }
    }
}
