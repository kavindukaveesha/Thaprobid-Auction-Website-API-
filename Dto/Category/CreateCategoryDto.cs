using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Field
{
    public class CreateCategoryDto

    {
        [Required]
        public string CategorydName { get; set; } = String.Empty;
        public string CategorydImageUrl { get; set; } = String.Empty;
        public string CategorydDescription { get; set; } = String.Empty;
    }
}