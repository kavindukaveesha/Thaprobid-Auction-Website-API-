using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Seller
    {
        public int SellerId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyImgUrl { get; set; }
        [EmailAddress]
        public string CompanyEmail { get; set; }

        public string CompanyMobile { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime DreatedDate { get; set; }
        public AppUser AppUser { get; set; }


    }
}