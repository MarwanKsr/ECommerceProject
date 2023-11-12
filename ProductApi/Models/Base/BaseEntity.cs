using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models.Base
{
    public abstract class BaseEntity
    {
        public long Id { get; init; }
    }

    public interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        public void AuditCreate(string createdBy);
        string ModifiedBy { get; }
        DateTime? ModifiedAt { get; }
        void AuditModify(string modifiedBy);
    }

    public abstract class AuditableBaseEntity : BaseEntity, IAuditable
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public void AuditCreate(string createdBy)
        {
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public string ModifiedBy { get; private set; }

        public DateTime? ModifiedAt { get; private set; }
        public void AuditModify(string modifiedBy)
        {
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }
        
    }
}
