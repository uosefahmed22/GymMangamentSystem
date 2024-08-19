using GymMangamentSystem.Core.Enums.Auth;
using GymMangamentSystem.Core.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Dtos.Auth
{
    public class UserDto : ApiResponse
    {
        public UserRoleEnum Role { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserCode { get; set; }

    }
}
