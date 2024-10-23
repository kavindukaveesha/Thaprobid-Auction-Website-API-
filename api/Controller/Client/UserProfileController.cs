using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller.Client
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        // GET: api/profile/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetailsById(int userId)
        {
            try
            {
                // string currentUserId = GetCurrentUserId();

                // // Security Check: Ensure the logged-in user can only access their own profile
                // if (currentUserId != userId.ToString())
                // {
                //     return Forbid(); // 403 Forbidden
                // }

                var userProfile = await _userProfileService.GetUserProfileAsync(userId);
                return Ok(userProfile);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // User not found
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching user details: {ex.Message}"); // For debugging only
                return StatusCode(500, "An error occurred while fetching user details.");
            }
        }


        [HttpPost("update-first-name")]
        public async Task<IActionResult> UpdateFirstName([FromBody] UpdateFirstNameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userProfileService.UpdateFirstNameAsync(request.UserId, request.FirstName);
            return Ok("First name updated successfully.");
        }

        [HttpPost("update-last-name")]
        public async Task<IActionResult> UpdateLastName([FromBody] UpdateLastNameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userProfileService.UpdateLastNameAsync(request.UserId, request.LastName);
            return Ok("Last name updated successfully.");
        }

        [HttpPost("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userProfileService.UpdateEmailAsync(request.UserId, request.Email);
            return Ok("Email updated successfully.");
        }

        [HttpPost("update-mobile")]
        public async Task<IActionResult> UpdateMobile([FromBody] UpdateMobileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userProfileService.UpdateMobileAsync(request.UserId, request.Mobile);
            return Ok("Mobile number updated successfully.");
        }

        [HttpPost("update-profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdateProfilePictureRequest request)
        {
            if (request.ProfilePicture == null || request.ProfilePicture.Length == 0)
            {
                return BadRequest("Profile picture is required.");
            }

            await _userProfileService.UpdateProfilePictureAsync(request.UserId, request.ProfilePicture);
            return Ok("Profile picture updated successfully.");
        }

        [HttpPost("update-address")]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userProfileService.UpdateAddressAsync(request.UserId, request.Address);
            return Ok("Address updated successfully.");
        }
        // Add more actions for other profile update operations (update email, etc.) 
        // using the same pattern as above.
        private string GetCurrentUserId()
        {
            // In ASP.NET Core, you can typically get the current user's ID from the User object in the HttpContext.
            // Example (assuming you are using JWT-based authentication and the user ID is stored as a claim):

            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_id"); // Replace "user_id" with the actual claim type you are using

            return userIdClaim?.Value; // This will return the user ID if found, otherwise null
        }
    }
}

// Request models for each update operation
public class UpdateFirstNameRequest
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
}

public class UpdateLastNameRequest
{
    public int UserId { get; set; }
    public string LastName { get; set; }
}

public class UpdateEmailRequest
{
    public int UserId { get; set; }
    public string Email { get; set; }
}

public class UpdateMobileRequest
{
    public int UserId { get; set; }
    public string Mobile { get; set; }
}

public class UpdateProfilePictureRequest
{
    public int UserId { get; set; }
    public IFormFile ProfilePicture { get; set; }
}

public class UpdateAddressRequest
{
    public int UserId { get; set; }
    public string Address { get; set; }
}
