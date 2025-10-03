using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace api.repository
{
    public class MobileVerificationService
    {
        private readonly Dictionary<string, string> _otpStore = new Dictionary<string, string>(); // In-memory store for OTPs
        private readonly ILogger<MobileVerificationService> _logger;

        public MobileVerificationService(ILogger<MobileVerificationService> logger)
        {
            _logger = logger;
        }

        // Generate a random 6-digit OTP
        public string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generates a 6-digit OTP
        }

        // Simulate sending OTP via SMS
        public async Task SendOtpAsync(string mobileNumber)
        {
            string otp = GenerateOtp();
            _otpStore[mobileNumber] = otp; // Store the OTP with the mobile number

            // Simulate sending SMS (replace with actual SMS service call)
            await Task.Run(() => _logger.LogInformation($"Sending OTP {otp} to {mobileNumber}"));
        }

        // Verify the OTP entered by the user
        public bool VerifyOtp(string mobileNumber, string otp)
        {
            if (_otpStore.TryGetValue(mobileNumber, out string storedOtp))
            {
                return storedOtp == otp;
            }
            return false;
        }
    }
}