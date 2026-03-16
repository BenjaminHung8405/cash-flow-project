using System.ComponentModel.DataAnnotations;

namespace CashFlow.API.Modules.Transactions.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string VoucherCode { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public Guid FundId { get; set; }
        public string FundName { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateTransactionRequest
    {
        [Required]
        [MaxLength(50)]
        public string VoucherCode { get; set; }

        [Required]
        [RegularExpression("^(IN|OUT)$", ErrorMessage = "TransactionType must be 'IN' or 'OUT'")]
        public string TransactionType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public Guid FundId { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; }

        public DateTime? TransactionDate { get; set; }
    }

    public class DonutChartDataDto
    {
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
