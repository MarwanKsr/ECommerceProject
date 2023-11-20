using ShoppingCartApi.Models.Base;

namespace ShoppingCartApi.Models
{
    public class Image : BaseEntity
    {
        public string ImageName { get; set; }

        public string DisplayName { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public string Path { get; set; }
    }
}
