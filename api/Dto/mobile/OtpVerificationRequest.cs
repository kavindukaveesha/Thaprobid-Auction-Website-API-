using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.mobile
{
    public class OtpVerificationRequest
    {
        public string MobileNumber { get; set; }
        public string Otp { get; set; }
    }
}