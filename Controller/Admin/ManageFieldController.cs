using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.dto.response;
using api.Dto.Field;
using api.Helpers;
using api.Mappers;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller.Client
{
    /// <summary>
    /// Controller for managing Fields. Provides endpoints for CRUD operations and retrieval.
    /// </summary>
    [Route("api/admin/manage-fields")] // Base route for all endpoints in this controller
    [ApiController] // Indicates this is an API controller
    [Authorize]

    public class ManageFieldController : ControllerBase
    {
        private readonly IFieldRepository _fieldRepo;

        /// <summary>
        /// Constructor for ManageFieldController. Injects dependencies.
        /// </summary>
        /// <param name="fieldRepo">The repository for accessing Field data.</param>
        public ManageFieldController(IFieldRepository fieldRepo)
        {
            _fieldRepo = fieldRepo;
        }

        /// <summary>
        /// Retrieves all fields. Supports filtering, sorting, and pagination through query parameters.
        /// </summary>
        /// <param name="queryObject">Object containing query parameters for filtering, sorting, and pagination.</param>
        /// <returns>An IActionResult containing a list of fields or an appropriate error response.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FieldQueryObject queryObject)
        {
            var fields = await _fieldRepo.GetAllFieldsAsync(queryObject);
            if (fields == null || !fields.Any())
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", "No fields found"));
            }
            var fieldDtos = fields.Select(f => f.ToFieldDto()); // Maps Field objects to FieldDtos
            return Ok(new ApiSuccessDto(200, "Fields retrieved successfully", fieldDtos));
        }

        /// <summary>
        /// Retrieves a specific field by its ID.
        /// </summary>
        /// <param name="id">The ID of the field to retrieve.</param>
        /// <returns>An IActionResult containing the field or an appropriate error response.</returns>
        [HttpGet("{id:int}")] // Attribute for GET requests with an integer ID parameter
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var field = await _fieldRepo.GetByIdFieldAsync(id);
            if (field == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"Field with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "Field retrieved successfully", field.ToFieldDto()));
        }

        /// <summary>
        /// Creates a new field.
        /// </summary>
        /// <param name="fieldDto">The field data to create.</param>
        /// <returns>An IActionResult indicating success or failure, including the created field if successful.</returns>
        [HttpPost] // Attribute for POST requests
        public async Task<IActionResult> Create([FromBody] CreateFieldDto fieldDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fieldModel = fieldDto.ToFieldFromFieldDto(); // Maps CreateFieldDto to Field
            await _fieldRepo.CreateFieldAsync(fieldModel);
            return CreatedAtAction(
                nameof(GetById),
                new { id = fieldModel.FieldId }, // Provides location header for the created resource
                new ApiSuccessDto(201, "Field created successfully", fieldModel.ToFieldDto())
            );
        }

        /// <summary>
        /// Updates an existing field.
        /// </summary>
        /// <param name="id">The ID of the field to update.</param>
        /// <param name="fieldDto">The updated field data.</param>
        /// <returns>An IActionResult indicating success or failure, including the updated field if successful.</returns>
        [HttpPut("{id:int}")] // Attribute for PUT requests with an integer ID parameter
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFieldDto fieldDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fieldModel = await _fieldRepo.UpdateFieldAsync(id, fieldDto);
            if (fieldModel == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"Field with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "Field updated successfully", fieldModel.ToFieldDto()));
        }

        /// <summary>
        /// Deletes a specific field by its ID.
        /// </summary>
        /// <param name="id">The ID of the field to delete.</param>
        /// <returns>An IActionResult indicating success or failure.</returns>
        [HttpDelete("{id:int}")] // Attribute for DELETE requests with an integer ID parameter
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fieldModel = await _fieldRepo.DeleteFieldAsync(id);
            if (fieldModel == null)
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", $"Field with id {id} not found"));
            }
            return Ok(new ApiSuccessDto(200, "Field deleted successfully"));
        }
    }
}