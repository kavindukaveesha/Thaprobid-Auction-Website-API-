using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Field
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = String.Empty;
        public string CategoryImageUrl { get; set; } = String.Empty;
        public string CategoryDescription { get; set; } = String.Empty;
        public bool IsEnabled { get; set; } = true;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;

        public int? FieldId { get; set; }
    }
}