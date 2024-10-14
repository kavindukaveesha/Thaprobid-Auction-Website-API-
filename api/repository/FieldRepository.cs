using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dto.Field;
using api.Handlers;
using api.Helpers;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.repository
{
    /// <summary>
    /// Repository class for managing Fields in the database.
    /// </summary>
    public class FieldRepository : IFieldRepository
    {
        private readonly ApplicationDBContext _context;

        /// <summary>
        /// Constructor for FieldRepository.
        /// </summary>
        /// <param name="context">The database context.</param>
        public FieldRepository(ApplicationDBContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Asynchronously retrieves a list of all Fields from the database based on query parameters.
        /// </summary>
        /// <param name="queryObject">Object containing filtering, sorting, and pagination parameters.</param>
        /// <returns>A Task representing the asynchronous operation, containing a list of Fields matching the query parameters.</returns>
        /// <exception cref="BadRequestException">Thrown when an error occurs while retrieving fields.</exception>
        public async Task<List<Field>> GetAllFieldsAsync(FieldQueryObject queryObject)
        {
            try
            {
                var fields = _context.Fields
                .Include(c => c.Categories)
                .ThenInclude(c => c.SubCategories)
                .AsQueryable();

                // Filtering
                if (!string.IsNullOrWhiteSpace(queryObject.FieldName))
                {
                    fields = fields.Where(s => s.FieldName.Contains(queryObject.FieldName));
                }

                // Sorting
                if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
                {
                    if (queryObject.SortBy.Equals("FieldName", StringComparison.OrdinalIgnoreCase))
                    {
                        fields = queryObject.IsDecsending
                            ? fields.OrderByDescending(s => s.FieldName)
                            : fields.OrderBy(s => s.FieldName);
                    }
                }

                // Pagination
                var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
                return await fields.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("An error occurred while retrieving fields. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a Field by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the Field to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation, containing the Field or null if not found.</returns>
        /// <exception cref="NotFoundExe">Thrown when a Field with the specified ID is not found.</exception>
        public async Task<Field?> GetByIdFieldAsync(int id)
        {
            try
            {
                return await _context.Fields
                    .Include(c => c.Categories)
                    .ThenInclude(c => c.SubCategories)
                    .FirstOrDefaultAsync(i => i.FieldId == id);
            }
            catch (Exception ex)
            {
                throw new NotFoundExe($"Field with id {id} not found", ex);
            }
        }
        /// <summary>
        /// Asynchronously creates a new Field in the database.
        /// </summary>
        /// <param name="fieldModel">The Field object to create.</param>
        /// <returns>A Task representing the asynchronous operation, containing the created Field.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the fieldModel is null.</exception>
        /// <exception cref="BadRequestException">Thrown when an error occurs during database update.</exception>
        public async Task<Field> CreateFieldAsync(Field fieldModel)
        {
            if (fieldModel == null)
            {
                throw new ArgumentNullException(nameof(fieldModel));
            }

            try
            {
                _context.Fields.Add(fieldModel);
                await _context.SaveChangesAsync();
                return fieldModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while creating a Field. Please try again later.", ex);
            }
        }


        /// <summary>
        /// Asynchronously checks if a Field with the given ID exists in the database.
        /// </summary>
        /// <param name="id">The ID of the Field to check.</param>
        /// <returns>A Task representing the asynchronous operation, containing a boolean value indicating if the Field exists.</returns>
        public async Task<bool> IsFieldExist(int id)
        {
            return await _context.Fields.AnyAsync(i => i.FieldId == id);
        }

        /// <summary>
        /// Asynchronously updates an existing Field in the database.
        /// </summary>
        /// <param name="id">The ID of the Field to update.</param>
        /// <param name="fieldDto">The updated Field data.</param>
        /// <returns>A Task representing the asynchronous operation, containing the updated Field or null if not found.</returns>
        /// <exception cref="NotFoundExe">Thrown when a Field with the specified ID is not found.</exception>
        /// <exception cref="BadRequestException">Thrown when an error occurs during database update.</exception>
        public async Task<Field?> UpdateFieldAsync(int id, UpdateFieldDto fieldDto)
        {
            var fieldModel = await _context.Fields.FindAsync(id);
            if (fieldModel == null)
            {
                throw new NotFoundExe($"Field with id {id} not found");
            }

            try
            {
                // Update the Field properties
                fieldModel.FieldName = fieldDto.FieldName;
                fieldModel.FieldImageUrl = fieldDto.FieldImageUrl;
                fieldModel.FieldDescription = fieldDto.FieldDescription;
                fieldModel.UpdatedDateTime = DateTime.Now;

                await _context.SaveChangesAsync();
                return fieldModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while updating a Field. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Asynchronously deletes a Field from the database.
        /// </summary>
        /// <param name="id">The ID of the Field to delete.</param>
        /// <returns>A Task representing the asynchronous operation, containing the deleted Field or null if not found.</returns>
        /// <exception cref="NotFoundExe">Thrown when a Field with the specified ID is not found.</exception>
        /// <exception cref="BadRequestException">Thrown when an error occurs during database update.</exception>
        public async Task<Field?> DeleteFieldAsync(int id)
        {
            var fieldModel = await _context.Fields.FindAsync(id);
            if (fieldModel == null)
            {
                throw new NotFoundExe($"Field with id {id} not found");
            }

            try
            {
                _context.Fields.Remove(fieldModel);
                await _context.SaveChangesAsync();
                return fieldModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while deleting a Field. Please try again later.", ex);
            }
        }
    }
}