using ShoppingCartApi.Models.Base;

namespace ShoppingCartApi.Models
{
    public class CardDetails : BaseEntity
    {
        protected CardDetails() { }
        public CardDetails(CardHeader cardHeader, Product product, int count) 
        {
            CardHeader = cardHeader;
            Product = product;
            Count = count;
        }
        public CardHeader CardHeader { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
