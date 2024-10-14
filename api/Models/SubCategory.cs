using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = String.Empty;
        public string SubCategoryImageUrl { get; set; } = String.Empty;
        public string SubCategoryDescription { get; set; } = String.Empty;
        public bool IsEnabled { get; set; } = true;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
        public int? CategoryId { get; set; }
    }
}