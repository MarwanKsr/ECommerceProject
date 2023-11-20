using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Services.Cards
{
    public interface ICardQueryService
    {
        Task<CardDto> GetCardByUserId(string userId);
    }
}
