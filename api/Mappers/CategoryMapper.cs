using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Field;
using api.Models;

namespace api.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategorydDto(this Category categoryModel)
        {
            return new CategoryDto
            {
                CategoryId = categoryModel.CategoryId,
                CategoryName = categoryModel.CategoryName,
                CategoryImageUrl = categoryModel.CategoryImageUrl,
                CategoryDescription = categoryModel.CategoryDescription,
                IsEnabled = categoryModel.IsEnabled,
                FieldId = categoryModel.FieldId

            };
        }

        public static Category ToCategoryFromCreate(this CreateCategoryDto categoryDto, int FieldId)
        {
            return new Category
            {
                CategoryName = categoryDto.CategorydName,
                CategoryImageUrl = categoryDto.CategorydImageUrl,
                CategoryDescription = categoryDto.CategorydDescription,
                FieldId = FieldId
            };
        }
        public static Category ToCategoryFromUpdate(this CreateCategoryDto categoryDto)
        {
            return new Category
            {
                CategoryName = categoryDto.CategorydName,
                CategoryImageUrl = categoryDto.CategorydImageUrl,
                CategoryDescription = categoryDto.CategorydDescription,
            };
        }
    }
}