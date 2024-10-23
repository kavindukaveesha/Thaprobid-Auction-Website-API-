using api.data;
using api.dto.response;
using api.Dto.Auth;
using api.Dto.EmailDto;
using api.Dto.mobile;
using api.Handlers;
using api.Interfaces;
using api.Models;
using api.Models.security;
using api.repository;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;



namespace api.Controller.auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _authService;
        private readonly IEmailRepository _emailService;
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly MobileVerificationService _mobileVerification;
        private readonly IUserProfileService _userProfileService;

        public AuthenticationController(
            IUserService authService,
            IEmailRepository emailService,
            IOptions<EmailSettings> emailSettings,
             MobileVerificationService mobileVerification,
             IUserProfileService userProfileService)
        {
            _authService = authService;
            _emailService = emailService;
            _emailSettings = emailSettings;
            _mobileVerification = mobileVerification;
            _userProfileService = userProfileService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string role = Role.Client.ToString();

            try
            {
                var newUser = await _authService.CreateAppUserAsync(registerDto, role);
                if (newUser == null)
                {
                    // Consider logging a warning here, as user creation failing is significant
                    return StatusCode(500, "User creation failed. Please try again later.");
                }

                var clientProfile = new ClientProfile
                {
                    UserId = newUser.Id,
                    ClientAddress = "",
                    IsClientBidder = false,
                };

                try
                {
                    await _userProfileService.CreateProfileAsync(newUser.Id, clientProfile);
                }
                catch (Exception profileEx)
                {
                    // Log the specific profile creation exception:
                    Console.WriteLine($"Profile creation error for user {newUser.Id}: {profileEx.Message}");

                    // 500 Internal Server Error, as this is unexpected
                    return StatusCode(500, "An error occurred while creating the user profile.");
                }

                try
                {
                    await _authService.SendConfirmationEmailAsync(newUser);
                }
                catch (Exception emailEx)
                {
                    // Log the email sending exception:
                    Console.WriteLine($"Error sending confirmation email to {newUser.Email}: {emailEx.Message}");

                    // Consider returning a 201 Created here, but with a warning message in the response body 
                    return CreatedAtAction(nameof(Register), newUser, // Assuming you have a GET action to retrieve a user
                                      new
                                      {
                                          Message = "Registration successful, but there was a problem sending the confirmation email. Please contact support.",
                                          User = newUser // Optionally return user details 
                                      });
                }

                return Ok(new { Message = "Registration successful, please confirm your email." });
            }
            catch (NotFoundExe ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request is suitable for NotFoundExe
            }
            catch (DbUpdateException dbEx)
            {
                // Log the database exception:
                Console.WriteLine($"Database error during registration: {dbEx.Message}");

                // Check for potential causes like duplicate keys (if applicable):
                if (dbEx.InnerException?.Message.Contains("duplicate key") == true)
                {
                    return Conflict("A user with these details already exists."); // 409 Conflict
                }

                return StatusCode(500, "A database error occurred during registration.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return StatusCode(500, "An error occurred during registration."); // 500 Internal Server Error 
            }
        }

        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid email confirmation request.");
            }

            try
            {
                bool success = await _authService.ConfirmEmailAsync(userId, token);
                if (success)
                {
                    return Ok("Email confirmed successfully!");
                }
                else
                {
                    return BadRequest("Email confirmation failed.");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred during email confirmation.");
            }
        }
        // Login Endpoint 
        // Login Endpoint 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Attempt to login the user
                var authResponse = await _authService.LoginAsync(loginDto);

                // Set user ID and token into cookies
                SetLoginSession(authResponse.User); // Assuming authResponse has the User object
                Response.Cookies.Append("UserId", authResponse.User.Id.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1) // Set expiration time
                });
                Response.Cookies.Append("Token", authResponse.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1) // Set expiration time
                });

                return Ok(new ApiSuccessDto(200, "Login Success", authResponse));
            }
            catch (InvalidCredentialException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (EmailNotConfirmedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Login error: {ex.Message}");
                return StatusCode(500, "An error occurred during login.");
            }
        }

        // Method to set session or cookie
        private void SetLoginSession(AppUser user)
        {
            // Configure cookie options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Expires = DateTimeOffset.UtcNow.AddHours(1) // Set cookie expiration time
            };

            // Append cookies for user ID and email
            Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);
            Response.Cookies.Append("UserEmail", user.Email, cookieOptions);
        }


        // Forgot Password Endpoint
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resetResponse = await _authService.ForgotPasswordAsync(forgotPasswordDto.Email);
                // You might want to send an email to the user here (using _emailService) 
                // with a link containing the resetResponse.Token.
                // Make sure this link points to your frontend password reset page.
                return Ok(resetResponse);
            }
            catch (NotFoundExe ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Forgot password error: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Reset Password Endpoint
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool success = await _authService.ResetPasswordAsync(resetPasswordDto);
                if (success)
                {
                    return Ok("Password reset successfully!");
                }
                else
                {
                    // This shouldn't happen if ResetPasswordAsync is implemented correctly,
                    // but it's good practice to handle it.
                    return StatusCode(500, "An error occurred during password reset.");
                }
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Reset password error: {ex.Message}");
                return StatusCode(500, "An error occurred during password reset.");
            }
        }

        // Send OTP to the mobile number
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] MobilenumberRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _mobileVerification.SendOtpAsync(request.MobileNumber);
            return Ok("OTP sent successfully.");
        }

        // Verify the OTP entered by the user
        [HttpPost("confirm-mobile")]
        public async Task<IActionResult> ConfirmMobile([FromBody] MobileConfirmationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _authService.ConfirmMobileNumberAsync(request.AppUserId, request.Otp);
                return Ok("Mobile number confirmed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return the error message if the confirmation fails
            }
        }



    }
}

