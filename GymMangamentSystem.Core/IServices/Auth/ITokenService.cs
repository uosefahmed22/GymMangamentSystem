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
        Task<string> CreateTokenAsync(AppUser user);
    }
}
