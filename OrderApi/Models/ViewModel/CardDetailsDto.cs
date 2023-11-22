namespace OrderApi.Models.ViewModel
{
    public class CardDetailsDto
    {
        public long Id { get; set; }
        public CardHeaderDto CardHeader { get; set; }
        public ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
