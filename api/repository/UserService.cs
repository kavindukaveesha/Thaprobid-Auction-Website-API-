using api.data;
using api.Dto.Auth;
using api.Dto.EmailDto;
using api.Handlers;
using api.Interfaces;
using api.Models;
using api.Models.security;
using api.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDBContext _context;
        private readonly ItockenService _tokenService;
        private readonly IEmailRepository _emailService;
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly MobileVerificationService _mobileVerification;

        public UserService(ApplicationDBContext context,
                           ItockenService tokenService,
                           IEmailRepository emailService,
                           IOptions<EmailSettings> emailSettings, MobileVerificationService mobileVerification)
        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
            _emailSettings = emailSettings;
            _mobileVerification = mobileVerification;
        }

        #region User Management

        public async Task<AppUser> CreateAppUserAsync(UserRegistrationRequestDto appUserDto, string role)
        {
            if (appUserDto == null)
            {
                throw new ArgumentNullException(nameof(appUserDto));
            }
            if (UserExists(appUserDto.Email))
            {
                throw new NotFoundExe("User with this email already exists.");
            }

            CreatePasswordHash(appUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new AppUser
            {
                FirstName = appUserDto.FirstName,
                LastName = appUserDto.LastName,
                Email = appUserDto.Email,
                MobileNumber = appUserDto.Mobile,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                ConfirmedEmail = false,
                ConfirmedMobile = false,
                CreatedDate = DateTime.Now,
                IsActive = true,
                Role = role

            };

            _context.AppUsers.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<AppUser> GetAppUserByIdAsync(int id)
        {
            return await _context.AppUsers.FindAsync(id);
        }

        public async Task<AppUser> GetAppUserByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        // Example implementation (replace with your own logic)
        public async Task<AppUser> UpdateAppUserAsync(int id, AppUser updatedUser)
        {
            var existingUser = await _context.AppUsers.FindAsync(id);

            if (existingUser == null)
            {
                throw new NotFoundExe("User not found.");
            }

            // Update properties (excluding sensitive data like passwords)
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            // ... update other allowed properties

            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Example implementation (replace with your own logic)
        public async Task<bool> DeleteAppUserAsync(int id)
        {
            var user = await _context.AppUsers.FindAsync(id);

            if (user == null)
            {
                throw new NotFoundExe("User not found.");
            }

            _context.AppUsers.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Authentication

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await GetAppUserByEmailAsync(loginDto.Email);

            if (user == null || !VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new InvalidCredentialException("Invalid email or password.");
            }

            if (!user.ConfirmedEmail)
            {
                throw new EmailNotConfirmedException("Email address not confirmed.");
            }

            var token = _tokenService.CreateToken(user);

            return new AuthResponseDto
            {

                User = user,
                UserId = user.Id,
                Token = token,
                Email = user.Email

            };
        }



        #endregion

        #region Email Confirmation

        public async Task<bool> ConfirmEmailAsync(string appUserId, string token)
        {
            // Retrieve the user by their ID
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Id.ToString() == appUserId);

            if (user == null)
            {
                // User not found
                throw new InvalidOperationException("User  not found.");
            }

            // Validate the token
            var isTokenValid = _tokenService.ValidateToken(token);

            if (!isTokenValid)
            {
                // Invalid token
                throw new InvalidOperationException("Invalid email confirmation token.");
            }

            if (user.ConfirmedEmail)
            {
                return true; // Already confirmed
            }

            // Confirm the user's email
            user.ConfirmedEmail = true;
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task SendConfirmationEmailAsync(AppUser user)
        {
            var token = _tokenService.CreateToken(user); // Generate a confirmation token 

            // In a real-world scenario, you'd likely store this token temporarily in the 
            // database and associate it with the user for later validation.

            var confirmationLink = $"http://localhost:5173/verification?userId={user.Id}&token={token}";

            var emailDto = new EmailConfigDto
            {
                RecieverEmail = user.Email,
                Subject = "Confirm Your Email Address",
                Body = $"<p>Please confirm your email address by clicking the following link:</p><a href='{confirmationLink}'>Confirm Email</a>"
            };

            await _emailService.SendEmailAsync(emailDto);
        }

        #endregion

        #region Password Reset

        public async Task<ResetPasswordResponseDto> ForgotPasswordAsync(string email)
        {
            var user = await GetAppUserByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundExe("User not found.");
            }

            // Generate and store a new token
            var resetToken = GeneratePasswordResetToken();
            var passwordResetToken = new PasswordResetToken
            {
                Token = resetToken,
                Expiration = DateTime.UtcNow.AddHours(1),
                AppUser = user // Associate the token with the user
            };
            _context.PasswordResetTokens.Add(passwordResetToken);
            await _context.SaveChangesAsync();

            return new ResetPasswordResponseDto { Email = user.Email };
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordDto)
        {
            // You might need .Include(u => u.PasswordResetToken) if you haven't eager loaded it
            var user = await _context.AppUsers
                .Include(u => u.PasswordResetToken)
                .FirstOrDefaultAsync(u => u.PasswordResetToken != null &&
                                       u.PasswordResetToken.Token == resetPasswordDto.Token &&
                                       u.PasswordResetToken.Expiration > DateTime.UtcNow);

            if (user == null)
            {
                throw new BadRequestException("Invalid or expired reset token.");
            }

            // Update the user's password
            CreatePasswordHash(resetPasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Remove the used token (or mark it as used if you need a history)
            _context.PasswordResetTokens.Remove(user.PasswordResetToken);

            await _context.SaveChangesAsync();
            return true;
        }

        //mobile number verification
        public async Task<bool> ConfirmMobileNumberAsync(string appUserId, string otp)
        {
            // Retrieve the user by their ID
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Id.ToString() == appUserId);

            if (user == null)
            {
                // User not found
                throw new InvalidOperationException("User  not found.");
            }

            // Verify the OTP
            bool isOtpVerified = _mobileVerification.VerifyOtp(user.MobileNumber, otp);

            if (!isOtpVerified)
            {
                // Invalid OTP
                throw new InvalidOperationException("Invalid OTP.");
            }

            if (user.ConfirmedMobile)
            {
                return true; // Already confirmed
            }

            // Confirm the user's mobile number
            user.ConfirmedMobile = true;
            await _context.SaveChangesAsync();

            return true;
        }























        private string GeneratePasswordResetToken()
        {
            // Generate a secure, unique token. 
            // In a real application, use a more robust method 
            // (e.g., RNGCryptoServiceProvider) and consider token length and complexity.
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        #endregion

        #region Helper Methods

        private bool UserExists(string email)
        {
            return _context.AppUsers.Any(u => u.Email.ToLower() == email.ToLower());
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }

        #endregion
    }

    [Serializable]
    internal class EmailNotConfirmedException : Exception
    {
        public EmailNotConfirmedException()
        {
        }

        public EmailNotConfirmedException(string? message) : base(message)
        {
        }

        public EmailNotConfirmedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}