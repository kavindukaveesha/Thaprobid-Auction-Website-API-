using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.security;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string MobileNumber { get; set; }
        public bool ConfirmedEmail { get; set; }
        public bool ConfirmedMobile { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Google Authentication Attributes
        public string? GoogleId { get; set; }
        public string? PictureUrl { get; set; }


        public PasswordResetToken? PasswordResetToken { get; set; }



    }
}