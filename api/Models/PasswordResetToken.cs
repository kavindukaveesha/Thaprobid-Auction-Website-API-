using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;


public class PasswordResetToken
{
    public int Id { get; set; } // Primary key 
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } // Navigation property back to AppUser

}
