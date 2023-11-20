namespace ShoppingCartApi.Models
{
    public class Card
    {
        public CardHeader CardHeader { get; set; }
        public IEnumerable<CardDetails> CardDetails { get; set; }
    }
}
