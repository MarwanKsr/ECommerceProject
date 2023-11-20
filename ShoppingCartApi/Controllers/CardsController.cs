using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCardApi.Models.ViewModel;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Services.Cards;

namespace ShoppingCardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardCommandService _cardCommandService;
        private readonly ICardQueryService _cardQueryService;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public CardsController(
            ICardCommandService cardCommandService,
            ICardQueryService cardQueryService,
            IMapper mapper)
        {
            _cardCommandService = cardCommandService;
            _cardQueryService = cardQueryService;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CardDto cartDto = await _cardQueryService.GetCardByUserId(userId);
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }



        [HttpPost("CreateCard")]
        public async Task<object> CreateCard([FromBody] CardCreateModel createModel)
        {
            try
            {
                var cardDto = _mapper.Map<CardDto>(createModel);
                CardDto model = await _cardCommandService.CreateUpdateCard(cardDto);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCard/{cardId}")]
        public async Task<object> RemoveCard(int cardId)
        {
            try
            {
                bool isSuccess = await _cardCommandService.RemoveFromCard(cardId);
                _response.Result = isSuccess;
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
