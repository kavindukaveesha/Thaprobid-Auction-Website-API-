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
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace api.repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;

        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }



        public async Task<Category> CreateCategoryAsync(Category categoryModel)
        {

            if (categoryModel == null)
            {
                throw new ArgumentNullException(nameof(categoryModel));
            }

            try
            {
                _context.Categories.Add(categoryModel);
                await _context.SaveChangesAsync();
                return categoryModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while creating a Category. Please try again later.", ex);
            }
        }

        public async Task<Category?> DeleteCategoryAsync(int id)
        {
            var categoryModel = await _context.Categories.FindAsync(id);
            if (categoryModel == null)
            {
                throw new NotFoundExe($"Category with id {id} not found");
            }

            try
            {
                _context.Categories.Remove(categoryModel);
                await _context.SaveChangesAsync();
                return categoryModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while deleting a Category. Please try again later.", ex);
            }
        }


        public async Task<List<Category>> GetAllCategorysAsync(CategoryQueryObject queryObject)
        {
            try
            {



                var categories = _context.Categories.Include(s => s.SubCategories).AsQueryable();
                if (!String.IsNullOrWhiteSpace(queryObject.CategoryName))
                {
                    categories = categories.Where(s => s.CategoryName.Contains(queryObject.CategoryName));
                }


                if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
                {
                    if (queryObject.SortBy.Equals("CategoryName", StringComparison.OrdinalIgnoreCase))
                    {
                        categories = queryObject.IsDecsending ? categories.OrderByDescending(s => s.CategoryName) : categories.OrderBy(s => s.CategoryName);
                    }
                }

                var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

                return await categories.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();

            }

            catch (Exception ex)
            {
                throw new BadRequestException("An error occurred while retrieving categories. Please try again later.", ex);
            }
        }

        public async Task<Category?> GetByIdCategoryAsync(int id)
        {
            try
            {
                return await _context.Categories.Include(s => s.SubCategories).FirstOrDefaultAsync(i => i.CategoryId == id);
            }
            catch (Exception ex)
            {
                throw new NotFoundExe($"Category with id {id} not found", ex);
            }
        }

        public async Task<bool> IsCategoryExist(int id)
        {
            return await _context.Categories.AnyAsync(i => i.CategoryId == id);
        }

        public async Task<Category?> UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto)
        {
            var categoryModel = await _context.Categories.FindAsync(id);
            if (categoryModel == null)
            {
                throw new NotFoundExe($"Category with id {id} not found");
            }

            try
            {
                categoryModel.CategoryName = categoryDto.CategorydName;
                categoryModel.CategoryImageUrl = categoryDto.CategorydImageUrl;
                categoryModel.CategoryDescription = categoryDto.CategorydDescription;

                await _context.SaveChangesAsync();
                return categoryModel;
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("An error occurred while updating a Category. Please try again later.", ex);
            }
        }


    }
}