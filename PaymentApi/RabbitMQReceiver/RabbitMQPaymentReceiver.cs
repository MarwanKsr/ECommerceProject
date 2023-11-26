using AutoMapper;
using Newtonsoft.Json;
using PaymentApi.Models;
using PaymentApi.Services.Orders;
using PaymentApi.Services.Payments;
using PaymentApi.Services.Payments.Providers.Iyzico;
using PaymentApi.Services.Products;
using PaymentApi.Services.ShoppingCard;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedLibrary.Configuration;
using SharedLibrary.Dtos;
using SharedLibrary.RabbitMQSender;
using System.Text;

namespace PaymentApi.RabbitMQReceiver
{
    public class RabbitMQPaymentReceiver : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMQSender _rabbitMQSender;
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQPaymentReceiver(
            IServiceProvider serviceProvider,
            IRabbitMQSender rabbitMQSender)
        {
            _serviceProvider = serviceProvider;
            _rabbitMQSender = rabbitMQSender;
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
                PaymentModel paymentModel = JsonConvert.DeserializeObject<PaymentModel>(content);
                HandleMessage(paymentModel).GetAwaiter().GetResult();

                _channel.BasicAck(deliverEvent.DeliveryTag, false);
            };
            _channel.BasicConsume("OrderPaymentProcessQueue", false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(PaymentModel paymentModel)
        {
            if (paymentModel == null)
            {
                throw new ArgumentException("Payment model is not found");
            }
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IProductService productService = scope.ServiceProvider.GetService<IProductService>();
                IPaymentService iyzicoPaymentService = scope.ServiceProvider.GetService<IPaymentService>();
                IOrderService orderService = scope.ServiceProvider.GetService<IOrderService>();
                IShoppingCardService shoppingCardService = scope.ServiceProvider.GetService<IShoppingCardService>();

                try
                {
                    iyzicoPaymentService.Pay(paymentModel);
                    foreach (var item in paymentModel.OrderDetails)
                    {
                        var stockResponse = await productService.DecreaseProductStockById<ResponseDto>(item.Product.ProductId, item.Count, default);
                        if (stockResponse == null || !stockResponse.IsSuccess)
                        {
                            if (stockResponse.ErrorMessages.Any())
                                throw new ArgumentException(string.Join(",", stockResponse.ErrorMessages));
                            throw new ArgumentException("Error occurs while call product's stock action");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                var response = await orderService.MakeOrderSuccess<ResponseDto>(paymentModel.OrderHeader.Id, default);
                if (response == null || !response.IsSuccess)
                {
                    if (response.ErrorMessages.Any())
                        throw new ArgumentException(string.Join(",", response.ErrorMessages));
                    throw new ArgumentException("Error occurs while make order status success");
                }

                var res = await shoppingCardService.ClearCard<ResponseDto>(paymentModel.OrderHeader.UserId, default);
                if (res == null || !res.IsSuccess)
                {
                    if (res.ErrorMessages.Any())
                        throw new ArgumentException(string.Join(",", res.ErrorMessages));
                    throw new ArgumentException("Error occurs while make order status success");
                }

                MailRequestModel mailRequestModel = new()
                {
                    UserId = paymentModel.OrderHeader.UserId,
                    FullName = paymentModel.OrderHeader.FullName,
                    Email = paymentModel.OrderHeader.Email
                };

                _rabbitMQSender.SendMessage(mailRequestModel, "mailQueue");
            }
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
                _channel.QueueDeclare(queue: "OrderPaymentProcessQueue", false, false, false, arguments: null);
            }
        }
    }
}
