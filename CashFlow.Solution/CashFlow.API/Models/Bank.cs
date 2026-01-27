using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.API.Models
{
    // DEPRECATED: This file is kept for backward compatibility only.
    // All new code should use entities from CashFlow.API.Domain.Entities namespace
    // See: CashFlow.API/Domain/Entities/ for the modern entity definitions

    public class Bank
    {
        [Obsolete("Use CashFlow.API.Domain.Entities.BaseTenantEntity instead")]
        public abstract class BaseTenantEntity
        {
            [Key]
            public Guid Id { get; set; } = Guid.NewGuid();
            public Guid TenantId { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public string CreatedBy { get; set; }
        }
    }
}
