using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Auth
{
    public class ResetPasswordResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}