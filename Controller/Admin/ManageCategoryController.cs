using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.dto.response;
using api.Dto.Field;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controller.Admin
{
    [Route("api/admin/manage-categories")]
    [ApiController]
    public class ManageCategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IFieldRepository _fieldRepo;

        public ManageCategoryController(ICategoryRepository categoryRepo, IFieldRepository fieldRepo)
        {
            _categoryRepo = categoryRepo;
            _fieldRepo = fieldRepo;
        }

        // GET: api/admin/manage-categories
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CategoryQueryObject queryObject)
        {
            var categories = await _categoryRepo.GetAllCategorysAsync(queryObject);
            if (categories == null || !categories.Any())
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", "No categories found"));
            }
            var categoryDtos = categories.Select(c => c.ToCategorydDto());
            return Ok(new ApiSuccessDto(200, "Categories retrieved successfully", categoryDtos));
        }

        // GET: api/admin/manage-categories/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryRepo.GetByIdCategoryAsync(id);
            if (category == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"Category with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "Category retrieved successfully", category.ToCategorydDto()));
        }

        // POST: api/admin/manage-categories/new
        [HttpPost("{fieldId:int}")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto categoryDto, [FromRoute] int fieldId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _fieldRepo.IsFieldExist(fieldId))
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"No stock found with ID: {fieldId}"));
            }
            var categoryModel = categoryDto.ToCategoryFromCreate(fieldId);
            await _categoryRepo.CreateCategoryAsync(categoryModel);
            return CreatedAtAction(
                nameof(GetById),
                new { id = categoryModel.CategoryId },
                new ApiSuccessDto(201, "Category created successfully", categoryModel.ToCategorydDto()));
        }

        // PUT: api/admin/manage-categories/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryModel = await _categoryRepo.UpdateCategoryAsync(id, categoryDto);
            if (categoryModel == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"Category with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "Category updated successfully", categoryModel.ToCategorydDto()));
        }

        // DELETE: api/admin/manage-categories/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryModel = await _categoryRepo.DeleteCategoryAsync(id);
            if (categoryModel == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"Category with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "Category deleted successfully"));
        }
    }

}
