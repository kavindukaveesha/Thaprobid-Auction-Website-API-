using api.Dto.profile;
using api.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IUserProfileService
    {
        #region User and Profile Retrieval

        Task<List<AppUserWithProfileDto>> GetAllUsersWithProfilesAsync();
        Task<AppUserWithProfileDto> GetUserProfileAsync(int userId);
        Task<ClientProfile> GetProfileAsync(int userId);
        Task<List<ClientProfile>> GetAllProfilesAsync();

        #endregion

        #region Profile Management

        Task CreateProfileAsync(int userId, ClientProfile profile);

        #endregion

        #region User Information Update

        Task UpdateFirstNameAsync(int userId, string firstName);
        Task UpdateLastNameAsync(int userId, string lastName);
        Task UpdateEmailAsync(int userId, string email);
        Task UpdateMobileAsync(int userId, string mobile);
        Task UpdateProfilePictureAsync(int userId, IFormFile profilePicture);

        #endregion

        #region Address Management

        Task UpdateAddressAsync(int userId, string address);

        #endregion

        #region Account Status Management

        Task ActivateUserAsync(int userId);
        Task DeactivateUserAsync(int userId);

        #endregion
    }
}