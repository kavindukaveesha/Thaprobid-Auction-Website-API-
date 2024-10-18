using api.Dto.Auth;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controller.auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ItockenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ItockenService tokenService,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find user by email (case-insensitive)
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());
            if (user == null)
                return Unauthorized("Invalid email or password");

            // Check if email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized(new
                {
                    Errors = new List<string> { "Email is not confirmed" }
                });
            }

            // Use SignInManager for password check and handling lockout, etc.
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid email or password");
            }

            // Generate and return token on successful login
            var token = _tokenService.CreateToken(user);
            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.Name,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        // Generate email confirmation token
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

                        // Create the confirmation URL
                        var confirmationUrl = Url.Action(
                            nameof(EmailVerification),
                            "Authentication",
                            new { email = appUser.Email, code = code },
                            protocol: HttpContext.Request.Scheme);

                        // Send email using an email service (e.g., SendGrid, SMTP)
                        await _emailSender.SendEmailAsync(appUser.Email, "Confirm your email",
                            $"Please confirm your email by clicking this link: <a href='{confirmationUrl}'>Confirm Email</a>");

                        return Ok(new
                        {
                            message = $"Please confirm your email by checking your inbox: {appUser.Email}"
                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // Email verification endpoint that handles email confirmation via the link
        [HttpGet("verify-email")]
        public async Task<IActionResult> EmailVerification(string email, string code)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
            {
                return BadRequest("Invalid payload. Email or code cannot be null.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid payload. User not found.");
            }

            // Pass the correct code to confirm the email
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    message = "Email confirmed successfully!"
                });
            }

            return BadRequest("Something went wrong while confirming the email.");
        }


    }
}
