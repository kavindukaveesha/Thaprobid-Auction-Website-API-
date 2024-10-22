using api.Dto.Auth;
using api.Models;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IUserService
    {
        #region User Management

        Task<AppUser> GetAppUserByIdAsync(int id);
        Task<AppUser> CreateAppUserAsync(UserRegistrationRequestDto appUserDto);
        Task<AppUser> UpdateAppUserAsync(int id, AppUser appUser);
        Task<bool> DeleteAppUserAsync(int id);
        Task<AppUser> GetAppUserByEmailAsync(string email);

        #endregion

        #region Authentication

        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        #endregion

        #region Email Confirmation

        Task<bool> ConfirmEmailAsync(string appUserId, string token);
        Task SendConfirmationEmailAsync(AppUser user);

        #endregion

        #region Password Reset

        Task<ResetPasswordResponseDto> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordDto);

        #endregion
    }
}