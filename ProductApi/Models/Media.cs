using Microsoft.AspNetCore.Mvc.Formatters;
using ProductApi.Models.Base;

namespace ProductApi.Models
{
    public class Image : AuditableBaseEntity
    {
        public string ImageName { get; set; }

        public string DisplayName { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }
    }
}
