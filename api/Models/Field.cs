using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Field
    {
        public int fieldId { get; set; }
        public string fieldName { get; set; }
        public string fieldImageUrl { get; set; }
        public string fieldDescription { get; set; }
        public bool isEnabled { get; set; }
        //  public List<Category> categoriesList{get;set;} = new List<Category>();

    }
}