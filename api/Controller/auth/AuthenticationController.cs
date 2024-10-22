using api.data;
using api.Dto.Auth;
using api.Dto.EmailDto;
using api.Handlers;
using api.Interfaces;
using api.Models;
using api.Models.security;
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

        public AuthenticationController(
            IUserService authService,
            IEmailRepository emailService,
            IOptions<EmailSettings> emailSettings)
        {
            _authService = authService;
            _emailService = emailService;
            _emailSettings = emailSettings;
        }

        // Registration Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = await _authService.CreateAppUserAsync(registerDto);
                if (newUser == null)
                {
                    return StatusCode(500, "User creation failed.");
                }

                await _authService.SendConfirmationEmailAsync(newUser);

                return Ok(new { Message = "Registration successful, please confirm your email." });
            }
            catch (NotFoundExe ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception 
                Console.WriteLine($"Registration error: {ex.Message}");
                return StatusCode(500, "An error occurred during registration.");
            }
        }

        // Email Confirmation Endpoint 
        [HttpGet("confirm-email")]
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
            catch (NotFoundExe ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred during email confirmation.");
            }
        }

        // Login Endpoint 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var authResponse = await _authService.LoginAsync(loginDto);
                return Ok(authResponse);
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
                // Log the exception
                Console.WriteLine($"Login error: {ex.Message}");
                return StatusCode(500, "An error occurred during login.");
            }
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

        // ... other endpoints you need ... 
    }
}