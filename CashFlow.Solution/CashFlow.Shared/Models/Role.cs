namespace CashFlow.Shared.Models
{
    /// <summary>
    /// Domain entity representing a user role.
    /// Roles define permissions and responsibilities within a tenant.
    /// </summary>
    public class Role
    {
        public int RoleId { get; set; }
        public int TenantId { get; set; }
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }

        // Navigation properties
        public virtual Tenant? Tenant { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
