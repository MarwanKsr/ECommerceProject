using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repository;

namespace ShoppingCartApi.Services.Cards
{
    public class CardQueryService : ICardQueryService
    {
        private readonly IRepository<CardHeader> _cartHeaderRepository;
        private readonly IRepository<CardDetails> _cartDetailsRepository;

        public CardQueryService(
            IRepository<CardHeader> cartHeaderRepository,
            IRepository<CardDetails> cartDetailsRepository)
        {
            _cartHeaderRepository = cartHeaderRepository;
            _cartDetailsRepository = cartDetailsRepository;
        }

        public async Task<CardDto> GetCardByUserId(string userId)
        {
            var cardHeader = await _cartHeaderRepository.QueryAll().AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            if (cardHeader == null)
            {
                return default;
            }
            var cardDetails = await _cartDetailsRepository.Query(e => e.CardHeader.Id == cardHeader.Id).AsNoTracking().ToListAsync();
            Card card = new()
            {
                CardHeader = cardHeader,
                CardDetails = cardDetails
            };
            var cardDetailsDto = new List<CardDetailsDto>();
            foreach (var item in cardDetails)
            {
                cardDetailsDto.Add(CardDetailsDto.FromEntity(item));
            }
            return CardDto.FromEntity(card, cardDetailsDto);
        }
    }
}
