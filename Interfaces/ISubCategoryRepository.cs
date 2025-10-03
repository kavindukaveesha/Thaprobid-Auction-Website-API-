using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Field;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<List<SubCategory>> GetAllSubCategorysAsync(SubCategoryQueryObjects queryObject);
        Task<SubCategory?> GetByIdSubCategoryAsync(int id);
        Task<SubCategory> CreateSubCategoryAsync(SubCategory subCategoryModel);
        Task<SubCategory?> UpdateSubCategoryAsync(int id, UpdateSubCategoryDto subCategoryDto);
        Task<SubCategory?> DeleteCategoryAsync(int id);
    }
}