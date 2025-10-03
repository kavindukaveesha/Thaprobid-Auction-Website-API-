using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Field;
using api.Models;

namespace api.Mappers
{
    public static class SubCategoryMapper
    {
        public static SubCategoryDto ToSubCategorydDto(this SubCategory subCategoryModel)
        {
            return new SubCategoryDto
            {
                SubCategoryId = subCategoryModel.SubCategoryId,
                SubCategoryName = subCategoryModel.SubCategoryName,
                SubCategoryImageUrl = subCategoryModel.SubCategoryImageUrl,
                SubCategoryDescription = subCategoryModel.SubCategoryDescription,
                IsEnabled = subCategoryModel.IsEnabled,
                CategoryId = subCategoryModel.CategoryId

            };
        }

        public static SubCategory ToSubCategoryFromCreate(this CreateSubCategoryDto subCategoryDto, int categoryId)
        {
            return new SubCategory
            {
                SubCategoryName = subCategoryDto.SubCategoryName,
                SubCategoryImageUrl = subCategoryDto.SubCategoryImageUrl,
                SubCategoryDescription = subCategoryDto.SubCategoryDescription,
                CategoryId = categoryId
            };
        }
        public static SubCategory ToSubCategoryFromUpdate(this UpdateSubCategoryDto updateSubCategoryDto)
        {
            return new SubCategory
            {
                SubCategoryName = updateSubCategoryDto.SubCategoryName,
                SubCategoryImageUrl = updateSubCategoryDto.SubCategoryImageUrl,
                SubCategoryDescription = updateSubCategoryDto.SubCategoryDescription,
            };
        }
    }
}