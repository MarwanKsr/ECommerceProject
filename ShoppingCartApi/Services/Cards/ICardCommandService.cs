using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Services.Cards
{
    public interface ICardCommandService
    {
        Task<CardDto> CreateUpdateCard(CardDto cardDto);
        Task<bool> RemoveFromCard(long cardDetailsId);
        Task<bool> ClearCard(string userId);
    }
}
