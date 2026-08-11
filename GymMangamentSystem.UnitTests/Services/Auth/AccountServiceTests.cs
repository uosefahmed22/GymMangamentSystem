using GymMangamentSystem.Core.Dtos.Auth;
using GymMangamentSystem.Core.Enums.Auth;
using GymMangamentSystem.Core.Enums.Business;
using GymMangamentSystem.Core.Models.Business;
using GymMangamentSystem.Core.Models.Identity;
using GymMangamentSystem.Reposatory.Services.Auth;
using GymMangamentSystem.UnitTests.TestDoubles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace GymMangamentSystem.UnitTests.Services.Auth;

public class AccountServiceTests
{
    [Fact]
    public async Task RegisterAsync_WhenEmailExists_ReturnsBadRequest()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User());
        var result = await h.Sut.RegisterAsync(Register(), (_, _) => "callback");
        Assert.Equal(400, result.StatusCode);
        Assert.Contains("already exists", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_WhenIdentityCreationFails_ReturnsBadRequest()
    {
        using var h = new Harness();
        h.Users.CreateHandler = (_, _) => Task.FromResult(IdentityResult.Failed(new IdentityError()));
        var result = await h.Sut.RegisterAsync(Register(), (_, _) => "callback");
        Assert.Equal(400, result.StatusCode);
        Assert.Empty(h.Email.Sent);
    }

    [Fact]
    public async Task RegisterAsync_WhenValid_CreatesCodedUserAddsRoleAndSendsConfirmation()
    {
        using var h = new Harness();
        h.Users.UsersValue.AddRange([User(), User()]);
        AppUser? created = null;
        string? role = null;
        h.Users.CreateHandler = (user, _) => { created = user; return Task.FromResult(IdentityResult.Success); };
        h.Users.AddToRoleHandler = (_, value) => { role = value; return Task.FromResult(IdentityResult.Success); };

        var result = await h.Sut.RegisterAsync(Register(), (token, id) => $"callback/{id}/{token}");

        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(created);
        Assert.StartsWith("G", created.UserCode);
        Assert.Equal(9, created.UserCode.Length);
        Assert.Equal("Member", role);
        var email = Assert.Single(h.Email.Sent);
        Assert.Equal("new@test.com", email.To);
        Assert.Contains("callback/", email.Body);
    }

    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ReturnsBadRequest()
    {
        using var h = new Harness();
        var result = await h.Sut.LoginAsync(new Login { Email = "x@test.com", Password = "password" });
        Assert.Equal(400, result.StatusCode);
        Assert.Contains("not found", result.Message);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsWrong_ReturnsBadRequest()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User(confirmed: true));
        var result = await h.Sut.LoginAsync(new Login { Email = "x@test.com", Password = "wrong" });
        Assert.Equal(400, result.StatusCode);
        Assert.Contains("Incorrect", result.Message);
    }

    [Fact]
    public async Task LoginAsync_WhenEmailIsUnconfirmed_ReturnsBadRequestWithoutCreatingToken()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User(confirmed: false));
        h.Users.CheckPasswordHandler = (_, _) => Task.FromResult(true);
        var tokenCalls = 0;
        h.Tokens.CreateHandler = _ => { tokenCalls++; return Task.FromResult(("jwt", new RefreshToken())); };
        var result = await h.Sut.LoginAsync(new Login { Email = "x@test.com", Password = "password" });
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(0, tokenCalls);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsTokensAndUserData()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User(confirmed: true));
        h.Users.CheckPasswordHandler = (_, _) => Task.FromResult(true);
        h.Tokens.CreateHandler = _ => Task.FromResult(("jwt-value", new RefreshToken { Token = "refresh-value" }));
        var result = await h.Sut.LoginAsync(new Login { Email = "x@test.com", Password = "password" });
        var data = Assert.IsType<UserDto>(result.Data);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("jwt-value", data.Token);
        Assert.Equal("refresh-value", data.RefreshToken);
    }

    [Fact]
    public async Task ForgetPassword_WhenUserDoesNotExist_ReturnsBadRequest()
    {
        using var h = new Harness();
        var result = await h.Sut.ForgetPassword("missing@test.com");
        Assert.Equal(400, result.StatusCode);
        Assert.Empty(h.Email.Sent);
    }

    [Fact]
    public async Task ForgetPassword_WhenUserExists_SendsOtpEmail()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User());
        h.Otp.GeneratedOtp = "654321";
        var result = await h.Sut.ForgetPassword("user@test.com");
        Assert.Equal(200, result.StatusCode);
        Assert.Contains("654321", Assert.Single(h.Email.Sent).Body);
    }

    [Fact]
    public async Task ForgetPassword_WhenEmailFails_ReturnsBadRequest()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User());
        h.Email.ExceptionToThrow = new InvalidOperationException("smtp unavailable");
        var result = await h.Sut.ForgetPassword("user@test.com");
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("smtp unavailable", result.Message);
    }

    [Theory]
    [InlineData(false, 400)]
    [InlineData(true, 200)]
    public void VerifyOtp_MapsOtpResult(bool valid, int expectedStatus)
    {
        using var h = new Harness();
        h.Otp.IsValid = valid;
        var result = h.Sut.VerfiyOtp(new VerifyOtp { Email = "user@test.com", Otp = "123456" });
        Assert.Equal(expectedStatus, result.StatusCode);
    }

    [Fact]
    public async Task ResetPassword_WhenUserDoesNotExist_ReturnsBadRequest()
    {
        using var h = new Harness();
        var result = await h.Sut.ResetPasswordAsync(Reset());
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task ResetPassword_WhenOtpWasNotVerified_ReturnsBadRequest()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User());
        var result = await h.Sut.ResetPasswordAsync(Reset());
        Assert.Equal(400, result.StatusCode);
        Assert.Contains("not verified", result.Message);
    }

    [Fact]
    public async Task ResetPassword_WhenSuccessful_ConsumesOtpVerification()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User());
        h.Cache.Set("user@test.com", true);
        var result = await h.Sut.ResetPasswordAsync(Reset());
        Assert.Equal(200, result.StatusCode);
        Assert.False(h.Cache.TryGetValue("user@test.com", out bool _));
    }

    [Fact]
    public async Task ResetPassword_WhenIdentityFails_ReturnsServerErrorWithDescriptions()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User());
        h.Users.ResetPasswordHandler = (_, _, _) => Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "weak password" }));
        h.Cache.Set("user@test.com", true);
        var result = await h.Sut.ResetPasswordAsync(Reset());
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("weak password", result.Message);
    }

    [Fact]
    public async Task ChangePassword_WhenUserDoesNotExist_ReturnsNotFound()
    {
        using var h = new Harness();
        var result = await h.Sut.ChangePasswordAsync(Guid.NewGuid(), "old", "new");
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_WhenOldPasswordIsWrong_ReturnsBadRequest()
    {
        using var h = new Harness();
        h.Users.FindByIdHandler = _ => Task.FromResult<AppUser?>(User());
        var result = await h.Sut.ChangePasswordAsync(Guid.NewGuid(), "old", "new");
        Assert.Equal(400, result.StatusCode);
        Assert.Contains("old password", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ChangePassword_WhenSuccessful_SignsOutCurrentSession()
    {
        using var h = new Harness();
        h.Users.FindByIdHandler = _ => Task.FromResult<AppUser?>(User());
        h.Users.CheckPasswordHandler = (_, _) => Task.FromResult(true);
        var result = await h.Sut.ChangePasswordAsync(Guid.NewGuid(), "old", "new");
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(1, h.SignIn.SignOutCalls);
    }

    [Fact]
    public async Task ChangePassword_WhenIdentityFails_DoesNotSignOut()
    {
        using var h = new Harness();
        h.Users.FindByIdHandler = _ => Task.FromResult<AppUser?>(User());
        h.Users.CheckPasswordHandler = (_, _) => Task.FromResult(true);
        h.Users.ChangePasswordHandler = (_, _, _) => Task.FromResult(IdentityResult.Failed(new IdentityError()));
        var result = await h.Sut.ChangePasswordAsync(Guid.NewGuid(), "old", "new");
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(0, h.SignIn.SignOutCalls);
    }

    [Fact]
    public async Task ConfirmUserEmail_WhenUserMissing_ReturnsFalse()
    {
        using var h = new Harness();
        Assert.False(await h.Sut.ConfirmUserEmailAsync("missing", "token"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ConfirmUserEmail_ReturnsIdentityResult(bool succeeds)
    {
        using var h = new Harness();
        h.Users.FindByIdHandler = _ => Task.FromResult<AppUser?>(User());
        h.Users.ConfirmEmailHandler = (_, _) => Task.FromResult(succeeds ? IdentityResult.Success : IdentityResult.Failed(new IdentityError()));
        Assert.Equal(succeeds, await h.Sut.ConfirmUserEmailAsync("id", "token"));
    }

    [Fact]
    public async Task ResendConfirmation_WhenAlreadyConfirmed_ReturnsBadRequest()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User(confirmed: true));
        var result = await h.Sut.ResendConfirmationEmailAsync("user@test.com", (_, _) => "callback");
        Assert.Equal(400, result.StatusCode);
        Assert.Empty(h.Email.Sent);
    }

    [Fact]
    public async Task ResendConfirmation_WhenUnconfirmed_SendsEmail()
    {
        using var h = new Harness();
        h.Users.FindByEmailHandler = _ => Task.FromResult<AppUser?>(User());
        var result = await h.Sut.ResendConfirmationEmailAsync("user@test.com", (_, _) => "callback");
        Assert.Equal(200, result.StatusCode);
        Assert.Single(h.Email.Sent);
    }

    [Theory]
    [InlineData(UserRoleEnum.Admin, "Admin")]
    [InlineData(UserRoleEnum.Member, "Member")]
    [InlineData(UserRoleEnum.Trainer, "Trainer")]
    [InlineData(UserRoleEnum.Receptionist, "Receptionist")]
    [InlineData((UserRoleEnum)999, "Unknown Role")]
    public void GetUserRoleName_MapsEveryRole(UserRoleEnum role, string expected)
    {
        using var h = new Harness();
        Assert.Equal(expected, h.Sut.GetUserRoleName(role));
    }

    private static AppUser User(bool confirmed = false) => new()
    {
        Id = Guid.NewGuid().ToString(), Email = "user@test.com", UserName = "user",
        DisplayName = "User", UserCode = "G1", UserRole = (int)UserRoleEnum.Member,
        EmailConfirmed = confirmed
    };

    private static Register Register() => new()
    {
        DisplayName = "New", Email = "new@test.com", Password = "password",
        ConfirmPassword = "password", UserRole = UserRoleEnum.Member,
        MembershipType = MembershipType.Gold_1Month
    };

    private static ResetPassword Reset() => new()
    {
        Email = "user@test.com", Password = "new-password", ConfirmPassword = "new-password"
    };

    private sealed class Harness : IDisposable
    {
        public FakeUserManager Users { get; } = new();
        public FakeTokenService Tokens { get; } = new();
        public FakeOtpService Otp { get; } = new();
        public FakeEmailService Email { get; } = new();
        public MemoryCache Cache { get; } = new(new MemoryCacheOptions());
        public FakeSignInManager SignIn { get; }
        public AccountService Sut { get; }
        public Harness()
        {
            SignIn = new FakeSignInManager(Users);
            Sut = new AccountService(Users, Tokens, Otp, Email, Cache, SignIn);
        }
        public void Dispose() { Cache.Dispose(); Users.Dispose(); }
    }
}
