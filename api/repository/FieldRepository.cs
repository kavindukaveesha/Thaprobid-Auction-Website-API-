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
    public class FieldRepository : IFieldRepository
    {
        private readonly ApplicationDBContext _context;

        public FieldRepository(ApplicationDBContext context)
        {
            _context = context;
        }

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

        public async Task<List<Field>> GetAllFieldsAsync(FieldQueryObject queryObject)
        {
            return _context.Fields.ToList();
            // try
            // {
            //     var fields = _context.Fields.Include(c => c.Comments).AsQueryable();
            //     if (!String.IsNullOrWhiteSpace(queryObject.FieldName))
            //     {
            //         fields = fields.Where(s => s.FieldName.Contains(queryObject.FieldName));
            //     }
            //     if (!String.IsNullOrWhiteSpace(queryObject.FieldDescription))
            //     {
            //         fields = fields.Where(s => s.FieldDescription.Contains(queryObject.FieldDescription));
            //     }

            //     if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            //     {
            //         if (queryObject.SortBy.Equals("FieldName", StringComparison.OrdinalIgnoreCase))
            //         {
            //             fields = queryObject.IsDecsending ? fields.OrderByDescending(s => s.FieldName) : fields.OrderBy(s => s.FieldName);
            //         }
            //     }

            //     var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

            //     return await fields.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            // }
            // catch (Exception ex)
            // {
            //     throw new BadRequestException("An error occurred while retrieving fields. Please try again later.", ex);
            // }
        }



        public async Task<Field?> GetByIdFieldAsync(int id)
        {
            try
            {
                //return await _context.Fields.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
                return await _context.Fields.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new NotFoundExe($"Field with id {id} not found", ex);
            }
        }

        public async Task<Field?> UpdateFieldAsync(int id, UpdateFieldDto fieldDto)
        {
            var fieldModel = await _context.Fields.FindAsync(id);
            if (fieldModel == null)
            {
                throw new NotFoundExe($"Field with id {id} not found");
            }

            try
            {
                fieldModel.FieldName = fieldDto.FieldName;
                fieldModel.FieldImageUrl = fieldDto.FieldImageUrl;
                fieldModel.FieldDescription = fieldDto.FieldDescription;

                await _context.SaveChangesAsync();
                return fieldModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while updating a Field. Please try again later.", ex);
            }
        }
    }
}