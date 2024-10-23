using api.data;
using api.Dto.profile;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace api.repository
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDBContext _context;

        public UserProfileService(ApplicationDBContext context)
        {
            _context = context;
        }

        #region User and Profile Retrieval

        /// <summary>
        /// Retrieves a list of all users with their associated profiles.
        /// </summary>
        /// <returns>A list of AppUserWithProfileDto objects.</returns>
        /// 
        public async Task<List<AppUserWithProfileDto>> GetAllUsersWithProfilesAsync()
        {
            return await _context.AppUsers
                .Include(u => u.ClientProfile)
                .Include(s => s.Seller) // Use SellerProfile instead of Seller
                .Select(user => new AppUserWithProfileDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    ConfirmedEmail = user.ConfirmedEmail,
                    MobileNumber = user.MobileNumber,
                    ConfirmedMobile = user.ConfirmedMobile,
                    PictureUrl = user.PictureUrl,
                    ClientAddress = user.ClientProfile.ClientAddress,
                    IsClientBidder = user.ClientProfile.IsClientBidder,
                    SellerId = user.Seller.SellerId
                })
                .ToListAsync();
        }

        public async Task<AppUserWithProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _context.AppUsers
                .Include(u => u.ClientProfile)
                .Include(s => s.Seller) // Use Seller instead of Seller
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return new AppUserWithProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ConfirmedEmail = user.ConfirmedEmail,
                MobileNumber = user.MobileNumber,
                ConfirmedMobile = user.ConfirmedMobile,
                PictureUrl = user.PictureUrl,
                ClientAddress = user.ClientProfile.ClientAddress,
                IsClientBidder = user.ClientProfile.IsClientBidder,
                SellerId = user.Seller?.SellerId
            };
        }

        /// <summary>
        /// Retrieves a client profile by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The client profile associated with the user.</returns>
        public async Task<ClientProfile> GetProfileAsync(int userId)
        {
            var profile = await _context.ClientProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                throw new InvalidOperationException("Profile not found.");
            }

            return profile;
        }

        /// <summary>
        /// Retrieves a list of all client profiles.
        /// </summary>
        /// <returns>A list of ClientProfile objects.</returns>
        public async Task<List<ClientProfile>> GetAllProfilesAsync()
        {
            return await _context.ClientProfiles.ToListAsync();
        }

        #endregion

        #region Profile Management

        /// <summary>
        /// Creates a new client profile and associates it with a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="profile">The client profile data.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task CreateProfileAsync(int userId, ClientProfile profile)
        {
            var user = await _context.AppUsers.FindAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            profile.UserId = userId;
            await _context.ClientProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region User Information Update Methods

        /// <summary>
        /// Updates a user's first name.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="firstName">The new first name.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateFirstNameAsync(int userId, string firstName)
        {
            await UpdateUserPropertyAsync(userId, user => user.FirstName = firstName);
        }

        /// <summary>
        /// Updates a user's last name.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="lastName">The new last name.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateLastNameAsync(int userId, string lastName)
        {
            await UpdateUserPropertyAsync(userId, user => user.LastName = lastName);
        }

        /// <summary>
        /// Updates a user's email address.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="email">The new email address.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateEmailAsync(int userId, string email)
        {
            // Consider adding email validation and duplicate email check
            await UpdateUserPropertyAsync(userId, user => user.Email = email);
        }

        /// <summary>
        /// Updates a user's mobile number.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="mobile">The new mobile number.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateMobileAsync(int userId, string mobile)
        {
            // Consider adding mobile number validation
            await UpdateUserPropertyAsync(userId, user => user.MobileNumber = mobile);
        }

        /// <summary>
        /// Updates a user's profile picture.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="profilePicture">The new profile picture file.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateProfilePictureAsync(int userId, IFormFile profilePicture)
        {
            var user = await _context.AppUsers.FindAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (profilePicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    user.PictureUrl = Convert.ToBase64String(memoryStream.ToArray());
                }

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates a client profile's address.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="address">The new address.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateAddressAsync(int userId, string address)
        {
            await UpdateProfilePropertyAsync(userId, profile => profile.ClientAddress = address);
        }

        #endregion

        #region User Account Status Methods

        /// <summary>
        /// Activates a user account.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task ActivateUserAsync(int userId)
        {
            await UpdateUserPropertyAsync(userId, user => user.IsActive = true);
        }

        /// <summary>
        /// Deactivates a user account.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task DeactivateUserAsync(int userId)
        {
            await UpdateUserPropertyAsync(userId, user => user.IsActive = false);
        }

        #endregion

        #region Private Helper Methods

        // A helper method to reduce code duplication when updating user properties
        private async Task UpdateUserPropertyAsync(int userId, Action<AppUser> updateAction)
        {
            var user = await _context.AppUsers.FindAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            updateAction(user); // Perform the update action on the user object
            await _context.SaveChangesAsync();
        }

        // A helper method to update client profile properties
        private async Task UpdateProfilePropertyAsync(int userId, Action<ClientProfile> updateAction)
        {
            var profile = await _context.ClientProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                throw new InvalidOperationException("Profile not found.");
            }

            updateAction(profile);
            await _context.SaveChangesAsync();
        }



        #endregion
    }
}