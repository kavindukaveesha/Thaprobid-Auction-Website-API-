using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Field
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; } = String.Empty;
        public string FieldImageUrl { get; set; } = String.Empty;
        public string FieldDescription { get; set; } = String.Empty;
        public bool IsEnabled { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;


        //  public List<Category> categoriesList{get;set;} = new List<Category>();

    }
}