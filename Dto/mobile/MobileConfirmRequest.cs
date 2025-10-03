using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.mobile
{
    public class MobileConfirmationRequest
    {
        public string AppUserId { get; set; }
        public string Otp { get; set; }
    }
}