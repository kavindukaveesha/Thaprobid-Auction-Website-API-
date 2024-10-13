using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.dto.response;
using api.Dto.Field;
using api.Helpers;
using api.Mappers;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controller.Client
{
    [Route("api/admin/manage-fields")]
    [ApiController]
    public class ManageFieldController : ControllerBase
    {
        private readonly IFieldRepository _fieldRepo;

        public ManageFieldController(IFieldRepository fieldRepo)
        {
            _fieldRepo = fieldRepo;
        }

        // GET: api/admin/manage-fields
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FieldQueryObject queryObject)
        {
            var fields = await _fieldRepo.GetAllFieldsAsync(queryObject);
            if (fields == null || !fields.Any())
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", "No fields found"));
            }
            var fieldDtos = fields.Select(f => f.ToFieldDto());
            return Ok(new ApiSuccessDto(200, "Fields retrieved successfully", fieldDtos));
        }

        // GET: api/admin/manage-fields/5
        [HttpGet("{id:int}")]
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

        // POST: api/admin/manage-fields/new
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFieldDto fieldDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fieldModel = fieldDto.ToFieldFromFieldDto();
            await _fieldRepo.CreateFieldAsync(fieldModel);
            return CreatedAtAction(
                nameof(GetById),
                new { id = fieldModel.FieldId },
                new ApiSuccessDto(201, "Field created successfully", fieldModel.ToFieldDto()));
        }

        // PUT: api/admin/manage-fields/5
        [HttpPut("{id:int}")]
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

        // DELETE: api/admin/manage-fields/5
        [HttpDelete("{id:int}")]
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