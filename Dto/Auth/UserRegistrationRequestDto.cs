using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dto.Auth
{
    public class UserRegistrationRequestDto
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Phone] // Optional: Add phone number validation
        public string? Mobile { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}