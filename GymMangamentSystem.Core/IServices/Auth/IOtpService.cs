using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Auth
{
    public interface IOtpService
    {
        public string GenerateOtp(string email);
        public bool IsValidOtp(string email, string otp);
    }
}
