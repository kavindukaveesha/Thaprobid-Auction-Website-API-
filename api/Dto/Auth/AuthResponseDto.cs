using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dto.Auth
{
    public class AuthResponseDto
    {


        public AppUser User { get; set; } // Ensure this is correct
        public string Token { get; set; }

        // If you want UserId to be separate:
        public int UserId { get; set; }
        public String Email { get; set; }
    }



}