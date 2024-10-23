using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Models
{
    public class ClientProfile
    {
        public int Id { get; set; }
        [Key]
        public int UserId { get; set; }

        public string ClientAddress { get; set; }
        public bool IsClientBidder { get; set; }

        // Navigation property
        [JsonIgnore]
        public AppUser AppUser { get; set; }
    }
}