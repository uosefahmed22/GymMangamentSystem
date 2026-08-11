using System.Security.Claims;
using GymMangamentSystem.Apis.Controllers;
using GymMangamentSystem.Apis.Helpers;
using GymMangamentSystem.Core.Errors;
using GymMangamentSystem.Core.IServices.Auth;
using GymMangamentSystem.Core.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GymMangamentSystem.UnitTests.Controllers;

public class AccountControllerTests
{
    [Theory]
    [InlineData(200)]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    public async Task Login_UsesStatusCodeReturnedByService(int serviceStatus)
    {
        var service = new FakeAccountService
        {
            LoginHandler = _ => Task.FromResult(new ApiResponse(serviceStatus, "result"))
        };
        var result = await CreateController(service).Login(new Login { Email = "a@b.com", Password = "password" });
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(serviceStatus, objectResult.StatusCode);
    }

    [Fact]
    public async Task Login_WhenServiceOmitsStatus_ReturnsServerError()
    {
        var service = new FakeAccountService
        {
            LoginHandler = _ => Task.FromResult(new ApiResponse())
        };
        var result = await CreateController(service).Login(new Login { Email = "a@b.com", Password = "password" });
        Assert.Equal(500, Assert.IsType<ObjectResult>(result).StatusCode);
    }

    [Fact]
    public async Task ChangePassword_WhenIdentityClaimMissing_ReturnsUnauthorized()
    {
        var controller = ControllerWithUser(new FakeAccountService(), []);
        var result = await controller.ChangePassword(new ChangePassword { OldPassword = "old", NewPassword = "new" });
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task ChangePassword_WhenIdentityClaimIsNotGuid_ReturnsBadRequest()
    {
        var controller = ControllerWithUser(new FakeAccountService(), [new Claim(ClaimTypes.NameIdentifier, "not-guid")]);
        var result = await controller.ChangePassword(new ChangePassword { OldPassword = "old", NewPassword = "new" });
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ChangePassword_UsesNotFoundStatusReturnedByService()
    {
        var service = new FakeAccountService
        {
            ChangePasswordHandler = (_, _, _) => Task.FromResult(new ApiResponse(404, "missing"))
        };
        var controller = ControllerWithUser(service, [new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())]);
        var result = await controller.ChangePassword(new ChangePassword { OldPassword = "old", NewPassword = "new" });
        Assert.Equal(404, Assert.IsType<ObjectResult>(result).StatusCode);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ConfirmUserEmail_MapsServiceResult(bool confirmed)
    {
        var service = new FakeAccountService { ConfirmResult = confirmed };
        var result = await CreateController(service).ConfirmUserEmail("id", "token");
        if (confirmed) Assert.IsType<RedirectResult>(result);
        else Assert.IsType<BadRequestObjectResult>(result);
    }

    private static AccountController CreateController(IAccountService service, string? redirectUrl = "https://client.example.com/email-confirmed") =>
        new(service, Options.Create(new ClientSettings { EmailConfirmationRedirectUrl = redirectUrl }));

    private static AccountController ControllerWithUser(IAccountService service, IEnumerable<Claim> claims)
    {
        var controller = CreateController(service);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"))
            }
        };
        return controller;
    }

    private sealed class FakeAccountService : IAccountService
    {
        public Func<Login, Task<ApiResponse>> LoginHandler { get; set; } = _ => Task.FromResult(new ApiResponse(200));
        public Func<Guid, string, string, Task<ApiResponse>> ChangePasswordHandler { get; set; } = (_, _, _) => Task.FromResult(new ApiResponse(200));
        public bool ConfirmResult { get; set; }
        public Task<ApiResponse> LoginAsync(Login dto) => LoginHandler(dto);
        public Task<ApiResponse> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword) => ChangePasswordHandler(userId, oldPassword, newPassword);
        public Task<bool> ConfirmUserEmailAsync(string userId, string token) => Task.FromResult(ConfirmResult);
        public Task<ApiResponse> RegisterAsync(Register user, Func<string, string, string> generateCallBackUrl) => Task.FromResult(new ApiResponse(200));
        public Task<ApiResponse> ForgetPassword(string email) => Task.FromResult(new ApiResponse(200));
        public ApiResponse VerfiyOtp(VerifyOtp dto) => new(200);
        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellation = default) => Task.CompletedTask;
        public Task<ApiResponse> ResetPasswordAsync(ResetPassword dto) => Task.FromResult(new ApiResponse(200));
        public Task<ApiResponse> ResendConfirmationEmailAsync(string email, Func<string, string, string> generateCallBackUrl) => Task.FromResult(new ApiResponse(200));
    }
}
