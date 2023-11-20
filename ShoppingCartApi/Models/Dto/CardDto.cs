namespace ShoppingCartApi.Models.Dto
{
    public class CardDto
    {
        public CardHeaderDto CardHeader { get; set; }
        public IEnumerable<CardDetailsDto> CardDetails { get; set; }

        public static CardDto FromEntity(Card card, IEnumerable<CardDetailsDto> cardDetailsDtos)
        {
            return new()
            {
                CardHeader = CardHeaderDto.FromEntity(card.CardHeader),
                CardDetails = cardDetailsDtos,
            };
        }
    }
}
