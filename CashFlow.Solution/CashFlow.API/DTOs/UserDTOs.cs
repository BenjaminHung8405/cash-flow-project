using System.ComponentModel.DataAnnotations;

namespace CashFlow.API.DTOs
{
    /// <summary>
    /// DTO for creating a new user account.
    /// Email is not required as users are provisioned by the Administrator.
    /// </summary>
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username must not exceed 50 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 100 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name must not exceed 100 characters")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Tenant is required")]
        public int TenantId { get; set; }

        public int? BranchId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO for updating an existing user account.
    /// </summary>
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name must not exceed 100 characters")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }

        public int? BranchId { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for returning user data in API responses.
    /// Sensitive data like PasswordHash is never returned to the client.
    /// </summary>
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public int TenantId { get; set; }
        public int? BranchId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// DTO for bulk user operations.
    /// </summary>
    public class UserListDto
    {
        public int TotalCount { get; set; }
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }
}