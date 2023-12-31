﻿using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using PaymentApi.Models;
using PaymentApi.Services.Payments.Providers.Iyzico.Models;
using System.Globalization;

namespace PaymentApi.Services.Payments.Providers.Iyzico
{
    public class IyzicoPaymentService : IPaymentService
    {
        private readonly Options _options;

        public IyzicoPaymentService(IyzicoPaymentSettings iyzicoSettings)
        {
            _options = new Options()
            {
                ApiKey = iyzicoSettings.ApiKey,
                SecretKey = iyzicoSettings.SecretKey,
                BaseUrl = iyzicoSettings.BaseUrl,
            };
        }
        public Payment Pay(PaymentModel payment)
        {
            var paymentCard = new PaymentCard
            {
                CardHolderName = payment.OrderHeader.FullName,
                CardNumber = payment.OrderHeader.CardNumber,
                ExpireMonth = payment.OrderHeader.ExpiryMonth,
                ExpireYear = payment.OrderHeader.ExpiryYear,
                Cvc = payment.OrderHeader.CVV,
                RegisterCard = 0,
                CardAlias = "ECommerce"
            };

            var buyer = new Buyer
            {
                Id = payment.OrderHeader.UserId,
                Name = payment.OrderHeader.FirstName,
                Surname = payment.OrderHeader.LastName,
                GsmNumber = payment.OrderHeader.Phone,
                Email = payment.OrderHeader.Email,
                IdentityNumber = "74300864791",
                LastLoginDate = "2015-10-05 12:43:35",
                RegistrationDate = "2013-04-21 15:12:09",
                RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                Ip = "85.34.78.112",
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34732"
            };

            var shippingAddress = new Address
            {
                ContactName = "Jane Doe",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ZipCode = "34742"
            };

            var billingAddress = new Address
            {
                ContactName = "Jane Doe",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ZipCode = "34742"
            };

            var basketItems = new List<BasketItem>();
            foreach (var item in payment.OrderDetails)
            {
                basketItems.Add(new()
                {
                    Id = item.Product.ProductId.ToString(),
                    Name = item.Product.Name,
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = item.Product.Price.ToString(CultureInfo.InvariantCulture),
                    Category1 = "Product"
                });
            }
            var request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = new Random().Next(1111, 9999).ToString(),
                Price = payment.OrderDetails.Sum(e => e.Product.Price).ToString(CultureInfo.InvariantCulture),
                PaidPrice = payment.OrderDetails.Sum(e => e.Product.Price).ToString(CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = payment.OrderHeader.Id.ToString(),
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                PaymentCard = paymentCard,
                Buyer = buyer,
                BasketItems = basketItems,
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress,
            };

            return Payment.Create(request, _options);
        }
    }
}
