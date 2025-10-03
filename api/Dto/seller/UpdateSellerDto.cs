using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.seller
{
    public class UpdateSellerDto
    {
        public string CompanyName { get; set; }
        public string CompanyImgUrl { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyMobile { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyDescription { get; set; }
        public bool IsActive { get; set; }
    }
}