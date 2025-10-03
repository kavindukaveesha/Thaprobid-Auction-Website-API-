using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.EmailDto
{
    public class EmailConfigDto
    {
        public String RecieverEmail { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
    }
}