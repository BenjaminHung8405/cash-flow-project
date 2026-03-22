namespace CashFlow.Shared.Models
{
    /// <summary>
    /// Domain entity representing a tenant (customer/organization).
    /// In a multi-tenant architecture, each tenant has isolated data.
    /// </summary>
    public class Tenant
    {
        public int TenantId { get; set; }
        public string TenantCode { get; set; } = null!;
        public string TenantName { get; set; } = null!;
        public string? TaxCode { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? ExpireDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
