using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dto.seller;
using api.Handlers;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.repository
{
    /// <summary>
    /// Repository class for managing Sellers in the database.
    /// </summary>
    public class SellerRepository : ISellerRepository
    {
        private readonly ApplicationDBContext _context;

        /// <summary>
        /// Constructor for SellerRepository.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SellerRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously creates a new Seller in the database.
        /// </summary>
        /// <param name="newSellerDto">DTO containing the new Seller's data.</param>
        /// <returns>A Task representing the asynchronous operation, containing the created SellerDto.</returns>
        /// <exception cref="NotFoundExe">Thrown when the associated user does not exist.</exception>
        public async Task<SellerDto> CreateSellerAsync(CreateSellerDto newSellerDto)
        {
            var userExists = await _context.AppUsers.AnyAsync(u => u.Id == newSellerDto.UserId);
            if (!userExists)
            {
                throw new NotFoundExe("User does not exist.");
            }

            try
            {
                var newSeller = new Seller
                {
                    UserId = newSellerDto.UserId,
                    CompanyName = newSellerDto.CompanyName,
                    CompanyImgUrl = newSellerDto.CompanyImgUrl,
                    CompanyEmail = newSellerDto.CompanyEmail,
                    CompanyMobile = newSellerDto.CompanyMobile,
                    CompanyAddress = newSellerDto.CompanyAddress,
                    CompanyDescription = newSellerDto.CompanyDescription,
                    DreatedDate = DateTime.Now,
                    IsActive = true
                };

                _context.Sellers.Add(newSeller);
                await _context.SaveChangesAsync();

                return new SellerDto
                {
                    SellerId = newSeller.SellerId,
                    CompanyName = newSeller.CompanyName,
                    CompanyEmail = newSeller.CompanyEmail,
                    CompanyMobile = newSeller.CompanyMobile,
                    CompanyAddress = newSeller.CompanyAddress,
                    CompanyDescription = newSeller.CompanyDescription,
                    IsActive = newSeller.IsActive
                };
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while creating the Seller. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of all Sellers from the database.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation, containing a list of SellerDto.</returns>
        public async Task<List<SellerDto>> GetAllSellersAsync()
        {
            try
            {
                var sellers = await _context.Sellers.ToListAsync();

                return sellers.Select(s => new SellerDto
                {
                    SellerId = s.SellerId,
                    CompanyName = s.CompanyName,
                    CompanyEmail = s.CompanyEmail,
                    CompanyMobile = s.CompanyMobile,
                    CompanyAddress = s.CompanyAddress,
                    CompanyDescription = s.CompanyDescription,
                    IsActive = s.IsActive
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("An error occurred while fetching the sellers. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a Seller by its ID.
        /// </summary>
        /// <param name="id">The ID of the Seller to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the SellerDto.</returns>
        /// <exception cref="NotFoundExe">Thrown when a Seller with the specified ID is not found.</exception>
        public async Task<SellerDto> GetSellerByIdAsync(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                throw new NotFoundExe("Seller not found.");
            }

            return new SellerDto
            {
                SellerId = seller.SellerId,
                CompanyName = seller.CompanyName,
                CompanyEmail = seller.CompanyEmail,
                CompanyMobile = seller.CompanyMobile,
                CompanyAddress = seller.CompanyAddress,
                CompanyDescription = seller.CompanyDescription,
                IsActive = seller.IsActive
            };
        }

        /// <summary>
        /// Asynchronously updates an existing Seller in the database.
        /// </summary>
        /// <param name="id">The ID of the Seller to update.</param>
        /// <param name="updatedSellerDto">DTO containing the updated Seller data.</param>
        /// <returns>A Task representing the asynchronous operation, containing the updated SellerDto.</returns>
        /// <exception cref="NotFoundExe">Thrown when a Seller with the specified ID is not found.</exception>
        public async Task<SellerDto> UpdateSellerAsync(int id, UpdateSellerDto updatedSellerDto)
        {
            var existingSeller = await _context.Sellers.FindAsync(id);
            if (existingSeller == null)
            {
                throw new NotFoundExe("Seller not found.");
            }

            try
            {
                existingSeller.CompanyName = updatedSellerDto.CompanyName;
                existingSeller.CompanyEmail = updatedSellerDto.CompanyEmail;
                existingSeller.CompanyMobile = updatedSellerDto.CompanyMobile;
                existingSeller.CompanyAddress = updatedSellerDto.CompanyAddress;
                existingSeller.CompanyDescription = updatedSellerDto.CompanyDescription;
                existingSeller.IsActive = updatedSellerDto.IsActive;

                await _context.SaveChangesAsync();

                return new SellerDto
                {
                    SellerId = existingSeller.SellerId,
                    CompanyName = existingSeller.CompanyName,
                    CompanyEmail = existingSeller.CompanyEmail,
                    CompanyMobile = existingSeller.CompanyMobile,
                    CompanyAddress = existingSeller.CompanyAddress,
                    CompanyDescription = existingSeller.CompanyDescription,
                    IsActive = existingSeller.IsActive
                };
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while updating the Seller. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously deletes a Seller from the database.
        /// </summary>
        /// <param name="id">The ID of the Seller to delete.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="NotFoundExe">Thrown when a Seller with the specified ID is not found.</exception>
        public async Task DeleteSellerAsync(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                throw new NotFoundExe("Seller not found.");
            }

            try
            {
                _context.Sellers.Remove(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while deleting the Seller. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously sets the active status of a Seller.
        /// </summary>
        /// <param name="id">The ID of the Seller to update.</param>
        /// <param name="isActive">The new active status.</param>
        /// <returns>A Task representing the asynchronous operation, containing the updated SellerDto.</returns>
        /// <exception cref="NotFoundExe">Thrown when a Seller with the specified ID is not found.</exception>
        public async Task<SellerDto> SetSellerActiveStatusAsync(int id, bool isActive)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                throw new NotFoundExe("Seller not found.");
            }

            try
            {
                seller.IsActive = isActive;
                await _context.SaveChangesAsync();

                return new SellerDto
                {
                    SellerId = seller.SellerId,
                    CompanyName = seller.CompanyName,
                    CompanyEmail = seller.CompanyEmail,
                    CompanyMobile = seller.CompanyMobile,
                    CompanyAddress = seller.CompanyAddress,
                    CompanyDescription = seller.CompanyDescription,
                    IsActive = seller.IsActive
                };
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while updating the Seller status. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a Seller by its associated User ID.
        /// </summary>
        /// <param name="userId">The User ID of the Seller to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the SellerDto.</returns>
        /// <exception cref="NotFoundExe">Thrown when a Seller with the specified User ID is not found.</exception>
        public async Task<SellerDto> GetSellerByUserIdAsync(int userId)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);
            if (seller == null)
            {
                throw new NotFoundExe("Seller not found.");
            }

            return new SellerDto
            {
                SellerId = seller.SellerId,
                CompanyName = seller.CompanyName,
                CompanyEmail = seller.CompanyEmail,
                CompanyMobile = seller.CompanyMobile,
                CompanyAddress = seller.CompanyAddress,
                CompanyDescription = seller.CompanyDescription,
                IsActive = seller.IsActive
            };
        }
    }
}