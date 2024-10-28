using System;
using System.Linq;
using System.Threading.Tasks;
using api.dto.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using api.Helpers;

namespace api.Controller.Client
{
    /// <summary>
    /// Controller for managing Fields for client-side requests. Provides simplified field details.
    /// </summary>
    [Route("api/fields")] // Base route for client-specific field retrieval
    [ApiController] // Indicates this is an API controller
    public class ClientFieldController : ControllerBase
    {
        private readonly IFieldRepository _fieldRepo;

        /// <summary>
        /// Constructor for ClientFieldController. Injects dependencies.
        /// </summary>
        /// <param name="fieldRepo">The repository for accessing Field data.</param>
        public ClientFieldController(IFieldRepository fieldRepo)
        {
            _fieldRepo = fieldRepo;
        }

        /// <summary>
        /// Retrieves all fields with their categories and subcategories for the client-side. Only returns id and name.
        /// </summary>
        /// <returns>An IActionResult containing a list of fields with categories and subcategories.</returns>
        [HttpGet]
        [AllowAnonymous] // Allows access to this API without authentication
        public async Task<IActionResult> GetAllFieldsForClient([FromQuery] FieldQueryObject queryObject)
        {
            var fields = await _fieldRepo.GetAllFieldsAsync(queryObject);
            if (fields == null || !fields.Any())
            {
                return NotFound(new ApiErrorDto(404, "NOT_FOUND", "No fields found"));
            }

            // Prepare a simplified response with only id and name for fields, categories, and subcategories
            var fieldDtos = fields.Select(f => new
            {
                fieldId = f.FieldId,
                fieldName = f.FieldName,
                categories = f.Categories.Select(c => new
                {
                    categoryId = c.CategoryId,
                    categoryName = c.CategoryName,
                    subCategories = c.SubCategories.Select(sc => new
                    {
                        subCategoryId = sc.SubCategoryId,
                        subCategoryName = sc.SubCategoryName
                    })
                })
            });

            return Ok(new ApiSuccessDto(200, "Fields retrieved successfully", fieldDtos));
        }
    }
}
