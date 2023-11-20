namespace ShoppingCardApi.Models.ViewModel
{
    public class ProductCreateModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
