using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.RabbitMQSender;
using ShoppingCardApi.Models.ViewModel;
using ShoppingCardApi.Services.Products;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Services.Cards;

namespace ShoppingCardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Consumer")]
    public class CheckOutController : ControllerBase
    {
        private readonly ICardQueryService _cardQueryService;
        private readonly ICardCommandService _cardCommandService;
        private readonly IProductCommandService _productCommandService;
        private readonly IProductService _productService;
        private readonly IRabbitMQSender _rabbitMQSender;
        protected ResponseDto _response;

        public CheckOutController(
            ICardQueryService cardQueryService,
            ICardCommandService cardCommandService,
            IProductCommandService productCommandService,
            IProductService productService,
            IRabbitMQSender rabbitMQSender)
        {
            _cardQueryService = cardQueryService;
            _cardCommandService = cardCommandService;
            _productCommandService = productCommandService;
            _productService = productService;
            _rabbitMQSender = rabbitMQSender;
            _response = new ResponseDto();
        }

        [HttpPost("DoCheckout")]
        public async Task<object> DoCheckout([FromBody] CheckoutModel checkoutModel)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                CardDto cardDto = await _cardQueryService.GetCardByUserId(checkoutModel.UserId);
                if (cardDto == null)
                {
                    return BadRequest();
                }

                var cardDetails = cardDto.CardDetails;
                foreach (var item in cardDetails)
                {
                    var response = await _productService.GetProductPriceById<ResponseDto>(item.Product.ProductId ,accessToken);
                    if (response == null || !response.IsSuccess)
                    {
                        throw new ArgumentException("Error occurs while call product's price action");
                    }
                    var productCurrentPrice = Convert.ToDouble(response.Result);
                    if (productCurrentPrice != item.Product.Price)
                    {
                        await _productCommandService.UpdateProductPrice(item.Product.Id, productCurrentPrice);
                        throw new ArgumentException($"{item.Product.Name}'s price has changed please refresh the page");
                    }
                }

                var rabbitMqCheckoutModel = new RabbitMQCheckoutModel()
                {
                    CheckoutModel = checkoutModel,
                    CardDetails = cardDetails,
                    MessageCreated = DateTime.UtcNow,
                    AccessToekn = accessToken
                };

                _rabbitMQSender.SendMessage(rabbitMqCheckoutModel, "checkoutqueue");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
