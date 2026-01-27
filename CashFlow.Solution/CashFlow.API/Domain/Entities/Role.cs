using System.ComponentModel.DataAnnotations;

namespace CashFlow.API.Domain.Entities
{
    /// <summary>
    /// Represents a Role in the system.
    /// </summary>
    public class Role : BaseTenantEntity
    {
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
