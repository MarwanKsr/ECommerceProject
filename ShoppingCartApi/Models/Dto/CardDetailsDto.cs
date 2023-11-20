namespace ShoppingCartApi.Models.Dto
{
    public class CardDetailsDto
    {
        public long Id { get; set; }
        public CardHeaderDto CardHeader { get; set; }
        public ProductDto Product { get; set; }
        public int Count { get; set; }

        public static CardDetailsDto FromEntity(CardDetails cardDetails)
        {
            return new()
            {
                Id = cardDetails.Id,
                CardHeader = CardHeaderDto.FromEntity(cardDetails.CardHeader),
                Product = ProductDto.FromEntity(cardDetails.Product),
                Count = cardDetails.Count
            };
        }
    }
}
