using System.ComponentModel.DataAnnotations;

namespace CashFlow.API.Domain.Entities
{
    /// <summary>
    /// Junction/Join entity for the many-to-many relationship between Role and Permission.
    /// </summary>
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        [MaxLength(50)]
        public string PermissionCode { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
