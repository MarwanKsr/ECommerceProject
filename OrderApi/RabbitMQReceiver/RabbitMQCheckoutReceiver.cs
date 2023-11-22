﻿using Newtonsoft.Json;
using OrderApi.Configuration;
using OrderApi.Models;
using OrderApi.Models.ViewModel;
using OrderApi.RabbitMQSender;
using OrderApi.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OrderApi.RabbitMQReceiver
{
    public class RabbitMQCheckoutReceiver : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMQSender _rabbitMQSender;
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQCheckoutReceiver(
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
                //OrderDetails = new List<OrderDetails>(),
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
            foreach (var detailList in rabbitMQCheckout.CardDetails)
            {
                OrderDetails orderDetails = new()
                {
                    Product = detailList.Product.ToEntity(),
                    Count = detailList.Count,
                    Id = detailList.Id,
                };
                orderHeader.CardTotalItems += detailList.Count;
                //orderHeader.OrderDetails.Add(orderDetails);
            }
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IOrderCommandService _orderCommandService =
                    scope.ServiceProvider.GetRequiredService<IOrderCommandService>();

                var isSuccess = await _orderCommandService.AddOrder(orderHeader);
                if (!isSuccess)
                {
                    throw new ArgumentException("An error occurs while Adding order");
                }
            }

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonth = orderHeader.ExpiryMonth,
                ExpiryYear = orderHeader.ExpiryYear,
                OrderId = orderHeader.Id,
                OrderTotal = orderHeader.OrderTotal,
                Email = orderHeader.Email
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
