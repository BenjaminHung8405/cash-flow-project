namespace CashFlow.Shared.Models
{
    /// <summary>
    /// Domain entity representing a branch/store location.
    /// Each branch belongs to a tenant and can have multiple users and funds.
    /// </summary>
    public class Branch
    {
        public int BranchId { get; set; }
        public int TenantId { get; set; }
        public string BranchName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Tenant? Tenant { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
