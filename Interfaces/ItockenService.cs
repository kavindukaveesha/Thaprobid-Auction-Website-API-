using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ItockenService
    {
        string CreateToken(AppUser user);
        bool ValidateToken(string token);
    }
}