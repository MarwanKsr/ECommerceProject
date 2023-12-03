using AutoMapper;
using Newtonsoft.Json;
using OrderApi.Models;
using OrderApi.Models.ViewModel;
using OrderApi.Services.Orders;
using OrderApi.Services.Products;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedLibrary.Configuration;
using SharedLibrary.Dtos;
using SharedLibrary.RabbitMQSender;
using System.Text;

namespace OrderApi.RabbitMQReceiver
{
    public class RabbitMQCheckoutReceiver : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMQSender _rabbitMQSender;
        private readonly IMapper _mapper;
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQCheckoutReceiver(
            IServiceProvider serviceProvider,
            IRabbitMQSender rabbitMQSender,
            IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _rabbitMQSender = rabbitMQSender;
            _mapper = mapper;
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
            OrderHeader orderHeader = new()
            {
                UserId = rabbitMQCheckout.CheckoutModel.UserId,
                FirstName = rabbitMQCheckout.CheckoutModel.FirstName,
                LastName = rabbitMQCheckout.CheckoutModel.LastName,
                CardNumber = rabbitMQCheckout.CheckoutModel.CardNumber,
                CVV = rabbitMQCheckout.CheckoutModel.CVV,
                Email = rabbitMQCheckout.CheckoutModel.Email,
                ExpiryMonth = rabbitMQCheckout.CheckoutModel.ExpiryMonth,
                ExpiryYear = rabbitMQCheckout.CheckoutModel.ExpiryYear,
                OrderTime = DateTime.Now,
                OrderTotal = rabbitMQCheckout.CheckoutModel.OrderTotal,
                IsSuccess = false,
                Phone = rabbitMQCheckout.CheckoutModel.Phone,
            };
            var orderDetailsList = new List<OrderDetails>();
            var orderDetailsDtoList = new List<OrderDetailsDto>();
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IProductService _productService = scope.ServiceProvider.GetService<IProductService>();  
                foreach (var detailList in rabbitMQCheckout.CardDetails)
                {
                    OrderDetails orderDetails = new()
                    {
                        Product = detailList.Product.ToEntity(),
                        Count = detailList.Count,
                        Id = detailList.Id,
                    };
                    orderHeader.CardTotalItems += detailList.Count;
                    orderDetailsList.Add(orderDetails);
                    var response = await _productService.GetProductPriceById<ResponseDto>(orderDetails.Product.ProductId, rabbitMQCheckout.AccessToekn);
                    if (response == null || !response.IsSuccess)
                    {
                        throw new ArgumentException("Error occurs while call product's price action");
                    }
                    var productCurrentPrice = Convert.ToDouble(response.Result);
                    if (productCurrentPrice != orderDetails.Product.Price)
                    {
                        throw new ArgumentException($"{orderDetails.Product.Name}'s price has changed please refresh the page");
                    }
                    orderDetailsDtoList.Add(_mapper.Map<OrderDetailsDto>(orderDetails));
                }

                IOrderCommandService _orderCommandService =
                    scope.ServiceProvider.GetRequiredService<IOrderCommandService>();

                var isSuccess = await _orderCommandService.AddOrder(orderHeader, orderDetailsList);
                if (!isSuccess)
                {
                    throw new ArgumentException("An error occurs while Adding order");
                }
            }
            PaymentRequestModel paymentRequestMessage = new()
            {
                OrderHeader = _mapper.Map<OrderHeaderDto>(orderHeader),
                OrderDetails = orderDetailsDtoList,
                AccessToekn = rabbitMQCheckout.AccessToekn,
                MessageCreated = DateTime.UtcNow,
            };

            _rabbitMQSender.SendMessage(paymentRequestMessage, "OrderPaymentProcessQueue");
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
