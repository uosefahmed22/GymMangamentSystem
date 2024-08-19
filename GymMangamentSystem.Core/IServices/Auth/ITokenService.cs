using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Auth
{
    public interface ITokenService
    {
        Task<(string, RefreshToken)> CreateTokenAsync(AppUser user);
        Task<(string, RefreshToken)> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }

}
