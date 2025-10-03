using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace api.Controllers.Admin
{
    [Route("api/admin/usemannagemet")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Requires the user to be in the "Admin" role
    public class UserManagementController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserManagementController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        // GET: api/admin/usermanagement/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsersWithProfiles()
        {
            try
            {
                var usersWithProfiles = await _userProfileService.GetAllUsersWithProfilesAsync();
                return Ok(usersWithProfiles);
            }
            catch (Exception ex)
            {
                // Log the exception using a logging framework (e.g., Serilog, NLog)
                Console.WriteLine($"Error fetching users: {ex.Message}"); // For debugging purposes only
                return StatusCode(500, "An error occurred while fetching users.");
            }
        }

        // GET: api/admin/usermanagement/users/{userId}
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserProfileById(int userId)
        {
            try
            {
                var userProfile = await _userProfileService.GetUserProfileAsync(userId);
                if (userProfile == null)
                {
                    return NotFound("User not found.");
                }
                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the user.");
            }
        }

        // POST: api/admin/usermanagement/users/{userId}/activate
        [HttpPost("users/{userId}/activate")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            try
            {
                await _userProfileService.ActivateUserAsync(userId);
                return Ok("User activated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error activating user: {ex.Message}");
                return StatusCode(500, "An error occurred while activating the user.");
            }
        }

        // POST: api/admin/usermanagement/users/{userId}/deactivate
        [HttpPost("users/{userId}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            try
            {
                await _userProfileService.DeactivateUserAsync(userId);
                return Ok("User deactivated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error deactivating user: {ex.Message}");
                return StatusCode(500, "An error occurred while deactivating the user.");
            }
        }

        // ... (Add more actions for other admin features: edit user, delete user, etc.)
    }
}