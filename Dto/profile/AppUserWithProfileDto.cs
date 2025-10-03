using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dto.profile
{
    public class AppUserWithProfileDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public bool ConfirmedEmail { get; set; }
        public bool ConfirmedMobile { get; set; }

        public string? PictureUrl { get; set; }


        public string ClientAddress { get; set; }
        public bool IsClientBidder { get; set; }
        public int? SellerId { get; set; }

    }
}