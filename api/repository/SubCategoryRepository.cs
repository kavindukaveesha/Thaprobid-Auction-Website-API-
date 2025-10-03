using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dto.Field;
using api.Handlers;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.repository
{
    /// <summary>
    /// Repository class for managing SubCategories in the database.
    /// </summary>
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly ApplicationDBContext _context;

        /// <summary>
        /// Constructor for SubCategoryRepository.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SubCategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously creates a new SubCategory in the database.
        /// </summary>
        /// <param name="subCategoryModel">The SubCategory object to create.</param>
        /// <returns>A Task representing the asynchronous operation, containing the created SubCategory.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the subCategoryModel is null.</exception>
        /// <exception cref="BadRequestException">Thrown when an error occurs during database update.</exception>

        public async Task<SubCategory> CreateSubCategoryAsync(SubCategory subCategoryModel)
        {
            if (subCategoryModel == null)
            {
                throw new ArgumentNullException(nameof(subCategoryModel));
            }

            try
            {
                _context.SubCategories.Add(subCategoryModel);
                await _context.SaveChangesAsync();
                return subCategoryModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while creating a SubCategory. Please try again later.", ex);
            }
        }



        /// <summary>
        /// Asynchronously deletes a SubCategory from the database.
        /// </summary>
        /// <param name="id">The ID of the SubCategory to delete.</param>
        /// <returns>A Task representing the asynchronous operation, containing the deleted SubCategory or null if not found.</returns>
        /// <exception cref="NotFoundExe">Thrown when a SubCategory with the specified ID is not found.</exception>
        /// <exception cref="BadRequestException">Thrown when an error occurs during database update.</exception>
        public async Task<SubCategory?> DeleteCategoryAsync(int id)
        {

            var subCategoryModel = await _context.SubCategories.FindAsync(id);
            if (subCategoryModel == null)
            {
                throw new NotFoundExe($"SubCategory with id {id} not found");
            }

            try
            {
                _context.SubCategories.Remove(subCategoryModel);
                await _context.SaveChangesAsync();
                return subCategoryModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while deleting a SubCategory. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of all SubCategories from the database.
        /// </summary>
        /// <param name="queryObject">Object containing query parameters for filtering, sorting, and pagination.</param>
        /// <returns>A Task representing the asynchronous operation, containing a list of SubCategories.</returns>
        /// <exception cref="BadRequestException">Thrown when an error occurs while retrieving subcategories.</exception>

        public async Task<List<SubCategory>> GetAllSubCategorysAsync(SubCategoryQueryObjects queryObject)
        {
            try
            {
                var subCategories = _context.SubCategories.AsQueryable();

                // Filtering (Add filtering logic based on queryObject properties if needed)
                // Example: 
                if (!string.IsNullOrWhiteSpace(queryObject.SubCategoryName))
                {
                    subCategories = subCategories.Where(s => s.SubCategoryName.Contains(queryObject.SubCategoryName));
                }


                if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
                {
                    if (queryObject.SortBy.Equals("SubCategoryName", StringComparison.OrdinalIgnoreCase))
                    {
                        subCategories = queryObject.IsDecsending
                            ? subCategories.OrderByDescending(s => s.SubCategoryName)
                            : subCategories.OrderBy(s => s.SubCategoryName);
                    }
                }


                var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
                return await subCategories.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();

            }
            catch (Exception ex)
            {
                throw new BadRequestException("An error occurred while retrieving subcategories. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a SubCategory by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the SubCategory to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the SubCategory or null if not found.</returns>
        /// <exception cref="NotFoundExe">Thrown when a SubCategory with the specified ID is not found.</exception>
        public async Task<SubCategory?> GetByIdSubCategoryAsync(int id)
        {
            try
            {
                return await _context.SubCategories.FirstOrDefaultAsync(i => i.SubCategoryId == id);
            }
            catch (Exception ex)
            {
                throw new NotFoundExe($"SubCategory with id {id} not found", ex);
            }
        }

        /// <summary>
        /// Asynchronously updates an existing SubCategory in the database.
        /// </summary>
        /// <param name="id">The ID of the SubCategory to update.</param>
        /// <param name="subCategoryDto">The updated SubCategory data.</param>
        /// <returns>A Task representing the asynchronous operation, containing the updated SubCategory or null if not found.</returns>
        /// <exception cref="NotFoundExe">Thrown when a SubCategory with the specified ID is not found.</exception>
        /// <exception cref="BadRequestException">Thrown when an error occurs during database update.</exception>
        public async Task<SubCategory?> UpdateSubCategoryAsync(int id, UpdateSubCategoryDto subCategoryDto)
        {
            var subCategoryModel = await _context.SubCategories.FindAsync(id);
            if (subCategoryModel == null)
            {
                throw new NotFoundExe($"SubCategory with id {id} not found");
            }

            try
            {
                // Update the SubCategory properties based on subCategoryDto
                subCategoryModel.SubCategoryName = subCategoryDto.SubCategoryName;
                subCategoryModel.SubCategoryImageUrl = subCategoryDto.SubCategoryImageUrl;
                subCategoryModel.SubCategoryDescription = subCategoryDto.SubCategoryDescription;
                // ... Update other properties as needed

                await _context.SaveChangesAsync();
                return subCategoryModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while updating a SubCategory. Please try again later.", ex);
            }
        }


    }
}