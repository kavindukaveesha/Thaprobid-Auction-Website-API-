using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Auth;
using api.Models;

namespace api.Mappers
{
    public static class AuthenticationMapper
    {
        public static AppUser ToUser(this UserRegistrationRequestDto registrationDto)
        {
            return new AppUser
            {
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Email = registrationDto.Email,
                MobileNumber = registrationDto.Mobile,
                ConfirmedEmail = false,
                ConfirmedMobile = false,
            };
        }
    }
}