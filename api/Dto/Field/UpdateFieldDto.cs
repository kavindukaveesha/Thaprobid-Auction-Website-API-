using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Field
{
    public class UpdateFieldDto
    {
        [Required]
        public string FieldName { get; set; } = String.Empty;
        public string FieldImageUrl { get; set; } = String.Empty;
        public string FieldDescription { get; set; } = String.Empty;

    }
}