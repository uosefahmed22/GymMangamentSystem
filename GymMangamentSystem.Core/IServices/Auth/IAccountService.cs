using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.Models.Identity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.IServices.Auth
{
    public interface IAccountService
    {
        Task<ApiResponse> RegisterAsync(Register user, Func<string, string, string> generateCallBackUrl);
        Task<ApiResponse> LoginAsync(Login dto);
        Task<ApiResponse> ForgetPassword(string email);
        ApiResponse VerfiyOtp(VerifyOtp dto);
        Task SendEmailAsync(string To, string Subject, string Body, CancellationToken Cancellation = default);
        Task<bool> ConfirmUserEmailAsync(string userId, string token);
        Task<ApiResponse> ResetPasswordAsync(ResetPassword dto);
        Task<ApiResponse> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
        Task<ApiResponse> ResendConfirmationEmailAsync(string email, Func<string, string, string> generateCallBackUrl);
    }
}