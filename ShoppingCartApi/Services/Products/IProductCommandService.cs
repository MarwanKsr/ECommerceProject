namespace ShoppingCardApi.Services.Products
{
    public interface IProductCommandService
    {
        Task<bool> UpdateProductPrice(long Id, double newPrice);
    }
}
