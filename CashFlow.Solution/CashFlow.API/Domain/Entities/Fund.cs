using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.API.Domain.Entities
{
    /// <summary>
    /// Represents a Fund/Account in the multi-tenant system (Cash or Bank Account).
    /// </summary>
    public class Fund : BaseTenantEntity
    {
        [Required]
        [MaxLength(100)]
        public string FundName { get; set; }

        [MaxLength(20)]
        public string FundType { get; set; } // "CASH", "BANK"

        [MaxLength(50)]
        public string AccountNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
