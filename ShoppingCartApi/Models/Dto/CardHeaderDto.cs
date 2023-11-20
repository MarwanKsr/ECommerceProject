namespace ShoppingCartApi.Models.Dto
{
    public class CardHeaderDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }

        public static CardHeaderDto FromEntity(CardHeader cartHeader)
        {
            return new()
            {
                Id = cartHeader.Id,
                UserId = cartHeader.UserId,
            };
        }
    }
}
