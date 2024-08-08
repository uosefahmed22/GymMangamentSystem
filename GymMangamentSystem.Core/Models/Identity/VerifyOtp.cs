using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Identity
{
    public class VerifyOtp
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Only digits allowed")]
        [StringLength(6, ErrorMessage = "OTP must be exactly 6 digits", MinimumLength = 6)]
        public string Otp { get; set; }
    }
}
