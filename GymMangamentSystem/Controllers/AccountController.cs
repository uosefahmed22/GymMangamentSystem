using GymMangamentSystem.Apis.Helpers;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GymMangamentSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ClientSettings _clientSettings;

        public AccountController(IAccountService accountService, IOptions<ClientSettings> clientSettings)
        {
            _accountService = accountService;
            _clientSettings = clientSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.LoginAsync(dto);
            return ToActionResult(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.RegisterAsync(model, GenerateCallBackUrl);
            return ToActionResult(result);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.ForgetPassword(email);
            return ToActionResult(result);
        }

        [Authorize]
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp(VerifyOtp dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _accountService.VerfiyOtp(dto);
            return ToActionResult(result);
        }

        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.ResetPasswordAsync(dto);
            return ToActionResult(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User not authenticated.");
            }

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }


            var result = await _accountService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
            return ToActionResult(result);
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.ResendConfirmationEmailAsync(email, GenerateCallBackUrl);
            return ToActionResult(result);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmUserEmail(string userId, string confirmationToken)
        {
            var result = await _accountService.ConfirmUserEmailAsync(userId!, confirmationToken!);

            if (result)
            {
                if (Uri.TryCreate(_clientSettings.EmailConfirmationRedirectUrl, UriKind.Absolute, out var redirectUrl))
                {
                    return Redirect(redirectUrl.ToString());
                }

                return Ok(new ApiResponse(200, "Email confirmed successfully."));
            }
            else
            {
                return BadRequest("Failed to confirm user email.");
            }
        }
        
        private IActionResult ToActionResult(ApiResponse result)
        {
            var statusCode = result.StatusCode ?? StatusCodes.Status500InternalServerError;
            return StatusCode(statusCode, result);
        }

        //Helper Method
        private string GenerateCallBackUrl(string token, string userId)
        {
            var encodedToken = Uri.EscapeDataString(token);
            var encodedUserId = Uri.EscapeDataString(userId);
            var callBackUrl = $"{Request.Scheme}://{Request.Host}/api/Account/confirm-email?userId={encodedUserId}&confirmationToken={encodedToken}";
            return callBackUrl;
        }
    }
}
