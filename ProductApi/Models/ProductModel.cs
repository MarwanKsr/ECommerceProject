namespace ProductApi.Models
{
    public class ProductModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }
    public class ProductCreateModel : ProductModel
    {
        
    }

    public class ProductUpdateModel : ProductModel 
    { 
        public long Id { get; set; }
    }
}
