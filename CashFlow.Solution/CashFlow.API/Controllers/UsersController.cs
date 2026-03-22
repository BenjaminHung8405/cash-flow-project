using CashFlow.API.DTOs;
using CashFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CashFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching user: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get all users for current tenant (paginated)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<UserListDto>> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var tenantIdClaim = User.FindFirst("TenantId");
                if (tenantIdClaim == null)
                    return Unauthorized(new { message = "TenantId not found in token" });

                if (!int.TryParse(tenantIdClaim.Value, out int tenantId))
                    return BadRequest(new { message = "Invalid TenantId in token" });

                var users = await _userService.GetAllUsersAsync(tenantId, pageNumber, pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching users: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Create a new user account (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserViewModel>> CreateUser([FromBody] UserCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = await _userService.CreateUserAsync(dto);
                var createdUser = await _userService.GetUserByIdAsync(userId);

                return CreatedAtAction(nameof(GetUserById), new { id = userId }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Update user information (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserViewModel>> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updated = await _userService.UpdateUserAsync(id, dto);
                if (!updated)
                    return NotFound(new { message = "User not found" });

                var updatedUser = await _userService.GetUserByIdAsync(id);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Delete a user (soft delete - Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var deleted = await _userService.DeleteUserAsync(id);
                if (!deleted)
                    return NotFound(new { message = "User not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}