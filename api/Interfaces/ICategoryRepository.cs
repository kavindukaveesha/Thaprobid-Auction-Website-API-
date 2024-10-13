using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Field;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategorysAsync(CategoryQueryObject queryObject);
        Task<Category?> GetByIdCategoryAsync(int id);
        Task<Category> CreateCategoryAsync(Category categoryModel);
        Task<Category?> UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto);
        Task<Category?> DeleteCategoryAsync(int id);
    }
}