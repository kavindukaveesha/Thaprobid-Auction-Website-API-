using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dto.Field
{
    public class FieldDto
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; } = String.Empty;
        public string FieldImageUrl { get; set; } = String.Empty;
        public string FieldDescription { get; set; } = String.Empty;
        public bool IsEnabled { get; set; } = true;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
        public List<CategoryDto> Categories { get; set; }
    }
}