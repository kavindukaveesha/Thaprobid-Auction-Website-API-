using api.Dto.Auth;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Controller.auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ItockenService _tokenService;
        private readonly IEmailRepository _emailSender;

        public AuthenticationController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ItockenService tokenService,
            IEmailRepository emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());
            if (user == null)
                return Unauthorized("Invalid email or password");

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized(new { Errors = new List<string> { "Email is not confirmed" } });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _tokenService.CreateToken(user);
            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            });
        }

        // Registration endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = registerDto.Name,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var confirmationUrl = Url.Action(
                        nameof(EmailVerification),
                        "Authentication",
                        new { email = appUser.Email, code = code },
                        protocol: HttpContext.Request.Scheme);

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

        // Email Verification endpoint
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
                return NotFound("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok(new { message = "Email confirmed successfully!" });
            }

            return BadRequest(new { Errors = result.Errors });
        }

        // Resend Email Confirmation endpoint
        // [HttpPost("resend-confirmation")]
        // public async Task<IActionResult> ResendConfirmation([FromBody] ResendEmailConfirmationDto model)
        // {
        //     var user = await _userManager.FindByEmailAsync(model.Email);
        //     if (user == null || await _userManager.IsEmailConfirmedAsync(user))
        //     {
        //         return BadRequest("Invalid request or email already confirmed.");
        //     }

        //     var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //     var confirmationUrl = Url.Action(nameof(EmailVerification), "Authentication", new { email = user.Email, code = code }, protocol: HttpContext.Request.Scheme);

        //     await _emailSender.SendEmailAsync(user.Email, "Resend Confirmation",
        //         $"Please confirm your email by clicking this link: <a href='{confirmationUrl}'>Confirm Email</a>");

        //     return Ok(new { message = $"A new confirmation email has been sent to {user.Email}." });
        // }
    }
}
