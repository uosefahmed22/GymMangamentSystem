using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Identity
{
    public class Login
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
