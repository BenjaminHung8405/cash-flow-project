using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.API.Domain.Entities
{
    /// <summary>
    /// Represents a Transaction/Voucher (Receipt or Payment) in the multi-tenant system.
    /// </summary>
    public class Transaction : BaseTenantEntity
    {
        [Required]
        [MaxLength(50)]
        public string VoucherCode { get; set; }

        [Required]
        [MaxLength(10)]
        public string TransactionType { get; set; } // "IN" (Receipt), "OUT" (Payment)

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public Guid FundId { get; set; }

        [ForeignKey(nameof(FundId))]
        public virtual Fund Fund { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
