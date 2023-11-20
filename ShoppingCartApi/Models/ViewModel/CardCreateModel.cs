namespace ShoppingCardApi.Models.ViewModel
{
    public class CardCreateModel
    {
        public CardHeaderCreateModel CardHeader { get; set; }
        public IEnumerable<CardDetailsCreateModel> cardDetails { get; set; }
    }
}
