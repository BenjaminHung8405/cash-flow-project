using System.ComponentModel.DataAnnotations;

namespace CashFlow.API.Domain.Entities
{
    /// <summary>
    /// Represents an Audit Log entry tracking all data changes across the system.
    /// </summary>
    public class AuditLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TenantId { get; set; }

        public string UserId { get; set; }

        [MaxLength(50)]
        public string Action { get; set; } // "INSERT", "UPDATE", "DELETE"

        [MaxLength(100)]
        public string TableName { get; set; }

        [MaxLength(50)]
        public string RecordId { get; set; }

        public string OldValues { get; set; } // JSON serialized

        public string NewValues { get; set; } // JSON serialized

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
