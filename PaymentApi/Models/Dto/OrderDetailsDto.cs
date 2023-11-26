namespace PaymentApi.Models.Dto
{
    public class OrderDetailsDto
    {
        public long Id { get; set; }
        public OrderHeaderDto OrderHeader { get; set; }
        public ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
