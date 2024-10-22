using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Auth
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public String Email { get; set; }
        public String Token { get; set; }
    }
}