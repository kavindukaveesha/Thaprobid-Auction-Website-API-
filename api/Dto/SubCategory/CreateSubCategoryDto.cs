using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Field
{
    public class CreateSubCategoryDto

    {
        [Required]
        public string SubCategoryName { get; set; } = String.Empty;
        public string SubCategoryImageUrl { get; set; } = String.Empty;
        public string SubCategoryDescription { get; set; } = String.Empty;
    }
}