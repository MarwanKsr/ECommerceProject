using SharedLibrary.Base.Entity;

namespace ShoppingCartApi.Models
{
    public class CardHeader : BaseEntity
    {
        public CardHeader(string userId) 
        { 
            UserId = userId;
        }
        public string UserId { get; set; }
    }
}
