using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.dto.response;
using api.Dto.Field;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller.Admin
{
    /// <summary>
    /// Controller for managing SubCategories. Provides endpoints for CRUD operations and retrieval.
    /// </summary>
    [Route("api/admin/manage-subcategories")] // Base route for all endpoints in this controller
    [ApiController] // Indicates this is an API controller
    public class ManageSubCategoryController : ControllerBase
    {
        private readonly ISubCategoryRepository _subCategoryRepo;
        private readonly ICategoryRepository _categoryRepo;

        /// <summary>
        /// Constructor for ManageSubCategoryController. Injects dependencies.
        /// </summary>
        /// <param name="subCategoryRepo">The repository for accessing SubCategory data.</param>
        public ManageSubCategoryController(ISubCategoryRepository subCategoryRepo, ICategoryRepository CategoryRepo)
        {
            _subCategoryRepo = subCategoryRepo;
            _categoryRepo = CategoryRepo;
        }

        /// <summary>
        /// Retrieves all subcategories. Supports filtering, sorting, and pagination through query parameters.
        /// </summary>
        /// <param name="queryObject">Object containing query parameters for filtering, sorting, and pagination.</param>
        /// <returns>An IActionResult containing a list of subcategories or an appropriate error response.</returns>

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SubCategoryQueryObjects queryObject)
        {
            var subCategories = await _subCategoryRepo.GetAllSubCategorysAsync(queryObject);
            if (subCategories == null || !subCategories.Any())
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", "No subcategories found"));
            }
            var subCategoryDtos = subCategories.Select(s => s.ToSubCategorydDto()); // Map to DTOs
            return Ok(new ApiSuccessDto(200, "Subcategories retrieved successfully", subCategoryDtos));
        }

        /// <summary>
        /// Retrieves a specific subcategory by its ID.
        /// </summary>
        /// <param name="id">The ID of the subcategory to retrieve.</param>
        /// <returns>An IActionResult containing the subcategory or an appropriate error response.</returns>

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var subCategory = await _subCategoryRepo.GetByIdSubCategoryAsync(id);
            if (subCategory == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"SubCategory with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "SubCategory retrieved successfully", subCategory.ToSubCategorydDto()));
        }

        /// <summary>
        /// Creates a new subcategory.
        /// </summary>
        /// <param name="subCategoryDto">The subcategory data to create.</param>
        /// <returns>An IActionResult indicating success or failure, including the created subcategory if successful.</returns>

        [HttpPost("{categoryId:int}")]
        public async Task<IActionResult> Create([FromRoute] int categoryId, [FromBody] CreateSubCategoryDto subCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _categoryRepo.IsCategoryExist(categoryId))
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"No stock found with ID: {categoryId}"));
            }
            var subCategoryModel = subCategoryDto.ToSubCategoryFromCreate(categoryId);
            await _subCategoryRepo.CreateSubCategoryAsync(subCategoryModel);
            return CreatedAtAction(
                nameof(GetById),
                new { id = subCategoryModel.SubCategoryId },
                new ApiSuccessDto(201, "SubCategory created successfully", subCategoryModel.ToSubCategorydDto())
            );
        }

        /// <summary>
        /// Updates an existing subcategory.
        /// </summary>
        /// <param name="id">The ID of the subcategory to update.</param>
        /// <param name="subCategoryDto">The updated subcategory data.</param>
        /// <returns>An IActionResult indicating success or failure, including the updated subcategory if successful.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSubCategoryDto subCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subCategoryModel = await _subCategoryRepo.UpdateSubCategoryAsync(id, subCategoryDto);
            if (subCategoryModel == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"SubCategory with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "SubCategory updated successfully", subCategoryModel.ToSubCategorydDto()));
        }

        /// <summary>
        /// Deletes a specific subcategory by its ID.
        /// </summary>
        /// <param name="id">The ID of the subcategory to delete.</param>
        /// <returns>An IActionResult indicating success or failure.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subCategoryModel = await _subCategoryRepo.DeleteCategoryAsync(id);
            if (subCategoryModel == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"SubCategory with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "SubCategory deleted successfully"));
        }
    }
}