using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Identity;
using GymMangamentSystem.Reposatory.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(ITokenService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            try
            {
                var (newJwtToken, newRefreshToken) = await _authService.RefreshTokenAsync(request.RefreshToken);
                return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken.Token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            var success = await _authService.RevokeTokenAsync(request.RefreshToken);

            if (!success)
                return BadRequest("Invalid token");

            return Ok("Token revoked successfully");
        }
    }
}
