namespace CashFlow.Shared.Models
{
    /// <summary>
    /// Domain entity representing a user account.
    /// User accounts are strictly provisioned by the Administrator.
    /// Email field is intentionally excluded as users are admin-provisioned.
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public int TenantId { get; set; }
        public int? BranchId { get; set; }
        public int RoleId { get; set; }

        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Role? Role { get; set; }
        public virtual Branch? Branch { get; set; }
        public virtual Tenant? Tenant { get; set; }
    }
}