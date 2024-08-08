using GymMangamentSystem.Core.Dtos.Auth;
using GymMangamentSystem.Core.Enums.Auth;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Identity;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Reposatory.Services.Auth
{
    public class AccountService : IAccountService
    {
        #region MyRegion
        private readonly UserManager<AppUser> _userManager;
        private readonly MailSettings _mailSettings;
        private readonly ITokenService _TokenService;
        private readonly IOtpService _otpService;
        private readonly IMemoryCache _cache;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager,
            IOptionsMonitor<MailSettings> options,
            ITokenService tokenService,
            IOtpService otpService,
            IMemoryCache cache,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _mailSettings = options.CurrentValue;
            _TokenService = tokenService;
            _otpService = otpService;
            _cache = cache;
            _signInManager = signInManager;
        }
        #endregion

        public async Task<ApiResponse> RegisterAsync(Register dto, Func<string, string, string> generateCallBackUrl)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is not null)
            {
                return new ApiResponse(400, "User with this email already exists.");
            }

            user = new AppUser
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.Email.Split('@')[0],
                UserRole = (int)dto.UserRole,
                EmailConfirmed = false
            };
            var Result = await _userManager.CreateAsync(user, dto.Password);

            if (!Result.Succeeded)
            {
                return new ApiResponse(400, "Something went wrong with the data you entered");
            }

            var roleName = GetUserRoleName(dto.UserRole);
            await _userManager.AddToRoleAsync(user, roleName);

            var EmailConfirmation = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callBackUrl = generateCallBackUrl(EmailConfirmation, user.Id);
            var emailBody = $"<h1>Dear {user.UserName}! Welcome To BNS360.</h1><p>Please <a href='{callBackUrl}'>Click Here</a> To Confirm Your Email.</p>";
            await SendEmailAsync(user.Email, "Email Confirmation", emailBody);

            return new ApiResponse(200, "Email verification has been sent to your email successfully. Please verify it!");

        }
        public async Task<ApiResponse> LoginAsync(Login dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new ApiResponse(400, "User not found.");
            }

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new ApiResponse(400, "Incorrect email or password.");
            }

            if (!user.EmailConfirmed)
            {
                return new ApiResponse(400, "Email not confirmed. Please check your email inbox to verify your email address.");
            }

            return new ApiResponse(200, "Login successful", new UserDto
            {
                Role = (UserRoleEnum)user.UserRole,
                Token = await _TokenService.CreateTokenAsync(user)
            });
        }

        public async Task<ApiResponse> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse(400, "User not found.");
            }
            var otp = _otpService.GenerateOtp(email);
            try
            {
                await SendEmailAsync(email,
                    "Verification Code",
                    $"<h1>Your Verification Code is: {otp}</h1><p>Please use this code to reset your password This otp will expire in 5 minutes.</p>");

                return new ApiResponse(200, "Password reset email sent successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, ex.Message);
            }
        }
        public ApiResponse VerfiyOtp(VerifyOtp dto)
        {
            var isValidOtp = _otpService.IsValidOtp(dto.Email, dto.Otp);
            if (!isValidOtp)
            {
                return new ApiResponse(400, "Invalid OTP.");
            }
            return new ApiResponse(200, "OTP verified successfully.");
        }
        public async Task<ApiResponse> ResetPasswordAsync(ResetPassword dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ApiResponse(400, "User not found.");
            }

            if (!_cache.TryGetValue(dto.Email, out bool isOtpValid) || !isOtpValid)
            {
                return new ApiResponse(400, "You have not verified your email address (OTP).");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, dto.Password);
            if (resetResult.Succeeded)
            {
                return new ApiResponse(200, "Password reset successfully.");
            }

            var errorMessages = string.Join(", ", resetResult.Errors.Select(e => e.Description));
            return new ApiResponse(500, $"Failed to reset password: {errorMessages}");
        }
        public async Task<ApiResponse> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new ApiResponse(400, "User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return new ApiResponse(200, "Password changed successfully.");
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new ApiResponse(400, $"Failed to change password. Errors: {errors}");
        }
        public async Task<ApiResponse> ResendConfirmationEmailAsync(string email, Func<string, string, string> generateCallBackUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ApiResponse(400, "User with this email does not exist.");
            }

            if (user.EmailConfirmed)
            {
                return new ApiResponse(400, "Email is already confirmed.");
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callBackUrl = generateCallBackUrl(emailConfirmationToken, user.Id);
            var emailBody = $"<h1>Dear {user.UserName}! Welcome To BNS360.</h1><p>Please <a href='{callBackUrl}'>Click Here</a> To Confirm Your Email.</p>";

            await SendEmailAsync(user.Email, "Email Confirmation", emailBody);

            return new ApiResponse(200, "Email verification has been resent to your email successfully. Please verify it!");
        }

        //Helper Methods
        public async Task<bool> ConfirmUserEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var confirmed = await _userManager.ConfirmEmailAsync(user, token);

            return confirmed.Succeeded;
        }
        public string GetUserRoleName(UserRoleEnum role)
        {
            return role switch
            {
                UserRoleEnum.Admin => "Admin",
                UserRoleEnum.Trainer => "Trainer",
                UserRoleEnum.Member => "Member",
                UserRoleEnum.Receptionist => "Receptionist",
                _ => "Unknown Role"
            };
        }
        public async Task SendEmailAsync(string To, string Subject, string Body, CancellationToken Cancellation = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.DisplayedName, _mailSettings.Email));
            message.To.Add(new MailboxAddress("", To));
            message.Subject = Subject;

            message.Body = new TextPart("html")
            {
                Text = Body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port,
                    SecureSocketOptions.StartTls, Cancellation);
                await client.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password, Cancellation);
                await client.SendAsync(message, Cancellation);
                await client.DisconnectAsync(true, Cancellation);
            }
        }
    }
}
