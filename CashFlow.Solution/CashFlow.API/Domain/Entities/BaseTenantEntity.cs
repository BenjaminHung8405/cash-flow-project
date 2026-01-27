using System.ComponentModel.DataAnnotations;

namespace CashFlow.API.Domain.Entities
{
    /// <summary>
    /// Base abstract class for all tenant-scoped entities in the multi-tenant system.
    /// Ensures every entity is isolated to a specific tenant and tracks creation metadata.
    /// </summary>
    public abstract class BaseTenantEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TenantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; }
    }
}
